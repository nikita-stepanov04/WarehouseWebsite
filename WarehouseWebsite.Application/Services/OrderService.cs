using System.Data;
using WarehouseWebsite.Application.Interfaces;
using WarehouseWebsite.Domain.Filtering;
using WarehouseWebsite.Domain.Interfaces;
using WarehouseWebsite.Domain.Interfaces.Repositories;
using WarehouseWebsite.Domain.Models.Items;
using WarehouseWebsite.Domain.Models.Orders;

namespace WarehouseWebsite.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderRepository _orderRepository;
        private readonly IAwaitingOrderRepository _awaitingOrderRepository;
        private readonly IMissingItemRepository _missingItemRepository;
        private readonly IItemRepository _itemRepository;

        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _orderRepository = unitOfWork.OrderRepository;
            _awaitingOrderRepository = unitOfWork.AwaitingOrderRepository;
            _itemRepository = unitOfWork.ItemRepository;
            _missingItemRepository = unitOfWork.MissingItemRepository;
        }

        public async Task<IEnumerable<Order>> GetAwaitingOrdersAsync(
            FilterParameters<AwaitingOrder> filter, CancellationToken token)
        {
            return await _awaitingOrderRepository.GetAwaitingOrdersAsync(filter, token);
        }

        public async Task<IEnumerable<Order>> GetTransitedOrdersAsync(
            FilterParameters<Order> filter, CancellationToken token)
        {
            return await _orderRepository.GetTransitedOrdersAsync(filter, token);
        }

        public async Task<IEnumerable<Order>> GetTransitingOrdersAsync(
            FilterParameters<Order> filter, CancellationToken token)
        {
            return await _orderRepository.GetTransitingOrdersAsync(filter, token);
        }

        public async Task StartShippingItemsAsync()
        {
            var jobExecutionStartTime = DateTime.UtcNow;
            var filter = new FilterParameters<AwaitingOrder>
            {
                Take = 2,
                Filter = o => o.OrderTime <= jobExecutionStartTime
            };

            while (true)
            {
                var awaitingOrders = ((await _awaitingOrderRepository.GetAwaitingOrdersAsync(
                    filter, token: default, withItems: false)) as List<AwaitingOrder>)!;

                if (awaitingOrders.Count == 0)
                    break;                

                foreach (var awaitingOrder in awaitingOrders)
                {
                    try
                    {
                        var cancellationSource = new CancellationTokenSource(TimeSpan.FromSeconds(2));
                        var token = cancellationSource.Token;
                        //var token = CancellationToken.None;
                        await _unitOfWork.BeginTransactionAsync(IsolationLevel.RepeatableRead, token);
                        
                        var itemIds = awaitingOrder.OrderItems.Select(i => i.ItemId).ToList();                        
                        var items = await _itemRepository.GetItemsByIdsAsNoTracking(itemIds, token);
                        
                        // temporary store for updated item quantity,
                        // is not applied to items if order is still awaiting
                        Dictionary<Item, int> itemNewQuantityDict = new();
                        bool isAwaiting = false;
                        foreach (var orderItem in awaitingOrder.OrderItems)
                        {                            
                            Item item = items.First(i => i.Id == orderItem.ItemId);
                            if (item.Quantity < orderItem.Quantity)
                            {
                                isAwaiting = true;
                                break;
                            }
                            else
                            {
                                itemNewQuantityDict.Add(item, item.Quantity - orderItem.Quantity);
                            }
                        }

                        if (isAwaiting)
                        {
                            await _unitOfWork.RollbackTransactionAsync();
                            filter.Skip++;
                        }
                        else 
                        {
                            var order = awaitingOrder.ToOrder();
                            await _orderRepository.AddAsync(order);
                            _unitOfWork.DetachItems();

                            UpdateItemQuantities(itemNewQuantityDict);

                            _awaitingOrderRepository.Remove(awaitingOrder);

                            await _unitOfWork.SaveAsync(token);
                            await _unitOfWork.CommitTransactionAsync(token);
                        }
                        _unitOfWork.DetachItems();
                    }
                    catch 
                    {
                        await _unitOfWork.RollbackTransactionAsync();
                    }
                }
                _unitOfWork.ClearContext();
            }
        }

        public async Task<Guid> PlaceOrderAsync(Order order, Guid customerId)
        {
            order.CustomerId = customerId;
            order.OrderTime = DateTime.UtcNow;
            order.Status = OrderStatus.Transiting;

            try
            {
                var cancellationSource = new CancellationTokenSource(TimeSpan.FromSeconds(2));
                var token = cancellationSource.Token;
                //var token = CancellationToken.None;
                await _unitOfWork.BeginTransactionAsync(IsolationLevel.RepeatableRead, token);

                var itemIds = order.OrderItems.Select(i => i.ItemId).ToList();

                var items = await _itemRepository.GetItemsByIdsAsNoTracking(itemIds, token);

                // temporary store for updated item quantity,
                // is not applied to items if order is an awaiting order
                Dictionary<Item, int> itemNewQuantityDict = new();

                List<MissingItem> addToMissingList = new();

                foreach (var orderItem in order.OrderItems)
                {
                    Item item = items.First(i => i.Id == orderItem.ItemId);
                    if (item.Quantity >= orderItem.Quantity)
                        itemNewQuantityDict.Add(item, item.Quantity - orderItem.Quantity);
                    else
                    {
                        addToMissingList.Add(new MissingItem
                        {
                            ItemId = item.Id,
                            Missing = orderItem.Quantity - item.Quantity
                        });
                    }

                    orderItem.Price = item.Price;
                    order.TotalPrice += item.Price * orderItem.Quantity;
                }

                if (addToMissingList.Any())
                {
                    var awaitingOrder = new AwaitingOrder(order);
                    await _awaitingOrderRepository.AddAsync(awaitingOrder);

                    await _missingItemRepository.AddToMissing(addToMissingList, token);

                    await _unitOfWork.SaveAsync();
                    await _unitOfWork.CommitTransactionAsync(token);

                    return awaitingOrder.Id;
                }
                else
                {
                    UpdateItemQuantities(itemNewQuantityDict);
                    await _orderRepository.AddAsync(order);
                    await _unitOfWork.SaveAsync(token);
                    await _unitOfWork.CommitTransactionAsync(token);
                    return order.Id;
                }
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task SetOrderAsTransitedByIdAsync(Guid id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order != null)
            {
                _orderRepository.SetOrderAsTransited(order);
            }
            await _unitOfWork.SaveAsync();
        }

        private void UpdateItemQuantities(Dictionary<Item, int> itemNewQuantityDict)
        {
            foreach (var itemKvp in itemNewQuantityDict)
            {
                var item = itemKvp.Key;
                item.Quantity = itemKvp.Value;
                _itemRepository.UpdateQuantity(item);
            }
        }
    }
}

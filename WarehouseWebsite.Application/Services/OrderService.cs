﻿using AutoMapper;
using System.Data;
using WarehouseWebsite.Application.Interfaces;
using WarehouseWebsite.Application.Models;
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
        private readonly IItemRepository _itemRepository;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _orderRepository = unitOfWork.OrderRepository;
            _awaitingOrderRepository = unitOfWork.AwaitingOrderRepository;
            _itemRepository = unitOfWork.ItemRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderDTO>> GetAwaitingOrdersAsync(
            FilterParameters<AwaitingOrder> filter, CancellationToken token)
        {
            var orders = await _awaitingOrderRepository.GetAwaitingOrdersAsync(filter, token);
            return _mapper.Map<IEnumerable<OrderDTO>>(orders.Select(o => (Order)o));
        }

        public async Task<IEnumerable<OrderDTO>> GetTransitedOrdersAsync(
            FilterParameters<Order> filter, CancellationToken token)
        {
            var orders = await _orderRepository.GetTransitedOrdersAsync(filter, token);
            return _mapper.Map<IEnumerable<OrderDTO>>(orders);
        }

        public async Task<IEnumerable<OrderDTO>> GetTransitingOrdersAsync(
            FilterParameters<Order> filter, CancellationToken token)
        {
            var orders = await _orderRepository.GetTransitingOrdersAsync(filter, token);
            return _mapper.Map<IEnumerable<OrderDTO>>(orders);
        }

        public async Task PlaceOrderAsync(OrderDTO orderDTO, Guid customerId)
        {
            Order order = _mapper.Map<Order>(orderDTO);
            order.CustomerId = customerId;
            order.OrderTime = DateTime.UtcNow;

            try
            {
                var cancellationSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(200));
                var token = cancellationSource.Token;
                await _unitOfWork.BeginTransactionAsync(IsolationLevel.RepeatableRead, token);

                var itemIds = order.OrderItems.Select(i => i.ItemId).ToList();
                var filter = new FilterParameters<Item>
                {
                    Filter = i => itemIds.Contains(i.Id)
                };

                var items = await _itemRepository.GetItemsByFilterAsync(filter, token);

                // temporary store for updated item quantity,
                // is not applied to items if order is an awaiting order
                Dictionary<Item, int> itemNewQuantityDict = new(); 
                bool awaitingOrder = false;

                foreach(var orderItem in order.OrderItems)
                {
                    Item item = items.First(i => i.Id == orderItem.ItemId);

                    if (item.Quantity >= orderItem.Quantity)                    
                        itemNewQuantityDict.Add(item, item.Quantity - orderItem.Quantity);                    
                    else
                        awaitingOrder = true;

                    orderItem.Price = item.Price;
                    order.TotalPrice += item.Price;
                }

                if (awaitingOrder)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    await _awaitingOrderRepository.AddAsync((order as AwaitingOrder)!);
                    await _unitOfWork.SaveAsync();
                }
                else
                {
                    foreach (var itemKvp in itemNewQuantityDict)
                    {
                        itemKvp.Key.Quantity = itemKvp.Value;
                    }
                    await _orderRepository.AddAsync(order);
                    await _unitOfWork.SaveAsync(token);
                    await _unitOfWork.CommitTransactionAsync(token);
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

        public Task StartShippingItemsAsync()
        {
            throw new NotImplementedException();
        }
    }
}

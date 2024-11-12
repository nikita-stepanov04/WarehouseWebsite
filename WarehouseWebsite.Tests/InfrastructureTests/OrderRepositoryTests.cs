using WarehouseWebsite.Domain.Filtering;
using WarehouseWebsite.Domain.Models.Orders;
using WarehouseWebsite.Infrastructure.Data;
using WarehouseWebsite.Infrastructure.Models;
using static WarehouseWebsite.Tests.UnitTestHelper;

namespace WarehouseWebsite.Tests.InfrastructureTests
{
    [TestFixture]
    internal class OrderRepositoryTests
    {
        [Test]
        public async Task OrderRepositoryGetTransitedOrdersReturnsTransitedOrders()
        {
            using var context = new DataContext(GetUnitTestDbOptions());
            var orderRepository = new OrderRepository(context);
            var orders = (await orderRepository
                .GetTransitedOrdersAsync(new FilterParameters<Order>(), default))
                .ToList();

            Assert.That(orders.Count, Is.EqualTo(1));
            Assert.That(orders.All(o => o.Status == OrderStatus.Transited));

            var order = orders.First(o => o.Id == Guids[1]);
            var expected = GetTransitedOrder();

            Assert.That(order, Is.EqualTo(expected).Using(new OrderEqualityComparer()));
            Assert.That(order.OrderItems, Is.EqualTo(expected.OrderItems)
                .Using(new OrderItemEqualityComparer()));
        }

        [Test]
        public void OrderRepositoryGetTransitedOrdersCancelsWhenTokenIsCancelled()
        {
            using var context = new DataContext(GetUnitTestDbOptions());
            var orderRepository = new OrderRepository(context);

            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();

            Assert.ThrowsAsync<OperationCanceledException>(async () =>
                await orderRepository.GetTransitedOrdersAsync(
                    new FilterParameters<Order>(), cancellationTokenSource.Token));
        }

        [Test]
        public async Task OrderRepositoryGetTransitingOrdersReturnsTransitingOrders()
        {
            using var context = new DataContext(GetUnitTestDbOptions());
            var orderRepository = new OrderRepository(context);
            var orders = (await orderRepository
                .GetTransitingOrdersAsync(new FilterParameters<Order>(), default))
                .ToList();

            Assert.That(orders.Count, Is.EqualTo(1));
            Assert.That(orders.All(o => o.Status == OrderStatus.Transiting));

            var order = orders.First(o => o.Id == Guids[2]);
            var expected = GetTransitingOrder();

            Assert.That(order, Is.EqualTo(expected).Using(new OrderEqualityComparer()));
            Assert.That(order.OrderItems, Is.EqualTo(expected.OrderItems)
                .Using(new OrderItemEqualityComparer()));
        }

        [Test]
        public void OrderRepositoryGetTransitingOrdersCancelsWhenTokenIsCancelled()
        {
            using var context = new DataContext(GetUnitTestDbOptions());
            var orderRepository = new OrderRepository(context);

            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();

            Assert.ThrowsAsync<OperationCanceledException>(async () =>
                await orderRepository.GetTransitingOrdersAsync(
                    new FilterParameters<Order>(), cancellationTokenSource.Token));
        }

        [Test]
        public async Task OrderRepositoryPlaceOrderAsyncAddsOrdersToDb()
        {
            using var context = new DataContext(GetUnitTestDbOptions());
            var orderRepository = new OrderRepository(context);

            var newOrder = new Order()
            {
                Id = Guids[4],
                CustomerId = Guids[1],
                OrderTime = new DateTime(2023, 11, 15, 10, 0, 0),
                Status = OrderStatus.Transited,
                TotalPrice = 68,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem
                    {
                        Id = Guids[5],
                        OrderId = Guids[4],
                        ItemId = Guids[2],
                        Quantity = 4,
                        Price = 17,
                    }
                }
            };
            await orderRepository.PlaceOrderAsync(newOrder);
            await context.SaveChangesAsync();

            Assert.That(context.Orders.Count(), Is.EqualTo(3));

            var order = await orderRepository.GetByIdAsync(Guids[4]);

            Assert.That(order, Is.EqualTo(newOrder).Using(new OrderEqualityComparer()));
        }

        [Test]
        public async Task OrderRepositorySetOrderAsTransitedWorks()
        {
            using var context = new DataContext(GetUnitTestDbOptions());
            var orderRepository = new OrderRepository(context);

            var transitingOrder = new Order { Id = Guids[2] };
            orderRepository.SetOrderAsTransited(transitingOrder);
            await context.SaveChangesAsync();

            var updatedOrder = await orderRepository.GetByIdAsync(Guids[2]);
            Assert.That(updatedOrder.Status, Is.EqualTo(OrderStatus.Transited));
        }        

        Order GetTransitedOrder() => new Order
        {
            Id = Guids[1],
            CustomerId = Guids[1],
            OrderTime = new DateTime(2023, 11, 15, 10, 0, 0),
            Status = OrderStatus.Transited,
            TotalPrice = 68,
            OrderItems = new List<OrderItem>
            {
                new OrderItem
                {
                    Id = Guids[2],
                    OrderId = Guids[1],
                    ItemId = Guids[2],
                    Quantity = 4,
                    Price = 17
                }
            }
        };

        Order GetTransitingOrder() => new Order
        {
            Id = Guids[2],
            CustomerId = Guids[0],
            OrderTime = new DateTime(2023, 11, 15, 10, 0, 0),
            Status = OrderStatus.Transiting,
            TotalPrice = 78,
            OrderItems = new List<OrderItem>
            {
                new OrderItem
                {
                    Id = Guids[3],
                    OrderId = Guids[2],
                    ItemId = Guids[2],
                    Quantity = 4,
                    Price = 17
                },
                new OrderItem
                {
                    Id = Guids[4],
                    OrderId = Guids[2],
                    ItemId = Guids[3],
                    Quantity = 2,
                    Price = 5
                }
            }
        };
    }
}

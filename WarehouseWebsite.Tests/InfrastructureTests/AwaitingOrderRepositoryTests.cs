using WarehouseWebsite.Domain.Filtering;
using WarehouseWebsite.Domain.Models.Orders;
using WarehouseWebsite.Infrastructure.Data;
using WarehouseWebsite.Infrastructure.Models;
using static WarehouseWebsite.Tests.UnitTestHelper;

namespace WarehouseWebsite.Tests.InfrastructureTests
{
    [TestFixture]
    public class AwaitingOrderRepositoryTests
    {
        [Test]
        public async Task AwaitingOrderRepositoryGetAwaitingOrdersReturnsAwaitingOrders()
        {
            using var context = new DataContext(GetUnitTestDbOptions());
            var orderRepository = new AwaitingOrderRepository(context);
            var orders = (await orderRepository
                .GetAwaitingOrdersAsync(new FilterParameters<AwaitingOrder>(), default))
                .ToList();

            Assert.That(orders.Count, Is.EqualTo(1));
            Assert.That(orders.All(o => o.Status == OrderStatus.Awaiting));

            var order = orders.First(o => o.Id == Guids[0]);
            var expected = GetAwaitingOrder();

            Assert.That(order, Is.EqualTo(expected).Using(new OrderEqualityComparer()));
            Assert.That(order.OrderItems, Is.EqualTo(expected.OrderItems)
                .Using(new OrderItemEqualityComparer()));
        }

        [Test]
        public void AwaitingOrderRepositoryGetAwaitingOrdersCancelsWhenTokenIsCancelled()
        {
            using var context = new DataContext(GetUnitTestDbOptions());
            var orderRepository = new AwaitingOrderRepository(context);

            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();

            Assert.ThrowsAsync<OperationCanceledException>(async () =>
                await orderRepository.GetAwaitingOrdersAsync(
                    new FilterParameters<AwaitingOrder>(), cancellationTokenSource.Token));
        }         

        private AwaitingOrder GetAwaitingOrder() =>
            new AwaitingOrder
            {
                Id = Guids[0],
                CustomerId = Guids[0],
                OrderTime = new DateTime(2023, 10, 20, 14, 30, 0),
                Status = OrderStatus.Awaiting,
                TotalPrice = 6501,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem
                    {
                        Id = Guids[0],
                        OrderId = Guids[0],
                        ItemId = Guids[0],
                        Quantity = 1,
                        Price = 1500,
                    },
                    new OrderItem
                    {
                        Id = Guids[1],
                        OrderId = Guids[0],
                        ItemId = Guids[1],
                        Quantity = 2,
                        Price = 2500.50m,
                    }
                }

            };
    }
}

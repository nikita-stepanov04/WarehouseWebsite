using WarehouseWebsite.Domain.Models.Items;
using WarehouseWebsite.Domain.Models.Orders;
using WarehouseWebsite.Infrastructure.Data;
using WarehouseWebsite.Infrastructure.Models;
using static WarehouseWebsite.Tests.UnitTestHelper;

namespace WarehouseWebsite.Tests.InfrastructureTests
{
    [TestFixture]
    public class RepositoryTests
    {
        #region GetById

        [Test]
        public async Task GenericRepositoryGetByIdAsyncReturnsSingleItemValue()
        {
            Guid id = Guids[1];

            using var context = new DataContext(GetUnitTestDbOptions());
            var itemRepository = new ItemRepository(context);
            var item = await itemRepository.GetByIdAsync(id);

            var expected = ExpectedItems.FirstOrDefault(i => i.Id == id);
            Assert.That(item, Is.EqualTo(expected).Using(new ItemEqualityComparer()),
                message: "Generic repository GetByIdAsync works incorrect with Item type");
        }
        
        [Test]
        public async Task GenericRepositoryGetByIdAsyncReturnsSingleOrderValue()
        {
            Guid id = Guids[1];

            using var context = new DataContext(GetUnitTestDbOptions());
            var orderRepository = new OrderRepository(context);
            var order = await orderRepository.GetByIdAsync(id);

            var expected = ExpectedOrders.FirstOrDefault(o => o.Id == id);
            Assert.That(order, Is.EqualTo(expected).Using(new OrderEqualityComparer()),
                message: "Generic repository GetByIdAsync works incorrect with Order type");
        }

        #endregion

        #region AddAsync

        [Test]
        public async Task GenericRepositoryAddAsyncAddsItemValue()
        {
            var item = new Item()
            {
                Id = Guids[4],
                Name = "Cooking book",
                Quantity = 120,
                Description = "Description for cooking book",
                Price = 8,
                Weight = 0.4,
                Category = ItemCategory.Books
            };

            using var context = new DataContext(GetUnitTestDbOptions());
            var itemRepository = new ItemRepository(context);
            await itemRepository.AddAsync(item);
            await context.SaveChangesAsync();

            var addedItem = await itemRepository.GetByIdAsync(Guids[4]);

            Assert.That(context.Items.Count(), Is.EqualTo(5),
                message: "Generic repository AddAsync don't add Item type");
            Assert.That(addedItem, Is.EqualTo(item).Using(new ItemEqualityComparer()),
                message: "Generic repository AddAsync adds Item type incorrectly");
        }

        [Test]
        public async Task GenericRepositoryAddAsyncAddsOrderValue()
        {
            var order = new Order()
            {
                Id = Guids[3],
                CustomerId = Guids[0],
                OrderTime = new DateTime(2023, 10, 20, 14, 30, 0),
                Status = OrderStatus.Awaiting,
                TotalPrice = 6501,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { Id = Guids[5], OrderId = Guids[2], ItemId = Guids[0], Quantity = 1, Price = 1500 },
                    new OrderItem { Id = Guids[6], OrderId = Guids[2], ItemId = Guids[1], Quantity = 2, Price = 2500.50m }
                }
            };

            using var context = new DataContext(GetUnitTestDbOptions());
            var orderRepository = new OrderRepository(context);
            await orderRepository.AddAsync(order);
            await context.SaveChangesAsync();

            var addedOrder = await orderRepository.GetByIdAsync(Guids[3]);

            Assert.That(context.Orders.Count(), Is.EqualTo(3),
                message: "Generic repository AddAsync don't add Order type");
            Assert.That(addedOrder, Is.EqualTo(order).Using(new OrderEqualityComparer()),
                message: "Generic repository AddAsync adds Order type incorrectly");
            Assert.That(context.OrderItems.Count(), Is.EqualTo(7),
                message: "Generic repository AddAsync add Order type correctly, but OrderItems were not added");
        }

        #endregion

        #region Update

        [Test]
        public async Task GenericRepositoryUpdateUpdatesItemValue()
        {
            var item = new Item()
            {
                Id = Guids[0],
                Name = "Updated name",
                Quantity = 1000,
                Description = "Updated description",
                Price = 1000,
                Weight = 1000,
                Category = ItemCategory.BuildingMaterials
            };

            using var context = new DataContext(GetUnitTestDbOptions());
            var itemRepository = new ItemRepository(context);
            itemRepository.Update(item);
            await context.SaveChangesAsync();

            var updatedItem = await itemRepository.GetByIdAsync(Guids[0]);

            Assert.That(updatedItem, Is.EqualTo(item).Using(new ItemEqualityComparer()),
                message: "Generic repository Update updates Item type incorrectly");
        }

        [Test]
        public async Task GenericRepositoryUpdateUpdatesOrderValue()
        {
            var order = new Order()
            {
                Id = Guids[1],
                CustomerId = Guids[1],
                OrderTime = new DateTime(2024, 1, 1, 1, 1, 1),
                Status = OrderStatus.Transited,
                TotalPrice = 1500
            };

            using var context = new DataContext(GetUnitTestDbOptions());
            var orderRepository = new OrderRepository(context);
            orderRepository.Update(order);
            await context.SaveChangesAsync();

            var updatedOrder = await orderRepository.GetByIdAsync(Guids[1]);

            Assert.That(updatedOrder, Is.EqualTo(order).Using(new OrderEqualityComparer()),
                message: "Generic repository Update updates Order type incorrectly");
        }

        #endregion

        #region Remove

        [Test]
        public async Task GenericRepositoryRemoveRemovesCustomerType()
        {
            using var context = new DataContext(GetUnitTestDbOptions());
            var customerRepository = new CustomerRepository(context);
            var customer = await customerRepository.GetByIdAsync(Guids[0]);
            customerRepository.Remove(customer);
            await context.SaveChangesAsync();

            var removedCustomer = await customerRepository.GetByIdAsync(Guids[0]);

            Assert.That(context.Customers.Count(), Is.EqualTo(1),
                message: "Generic repository Remove don't remove Customer type");
            Assert.That(removedCustomer, Is.Null,
                message: "Generic repository Remove don't remove Customer type");            
        }

        #endregion        

        private static IEnumerable<Item> ExpectedItems =>
        [
            new Item() { Id = Guids[0], Name = "Computer", Quantity = 10, Description = "Description for computer", Price = 1500, Weight = 10, Category = ItemCategory.Electronics },
            new Item() { Id = Guids[1], Name = "Big Computer", Quantity = 0, Description = "Description for big computer", Price = 2500.50m, Weight = 15, Category = ItemCategory.Electronics },
        ];
        
        private static IEnumerable<Order> ExpectedOrders =>
        [
            new Order { Id = Guids[0], CustomerId = Guids[0], OrderTime = new DateTime(2023, 10, 20, 14, 30, 0), Status = OrderStatus.Awaiting, TotalPrice = 6501 },
            new Order { Id = Guids[1], CustomerId = Guids[1], OrderTime = new DateTime(2023, 11, 15, 10, 0, 0), Status = OrderStatus.Transited, TotalPrice = 68 } 
        ];
    }
}

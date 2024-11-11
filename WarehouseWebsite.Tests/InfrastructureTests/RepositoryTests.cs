using WarehouseWebsite.Domain.Models.Items;
using WarehouseWebsite.Domain.Models.Orders;
using WarehouseWebsite.Infrastructure.Data;
using WarehouseWebsite.Infrastructure.Models;

namespace WarehouseWebsite.Tests.InfrastructureTests
{
    [TestFixture]
    public class RepositoryTests
    {
        #region GetById

        [TestCase(0)]
        [TestCase(1)]
        public async Task GenericRepositoryGetByIdAsyncReturnsSingleItemValue(int index)
        {
            Guid id = GetGuid(index);

            using var context = new DataContext(UnitTestHelper.GetUnitTestDbOptions());
            var itemRepository = new ItemRepository(context);
            var item = await itemRepository.GetByIdAsync(id);

            var expected = ExpectedItems.FirstOrDefault(i => i.Id == id);
            Assert.That(item, Is.EqualTo(expected).Using(new ItemEqualityComparer()),
                message: "Generic repository GetByIdAsync works incorrect with Item type");
        }
        
        [TestCase(0)]
        [TestCase(1)]
        public async Task GenericRepositoryGetByIdAsyncReturnsSingleOrderValue(int index)
        {
            Guid id = GetGuid(index);

            using var context = new DataContext(UnitTestHelper.GetUnitTestDbOptions());
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
                Id = GetGuid(4),
                Name = "Cooking book",
                Quantity = 120,
                Description = "Description for cooking book",
                Price = 8,
                Weight = 0.4,
                SupplierId = GetGuid(1),
                Category = ItemCategory.Books
            };

            using var context = new DataContext(UnitTestHelper.GetUnitTestDbOptions());
            var itemRepository = new ItemRepository(context);
            await itemRepository.AddAsync(item);
            await context.SaveChangesAsync();

            var addedItem = await itemRepository.GetByIdAsync(GetGuid(4));

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
                Id = GetGuid(2),
                CustomerId = GetGuid(0),
                OrderTime = new DateTime(2023, 10, 20, 14, 30, 0),
                Status = OrderStatus.Queued,
                TotalPrice = 6501,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { Id = GetGuid(3), OrderId = GetGuid(2), ItemId = GetGuid(0), Quantity = 1, Price = 1500 },
                    new OrderItem { Id = GetGuid(4), OrderId = GetGuid(2), ItemId = GetGuid(1), Quantity = 2, Price = 2500.50m }
                }
            };

            using var context = new DataContext(UnitTestHelper.GetUnitTestDbOptions());
            var orderRepository = new OrderRepository(context);
            await orderRepository.AddAsync(order);
            await context.SaveChangesAsync();

            var addedOrder = await orderRepository.GetByIdAsync(GetGuid(2));

            Assert.That(context.Orders.Count(), Is.EqualTo(3),
                message: "Generic repository AddAsync don't add Order type");
            Assert.That(addedOrder, Is.EqualTo(order).Using(new OrderEqualityComparer()),
                message: "Generic repository AddAsync adds Order type incorrectly");
            Assert.That(context.OrderItems.Count(), Is.EqualTo(5),
                message: "Generic repository AddAsync add Order type correctly, but OrderItems were not added");
        }

        #endregion

        #region Update

        [Test]
        public async Task GenericRepositoryUpdateUpdatesItemValue()
        {
            var item = new Item()
            {
                Id = GetGuid(0),
                Name = "Updated name",
                Quantity = 1000,
                Description = "Updated description",
                Price = 1000,
                Weight = 1000,
                SupplierId = GetGuid(2),
                Category = ItemCategory.BuildingMaterials
            };

            using var context = new DataContext(UnitTestHelper.GetUnitTestDbOptions());
            var itemRepository = new ItemRepository(context);
            itemRepository.Update(item);
            await context.SaveChangesAsync();

            var updatedItem = await itemRepository.GetByIdAsync(GetGuid(0));

            Assert.That(updatedItem, Is.EqualTo(item).Using(new ItemEqualityComparer()),
                message: "Generic repository Update updates Item type incorrectly");
        }

        [Test]
        public async Task GenericRepositoryUpdateUpdatesOrderValue()
        {
            var order = new Order()
            {
                Id = GetGuid(0),
                CustomerId = GetGuid(1),
                OrderTime = new DateTime(2024, 1, 1, 1, 1, 1),
                Status = OrderStatus.Delivered,
                TotalPrice = 1500
            };

            using var context = new DataContext(UnitTestHelper.GetUnitTestDbOptions());
            var orderRepository = new OrderRepository(context);
            orderRepository.Update(order);
            await context.SaveChangesAsync();

            var updatedOrder = await orderRepository.GetByIdAsync(GetGuid(0));

            Assert.That(updatedOrder, Is.EqualTo(order).Using(new OrderEqualityComparer()),
                message: "Generic repository Update updates Order type incorrectly");
        }

        #endregion

        #region Remove

        [Test]
        public async Task GenericRepositoryRemoveRemovesSupplierType()
        {
            using var context = new DataContext(UnitTestHelper.GetUnitTestDbOptions());
            var supplierRepository = new SupplierRepository(context);
            var supplier = await supplierRepository.GetByIdAsync(GetGuid(0));
            supplierRepository.Remove(supplier);
            await context.SaveChangesAsync();

            var removedSupplier = await supplierRepository.GetByIdAsync(GetGuid(0));

            Assert.That(context.Suppliers.Count(), Is.EqualTo(2),
                message: "Generic repository Remove don't remove Supplier type");
            Assert.That(removedSupplier, Is.Null,
                message: "Generic repository Remove don't remove Supplier type");            
        }

        #endregion

        private static Guid GetGuid(int index) => UnitTestHelper.Guids[index];

        private static IEnumerable<Item> ExpectedItems =>
        [
            new Item() { Id = GetGuid(0), Name = "Computer", Quantity = 10, Description = "Description for computer", Price = 1500, Weight = 10, SupplierId = GetGuid(0), Category = ItemCategory.Electronics },
            new Item() { Id = GetGuid(1), Name = "Big Computer", Quantity = 0, Description = "Description for big computer", Price = 2500.50m, Weight = 15, SupplierId = GetGuid(0), Category = ItemCategory.Electronics },
        ];
        
        private static IEnumerable<Order> ExpectedOrders =>
        [
            new Order { Id = GetGuid(0), CustomerId = GetGuid(0), OrderTime = new DateTime(2023, 10, 20, 14, 30, 0), Status = OrderStatus.Queued, TotalPrice = 6501 },
            new Order { Id = GetGuid(1), CustomerId = GetGuid(1), OrderTime = new DateTime(2023, 11, 15, 10, 0, 0), Status = OrderStatus.Delivered, TotalPrice = 68 } 
        ];
    }
}

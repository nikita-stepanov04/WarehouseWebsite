using Microsoft.EntityFrameworkCore;
using WarehouseWebsite.Domain.Models.Customers;
using WarehouseWebsite.Domain.Models.Items;
using WarehouseWebsite.Domain.Models.Orders;
using WarehouseWebsite.Infrastructure.Models;

namespace WarehouseWebsite.Tests
{
    internal static class UnitTestHelper
    {
        public static Guid[] Guids { get; set; }

        static UnitTestHelper()
        {
            Guids = Enumerable.Range(0, 10).Select(_ => Guid.NewGuid()).ToArray();
        }

        public static DbContextOptions<DataContext> GetUnitTestDbOptions()
        {
            var opts = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using (var context = new DataContext(opts))
            {
                SeedTestData(context);
            }
            return opts;
        }

        public static void SeedTestData(DataContext context)
        {
            context.Suppliers.AddRange(
                new Supplier { Id = Guids[0], Name = "Technics supplier", Address = "China", Phone = "+86 10 12345678" },
                new Supplier { Id = Guids[1], Name = "Books supplier", Address = "Japan", Phone = "+81 3 87654321" },
                new Supplier { Id = Guids[2], Name = "Home goods supplier", Address = "Italy", Phone = "+39 06 98765432" }
            );
            context.Items.AddRange(
                new Item() { Id = Guids[0], Name = "Computer", Quantity = 10, Description = "Description for computer", Price = 1500, Weight = 10, SupplierId = Guids[0], Category = ItemCategory.Electronics },
                new Item() { Id = Guids[1], Name = "Big Computer", Quantity = 0, Description = "Description for big computer", Price = 2500.50m, Weight = 15, SupplierId = Guids[0], Category = ItemCategory.Electronics },
                new Item() { Id = Guids[2], Name = "Computer master book", Quantity = 10, Description = "Description for computer master book", Price = 17, Weight = 0.8, SupplierId = Guids[1], Category = ItemCategory.Books },
                new Item() { Id = Guids[3], Name = "Screwdriver", Quantity = 10, Description = "Description for screwdriver", Price = 5, Weight = 0.15, SupplierId = Guids[2], Category = ItemCategory.HomeGoods }
            );
            context.Customers.AddRange(
                new Customer { Id = Guids[0], Name = "John", Surname = "Doe", Address = "123 Main St", Email = "john.doe@example.com" },
                new Customer { Id = Guids[1], Name = "Jane", Surname = "Smith", Address = "456 Elm St", Email = "jane.smith@example.com" }
            );
            context.Orders.AddRange(
                new Order { 
                    Id = Guids[0],
                    CustomerId = Guids[0], 
                    OrderTime = new DateTime(2023, 10, 20, 14, 30, 0), 
                    Status = OrderStatus.Queued,
                    TotalPrice = 6501, 
                    OrderItems = new List<OrderItem> 
                    { 
                        new OrderItem { Id = Guids[0], OrderId = Guids[0], ItemId = Guids[0], Quantity = 1, Price = 1500 }, 
                        new OrderItem { Id = Guids[1], OrderId = Guids[0], ItemId = Guids[1], Quantity = 2, Price = 2500.50m }
                    } 
                },
                new Order 
                {
                    Id = Guids[1],
                    CustomerId = Guids[1],
                    OrderTime = new DateTime(2023, 11, 15, 10, 0, 0),
                    Status = OrderStatus.Delivered,
                    TotalPrice = 68, 
                    OrderItems = new List<OrderItem> 
                    {
                        new OrderItem { Id = Guids[2], OrderId = Guids[1], ItemId = Guids[2], Quantity = 4, Price = 17 } 
                    } 
                }
            );
            context.MissingItems.AddRange(
                new MissingItem() { ItemId = Guids[1], Missing = 2 }
            );
            context.SaveChanges();
        }
    }
}

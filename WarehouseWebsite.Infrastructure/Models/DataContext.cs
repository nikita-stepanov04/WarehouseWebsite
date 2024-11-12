using Microsoft.EntityFrameworkCore;
using WarehouseWebsite.Domain.Models.Customers;
using WarehouseWebsite.Domain.Models.Items;
using WarehouseWebsite.Domain.Models.Orders;

namespace WarehouseWebsite.Infrastructure.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> opts) 
            : base(opts) { }

        public DbSet<Order> Orders => Set<Order>();
        public DbSet<AwaitingOrder> AwaitingOrders => Set<AwaitingOrder>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        public DbSet<Item> Items => Set<Item>();
        public DbSet<MissingItem> MissingItems => Set<MissingItem>();
        public DbSet<Customer> Customers => Set<Customer>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AwaitingOrder>()
                .ToTable("AwaitingOrders")
                .HasBaseType((Type)null);
        }
    }
}

﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WarehouseWebsite.Domain.Models;
using WarehouseWebsite.Domain.Models.Customers;
using WarehouseWebsite.Domain.Models.Items;
using WarehouseWebsite.Domain.Models.Orders;

namespace WarehouseWebsite.Infrastructure.Models
{
    public class DataContext : IdentityDbContext<AppUser>
    {
        public DataContext(DbContextOptions<DataContext> opts) 
            : base(opts) { }

        public DbSet<Order> Orders => Set<Order>();
        public DbSet<AwaitingOrder> AwaitingOrders => Set<AwaitingOrder>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        public DbSet<Item> Items => Set<Item>();
        public DbSet<MissingItem> MissingItems => Set<MissingItem>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Item>()
                .Ignore(i => i.PhotoUrl);

            modelBuilder.Entity<AwaitingOrder>()
                .ToTable("AwaitingOrders")
                .HasBaseType((Type)null)
                .HasMany(o => o.OrderItems)
                .WithOne(oi => oi.AwaitingOrder)
                .HasForeignKey(oi => oi.AwaitingOrderId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.AwaitingOrder)
                .WithMany(ao => ao.OrderItems)
                .HasForeignKey(oi => oi.AwaitingOrderId)
                .OnDelete(DeleteBehavior.SetNull);               
        }
    }
}

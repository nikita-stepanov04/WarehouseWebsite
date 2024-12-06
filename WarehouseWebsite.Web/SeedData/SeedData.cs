using Azure.Core;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using WarehouseWebsite.Application.Interfaces;
using WarehouseWebsite.Domain.Models.Customers;
using WarehouseWebsite.Domain.Models.Items;
using WarehouseWebsite.Infrastructure.Models;
using WarehouseWebsite.Web.Identity;

namespace WarehouseWebsite.Web.SeedData
{
    public static class SeedData
    {
        public static IServiceProvider SeedWithTestData(this IServiceProvider provider)
        {
            SeedWithTestDataAsync(provider).Wait();
            return provider;
        }

        private static async Task SeedWithTestDataAsync(IServiceProvider provider)
        {
            using var scope = provider.CreateScope();
            IItemService itemService = scope.ServiceProvider.GetRequiredService<IItemService>();
            DataContext context = scope.ServiceProvider.GetRequiredService<DataContext>();

            if (!context.Items.Any())
            {
                var items = new List<Item>
                {
                    new Item { Name = "Smartphone", Quantity = 10, Description = "Latest model with advanced features", Price = 899.99m, Weight = 0.3, Category = ItemCategory.Electronics, PhotoBlobId = Guid.NewGuid() },
                    new Item { Name = "Vacuum Cleaner", Quantity = 5, Description = "High-efficiency vacuum cleaner for home use", Price = 150.00m, Weight = 2.5, Category = ItemCategory.HomeGoods, PhotoBlobId = Guid.NewGuid() },
                    new Item { Name = "Jeans", Quantity = 20, Description = "Comfortable and stylish jeans", Price = 49.99m, Weight = 0.5, Category = ItemCategory.Clothing, PhotoBlobId = Guid.NewGuid() },
                    new Item { Name = "Cement Bags", Quantity = 50, Description = "Premium quality cement for construction", Price = 8.50m, Weight = 40.0, Category = ItemCategory.BuildingMaterials, PhotoBlobId = Guid.NewGuid() },
                    new Item { Name = "Cooking Pot", Quantity = 30, Description = "Durable and heat-resistant cooking pot", Price = 25.75m, Weight = 1.8, Category = ItemCategory.HomeGoods, PhotoBlobId = Guid.NewGuid() },
                    new Item { Name = "Laptop", Quantity = 15, Description = "High-performance laptop for work and play", Price = 1200.00m, Weight = 2.0, Category = ItemCategory.Electronics, PhotoBlobId = Guid.NewGuid() },
                    new Item { Name = "Bookshelf", Quantity = 7, Description = "Modern bookshelf with multiple compartments", Price = 99.99m, Weight = 10.0, Category = ItemCategory.HomeGoods, PhotoBlobId = Guid.NewGuid() },
                    new Item { Name = "Winter Coat", Quantity = 12, Description = "Warm and stylish winter coat", Price = 120.00m, Weight = 1.5, Category = ItemCategory.Clothing, PhotoBlobId = Guid.NewGuid() },
                    new Item { Name = "Bricks", Quantity = 100, Description = "High-quality bricks for construction", Price = 0.75m, Weight = 2.2, Category = ItemCategory.BuildingMaterials, PhotoBlobId = Guid.NewGuid() },
                    new Item { Name = "Novel", Quantity = 25, Description = "Bestselling novel by a famous author", Price = 15.99m, Weight = 0.4, Category = ItemCategory.Books, PhotoBlobId = Guid.NewGuid() }
                };

                string imgPath = Path.Combine(AppContext.BaseDirectory, "SeedData", "img");
                foreach((int i, var item) in items.Index())
                {
                    var memoryStream = new MemoryStream();
                    string file = Path.Combine(imgPath, $"{i + 1}.jpg");

                    memoryStream.Write(File.ReadAllBytes(file));
                    memoryStream.Flush();
                    memoryStream.Position = 0;

                    await itemService.AddItemAsync(item, memoryStream);
                }
            }
        }

        public static IServiceProvider SeedWithAdmins(this IServiceProvider provider)
        {
            SeedWithAdminsAsync(provider).Wait();
            return provider;
        }

        private static async Task SeedWithAdminsAsync(IServiceProvider provider)
        {
            using var scope = provider.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

            string GetEmail(int i) => $"admin{i}@mail.com";

            List<AppUser> admins = Enumerable.Range(1, 3).Select(i =>
                new AppUser
                {
                    Email = GetEmail(i),
                    UserName = GetEmail(i),
                    Customer = new Customer()
                    {
                        Name = "Admin",
                        Surname = "Admin",
                        Address = "",
                        Email = GetEmail(i)
                    }
                }).ToList();

            foreach (var admin in admins)
            {
                var adminResult = await userManager.FindByNameAsync(admin.UserName!);
                if (adminResult == null)
                {
                    await userManager.CreateAsync(admin, "password");
                    await userManager.AddToRoleAsync(admin, nameof(Roles.Admin));
                    await userManager.AddToRoleAsync(admin, nameof(Roles.User));
                    await userManager.AddClaimAsync(admin, new Claim("CustomerId", admin.CustomerId.ToString()));
                }
            }
        }

        public static IServiceProvider CreateAzureBlobContainer(this IServiceProvider provider)
        {
            var azureSettings = provider.GetRequiredService<IOptions<AzureSettings>>().Value;

            BlobServiceClient blobServiceClient = new BlobServiceClient(azureSettings.ImageConnection);
            BlobContainerClient blobContainerClient = blobServiceClient
                .GetBlobContainerClient(azureSettings.ImageContainer);

            blobContainerClient.CreateIfNotExists(PublicAccessType.Blob);
            return provider;
        }
    }
}

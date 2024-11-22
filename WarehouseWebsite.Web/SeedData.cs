using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Options;
using WarehouseWebsite.Domain.Models.Items;
using WarehouseWebsite.Infrastructure.Models;

namespace WarehouseWebsite.Web
{
    public static class SeedData
    {
        public static IServiceProvider SeedWithTestData(this IServiceProvider provider)
        {
            using var scope = provider.CreateScope();
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
                context.Items.AddRange(items);
                context.SaveChanges();
            }
            return provider;
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

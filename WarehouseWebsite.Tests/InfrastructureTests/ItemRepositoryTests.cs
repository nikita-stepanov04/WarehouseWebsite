using WarehouseWebsite.Domain.Filtering;
using WarehouseWebsite.Domain.Models.Items;
using WarehouseWebsite.Infrastructure.Data;
using WarehouseWebsite.Infrastructure.Models;
using static WarehouseWebsite.Tests.UnitTestHelper;

namespace WarehouseWebsite.Tests.InfrastructureTests
{
    [TestFixture]
    public class ItemRepositoryTests
    {
        [Test]
        public async Task ItemRepositoryGetItemsByFilterAsyncReturnsValidResult()
        {
            using var context = new DataContext(GetUnitTestDbOptions());
            var itemRepository = new ItemRepository(context);

            var result = await itemRepository.GetItemsByFilterAsync(new FilterParameters<Item>(), default);
            var items = result.ToList();
            var expected = GetExpectedItems();

            Assert.That(items.Count, Is.EqualTo(4));
            Assert.That(items, Is.EqualTo(expected).Using(new ItemEqualityComparer()));
        }

        [Test]
        public void ItemRepositoryGetItemsByFilterAsyncCancelsWhenTokenIsCancelled()
        {
            using var context = new DataContext(GetUnitTestDbOptions());
            var itemRepository = new ItemRepository(context);

            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();

            Assert.ThrowsAsync<OperationCanceledException>(async () =>
                await itemRepository.GetItemsByFilterAsync(
                    new FilterParameters<Item>(), cancellationTokenSource.Token));
        }

        [Test]
        public async Task ItemRepositoryGetByIdShortenAsyncReturnsSingleValue()
        {
            using var context = new DataContext(GetUnitTestDbOptions());
            var itemRepository = new ItemRepository(context);

            var item = await itemRepository.GetByIdShortenAsync(Guids[1]);
            var expected = GetExpectedItems().First(i => i.Id == Guids[1]);

            Assert.That(item, Is.EqualTo(expected).Using(new ItemEqualityComparer()));
        }        

        private static List<Item> GetExpectedItems() => new List<Item>()
        {
            new Item() { Id = Guids[0], Name = "Computer", Quantity = 10, Price = 1500, Weight = 10, Category = ItemCategory.Electronics },
            new Item() { Id = Guids[1], Name = "Big Computer", Quantity = 0, Price = 2500.50m, Weight = 15, Category = ItemCategory.Electronics },
            new Item() { Id = Guids[2], Name = "Computer master book", Quantity = 10, Price = 17, Weight = 0.8, Category = ItemCategory.Books },
            new Item() { Id = Guids[3], Name = "Screwdriver", Quantity = 10, Price = 5, Weight = 0.15, Category = ItemCategory.HomeGoods }
        };
    }
}

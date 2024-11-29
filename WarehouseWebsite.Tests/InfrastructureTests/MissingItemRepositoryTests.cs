using WarehouseWebsite.Domain.Filtering;
using WarehouseWebsite.Domain.Models.Items;
using WarehouseWebsite.Infrastructure.Data;
using WarehouseWebsite.Infrastructure.Models;
using static WarehouseWebsite.Tests.UnitTestHelper;

namespace WarehouseWebsite.Tests.InfrastructureTests
{
    [TestFixture]
    public class MissingItemRepositoryTests
    {
        [Test]
        public async Task MissingItemRepositoryGetItemsByFilterAsyncReturnsValidResult()
        {
            using var context = new DataContext(GetUnitTestDbOptions());
            var missingItemRepository = new MissingItemRepository(context);

            var result = await missingItemRepository.GetItemsByFilterAsync(new FilterParameters<MissingItem>(), default);
            var items = result.ToList();

            Assert.That(items.Count, Is.EqualTo(1));
            Assert.That(items[0].Missing, Is.EqualTo(expected.Missing));
            Assert.That(items[0].Item, Is.EqualTo(expected.Item).Using(new ItemEqualityComparer()));
        }

        [Test]
        public void MissingItemRepositoryGetItemsByFilterAsyncCancelsWhenTokenIsCancelled()
        {
            using var context = new DataContext(GetUnitTestDbOptions());
            var itemRepository = new MissingItemRepository(context);

            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();

            Assert.ThrowsAsync<OperationCanceledException>(async () =>
                await itemRepository.GetItemsByFilterAsync(
                    new FilterParameters<MissingItem>(), cancellationTokenSource.Token));
        }

        [Test]
        public async Task MissingItemRepositoryAddToMissingAddsToMissingCorrectly()
        {
            using var context = new DataContext(GetUnitTestDbOptions());
            var itemRepository = new MissingItemRepository(context);

            var addToMissingList = new List<MissingItem> 
            { 
                new MissingItem { ItemId = Guids[1], Missing = 2 },
                new MissingItem { ItemId = Guids[2], Missing = 2 }
            };

            await itemRepository.AddToMissing(addToMissingList, default);
            await context.SaveChangesAsync();

            var missing = await itemRepository.GetItemsByFilterAsync(new FilterParameters<MissingItem>(), default);
            Assert.That(missing.First(m => m.ItemId == Guids[1]).Missing, Is.EqualTo(4));
            Assert.That(missing.First(m => m.ItemId == Guids[2]).Missing, Is.EqualTo(2));
        }

        [Test]
        public void MissingItemRepositoryAddToMissingCancelsWhenTokenIsCancelled()
        {
            using var context = new DataContext(GetUnitTestDbOptions());
            var itemRepository = new MissingItemRepository(context);

            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();

            Assert.ThrowsAsync<OperationCanceledException>(async () =>
                await itemRepository.AddToMissing([], cancellationTokenSource.Token));
        }
        
        [Test]
        public async Task MissingItemRepositoryGetByItemIdNotPopulatedReturnsItem()
        {
            using var context = new DataContext(GetUnitTestDbOptions());
            var itemRepository = new MissingItemRepository(context);

            var missing = await itemRepository.GetByItemIdNotPopulated(Guids[1]);
            Assert.That(missing.Missing, Is.EqualTo(2));
            Assert.That(missing.ItemId, Is.EqualTo(Guids[1]));
        }

        [Test]
        public async Task MissingItemRepositoryGetByItemIdNotPopulatedReturnsNullWhenNotFound()
        {
            using var context = new DataContext(GetUnitTestDbOptions());
            var itemRepository = new MissingItemRepository(context);

            var missing = await itemRepository.GetByItemIdNotPopulated(Guids[2]);
            Assert.That(missing, Is.Null);
        }

        MissingItem expected = new MissingItem()
        {
            ItemId = Guids[1],
            Missing = 2,
            Item = new Item() { Id = Guids[1], Name = "Big Computer", Quantity = 0, Price = 2500.50m, Weight = 15, Category = ItemCategory.Electronics }
        };
    }
}

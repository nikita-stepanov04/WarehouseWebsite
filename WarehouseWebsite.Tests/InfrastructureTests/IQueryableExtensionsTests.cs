using WarehouseWebsite.Domain.Filtering;
using WarehouseWebsite.Domain.Models.Items;
using WarehouseWebsite.Infrastructure.Filtering;
using static WarehouseWebsite.Tests.UnitTestHelper;

namespace WarehouseWebsite.Tests.InfrastructureTests
{
    [TestFixture]
    public class IQueryableExtensionsTests
    {
        [Test]
        public void IQueryableExtensionsWithFilterAppliesFilter()
        {
            var filter = new FilterParameters<Item>
            {
                Filter = i => i.Category == ItemCategory.Electronics
            };

            var items = GetItems().WithFilter(filter);

            Assert.That(items.Count(), Is.EqualTo(2));
            Assert.IsTrue(items.All(i => i.Category == ItemCategory.Electronics));
        }

        [Test]
        public void IQueryableExtensionsWithFilterSkipAndTakeCorrectNumberOfElements()
        {
            var filter = new FilterParameters<Item>
            {
                Skip = 1,
                Take = 2
            };

            var itemsList = GetItems().WithFilter(filter).ToList();

            Assert.That(itemsList.Count, Is.EqualTo(2));
            Assert.That(itemsList[0].Id, Is.EqualTo(Guids[1]));
            Assert.That(itemsList[1].Id, Is.EqualTo(Guids[2]));
        }

        [Test]
        public void IQueryableExtensionsWithFilterReturnAllElementsIfNoParameters()
        {
            var items = GetItems().WithFilter(new FilterParameters<Item>());

            Assert.That(items.Count(), Is.EqualTo(4));
        }

        private IQueryable<Item> GetItems() => 
            new List<Item>
            {
                new Item() { Id = Guids[0], Name = "Computer", Quantity = 10, Description = "Description for computer", Price = 1500, Weight = 10, Category = ItemCategory.Electronics },
                new Item() { Id = Guids[1], Name = "Big Computer", Quantity = 0, Description = "Description for big computer", Price = 2500.50m, Weight = 15, Category = ItemCategory.Electronics },
                new Item() { Id = Guids[2], Name = "Computer master book", Quantity = 10, Description = "Description for computer master book", Price = 17, Weight = 0.8, Category = ItemCategory.Books },
                new Item() { Id = Guids[3], Name = "Screwdriver", Quantity = 10, Description = "Description for screwdriver", Price = 5, Weight = 0.15, Category = ItemCategory.HomeGoods }
            }.AsQueryable();
    }
}

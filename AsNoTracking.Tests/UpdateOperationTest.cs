using AsNoTracking.Domain;
using Microsoft.EntityFrameworkCore;

namespace AsNoTracking.Tests
{
    [TestClass]
    public class UpdateOperationTest
    {
        [TestMethod]
        public void TestUpdateWithAsNoTracking()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<MyDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new MyDbContext(options))
            {
                context.Products.Add(new Product { Id = 1, Name = "Initial Name" });
                context.SaveChanges();
            }

            // Act
            using (var context = new MyDbContext(options))
            {
                // Retrieve a product without change tracking (AsNoTracking)
                var product = context.Products.AsNoTracking().FirstOrDefault(p => p.Id == 1);

                // Modify the product
                product.Name = "Updated Name";
                context.SaveChanges();

                // Assert that the product was not updated
                Assert.AreEqual("Initial Name", context.Products.FirstOrDefault(p => p.Id == 1).Name);
            }
        }
    }
}

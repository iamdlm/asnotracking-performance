using AsNoTracking.Domain;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace AsNoTracking.Tests
{
    [TestClass]
    public class PerformanceTest
    {
        [TestMethod]
        public void TestPerformanceWithAndWithoutAsNoTracking()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<MyDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var context = new MyDbContext(options))
            {
                // Seed the database with a large number of products
                for (int i = 0; i < 100000; i++)
                {
                    context.Products.Add(new Product { Name = $"Product{i}" });
                }
                context.SaveChanges();
            }

            // Act
            var stopwatch = new Stopwatch();

            // Without AsNoTracking
            stopwatch.Start();
            using (var context = new MyDbContext(options))
            {
                var products = context.Products.ToList();
            }
            stopwatch.Stop();
            var withoutAsNoTrackingTime = stopwatch.ElapsedMilliseconds;

            // With AsNoTracking
            stopwatch.Restart();
            using (var context = new MyDbContext(options))
            {
                var products = context.Products.AsNoTracking().ToList();
            }
            stopwatch.Stop();
            var withAsNoTrackingTime = stopwatch.ElapsedMilliseconds;

            // Assert
            Console.WriteLine($"Time without AsNoTracking: {withoutAsNoTrackingTime} ms");
            Console.WriteLine($"Time with AsNoTracking: {withAsNoTrackingTime} ms");

            // You can assert that using AsNoTracking is faster than without it
            Assert.IsTrue(withAsNoTrackingTime < withoutAsNoTrackingTime);
        }
    }

}
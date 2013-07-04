namespace NHibernateMappingSample.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NHibernate;
    using NHibernateMappingsSample;
    using NHibernateMappingsSample.BasicMapping.Model;
    using System.Collections.Generic;

    [TestClass]
    public class BasicMappingFixture
    {
        [TestMethod]
        public void SaveProducts_WithCategory_ShouldSaveAssociation()
        {
            // Create the category and the products
            var category = new Category { Name = "Category1" };
            var product1 = new Product { Name = "Product1", Discontinued = false };
            var product2 = new Product { Name = "Product2", Discontinued = false };
            var product3 = new Product { Name = "Product3", Discontinued = false };

            // Associate the products with the category
            product1.Category = category;
            product2.Category = category;
            product3.Category = category;

            // Save everything in the session and commit the transaction
            using (var session = NHibernateHelper.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                session.Save(category);
                session.Save(product1);
                session.Save(product2);
                session.Save(product3);
                transaction.Commit();
            }

            // Get the values from the DB and check the results
            using (var session = NHibernateHelper.OpenSession())
            {
                var categoryFromDb = session.Get<Category>(category.Id);
                Assert.IsNotNull(categoryFromDb.Products);
                Assert.IsTrue(categoryFromDb.Products.Count == 3);
            }
        }

        [TestMethod]
        public void SaveCategory_WithProducts_ShouldSaveAssociation()
        {
            // Create the category and the products
            var category = new Category { Name = "Category1" };
            var product1 = new Product { Name = "Product1", Discontinued = false };
            var product2 = new Product { Name = "Product2", Discontinued = false };
            var product3 = new Product { Name = "Product3", Discontinued = false };

            // Associate the products with the category
            category.Products = new List<Product> { product1, product2, product3 };

            // Save everything in the session and commit the transaction
            using (var session = NHibernateHelper.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                session.Save(category);
                session.Save(product1);
                session.Save(product2);
                session.Save(product3);
                transaction.Commit();
            }

            // Get the values from the DB and check the results
            using (var session = NHibernateHelper.OpenSession())
            {
                var categoryFromDb = session.Get<Category>(category.Id);
                Assert.IsNotNull(categoryFromDb.Products);
                Assert.IsTrue(categoryFromDb.Products.Count == 3);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(TransientObjectException))]
        public void SaveCategoryOnly_WithProducts_ShouldThrowException()
        {
            // Create the category and the products
            var category = new Category { Name = "Category1" };
            var product1 = new Product { Name = "Product1", Discontinued = false };
            var product2 = new Product { Name = "Product2", Discontinued = false };
            var product3 = new Product { Name = "Product3", Discontinued = false };

            // Associate the products with the category
            category.Products = new List<Product> { product1, product2, product3 };

            // Save everything in the session and commit the transaction
            using (var session = NHibernateHelper.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                session.Save(category);

                // NHibernate should throw a TransientObjectException here
                // because the 'cascade' attribute is set to 'none'
                transaction.Commit();
            }
        }

        [TestInitialize]
        public void SetUp()
        {
            NHibernateHelper.CreateDb();
        }

        [TestCleanup]
        public void TearDown()
        {
            NHibernateHelper.DropDb();
        }
    }
}

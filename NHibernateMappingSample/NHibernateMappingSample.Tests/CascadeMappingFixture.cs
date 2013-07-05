namespace NHibernateMappingSample.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NHibernateMappingSample;
    using NHibernateMappingSample.CascadeMapping.Model;
    using System.Collections.Generic;
    using System.Reflection;

    [TestClass]
    public class CascadeMappingFixture
    {
        private NHibernateSessionFactory sessionFactory;

        [TestMethod]
        public void SaveOnlyCategory_WithProducts_ShouldSaveAssociations_And_Entities()
        {
            // Create the category and the products
            var category = new Category { Name = "Category1" };
            var product1 = new Product { Name = "Product1", Discontinued = false };
            var product2 = new Product { Name = "Product2", Discontinued = false };
            var product3 = new Product { Name = "Product3", Discontinued = false };

            // Associate the products with the category
            category.Products = new List<Product> { product1, product2, product3 };

            // Save everything in the session and commit the transaction
            using (var session = this.sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                session.Save(category);
                transaction.Commit();
            }
        }

        [TestInitialize]
        public void SetUp()
        {
            this.sessionFactory = new NHibernateSessionFactory();
            this.sessionFactory.AddMappingAssembly(Assembly.GetAssembly(typeof(Category)));
            this.sessionFactory.CreateDb();
        }

        [TestCleanup]
        public void TearDown()
        {
            this.sessionFactory.DropDb();
            this.sessionFactory.CloseSessionFactory();
        }
    }
}

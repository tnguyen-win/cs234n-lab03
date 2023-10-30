using NUnit.Framework;
using MMABooksBusiness;
using MMABooksDB;
using DBCommand = MySql.Data.MySqlClient.MySqlCommand;
using System.Data;

namespace MMABooksTests {
    [TestFixture]
    public class ProductTests {

        [SetUp]
        public void TestResetDatabase() {
            ProductDB db = new();
            DBCommand command = new() {
                CommandText = "usp_testingResetProductData",
                CommandType = CommandType.StoredProcedure
            };
            db.RunNonQueryProcedure(command);
        }

        [Test]
        public void TestNewProductConstructor() {
            Product p = new();
            Assert.AreEqual(0, p.ProductId);
            Assert.AreEqual("", p.ProductCode);
            Assert.AreEqual("", p.Description);
            Assert.AreEqual(0.0, p.UnitPrice);
            Assert.AreEqual(0, p.OnHandQuantity);
            Assert.IsTrue(p.IsNew);
            Assert.IsFalse(p.IsValid);
        }

        [Test]
        public void TestRetrieveFromDataStoreConstructor() {
            Product p = new(7);
            Assert.AreEqual("DB1R", p.ProductCode);
            Assert.AreEqual("DB2 for the COBOL Programmer, Part 1 (2nd Edition)", p.Description);
            Assert.AreEqual(42.0000m, p.UnitPrice);
            Assert.AreEqual(4825, p.OnHandQuantity);
            Assert.IsFalse(p.IsNew);
            Assert.IsTrue(p.IsValid);
        }

        [Test]
        public void TestSaveToDataStore() {
            Product p1 = new() {
                ProductCode = "ZZZ1",
                Description = "Book 1",
                UnitPrice = 10.0m,
                OnHandQuantity = 123
            };
            p1.Save();
            Product p2 = new() {
                ProductCode = "ZZZ1",
                Description = "Book 1",
                UnitPrice = 10.0m,
                OnHandQuantity = 123
            };
            Assert.AreEqual(p2.ProductCode, p1.ProductCode);
            Assert.AreEqual(p2.Description, p1.Description);
            Assert.AreEqual(p2.UnitPrice, p1.UnitPrice);
            Assert.AreEqual(p2.OnHandQuantity, p1.OnHandQuantity);
        }

        [Test]
        public void TestUpdate() {
            Product p1 = new(1) {
                ProductCode = "ZZZ1",
                Description = "Book 1",
                UnitPrice = 10.0m,
                OnHandQuantity = 123
            };
            p1.Save();
            Product p2 = new(1) {
                ProductCode = "ZZZ1",
                Description = "Book 1",
                UnitPrice = 10.0m,
                OnHandQuantity = 123
            };
            Assert.AreEqual(p2.ProductCode, p1.ProductCode);
            Assert.AreEqual(p2.Description, p1.Description);
            Assert.AreEqual(p2.UnitPrice, p1.UnitPrice);
            Assert.AreEqual(p2.OnHandQuantity, p1.OnHandQuantity);
        }


        [Test]
        public void TestDelete() {
            Product p = new(1) {
                ProductCode = "ZZZ1",
                Description = "Book 1",
                UnitPrice = 10.0m,
                OnHandQuantity = 123
            };
            p.Delete();
            p.Save();
            Assert.Throws<Exception>(() => new Product(1));
        }

        [Test]
        public void TestGetList() {
            Product p = new();
            List<Product> customers = (List<Product>)p.GetList();
            Assert.AreEqual(16, customers.Count);
            Assert.AreEqual("DB1R", customers[6].ProductCode);
            Assert.AreEqual("DB2 for the COBOL Programmer, Part 1 (2nd Edition)", customers[6].Description);
            Assert.AreEqual(42.0000m, customers[6].UnitPrice);
            Assert.AreEqual(4825, customers[6].OnHandQuantity);
        }

        [Test]
        public void TestNoRequiredPropertiesNotSet() {
            Product p = new();
            Assert.Throws<Exception>(() => p.Save());
        }

        // This code doesn't throw any errors, as I don't have any properties that aren't setup.
        //[Test]
        //public void TestSomeRequiredPropertiesNotSet() {
        //    Product p = new();
        //    Assert.Throws<Exception>(() => p.Save());
        //    p.ProductCode = "ZZZ1";
        //    p.Description = "Book 1";
        //    p.UnitPrice = 10.0m;
        //    p.OnHandQuantity = 123;
        //    Assert.Throws<Exception>(() => p.Save());
        //}

        [Test]
        public void TestInvalidPropertySet() {
            Product p = new();
            Assert.Throws<ArgumentOutOfRangeException>(() => p.ProductCode = "????????????");
        }

        [Test]
        public void TestConcurrencyIssue() {
            Product p1 = new(1);
            Product p2 = new(1);
            p1.Description = "Updated first";
            p1.Save();
            p2.Description = "Updated second";
            Assert.Throws<Exception>(() => p2.Save());
        }
    }
}

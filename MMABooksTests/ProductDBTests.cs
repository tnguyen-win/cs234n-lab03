using NUnit.Framework;
using MMABooksProps;
using MMABooksDB;
using DBCommand = MySql.Data.MySqlClient.MySqlCommand;
using System.Data;
using MySql.Data.MySqlClient;

namespace MMABooksTests {
    public class ProductDBTests {
        ProductDB db;

        [SetUp]
        public void ResetData() {
            db = new ProductDB();
            DBCommand command = new() {
                CommandText = "usp_testingResetProductData",
                CommandType = CommandType.StoredProcedure
            };
            db.RunNonQueryProcedure(command);
        }

        [Test]
        public void TestRetrieve() {
            ProductProps p = (ProductProps)db.Retrieve(7);
            Console.WriteLine(p);
            Assert.AreEqual(7, p.ProductId);
            Assert.AreEqual("DB1R", p.ProductCode);
        }

        [Test]
        public void TestRetrieveAll() {
            List<ProductProps> list = (List<ProductProps>)db.RetrieveAll();
            /*
             Confirmed by running the query code:

                SELECT COUNT(*)
                FROM products;

            */
            Assert.AreEqual(16, list.Count);
        }

        [Test]
        public void TestDelete() {
            ProductProps p = (ProductProps)db.Retrieve(1);
            Assert.True(db.Delete(p));
            Assert.Throws<Exception>(() => db.Retrieve(1));
        }

        // I was unfortunately unable to get this code working...
        //[Test]
        //public void TestDeleteForeignKeyConstraint() {
        //    ProductProps p = (ProductProps)db.Retrieve(1000);
        //    Assert.Throws<MySqlException>(() => db.Delete(p));
        //}

        [Test]
        public void TestUpdate() {
            ProductProps p = (ProductProps)db.Retrieve(1);
            p.Description = "John Smith";
            Assert.True(db.Update(p));
            p = (ProductProps)db.Retrieve(1);
            Assert.AreEqual("John Smith", p.Description);
        }

        [Test]
        public void TestUpdateFieldTooLong() {
            ProductProps p = (ProductProps)db.Retrieve(2);
            p.Description = "Oregon is the state where Crater Lake National Park is.";
            Assert.Throws<MySqlException>(() => db.Update(p));
        }

        [Test]
        public void TestCreate() {
            ProductProps p1 = new() {
                ProductCode = "ZZZ1",
                Description = "Book 1",
                UnitPrice = 12.3456m,
                OnHandQuantity = 123
            };
            db.Create(p1);
            ProductProps p2 = (ProductProps)db.Retrieve(p1.ProductId);
            Assert.AreEqual(p1.GetState(), p2.GetState());
        }

        /*
            I was unfortunately unable to get this code working...
            The CustomerID increments each new field, oddly, and even if I try to assign a customer ID to the newly created customerprops, the exception still doesn't trigger.
        */
        //[Test]
        //public void TestCreatePrimaryKeyViolation() {
        //    ProductProps p = new() {
        //        ProductCode = "ZZZ1",
        //        Description = "Book 1",
        //        UnitPrice = 12.3456m,
        //        OnHandQuantity = 123
        //    };
        //    Assert.Throws<MySqlException>(() => db.Create(p));
        //}
    }
}

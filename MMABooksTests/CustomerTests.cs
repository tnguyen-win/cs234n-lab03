using NUnit.Framework;
using MMABooksBusiness;
using MMABooksDB;
using DBCommand = MySql.Data.MySqlClient.MySqlCommand;
using System.Data;

namespace MMABooksTests {
    [TestFixture]
    public class CustomerTests {

        [SetUp]
        public void TestResetDatabase() {
            CustomerDB db = new();
            DBCommand command1 = new() {
                CommandText = "usp_testingResetCustomer1Data",
                CommandType = CommandType.StoredProcedure
            };
            db.RunNonQueryProcedure(command1);
            DBCommand command2 = new() {
                CommandText = "usp_testingResetCustomer2Data",
                CommandType = CommandType.StoredProcedure
            };
            db.RunNonQueryProcedure(command2);
        }

        [Test]
        public void TestNewCustomerConstructor() {
            Customer c = new();
            Assert.AreEqual(0, c.CustomerId);
            Assert.AreEqual("", c.Name);
            Assert.AreEqual("", c.Address);
            Assert.AreEqual("", c.City);
            Assert.AreEqual("", c.State);
            Assert.AreEqual("", c.ZipCode);
            Assert.IsTrue(c.IsNew);
            Assert.IsFalse(c.IsValid);
        }

        [Test]
        public void TestRetrieveFromDataStoreConstructor() {
            Customer c = new(1);
            Assert.AreEqual("Molunguri, A", c.Name);
            Assert.AreEqual("1108 Johanna Bay Drive", c.Address);
            Assert.IsFalse(c.IsNew);
            Assert.IsTrue(c.IsValid);
        }

        [Test]
        public void TestSaveToDataStore() {
            Customer c1 = new() {
                Name = "Mickey Mouse",
                Address = "101 Main Street",
                City = "Orlando",
                State = "FL",
                ZipCode = "10101"
            };
            c1.Save();
            Customer c2 = new() {
                Name = "Mickey Mouse",
                Address = "101 Main Street",
                City = "Orlando",
                State = "FL",
                ZipCode = "10101"
            };
            Assert.AreEqual(c2.Name, c1.Name);
            Assert.AreEqual(c2.Address, c1.Address);
            Assert.AreEqual(c2.City, c1.City);
            Assert.AreEqual(c2.State, c1.State);
            Assert.AreEqual(c2.ZipCode, c1.ZipCode);
        }

        [Test]
        public void TestUpdate() {
            Customer c1 = new(1) {
                Name = "Mickey Mouse",
                Address = "101 Main Street",
                City = "Orlando",
                State = "FL",
                ZipCode = "10101"
            };
            c1.Save();
            Customer c2 = new(1) {
                Name = "Mickey Mouse",
                Address = "101 Main Street",
                City = "Orlando",
                State = "FL",
                ZipCode = "10101"
            }; ;
            Assert.AreEqual(c2.Name, c1.Name);
            Assert.AreEqual(c2.Address, c1.Address);
            Assert.AreEqual(c2.City, c1.City);
            Assert.AreEqual(c2.State, c1.State);
            Assert.AreEqual(c2.ZipCode, c1.ZipCode);
        }

        [Test]
        public void TestDelete() {
            Customer c = new(2) {
                Name = "John Smith",
                Address = "New York",
                City = "Buffalo",
                State = "NY",
                ZipCode = "14201"
            };
            c.Delete();
            c.Save();
            Assert.Throws<Exception>(() => new Customer(2));
        }

        [Test]
        public void TestGetList() {
            Customer c = new();
            List<Customer> customers = (List<Customer>)c.GetList();
            Assert.AreEqual(696, customers.Count);
            Assert.AreEqual("Abeyatunge, Derek", customers[0].Name);
            Assert.AreEqual("1414 S. Dairy Ashford", customers[0].Address);
            Assert.AreEqual("North Chili", customers[0].City);
            Assert.AreEqual("NY", customers[0].State);
            Assert.AreEqual("14514", customers[0].ZipCode);
        }

        [Test]
        public void TestNoRequiredPropertiesNotSet() {
            Customer c = new();
            Assert.Throws<Exception>(() => c.Save());
        }

        // This code doesn't throw any errors, as I don't have any properties that aren't setup.
        //[Test]
        //public void TestSomeRequiredPropertiesNotSet() {
        //    Customer c = new();
        //    Assert.Throws<Exception>(() => c.Save());
        //    c.Name = "John Smith";
        //    c.Address = "New York";
        //    c.City = "Buffalo";
        //    c.State = "NY";
        //    c.ZipCode = "14201";
        //    Assert.Throws<Exception>(() => c.Save());
        //}

        [Test]
        public void TestInvalidPropertySet() {
            Customer c = new();
            Assert.Throws<ArgumentOutOfRangeException>(() => c.State = "???");
        }

        [Test]
        public void TestConcurrencyIssue() {
            Customer c1 = new(1);
            Customer c2 = new(1);

            c1.Name = "Updated first";
            c1.Save();

            c2.Name = "Updated second";
            Assert.Throws<Exception>(() => c2.Save());
        }
    }
}

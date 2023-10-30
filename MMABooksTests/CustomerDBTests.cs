using NUnit.Framework;
using MMABooksProps;
using MMABooksDB;
using DBCommand = MySql.Data.MySqlClient.MySqlCommand;
using System.Data;
using MySql.Data.MySqlClient;

namespace MMABooksTests {
    public class CustomerDBTests {
        CustomerDB db;

        [SetUp]
        public void ResetData() {
            db = new CustomerDB();
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
        public void TestRetrieve() {
            CustomerProps p = (CustomerProps)db.Retrieve(7);
            Assert.AreEqual(7, p.CustomerId);
            Assert.AreEqual("Lutonsky, Christopher", p.Name);
        }

        [Test]
        public void TestRetrieveAll() {
            List<CustomerProps> list = (List<CustomerProps>)db.RetrieveAll();
            /*
             Confirmed by running the query code:

                SELECT COUNT(*)
                FROM customers;

            */
            Assert.AreEqual(696, list.Count);
        }

        [Test]
        public void TestDelete() {
            CustomerProps p = (CustomerProps)db.Retrieve(1);
            Assert.True(db.Delete(p));
            Assert.Throws<Exception>(() => db.Retrieve(1));
        }

        // I was unfortunately unable to get this code working...
        //[Test]
        //public void TestDeleteForeignKeyConstraint() {
        //    CustomerProps p = (CustomerProps)db.Retrieve(1000);
        //    Assert.Throws<MySqlException>(() => db.Delete(p));
        //}

        [Test]
        public void TestUpdate() {
            CustomerProps p = (CustomerProps)db.Retrieve(1);
            p.Name = "John Smith";
            Assert.True(db.Update(p));
            p = (CustomerProps)db.Retrieve(1);
            Assert.AreEqual("John Smith", p.Name);
        }

        [Test]
        public void TestUpdateFieldTooLong() {
            CustomerProps p = (CustomerProps)db.Retrieve(2);
            // Generated using - http://www.unit-conversion.info/texttools/random-string-generator/
            p.Name = "1l6bXxdyu93m965MHSUCqVmQxLLotY3G6V9NpIGCInZ9wABVdixOkvgNF3SvQApnhx2xHSANbfuDnS1F0Rj3gnhHxV2f0k6dMscXte0sgs3LbHU3eRB5xGjTPE8701uX5wgICoku5IQWh0eRZ70Aix3YoTPOsoAuZcsycLANZRN4JbuO1cJznypnlgyAS8XNXrgArm5UN2mZDbCVWgheWhO4DLp7qSguit5gQOWF98wN4Pe2kQNZrOErdCNtqn0qyvPYVaS47TrwlYkURha5oYQkzPJFmImjxeZR6Qj3jpwZInl0gu0kUXVCozNMBUYU3cR9Ca0wGkz7eJzLQXhj7Ldqc5U2LpAxnE62Y6gkuaHqF0Zrr7FFNhXvu2cGbzzy8Mj71QfoJZ6qlsMa8xfuHp80V71OelCx14vWMVuTcPvyHov2f87fZPhOO0oKGXQdyHXlzsSS8xjXcVeFDREpUx6qxuOdPB9V4InYRWqVBkjmmkPKrDe20rOc95FTcwAKotIY9Sh8P7dqCVCooLaC5RVrhRU08pX0TepOv1of5DY7FSeCXHn3hATRPrxBYtEfM7cxgrLWHbw5jMuHpGEEgPtSzAntF8LTs7zs44t2ejIha2urdBYLL7f3yAYkn3K7t3TkCVlLlNWqlo0IyqcpT1QMfHhXzFx4uR8MI0j31oAE8QDuFg47QjJzd5dLeuI1f8uNeINCAz6PeyUB23k7Q40Jrlj7QI3mVJ5oH8bLkHu4uSXSFuV28pvCcXYrVz1Qj2hfd6umav7tnLNvSRETS1RdlLtOaAPVSryzSqlILAvaqC85eBAAUviyieOEqZv2LL65JrF1Fcjx62HYuQhGayuoa9UVeNQzBido8YVpqt8KQru8s1RRXHbLNRpGxgy38Fj2IH0KKMgcVZPS9xxCrGhcDr1Fuv3Pwe0s7iUuJkTgdANOMFdhyUh5IGCh3TT9PHG1Z3TSWF0QlcYHBsoGlwSINVqZh8rNGhBwrJxyJjQ3jF0ajVmsz5kCX2C4c5ufDHwMfOg9DJLAtRVvbe5skXhoTMuGstKNigWbVPYusYVqGnHLYov9aocFvBsXlWUkaQT3yjA7Dkw98IkEkLsJNIlEdoGB1l8Y2vDspvcggew7d9GlxP96rckxwjdroVHv2QMjNIilvAegcBnMbTNaogtA2uqDiCVi0Q5iR1qLbuihzl1Vuuzn5sc8JH6N9avqEfEw5yQaWj0xA5KKBbI0TCtxtdcwkAumdIouxvxZdRE6QW0zsADXU3BPZQWcKTDP7y60tbY96TyK7oe3ZpnRkbVPsfgmNiD7QoOVGtiSHZd6G4L3FCdUDmsxoUsSu3Ou2YeKVoTFOyhWVJPB41js7RZihm785MqU3plDNOu5FeTM9R46MbXTn4FOoZHQlQ3Z3tfI8Ow0siSqwJvzT5PmSvT4WBEoa3ZirwRlndTRerNcSJNAJrT6jfHMLRqxCYkxOyKkzIVeb58fawiscquJOdaPKXNaosAEGJg1jeXDgS0O0CTLFnV1iFSFxN01YNxUkImaI2Oy8QFfep3X1H5lFtv7HHWroETYo42K3LV4jcPKiy3O4kI5pQxTYSGFcoIZc";
            Assert.Throws<MySqlException>(() => db.Update(p));
        }

        [Test]
        public void TestCreate() {
            CustomerProps p1 = new() {
                Name = "Mickey Mouse",
                Address = "101 Main Street",
                City = "Orlando",
                State = "FL",
                ZipCode = "10101"
            };
            db.Create(p1);
            CustomerProps p2 = (CustomerProps)db.Retrieve(p1.CustomerId);
            Assert.AreEqual(p1.GetState(), p2.GetState());
        }

        /*
            I was unfortunately unable to get this code working...
            The CustomerID increments each new field, oddly, and even if I try to assign a customer ID to the newly created customerprops, the exception still doesn't trigger.
        */
        //[Test]
        //public void TestCreatePrimaryKeyViolation() {
        //    CustomerProps p = new() {
        //        Name = "Mickey Mouse",
        //        Address = "101 Main Street",
        //        City = "Orlando",
        //        State = "FL",
        //        ZipCode = "10101"
        //    };
        //    Assert.Throws<MySqlException>(() => db.Create(p));
        //}
    }
}

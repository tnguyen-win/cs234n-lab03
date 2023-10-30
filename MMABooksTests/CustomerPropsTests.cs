using NUnit.Framework;
using MMABooksProps;

namespace MMABooksTests {
    [TestFixture]
    public class CustomerPropsTests {
        CustomerProps props;

        [SetUp]
        public void Setup() {
            props = new CustomerProps {
                CustomerId = 1,
                Name = "Mickey Mouse",
                Address = "101 Main Street",
                City = "Orlando",
                State = "FL",
                ZipCode = "10101"
            };
        }

        [Test]
        public void TestGetState() {
            string jsonString = props.GetState();
            Console.WriteLine(jsonString);
            Assert.IsTrue(jsonString.Contains(props.Name));
            Assert.IsTrue(jsonString.Contains(props.Address));
            Assert.IsTrue(jsonString.Contains(props.City));
            Assert.IsTrue(jsonString.Contains(props.State));
            Assert.IsTrue(jsonString.Contains(props.ZipCode));
        }

        [Test]
        public void TestSetState() {
            string jsonString = props.GetState();
            CustomerProps newProps = new();
            newProps.SetState(jsonString);
            Assert.AreEqual(props.CustomerId, newProps.CustomerId);
            Assert.AreEqual(props.Name, newProps.Name);
            Assert.AreEqual(props.Address, newProps.Address);
            Assert.AreEqual(props.City, newProps.City);
            Assert.AreEqual(props.State, newProps.State);
            Assert.AreEqual(props.ZipCode, newProps.ZipCode);
            Assert.AreEqual(props.ConcurrencyID, newProps.ConcurrencyID);
        }

        [Test]
        public void TestClone() {
            CustomerProps newProps = (CustomerProps)props.Clone();
            Assert.AreEqual(props.CustomerId, newProps.CustomerId);
            Assert.AreEqual(props.Name, newProps.Name);
            Assert.AreEqual(props.Address, newProps.Address);
            Assert.AreEqual(props.City, newProps.City);
            Assert.AreEqual(props.State, newProps.State);
            Assert.AreEqual(props.ZipCode, newProps.ZipCode);
            Assert.AreEqual(props.ConcurrencyID, newProps.ConcurrencyID);
        }
    }
}

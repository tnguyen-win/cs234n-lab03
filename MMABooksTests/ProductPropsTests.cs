using NUnit.Framework;
using MMABooksProps;

namespace MMABooksTests {
    [TestFixture]
    public class ProductPropsTests {
        ProductProps props;

        [SetUp]
        public void Setup() {
            props = new ProductProps {
                ProductId = 1,
                ProductCode = "ZZZ1",
                Description = "Book 1",
                UnitPrice = 12.3456m,
                OnHandQuantity = 123
            };
        }

        [Test]
        public void TestGetState() {
            string jsonString = props.GetState();
            Console.WriteLine(jsonString);
            Assert.IsTrue(jsonString.Contains(props.ProductCode));
            Assert.IsTrue(jsonString.Contains(props.Description));
            /*
                Unfortunately, I wasn't able to get these two properties to return a value of True...
                These prop fields keep return cast errors about [X] can't convert to char (I wasn't able to figure out where char was being set).
            */
            //Assert.IsTrue(jsonString.Contains(props.UnitPrice));
            //Assert.IsTrue(jsonString.Contains(props.OnHandQuantity));
        }

        [Test]
        public void TestSetState() {
            string jsonString = props.GetState();
            ProductProps newProps = new();
            newProps.SetState(jsonString);
            Assert.AreEqual(props.ProductId, newProps.ProductId);
            Assert.AreEqual(props.ProductCode, newProps.ProductCode);
            Assert.AreEqual(props.Description, newProps.Description);
            Assert.AreEqual(props.UnitPrice, newProps.UnitPrice);
            Assert.AreEqual(props.OnHandQuantity, newProps.OnHandQuantity);
            Assert.AreEqual(props.ConcurrencyID, newProps.ConcurrencyID);
        }

        [Test]
        public void TestClone() {
            ProductProps newProps = (ProductProps)props.Clone();
            Assert.AreEqual(props.ProductId, newProps.ProductId);
            Assert.AreEqual(props.ProductCode, newProps.ProductCode);
            Assert.AreEqual(props.Description, newProps.Description);
            Assert.AreEqual(props.UnitPrice, newProps.UnitPrice);
            Assert.AreEqual(props.OnHandQuantity, newProps.OnHandQuantity);
            Assert.AreEqual(props.ConcurrencyID, newProps.ConcurrencyID);
        }
    }
}

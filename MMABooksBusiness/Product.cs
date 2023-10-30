using MMABooksTools;
using MMABooksProps;
using MMABooksDB;

namespace MMABooksBusiness {
    public class Product : BaseBusiness {
        public int ProductId {
            get => ((ProductProps)mProps).ProductId;
        }

        public string ProductCode {
            get => ((ProductProps)mProps).ProductCode;
            set {
                if (!(value == ((ProductProps)mProps).ProductCode)) {
                    if (value.Trim().Length >= 1 && value.Trim().Length <= 10) {
                        mRules.RuleBroken("ProductCode", false);
                        ((ProductProps)mProps).ProductCode = value;
                        mIsDirty = true;
                    } else throw new ArgumentOutOfRangeException("The code value must be (>= 1 AND <= 10).");
                }
            }
        }

        public string Description {
            get => ((ProductProps)mProps).Description;
            set {
                if (!(value == ((ProductProps)mProps).Description)) {
                    if (value.Trim().Length >= 1 && value.Trim().Length <= 50) {
                        mRules.RuleBroken("Description", false);
                        ((ProductProps)mProps).Description = value;
                        mIsDirty = true;
                    } else throw new ArgumentOutOfRangeException("The description value must be (>= 1 AND <= 50).");
                }
            }
        }

        public decimal UnitPrice {
            get => ((ProductProps)mProps).UnitPrice;
            set {
                if (!(value == ((ProductProps)mProps).UnitPrice)) {
                    if (value <= 10) {
                        mRules.RuleBroken("UnitPrice", false);
                        ((ProductProps)mProps).UnitPrice = Math.Round(value, 4);
                        mIsDirty = true;
                    } else throw new ArgumentOutOfRangeException("The unit price value must be (<= 10 AND rounded to 4 decimal points).");
                }
            }
        }

        public int OnHandQuantity {
            get => ((ProductProps)mProps).OnHandQuantity;
            set {
                if (!(value == ((ProductProps)mProps).OnHandQuantity)) {
                    mRules.RuleBroken("OnHandQuantity", false);
                    ((ProductProps)mProps).OnHandQuantity = value;
                    mIsDirty = true;
                }
            }
        }

        public override object GetList() {
            List<Product> products = new();
            _ = new List<ProductProps>();

            List<ProductProps> props = (List<ProductProps>)mdbReadable.RetrieveAll();
            foreach (ProductProps prop in props) {
                Product p = new(prop);

                products.Add(p);
            }

            return products;
        }

        protected override void SetDefaultProperties() { }

        protected override void SetRequiredRules() {
            mRules.RuleBroken("ProductCode", true);
            mRules.RuleBroken("Description", true);
            mRules.RuleBroken("UnitPrice", true);
            mRules.RuleBroken("OnHandQuantity", true);
        }

        protected override void SetUp() {
            mProps = new ProductProps();
            mOldProps = new ProductProps();

            mdbReadable = new ProductDB();
            mdbWriteable = new ProductDB();
        }

        #region constructors

        public Product() : base() { }

        public Product(int key) : base(key) { }

        private Product(ProductProps props) : base(props) { }

        #endregion
    }
}

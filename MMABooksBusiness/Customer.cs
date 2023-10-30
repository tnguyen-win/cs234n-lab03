using MMABooksTools;
using MMABooksProps;
using MMABooksDB;

namespace MMABooksBusiness {
    public class Customer : BaseBusiness {
        public int CustomerId {
            get => ((CustomerProps)mProps).CustomerId;
        }

        public string Name {
            get => ((CustomerProps)mProps).Name;
            set {
                if (!(value == ((CustomerProps)mProps).Name)) {
                    if (value.Trim().Length >= 1 && value.Trim().Length <= 100) {
                        mRules.RuleBroken("Name", false);
                        ((CustomerProps)mProps).Name = value;
                        mIsDirty = true;
                    } else throw new ArgumentOutOfRangeException("The ZIP code must be (>= 1 AND <= 100).");
                }
            }
        }

        public string Address {
            get => ((CustomerProps)mProps).Address;
            set {
                if (!(value == ((CustomerProps)mProps).Address)) {
                    if (value.Trim().Length >= 1 && value.Trim().Length <= 50) {
                        mRules.RuleBroken("Address", false);
                        ((CustomerProps)mProps).Address = value;
                        mIsDirty = true;
                    } else throw new ArgumentOutOfRangeException("The address value must be (>= 1 AND <= 50).");
                }
            }
        }

        public string City {
            get => ((CustomerProps)mProps).City;
            set {
                if (!(value == ((CustomerProps)mProps).City)) {
                    if (value.Trim().Length >= 1 && value.Trim().Length <= 20) {
                        mRules.RuleBroken("City", false);
                        ((CustomerProps)mProps).City = value;
                        mIsDirty = true;
                    } else throw new ArgumentOutOfRangeException("The city value must be (>= 1 AND <= 20).");
                }
            }
        }

        public string State {
            get => ((CustomerProps)mProps).State;
            set {
                if (!(value == ((CustomerProps)mProps).State)) {
                    if (value.Trim().Length == 2) {
                        mRules.RuleBroken("State", false);
                        ((CustomerProps)mProps).State = value;
                        mIsDirty = true;
                    } else throw new ArgumentOutOfRangeException("The state value must be exactly 2 characters long.");
                }
            }
        }

        public string ZipCode {
            get => ((CustomerProps)mProps).ZipCode;
            set {
                if (!(value == ((CustomerProps)mProps).ZipCode)) {
                    if (value.Trim().Length >= 1 && value.Trim().Length <= 15) {
                        mRules.RuleBroken("ZipCode", false);
                        ((CustomerProps)mProps).ZipCode = value;
                        mIsDirty = true;
                    } else throw new ArgumentOutOfRangeException("The ZIP code must be (>= 1 AND <= 15).");
                }
            }
        }

        public override object GetList() {
            List<Customer> customers = new();
            _ = new List<CustomerProps>();

            List<CustomerProps> props = (List<CustomerProps>)mdbReadable.RetrieveAll();
            foreach (CustomerProps prop in props) {
                Customer s = new(prop);

                customers.Add(s);
            }

            return customers;
        }

        protected override void SetDefaultProperties() { }

        protected override void SetRequiredRules() {
            mRules.RuleBroken("Name", true);
            mRules.RuleBroken("Address", true);
            mRules.RuleBroken("City", true);
            mRules.RuleBroken("State", true);
            mRules.RuleBroken("ZipCode", true);
        }

        protected override void SetUp() {
            mProps = new CustomerProps();
            mOldProps = new CustomerProps();

            mdbReadable = new CustomerDB();
            mdbWriteable = new CustomerDB();
        }

        #region constructors

        public Customer() : base() { }

        public Customer(int key) : base(key) { }

        private Customer(CustomerProps props) : base(props) { }

        #endregion
    }
}

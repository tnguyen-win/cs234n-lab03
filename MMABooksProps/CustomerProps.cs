using MMABooksTools;
using DBDataReader = MySql.Data.MySqlClient.MySqlDataReader;
using System.Text.Json;

namespace MMABooksProps {
    [Serializable()]
    public class CustomerProps : IBaseProps {
        #region Auto-implemented Properties
        public int CustomerId { get; set; } = 0;

        public string Name { get; set; } = "";

        public string Address { get; set; } = "";

        public string City { get; set; } = "";

        public string State { get; set; } = "";

        public string ZipCode { get; set; } = "";

        public int ConcurrencyID { get; set; } = 0;
        #endregion

        public object Clone() {
            CustomerProps p = new() {
                CustomerId = CustomerId,
                Name = Name,
                Address = Address,
                City = City,
                State = State,
                ZipCode = ZipCode,
                ConcurrencyID = ConcurrencyID
            };

            return p;
        }

        public string GetState() {
            string jsonString;

            jsonString = JsonSerializer.Serialize(this);

            return jsonString;
        }

        public void SetState(string jsonString) {
            CustomerProps p = JsonSerializer.Deserialize<CustomerProps>(jsonString);

            CustomerId = p.CustomerId;
            Name = p.Name;
            Address = p.Address;
            City = p.City;
            State = p.State;
            ZipCode = p.ZipCode;
            ConcurrencyID = p.ConcurrencyID;
        }

        public void SetState(DBDataReader dr) {
            CustomerId = (int)dr["CustomerID"];
            Name = (string)dr["Name"];
            Address = (string)dr["Address"];
            City = (string)dr["City"];
            State = (string)dr["State"];
            ZipCode = (string)dr["ZipCode"];
            ConcurrencyID = (int)dr["ConcurrencyID"];
        }
    }
}

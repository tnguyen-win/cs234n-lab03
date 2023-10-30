using MMABooksTools;
using DBDataReader = MySql.Data.MySqlClient.MySqlDataReader;
using System.Text.Json;

namespace MMABooksProps {
    [Serializable()]
    public class ProductProps : IBaseProps {
        #region Auto-implemented Properties
        public int ProductId { get; set; } = 0;

        public string ProductCode { get; set; } = "";

        public string Description { get; set; } = "";

        public decimal UnitPrice { get; set; } = 0.0m;

        public int OnHandQuantity { get; set; } = 0;

        public int ConcurrencyID { get; set; } = 0;
        #endregion

        public object Clone() {
            ProductProps p = new() {
                ProductId = ProductId,
                ProductCode = ProductCode,
                Description = Description,
                UnitPrice = UnitPrice,
                OnHandQuantity = OnHandQuantity,
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
            ProductProps p = JsonSerializer.Deserialize<ProductProps>(jsonString);

            ProductId = p.ProductId;
            ProductCode = p.ProductCode;
            Description = p.Description;
            UnitPrice = p.UnitPrice;
            OnHandQuantity = p.OnHandQuantity;
            ConcurrencyID = p.ConcurrencyID;
        }

        public void SetState(DBDataReader dr) {
            ProductId = (int)dr["ProductID"];
            ProductCode = (string)dr["ProductCode"];
            Description = (string)dr["Description"];
            UnitPrice = (decimal)dr["UnitPrice"];
            OnHandQuantity = (int)dr["OnHandQuantity"];
            ConcurrencyID = (int)dr["ConcurrencyID"];
        }
    }
}

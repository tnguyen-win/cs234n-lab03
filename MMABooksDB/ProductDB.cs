using MMABooksTools;
using MMABooksProps;
using System.Data;
using DBBase = MMABooksTools.BaseSQLDB;
using DBConnection = MySql.Data.MySqlClient.MySqlConnection;
using DBCommand = MySql.Data.MySqlClient.MySqlCommand;
using DBDataReader = MySql.Data.MySqlClient.MySqlDataReader;
using DBDbType = MySql.Data.MySqlClient.MySqlDbType;

namespace MMABooksDB {
    public class ProductDB : DBBase, IReadDB, IWriteDB {

        public ProductDB() : base() { }
        public ProductDB(DBConnection cn) : base(cn) { }

        public IBaseProps Create(IBaseProps p) {
            int rowsAffected;
            ProductProps props = (ProductProps)p;

            DBCommand command = new() {
                CommandText = "usp_ProductCreate",
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.Add("prodId", DBDbType.Int32);
            command.Parameters.Add("code_p", DBDbType.VarChar);
            command.Parameters.Add("desc_p", DBDbType.VarChar);
            command.Parameters.Add("price_p", DBDbType.Decimal);
            command.Parameters.Add("quantity_p", DBDbType.Int32);
            command.Parameters[0].Direction = ParameterDirection.Output;
            command.Parameters["code_p"].Value = props.ProductCode;
            command.Parameters["desc_p"].Value = props.Description;
            command.Parameters["price_p"].Value = props.UnitPrice;
            command.Parameters["quantity_p"].Value = props.OnHandQuantity;

            try {
                rowsAffected = RunNonQueryProcedure(command);

                if (rowsAffected == 1) {
                    props.ProductId = (int)command.Parameters[0].Value;
                    props.ConcurrencyID = 1;

                    return props;
                } else throw new Exception("Unable to insert record. " + props.GetState());
            } catch (Exception) {
                throw;
            } finally {
                if (mConnection.State == ConnectionState.Open) mConnection.Close();
            }
        }

        public bool Delete(IBaseProps p) {
            ProductProps props = (ProductProps)p;
            int rowsAffected;

            DBCommand command = new() {
                CommandText = "usp_ProductDelete",
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.Add("prodId", DBDbType.Int32);
            command.Parameters.Add("conCurrId", DBDbType.Int32);
            command.Parameters["prodId"].Value = props.ProductId;
            command.Parameters["conCurrId"].Value = props.ConcurrencyID;

            try {
                rowsAffected = RunNonQueryProcedure(command);

                if (rowsAffected == 1) return true;
                else {
                    string message = "Record cannot be deleted. It has been edited by another user.";
                    throw new Exception(message);
                }
            } catch (Exception) {
                throw;
            } finally {
                if (mConnection.State == ConnectionState.Open) mConnection.Close();
            }
        }

        public IBaseProps Retrieve(object key) {
            DBDataReader? data = null;
            ProductProps props = new();
            DBCommand command = new() {
                CommandText = "usp_ProductSelect",
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.Add("prodId", DBDbType.Int32);
            command.Parameters["prodId"].Value = (int)key;

            try {
                data = RunProcedure(command);

                if (!data.IsClosed) {
                    if (data.Read()) props.SetState(data);
                    else throw new Exception("Record does not exist in the database.");
                }

                return props;
            } catch (Exception) {
                throw;
            } finally {
                if (data != null) if (!data.IsClosed) data.Close();
            }
        }

        public object RetrieveAll() {
            List<ProductProps> list = new();
            DBDataReader? reader = null;
            ProductProps props;

            try {
                reader = RunProcedure("usp_ProductSelectAll");

                if (!reader.IsClosed) {
                    while (reader.Read()) {
                        props = new ProductProps();
                        props.SetState(reader);
                        list.Add(props);
                    }
                }

                return list;
            } catch (Exception) {
                throw;
            } finally {
                if (!reader.IsClosed) reader.Close();
            }
        }

        public bool Update(IBaseProps p) {
            int rowsAffected;
            ProductProps props = (ProductProps)p;

            DBCommand command = new() {
                CommandText = "usp_ProductUpdate",
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.Add("prodId", DBDbType.Int32);
            command.Parameters.Add("desc_p", DBDbType.VarChar);
            command.Parameters.Add("conCurrId", DBDbType.Int32);
            command.Parameters["prodId"].Value = props.ProductId;
            command.Parameters["desc_p"].Value = props.Description;
            command.Parameters["conCurrId"].Value = props.ConcurrencyID;

            try {
                rowsAffected = RunNonQueryProcedure(command);

                if (rowsAffected == 1) {
                    props.ConcurrencyID++;

                    return true;
                } else {
                    string message = "Record cannot be updated. It has been edited by another user.";
                    throw new Exception(message);
                }
            } catch (Exception) {
                throw;
            } finally {
                if (mConnection.State == ConnectionState.Open) mConnection.Close();
            }
        }
    }
}

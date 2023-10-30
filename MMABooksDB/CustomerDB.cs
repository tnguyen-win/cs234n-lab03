using MMABooksTools;
using MMABooksProps;
using System.Data;
using DBBase = MMABooksTools.BaseSQLDB;
using DBConnection = MySql.Data.MySqlClient.MySqlConnection;
using DBCommand = MySql.Data.MySqlClient.MySqlCommand;
using DBDataReader = MySql.Data.MySqlClient.MySqlDataReader;
using DBDbType = MySql.Data.MySqlClient.MySqlDbType;

namespace MMABooksDB {
    public class CustomerDB : DBBase, IReadDB, IWriteDB {

        public CustomerDB() : base() { }
        public CustomerDB(DBConnection cn) : base(cn) { }

        public IBaseProps Create(IBaseProps p) {
            int rowsAffected;
            CustomerProps props = (CustomerProps)p;

            DBCommand command = new() {
                CommandText = "usp_CustomerCreate",
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.Add("custId", DBDbType.Int32);
            command.Parameters.Add("name_p", DBDbType.VarChar);
            command.Parameters.Add("address_p", DBDbType.VarChar);
            command.Parameters.Add("city_p", DBDbType.VarChar);
            command.Parameters.Add("state_p", DBDbType.VarChar);
            command.Parameters.Add("zipcode_p", DBDbType.VarChar);
            command.Parameters[0].Direction = ParameterDirection.Output;
            command.Parameters["name_p"].Value = props.Name;
            command.Parameters["address_p"].Value = props.Address;
            command.Parameters["city_p"].Value = props.City;
            command.Parameters["state_p"].Value = props.State;
            command.Parameters["zipcode_p"].Value = props.ZipCode;

            try {
                rowsAffected = RunNonQueryProcedure(command);

                if (rowsAffected == 1) {
                    props.CustomerId = (int)command.Parameters[0].Value;
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
            CustomerProps props = (CustomerProps)p;
            int rowsAffected;

            DBCommand command = new() {
                CommandText = "usp_CustomerDelete",
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.Add("custId", DBDbType.Int32);
            command.Parameters.Add("conCurrId", DBDbType.Int32);
            command.Parameters["custId"].Value = props.CustomerId;
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
            CustomerProps props = new();
            DBCommand command = new() {
                CommandText = "usp_CustomerSelect",
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.Add("custId", DBDbType.Int32);
            command.Parameters["custId"].Value = (int)key;

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
            List<CustomerProps> list = new();
            DBDataReader? reader = null;
            CustomerProps props;

            try {
                reader = RunProcedure("usp_CustomerSelectAll");

                if (!reader.IsClosed) {
                    while (reader.Read()) {
                        props = new CustomerProps();
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
            CustomerProps props = (CustomerProps)p;

            DBCommand command = new() {
                CommandText = "usp_CustomerUpdate",
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.Add("custId", DBDbType.Int32);
            command.Parameters.Add("name_p", DBDbType.VarChar);
            command.Parameters.Add("conCurrId", DBDbType.Int32);
            command.Parameters["custId"].Value = props.CustomerId;
            command.Parameters["name_p"].Value = props.Name;
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

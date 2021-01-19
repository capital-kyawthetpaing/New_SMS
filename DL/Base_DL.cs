using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace DL
{
    public class Base_DL
    {
        #region Variables
        protected bool UseTransaction = false;
        public SqlConnection connection;
        protected SqlCommand command;
        protected SqlDataAdapter adapter;
        protected SqlTransaction transaction;
        public static Ini_Entity iniEntity = new Ini_Entity();
        public static Ini_Entity Ini_Entity_CDP { get; set; } = new Ini_Entity();
        #endregion

        public Base_DL() { }

        public SqlConnection GetConnection()
        {
            if (connection == null)
                connection = new SqlConnection(GetConnectionString());
            return connection;
        }

        public void StartTransaction()
        {
            connection = GetConnection();
            if (connection.State == ConnectionState.Closed)
                connection.Open();

            transaction = connection.BeginTransaction();
        }

        public void CommitTransaction()
        {
            connection = GetConnection();
            if (transaction != null)
                transaction.Commit();

            if (connection.State == ConnectionState.Open)
                connection.Close();

            UseTransaction = false;
        }

        public void RollBackTransaction()
        {
            connection = GetConnection();
            if (transaction != null)
                transaction.Rollback();

            if (connection.State == ConnectionState.Open)
                connection.Close();

            UseTransaction = false;
        }

        public string GetConnectionString()
        {

            return "Data Source=" + iniEntity.DatabaseServer +
                   ";Initial Catalog=" + iniEntity.DatabaseName +
                   ";Persist Security Info=True;User ID=" + iniEntity.DatabaseLoginID +
                   ";Password=" + iniEntity.DatabasePassword +
                   ";Connection Timeout=" + iniEntity.TimeoutValues;

        }

        protected void AddParam(SqlCommand cmd, string key, SqlDbType dbType, string value)
        {
            if (dbType == SqlDbType.Date)
                value = value.Replace("/", "-");
            if (string.IsNullOrWhiteSpace(value))
                cmd.Parameters.Add(key.Replace("_NoTrim", ""), dbType).Value = DBNull.Value;
            else
                cmd.Parameters.Add(key.Replace("_NoTrim", ""), dbType).Value = key.Contains("_NoTrim") ? value : value.Trim();
        }

        protected void AddParamForDataTable(SqlCommand cmd, string key, SqlDbType dbType, DataTable dt)
        {
            cmd.Parameters.Add(key, dbType).Value = dt;
        } 

        public DataTable SelectData(Dictionary<string, ValuePair> dic, string sp)
        {
           
            DataTable dt = new DataTable();
            try
            {
                command = new SqlCommand(sp, GetConnection())
                {
                    CommandType = CommandType.StoredProcedure,
                };
                foreach (KeyValuePair<string, ValuePair> pair in dic)
                {
                    ValuePair vp = pair.Value;
                    AddParam(command, pair.Key, vp.value1, vp.value2);
                }
                
                command.CommandTimeout = string.IsNullOrWhiteSpace(iniEntity.TimeoutValues) ? 0 : Convert.ToInt32(iniEntity.TimeoutValues);
                adapter = new SqlDataAdapter(command);
                adapter.Fill(dt);
            }
            catch(Exception ex) {
                
               var msg=  ex.Message;
            }
            return dt;
        }
        public DataSet SelectSetData(Dictionary<string, ValuePair> dic, string sp)
        {

            DataSet dt = new DataSet();
            try
            {
                command = new SqlCommand(sp, GetConnection())
                {
                    CommandType = CommandType.StoredProcedure,
                };
                foreach (KeyValuePair<string, ValuePair> pair in dic)
                {
                    ValuePair vp = pair.Value;
                    AddParam(command, pair.Key, vp.value1, vp.value2);
                }
                command.CommandTimeout = string.IsNullOrWhiteSpace(iniEntity.TimeoutValues) ? 0 : Convert.ToInt32(iniEntity.TimeoutValues);
                adapter = new SqlDataAdapter(command);
                adapter.Fill(dt);
            }
            catch (Exception ex)
            {
                //var msg = ex.Message;
                throw ex;
            }
            return dt;
        }
        /// <summary>
        /// ktp 2019-05-29 to select easily 
        /// </summary>
        /// <param name="fields">selectしたい項目</param>
        /// <param name="tableName">テーブルめい</param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        //public DataTable SimpleSelect(string fields, string tableName, string condition)
        //{
        //    Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
        //    {
        //        { "@Fields", new ValuePair { value1 = SqlDbType.VarChar, value2 = fields } },
        //        { "@Table", new ValuePair { value1 = SqlDbType.VarChar, value2 = tableName } },
        //        { "@Condition", new ValuePair { value1 = SqlDbType.VarChar, value2 = condition } }
        //    };

        //    DataTable dt = new DataTable();
        //    command = new SqlCommand("Simple_Select", GetConnection())
        //    {
        //        CommandType = CommandType.StoredProcedure,
        //    };

        //    foreach (KeyValuePair<string, ValuePair> pair in dic)
        //    {
        //        ValuePair vp = pair.Value;
        //        AddParam(command, pair.Key, vp.value1, vp.value2);
        //    }

        //    adapter = new SqlDataAdapter(command);
        //    adapter.Fill(dt);

        //    return dt;
        //}

        public bool InsertUpdateDeleteData(Dictionary<string, ValuePair> dic, string sp)
        {
            try
            {
                if (UseTransaction)
                {
                    StartTransaction();
                    command = new SqlCommand(sp, GetConnection(), transaction);
                }
                else
                    command = new SqlCommand(sp, GetConnection());
                command.CommandType = CommandType.StoredProcedure;
                foreach (KeyValuePair<string, ValuePair> pair in dic)
                {
                    ValuePair vp = pair.Value;
                    AddParam(command, pair.Key, vp.value1, vp.value2);
                }

                if (!UseTransaction)
                    command.Connection.Open();

                command.ExecuteNonQuery();

                if (UseTransaction)
                    CommitTransaction();

                return true;
            }
            catch (Exception e)
            {
                if (UseTransaction)
                    RollBackTransaction();
                //return false;     2019.6.12 chg
                throw e;
            }
            finally
            {
                command.Connection.Close();
            }
        }

        public DataTable Select_SearchName(Dictionary<string, ValuePair> dic, string sp)
        {

            DataTable dt = new DataTable();
            command = new SqlCommand("Select_SearchName", GetConnection())
            {
                CommandType = CommandType.StoredProcedure,
            };

            foreach (KeyValuePair<string, ValuePair> pair in dic)
            {
                ValuePair vp = pair.Value;
                AddParam(command, pair.Key, vp.value1, vp.value2);
            }

            adapter = new SqlDataAdapter(command);
            adapter.Fill(dt); 

            return dt;
        }
        /// <summary>
        /// Datatableをパラメータに使用する場合はこちらのメソッドを使用
        /// </summary>
        /// <param name="sp"></param>
        /// <returns></returns>
        public bool InsertUpdateDeleteData(string sp, ref string outPutParam)
        {
            try
            {
                if (UseTransaction)
                {
                    StartTransaction();
                    command.Transaction = transaction;
                }

                if (!UseTransaction)
                    command.Connection.Open();

                command.ExecuteNonQuery();

                if (!string.IsNullOrWhiteSpace(outPutParam))
                {
                    outPutParam = command.Parameters[outPutParam].Value.ToString();
                }

                if (UseTransaction)
                    CommitTransaction();

                return true;
            }
            catch (Exception ex)
            {
                if (UseTransaction)
                    RollBackTransaction();
                throw (ex);
            }
            finally
            {
                command.Connection.Close();
            }
        }
        public bool InsertUpdateDeleteBinary(Dictionary<string, ValuePairBinary> dic, string sp)
        {
            try
            {
                if (UseTransaction)
                {
                    StartTransaction();
                    command = new SqlCommand(sp, GetConnection(), transaction);
                }
                else
                    command = new SqlCommand(sp, GetConnection());
                command.CommandType = CommandType.StoredProcedure;
                foreach (KeyValuePair<string, ValuePairBinary> pair in dic)
                {
                    ValuePairBinary vp = pair.Value;
                    
                        AddParamBinary(command, pair.Key, vp.value1, vp.value2,vp.value3);
                }

                if (!UseTransaction)
                    command.Connection.Open();

                command.ExecuteNonQuery();

                if (UseTransaction)
                    CommitTransaction();

                return true;
            }
            catch (Exception e)
            {
                if (UseTransaction)
                    RollBackTransaction();
                //return false;     2019.6.12 chg
                throw e;
            }
            finally
            {
                command.Connection.Close();
            }
        }
        protected void AddParamBinary(SqlCommand cmd, string key, SqlDbType dbType, string value, byte[] byt)
        {
            if (byt == null)
            {
                if (dbType == SqlDbType.Date)
                    value = value.Replace("/", "-");
                if (string.IsNullOrWhiteSpace(value))
                    cmd.Parameters.Add(key.Replace("_NoTrim", ""), dbType).Value = DBNull.Value;
                else
                    cmd.Parameters.Add(key.Replace("_NoTrim", ""), dbType).Value = key.Contains("_NoTrim") ? value : value.Trim();
            }
            else
            {
                cmd.Parameters.Add(key.Replace("_NoTrim", ""), dbType).Value = key.Contains("_NoTrim") ? byt : byt;
            }
        }
    }


    public struct ValuePair
    {
        public SqlDbType value1;
        public string value2;
    }
    public struct ValuePairBinary
    {
        public SqlDbType value1;
        public string value2;
        public byte[] value3;
    }
}

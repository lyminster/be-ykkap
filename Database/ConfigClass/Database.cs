using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.ConfigClass
{

    public class Database
    {
        public class DBConnection
        {
            public DBConnection()
            {
                sqlConnection = new SqlConnection();
            }

            public void CreateConnection(ConnectionStringSettings connectionStringSettings)
            {
                string empty = string.Empty;
                if (sqlConnection.State != ConnectionState.Open)
                {
                    sqlConnection.ConnectionString = connectionStringSettings.ConnectionString;
                    sqlConnection.Open();
                }
            }

            public SqlTransaction StartTransaction()
            {
                return sqlConnection.BeginTransaction(IsolationLevel.ReadCommitted);
            }

            public void CloseConnection()
            {
                if (sqlConnection != null)
                {
                    sqlConnection.Close();
                }
                if (sqlConnection != null)
                {
                    sqlConnection.Dispose();
                }
                if (sqlTransaction != null)
                {
                    sqlTransaction.Dispose();
                }
                if (sqlCommand != null)
                {
                    sqlCommand.Dispose();
                }
            }

            public SqlConnection ThrowConnection()
            {
                return sqlConnection;
            }
        }

        public class DBTransaction
        {
            public void FetchTransaction(SqlTransaction sqlTransactionUI)
            {
                sqlTransaction = sqlTransactionUI;
            }

            public void ExecuteTransaction(bool bCommit)
            {
                if (bCommit)
                {
                    sqlTransaction.Commit();
                }
                else
                {
                    sqlTransaction.Rollback();
                }
            }

            public SqlTransaction ThrowTransaction()
            {
                return sqlTransaction;
            }
        }

        public class DBCommand
        {
            public DBCommand()
            {
                sqlCommand = new SqlCommand();
                sqlCommand.CommandTimeout = 0;
            }

            public void GetConnection(SqlConnection sqlConnectionUI)
            {
                sqlCommand.Connection = sqlConnectionUI;
                sqlCommand.Parameters.Clear();
            }

            public void GetTransaction(SqlTransaction sqlTransactionUI)
            {
                sqlCommand.Connection = sqlConnection;
                sqlCommand.Transaction = sqlTransactionUI;
            }

            public SqlCommand GetTextCommand()
            {
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.Parameters.Clear();
                return sqlCommand;
            }

            public SqlCommand GetSPCommand()
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.Clear();
                return sqlCommand;
            }
        }

        public class DBDataReader
        {
            public static ArrayList DataReaderToArrayList(SqlDataReader dataReader)
            {
                ArrayList fieldNameList = null;
                return p_DataReaderToArrayList(dataReader, bGetFieldNameList: false, ref fieldNameList);
            }

            public static ArrayList DataReaderToArrayList(SqlDataReader dataReader, ref ArrayList fieldNameList)
            {
                return p_DataReaderToArrayList(dataReader, bGetFieldNameList: true, ref fieldNameList);
            }

            protected static ArrayList p_DataReaderToArrayList(SqlDataReader dataReader, bool bGetFieldNameList, ref ArrayList fieldNameList)
            {
                ArrayList arrayList = new ArrayList();
                for (int i = 0; i < dataReader.FieldCount; i++)
                {
                    arrayList.Add(new ArrayList());
                }
                while (dataReader.Read())
                {
                    for (int i = 0; i < dataReader.FieldCount; i++)
                    {
                        if (dataReader.IsDBNull(i))
                        {
                            if (dataReader.GetFieldType(i).Equals(typeof(bool)))
                            {
                                ((ArrayList)arrayList[i]).Add(false);
                                continue;
                            }
                            if (dataReader.GetFieldType(i).Equals(typeof(byte)))
                            {
                                ((ArrayList)arrayList[i]).Add(0);
                                continue;
                            }
                            if (dataReader.GetFieldType(i).Equals(typeof(char)))
                            {
                                ((ArrayList)arrayList[i]).Add('\0');
                                continue;
                            }
                            if (dataReader.GetFieldType(i).Equals(typeof(DateTime)))
                            {
                                ((ArrayList)arrayList[i]).Add(DateTime.Now);
                                continue;
                            }
                            if (dataReader.GetFieldType(i).Equals(typeof(decimal)))
                            {
                                ((ArrayList)arrayList[i]).Add(0m);
                                continue;
                            }
                            if (dataReader.GetFieldType(i).Equals(typeof(double)))
                            {
                                ((ArrayList)arrayList[i]).Add(double.MinValue);
                                continue;
                            }
                            if (dataReader.GetFieldType(i).Equals(typeof(float)))
                            {
                                ((ArrayList)arrayList[i]).Add(float.MinValue);
                                continue;
                            }
                            if (dataReader.GetFieldType(i).Equals(typeof(short)))
                            {
                                ((ArrayList)arrayList[i]).Add(0);
                                continue;
                            }
                            if (dataReader.GetFieldType(i).Equals(typeof(int)))
                            {
                                ((ArrayList)arrayList[i]).Add(int.MinValue);
                                continue;
                            }
                            if (dataReader.GetFieldType(i).Equals(typeof(long)))
                            {
                                ((ArrayList)arrayList[i]).Add(long.MinValue);
                                continue;
                            }
                            if (dataReader.GetFieldType(i).Equals(typeof(string)))
                            {
                                ((ArrayList)arrayList[i]).Add(string.Empty);
                                continue;
                            }
                            if (!dataReader.GetFieldType(i).Equals(typeof(byte[])))
                            {
                                throw new Exception("Unknown Field Type");
                            }
                            ((ArrayList)arrayList[i]).Add(Guid.Empty);
                        }
                        else if (dataReader.GetFieldType(i).Equals(typeof(bool)))
                        {
                            ((ArrayList)arrayList[i]).Add(dataReader.GetString(i));
                        }
                        else if (dataReader.GetFieldType(i).Equals(typeof(byte)))
                        {
                            ((ArrayList)arrayList[i]).Add(dataReader.GetByte(i));
                        }
                        else if (dataReader.GetFieldType(i).Equals(typeof(char)))
                        {
                            ((ArrayList)arrayList[i]).Add(dataReader.GetChar(i));
                        }
                        else if (dataReader.GetFieldType(i).Equals(typeof(DateTime)))
                        {
                            ((ArrayList)arrayList[i]).Add(dataReader.GetDateTime(i));
                        }
                        else if (dataReader.GetFieldType(i).Equals(typeof(decimal)))
                        {
                            ((ArrayList)arrayList[i]).Add(dataReader.GetDecimal(i));
                        }
                        else if (dataReader.GetFieldType(i).Equals(typeof(double)))
                        {
                            ((ArrayList)arrayList[i]).Add(dataReader.GetDouble(i));
                        }
                        else if (dataReader.GetFieldType(i).Equals(typeof(float)))
                        {
                            ((ArrayList)arrayList[i]).Add(dataReader.GetFloat(i));
                        }
                        else if (dataReader.GetFieldType(i).Equals(typeof(short)))
                        {
                            ((ArrayList)arrayList[i]).Add(dataReader.GetInt16(i));
                        }
                        else if (dataReader.GetFieldType(i).Equals(typeof(int)))
                        {
                            ((ArrayList)arrayList[i]).Add(dataReader.GetInt32(i));
                        }
                        else if (dataReader.GetFieldType(i).Equals(typeof(long)))
                        {
                            ((ArrayList)arrayList[i]).Add(dataReader.GetInt64(i));
                        }
                        else if (dataReader.GetFieldType(i).Equals(typeof(string)))
                        {
                            ((ArrayList)arrayList[i]).Add(dataReader.GetString(i));
                        }
                        else
                        {
                            if (!dataReader.GetFieldType(i).Equals(typeof(byte[])))
                            {
                                throw new Exception("Unknown Field Type");
                            }
                            byte[] array = new byte[16];
                            dataReader.GetBytes(i, 0L, array, 0, 16);
                            Guid guid = new Guid(array);
                            ((ArrayList)arrayList[i]).Add(guid);
                        }
                    }
                }
                if (bGetFieldNameList)
                {
                    fieldNameList = new ArrayList();
                    for (int i = 0; i < dataReader.FieldCount; i++)
                    {
                        fieldNameList.Add(dataReader.GetName(i));
                    }
                }
                return arrayList;
            }
        }

        private static SqlConnection sqlConnection = null;

        private static SqlTransaction sqlTransaction = null;

        private static SqlCommand sqlCommand = null;
    }
}
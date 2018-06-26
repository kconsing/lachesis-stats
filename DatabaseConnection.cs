using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LachesisStats
{
    public class DatabaseConnection
    {
        // fields:
        public string client;
        public string haku;
        public string server;
        public string catalog;
        public SqlDataReader reader;

        public SqlConnection connection;
        public string connectionString;
        public string commandText;

        // constructors:
        public DatabaseConnection() { }

        public DatabaseConnection(string cli, string pw, string svr, string cat)
        {
            this.client = cli;
            this.haku = pw;
            this.server = svr;
            this.catalog = cat;

            Init();
        }

        // methods:
        public void Init()
        {
            connectionString = "data source=" + this.server +
                                ";initial catalog =" + this.catalog +
                                ";user id=" + this.client;
            connectionString += ";password=" + this.haku;
        }

        public bool OpenDatabase()
        {
            bool result = false;

            try
            {
                connection.Open();
                result = true;
            }
            catch (Exception e)
            {
                result = false;
                Console.WriteLine(e.Message);
            }

            return result;
        }

        public void CloseConnection()
        {
            this.connection.Close();
        }

        //
        // executes a single query and returns a single result as 
        //an object
        //
        public object ExecuteScalarQuery(string query)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    object result = command.ExecuteScalar();
                    return result;
                }
            }
        }

        //
        // executes a single query and returns results in Dataset
        //
        public DataSet ExecuteNonScalarQuery(string query)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataSet ds = new DataSet();

                    command.CommandText = query;

                    adapter.Fill(ds);

                    return ds;
                }
            }
        }

        //
        // executes a single action query such as Insert, Update, or Delte query,
        // and returns the number of records modified
        //
        public int ExecuteActionQuery(string query)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    int rowsModified = command.ExecuteNonQuery();

                    return rowsModified;
                }
            }
        }
    }
}

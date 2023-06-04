using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace C3Apparel.Data.Sql
{
    public class BaseRepository
    {
        private readonly IConfigurationService _configurationService;

        public BaseRepository(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(_configurationService.ConnectionString);
        }

        public object ExecuteScalar(string sql)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand(sql, conn);
                cmd.CommandText = sql;
                var returnValue = cmd.ExecuteScalar();
                conn.Close();
                return returnValue;
            }
        }
        
        public object ExecuteScalar(SqlConnection connection, SqlTransaction transaction, string sql)
        {
            if (connection.State != ConnectionState.Open)
            {
                throw new Exception("Connection not opened.");
            }

            var cmd = new SqlCommand(sql, connection);
            cmd.Transaction = transaction;
            cmd.CommandText = sql;
            var returnValue = cmd.ExecuteScalar();
            return returnValue;

        }

        public object ExecuteScalar(string sql, Dictionary<string, object> parameters)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand(sql, conn);
                cmd.CommandText = sql;

                foreach (var param in parameters)
                {
                    cmd.Parameters.Add(new SqlParameter(param.Key, param.Value));
                }

                var returnValue = cmd.ExecuteScalar();
                conn.Close();
                return returnValue;
            }
        }

        public object ExecuteScalar(SqlConnection connection, SqlTransaction transaction, string sql, Dictionary<string, object> parameters)
        {
            if (connection.State != ConnectionState.Open)
            {
                throw new Exception("Connection not opened.");
            }
            
            var cmd = new SqlCommand(sql, connection);
            cmd.CommandText = sql;
            cmd.Transaction = transaction;
            foreach (var param in parameters)
            {
                cmd.Parameters.Add(new SqlParameter(param.Key, param.Value));
            }

            var returnValue = cmd.ExecuteScalar();
            return returnValue;
        }
        public DataSet ExecuteQuery(string sql)
        {
            using (var conn = GetConnection())
            {
                var adapter = new SqlDataAdapter();
                adapter.SelectCommand = new SqlCommand(sql, conn);

                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }
        }

        public DataSet ExecuteQuery(string sql, Dictionary<string, object> parameters)
        {
            using (var conn = GetConnection())
            {
                var adapter = new SqlDataAdapter();
                var cmd = new SqlCommand(sql, conn);
                foreach (var param in parameters)
                {
                    cmd.Parameters.Add(new SqlParameter(param.Key, param.Value));
                }

                adapter.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }
        }

        public DataSet ExecuteStoredProcedureQuery(string queryName, Dictionary<string, object> parameters)
        {
            using (var conn = GetConnection())
            {
                var adapter = new SqlDataAdapter();
                var cmd = new SqlCommand(queryName, conn);
                cmd.CommandType = CommandType.StoredProcedure;

                foreach (var param in parameters)
                {
                    cmd.Parameters.Add(new SqlParameter(param.Key, param.Value));
                }

                adapter.SelectCommand = cmd;

                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }
        }

        public void ExecuteCommand(string query)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand(query, conn);

                cmd.CommandType = CommandType.Text;

                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        public DataSet ExecuteQuery(SqlConnection connection, SqlTransaction transaction,string query, Dictionary<string, object> parameters)
        {
            if (connection.State != ConnectionState.Open)
            {
                throw new Exception("Connection not opened.");
            }
            var adapter = new SqlDataAdapter();
            var cmd = new SqlCommand(query, connection);
            cmd.Transaction = transaction;

            foreach (var param in parameters)
            {
                cmd.Parameters.Add(new SqlParameter(param.Key, param.Value));
            }

            adapter.SelectCommand = cmd;

            var ds = new DataSet();
            adapter.Fill(ds);
            return ds;
        }
        
        public DataSet ExecuteStoredProcedure(SqlConnection connection, SqlTransaction transaction,string storedProcedure, Dictionary<string, object> parameters)
        {
            if (connection.State != ConnectionState.Open)
            {
                throw new Exception("Connection not opened.");
            }
            var adapter = new SqlDataAdapter();
            var cmd = new SqlCommand(storedProcedure, connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Transaction = transaction;

            foreach (var param in parameters)
            {
                cmd.Parameters.Add(new SqlParameter(param.Key, param.Value));
            }

            adapter.SelectCommand = cmd;

            DataSet ds = new DataSet();
            adapter.Fill(ds);
            return ds;
        }
        public void ExecuteCommand(SqlConnection connection, SqlTransaction transaction,string query)
        {
            if (connection.State != ConnectionState.Open)
            {
                throw new Exception("Connection not opened.");
            }
            var cmd = new SqlCommand(query, connection);
            cmd.Transaction = transaction;
            cmd.CommandType = CommandType.Text;

            cmd.ExecuteNonQuery();
        }

        public void ExecuteCommand(string query, Dictionary<string, object> parameters)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                
                var cmd = new SqlCommand(query, conn);

                cmd.CommandType = CommandType.Text;

                foreach (var param in parameters)
                {
                    cmd.Parameters.Add(new SqlParameter(param.Key, param.Value));
                }

                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        
        public void ExecuteCommand(SqlConnection connection, SqlTransaction transaction, string query, Dictionary<string, object> parameters)
        {
            if (connection.State != ConnectionState.Open)
            {
                throw new Exception("Connection not opened.");
            }

            var cmd = new SqlCommand(query, connection);

            cmd.Transaction = transaction;
            cmd.CommandType = CommandType.Text;

            foreach (var param in parameters)
            {
                cmd.Parameters.Add(new SqlParameter(param.Key, param.Value));
            }

            cmd.ExecuteNonQuery();
        }

        public void ExecuteStoredProcedureCommand(string queryName, Dictionary<string, object> parameters)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand(queryName, conn);

                cmd.CommandType = CommandType.StoredProcedure;

                foreach (var param in parameters)
                {
                    cmd.Parameters.Add(new SqlParameter(param.Key, param.Value));
                }

                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        
        public void ExecuteStoredProcedureCommand(SqlConnection connection, SqlTransaction transaction, string queryName, Dictionary<string, object> parameters)
        {
            if (connection.State != ConnectionState.Open)
            {
                throw new Exception("Connection not opened.");
            }

            var cmd = new SqlCommand(queryName, connection);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Transaction = transaction;
            foreach (var param in parameters)
            {
                cmd.Parameters.Add(new SqlParameter(param.Key, param.Value));
            }

            cmd.ExecuteNonQuery();
        }
    }
}
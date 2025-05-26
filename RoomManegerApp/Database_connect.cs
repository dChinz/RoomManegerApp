using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomManegerApp
{
    public static class Database_connect
    {
        public static SQLiteConnection connection()
        {
            string conn = "Data Source=../../Database/dtb_roommanager.db;Version=3;";
            return new SQLiteConnection(conn);
        }

        public static object ExecuteScalar(string sql, Dictionary<string, object> parameters = null)
        {
            using (var con = connection())
            {
                con.Open();
                using (var cmd = new SQLiteCommand(sql, con))
                {
                    if (parameters != null)
                    {
                        AddParameters(cmd, parameters);
                    }
                    return cmd.ExecuteScalar();
                }
            }
        }
        public static int ExecuteNonQuery(string sql, Dictionary<string, object> parameters = null)
        {
            using (var con = connection())
            {
                con.Open();
                using (var cmd = new SQLiteCommand(sql, con))
                {
                    if (parameters != null)
                    {
                        AddParameters(cmd, parameters);
                    }
                    return cmd.ExecuteNonQuery();
                }
            }
        }
        public static List<Dictionary<string, object>> ExecuteReader(string sql, Dictionary<string, object> parameters = null)
        {
            var result = new List<Dictionary<string, object>>();
            using (var con = connection())
            {
                con.Open();
                using (var cmd = new SQLiteCommand(sql, con))
                {
                    if (parameters != null)
                    {
                        AddParameters(cmd, parameters);
                    }
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var row = new Dictionary<string, object>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                row[reader.GetName(i)] = reader.GetValue(i);
                            }
                            result.Add(row);
                        }
                    }
                }
            }
            return result;
        }

        public static long ExecutiveInsertAndGetId(string sql, Dictionary<string, object> parameters)
        {
            using(var con = connection())
            {
                con.Open();
                using (var cmd = new SQLiteCommand(sql + "; select last_insert_rowid();", con))
                {
                    foreach (var param in parameters)
                    {
                        AddParameters(cmd, parameters);
                    }
                    return (long)cmd.ExecuteScalar();
                }
            }
        }

        private static void AddParameters(SQLiteCommand cmd, Dictionary<string, object> parameters)
        {
            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    cmd.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                }
            }
        }
    }
}

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
        public static SqlConnection GetSqlConnection()
        {
            string conn = "Data Source=DESKTOP-DGS0JSE\\SQL2022;Initial Catalog=QLKS;Integrated Security=True;";
            return new SqlConnection(conn);
        }

        public static SQLiteConnection connection()
        {
            string conn = "Data Source=../../Database/dtb_roommanager.db;Version=3;";
            return new SQLiteConnection(conn);
        }
    }
}

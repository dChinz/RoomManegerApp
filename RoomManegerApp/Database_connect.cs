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
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RoomManegerApp.Bills
{
    public partial class FormAdd_bill : Form
    {
        private string roomName;
        public FormAdd_bill(string name)
        {
            InitializeComponent();
            roomName = name;
        }

        string sql;
        SQLiteConnection ketnoi;
        SQLiteCommand thuchien;
        SQLiteDataReader doc;

        private void FormAdd_bill_Load(object sender, EventArgs e)
        {
            load_add_bill();
        }

        private void load_add_bill()
        {
            sql = @"select tenants.name as t_name, rooms.name as r_name, check_in.start_date as c_s_date, check_in.end_date as c_e_date, check_in.desposit as c_desposit
                    from check_in
                    inner join rooms on check_in.room_id = rooms.id
                    inner join tenants on check_in.tenant_id = tenants.id
                    where rooms.name = @name";
            using (ketnoi = Database_connect.connection())
            {
                ketnoi.Open();
                using (thuchien = new SQLiteCommand(sql, ketnoi))
                {
                    thuchien.Parameters.AddWithValue("@name", "Room " + roomName);
                    using (doc = thuchien.ExecuteReader())
                    {
                        if (doc.Read())
                        {
                            label2.Text = doc["r_name"].ToString();
                            label4.Text = doc["t_name"].ToString();
                            label6.Text = (Convert.ToInt32(doc["c_e_date"].ToString()) - Convert.ToInt32(doc["c_s_date"].ToString())).ToString();
                            //label8.Text = doc["b_rent"].ToString();
                            label10.Text = doc["c_desposit"].ToString();
                            //label12.Text =
                        }
                    }
                }
            }
        }
    }
}

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
using RoomManegerApp.Bills;

namespace RoomManegerApp.Forms
{
    public partial class FormBills : Form
    {
        public FormBills()
        {
            InitializeComponent();
        }

        string sql;
        SQLiteConnection ketnoi;
        SQLiteCommand thuchien;
        SQLiteDataReader doc;

        private void FormBills_Load(object sender, EventArgs e)
        {
            load_bill();
        }
        private void load_bill()
        {
            using(ketnoi = Database_connect.connection())
            {
                ketnoi.Open();

                sql = @"create table if not exists bills (
                    id integer primary key autoincrement,
                    check_in_id integer,
                    total_days integer,
                    rent real,
                    desposit real,
                    total real,
                    status text,
                    note text,
                    foreign key (check_in_id) references check_in(id) on delete cascade
                    foreign key (desposit) references check_in(desposit) on delete cascade)";
                using (thuchien = new SQLiteCommand(sql, ketnoi))
                {
                    thuchien.ExecuteNonQuery();
                }

                sql = @"select id, check_in_id, total_days, rent, desposit, total, status, note from bills";
                using (thuchien = new SQLiteCommand(sql, ketnoi))
                {
                    using (doc = thuchien.ExecuteReader())
                    {
                        while (doc.Read())
                        {
                            dataGridView1.Rows.Add(doc["id"], doc["check_in_id"], doc["total_days"], doc["rent"], doc["desposit"], doc["total"], doc["status"], doc["note"]);
                        }
                    }
                }
            }
        }

        private void buttonAdd_new_Click(object sender, EventArgs e)
        {
            FormStatus_room_2 form = new FormStatus_room_2();
            form.Show();
        }
    }
}

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
using RoomManegerApp.Contracts;
using RoomManegerApp.Tetants;

namespace RoomManegerApp.Check_in
{
    public partial class FormAvailable_Guest : Form
    {
        private string nameRoom;
        private string type;
        private Action _callback;
        public FormAvailable_Guest(string roomName, string roomType, Action callback)
        {
            InitializeComponent();
            nameRoom = roomName;
            type = roomType;
            _callback = callback;
        }

        string sql;
        SQLiteConnection ketnoi;
        SQLiteCommand thuchien;
        SQLiteDataReader doc;

        private void FormAvailable_Guest_Load(object sender, EventArgs e)
        {
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.Rows.Clear();

            sql = @"select tenants.id, name, phone, id_card, gender, address from tenants
                    left join checkins on checkins.tenant_id = tenants.id
                    where checkins.tenant_id is null";
            using (ketnoi = Database_connect.connection())
            {
                ketnoi.Open();
                using (thuchien = new SQLiteCommand(sql, ketnoi))
                {
                    //dataGridView1.Rows.Clear();
                    using (doc = thuchien.ExecuteReader())
                    {
                        while (doc.Read())
                        {
                            dataGridView1.Rows.Add(doc["id"], doc["name"], doc["phone"], doc["id_card"], doc["gender"], doc["address"]);
                        }
                    }
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            sql = @"select tenants.id, name, phone, id_card, gender, address from tenants
                    left join checkins on checkins.tenant_id = tenants.id
                    where checkins.tenant_id is null
                    and tenants.name like '%'|| @find ||'%' or id_card like '%'|| @find ||'%'";
            using (ketnoi = Database_connect.connection())
            {
                ketnoi.Open();
                using (thuchien = new SQLiteCommand(sql, ketnoi))
                {
                    thuchien.Parameters.AddWithValue("@find", textBox1.Text);
                    using (doc = thuchien.ExecuteReader())
                    {
                        dataGridView1.Rows.Clear();
                        while (doc.Read())
                        {
                            dataGridView1.Rows.Add(doc["id"], doc["name"], doc["phone"], doc["id_card"], doc["gender"], doc["address"]);
                        }
                    }
                }
            }
        }

        private void chọnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string nameGuest = get_name();
            FormAdd_check_in f = new FormAdd_check_in(nameRoom, nameGuest, type, _callback);
            f.Show();
            this.Close();
        }

        private void dataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var hit = dataGridView1.HitTest(e.X, e.Y);
                if (hit.RowIndex >= 0)
                {
                    // Chọn dòng chuột phải vào
                    dataGridView1.ClearSelection();
                    dataGridView1.Rows[hit.RowIndex].Selected = true;

                    // Hiển thị menu tại vị trí chuột
                    contextMenuStrip1.Show(dataGridView1, e.Location);
                }
            }
        }

        public string get_name()
        {
            string name = "";
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var row = dataGridView1.SelectedRows[0];
                name = row.Cells["name"].Value.ToString();
            }
            return name;
        }

        private void buttonCreat_Click(object sender, EventArgs e)
        {
            sql = @"select tenants.id, name, phone, id_card, gender, address from tenants
                    left join checkins on checkins.tenant_id = tenants.id
                    where checkins.tenant_id is null
                    and tenants.name like '%'|| @find ||'%' or id_card like '%'|| @find ||'%'";
            using (ketnoi = Database_connect.connection())
            {
                ketnoi.Open();
                using (thuchien = new SQLiteCommand(sql, ketnoi))
                {
                    thuchien.Parameters.AddWithValue("@find", textBox1.Text);
                    using (doc = thuchien.ExecuteReader())
                    {
                        if (doc.HasRows)
                        {
                            MessageBox.Show("Dữ liệu đã tồn tại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            string Tenant = textBox1.Text;
                            FormAdd_one_tenant f = new FormAdd_one_tenant(Tenant);
                            f.Show();
                        }
                    }
                }
            }
        }
    }
}

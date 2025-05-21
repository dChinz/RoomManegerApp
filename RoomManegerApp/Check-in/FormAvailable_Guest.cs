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

        private void FormAvailable_Guest_Load(object sender, EventArgs e)
        {
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.Rows.Clear();

            string sql = @"select tenants.id, name, phone, id_card, gender, address from tenants
                    left join checkins on checkins.tenant_id = tenants.id";
            var data = Database_connect.ExecuteReader(sql);
            foreach(var row in data)
            {
                dataGridView1.Rows.Add(row["id"], row["name"], row["phone"], row["id_card"], row["gender"], row["address"]);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            string sql = @"select tenants.id, name, phone, id_card, gender, address from tenants
                    left join checkins on checkins.tenant_id = tenants.id
                    where tenants.name like '%'|| @find ||'%' or id_card like '%'|| @find ||'%'";
            var data = Database_connect.ExecuteReader(sql, new Dictionary<string, object> { { "@find", textBox1.Text} });
            foreach (var row in data)
            {
                dataGridView1.Rows.Add(row["id"], row["name"], row["phone"], row["id_card"], row["gender"], row["address"]);
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
            string Tenant = textBox1.Text;
            FormAdd_one_tenant f = new FormAdd_one_tenant(Tenant);
            f.Show();
        }
    }
}

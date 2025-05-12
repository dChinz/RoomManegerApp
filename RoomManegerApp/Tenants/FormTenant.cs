using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RoomManegerApp.Tetants;

namespace RoomManegerApp.Forms
{
    public partial class FormTenant : Form
    {
        public FormTenant()
        {
            InitializeComponent();

            // Bật DoubleBuffering để giảm giật khi cuộn
            typeof(DataGridView).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.SetProperty,
                null, dataGridView1, new object[] { true });
        }

        string sql;
        SQLiteConnection ketnoi;
        SQLiteCommand thuchien;
        SQLiteDataReader doc;

        private void FormTenant_Load(object sender, EventArgs e)
        {

            load_tentant();

            dataGridView1.ReadOnly = true;

            this.AcceptButton = buttonSearch;

            SetPlaceholderText(textBoxName, "Nhập tên...");
            SetPlaceholderText(textBoxId_card, "Nhập số CCCD...");
        }

        public void load_tentant()
        {
            positionIndex();
            if (scrollPosition >= 0 && scrollPosition < dataGridView1.Rows.Count)
            {
                dataGridView1.FirstDisplayedScrollingRowIndex = scrollPosition;
            }

            dataGridView1.RowHeadersVisible = false;
            dataGridView1.Rows.Clear();

            sql = @"select * from tenants";
            using(ketnoi = Database_connect.connection())
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

        private int scrollPosition = 0;
        private void positionIndex()
        {
            scrollPosition = dataGridView1.FirstDisplayedScrollingRowIndex;
        }

        private void buttonAdd_one_tentant_Click(object sender, EventArgs e)
        {
            FormAdd_one_tenant f = new FormAdd_one_tenant();
            f.Show();

            f.tentant_added += (s, args) => load_tentant();
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

        public int get_id_room()
        {
            int id = -1;
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var row = dataGridView1.SelectedRows[0];
                id = Int32.Parse(row.Cells[0].Value.ToString()); //lấy id từ dòng đã chọn
            }
            return id;
        }

        private void sửaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(get_id_room().ToString());

                FormAdd_one_tenant f = new FormAdd_one_tenant(id);

                // Đăng ký sự kiện reload lại danh sách khi form edit cập nhật
                positionIndex();
                //f.tentant_updateded += (s, args) => load_tentant();

                f.Show();
            }
        }

        private void xóaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sql = @"delete from tenants where id = @id";
            using(ketnoi = Database_connect.connection())
            {
                ketnoi.Open();
                using (thuchien = new SQLiteCommand(sql, ketnoi))
                {
                    thuchien.Parameters.AddWithValue("@id", get_id_room());
                    thuchien.ExecuteNonQuery();
                }
            }
            MessageBox.Show("Xoá thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            positionIndex();
            load_tentant();
        }
        private void SetPlaceholderText(TextBox textBox, string placeholder)
        {
            // Nếu TextBox rỗng, đặt giá trị là placeholder và thay đổi màu chữ thành Gray
            if (textBox.Text == "")
            {
                textBox.Text = placeholder;
                textBox.ForeColor = Color.Gray;
            }

            // Khi người dùng click vào TextBox (focus vào)
            textBox.Enter += (s, e) =>
            {
                if (textBox.Text == placeholder)
                {
                    textBox.Text = "";
                    textBox.ForeColor = Color.Black;
                }
            };

            // Khi người dùng rời khỏi TextBox (mất focus)
            textBox.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    textBox.Text = placeholder;
                    textBox.ForeColor = Color.Gray;
                }
            };
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            string name = textBoxName.Text;
            string id_card = textBoxId_card.Text;
            sql = "select * from tenants";
            List<string> list = new List<string>();

            if(name != "" && name != "Nhập tên...")
            {
                list.Add("name like '%' || @name || '%'");
            }
            if(id_card != "" & Int64.TryParse(id_card, out long checkId_card))
            {
                list.Add("id_card = @id_card");
            }
            if(list.Count > 0)
            {
                sql += " where " + string.Join(" and ", list);
            }

            using(ketnoi = Database_connect.connection())
            {
                ketnoi.Open();
                using(thuchien = new SQLiteCommand(sql, ketnoi))
                {
                    if (name != "" && name != "Nhập tên...")
                    {
                        thuchien.Parameters.AddWithValue("@name", name);
                    }
                    if (id_card != "" & checkId_card != 0)
                    {
                        thuchien.Parameters.AddWithValue("@id_card", id_card);
                    }
                    dataGridView1.Rows.Clear();
                    using(doc = thuchien.ExecuteReader())
                    {
                        while (doc.Read())
                        {
                            dataGridView1.Rows.Add(doc["id"], doc["name"], doc["phone"], doc["id_card"], doc["gender"], doc["address"], doc["note"]);
                        }
                    }
                }
            }
        }
    }
}

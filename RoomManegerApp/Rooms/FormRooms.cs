using RoomManegerApp.Forms;
using RoomManegerApp.Romms;
using RoomManegerApp.Rooms;
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

namespace RoomManegerApp
{
    public partial class FormRooms : Form
    {
        public FormRooms()
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

        private void FormRooms_Load(object sender, EventArgs e)
        {
            load_rooms();

            dataGridView1.RowHeadersVisible = false;
            dataGridView1.ReadOnly = false;
            dataGridView1.Columns[1].ReadOnly = true;
            dataGridView1.Columns[2].ReadOnly = true;
            dataGridView1.Columns[3].ReadOnly = true;
            dataGridView1.Columns[4].ReadOnly = true;
            dataGridView1.Columns[5].ReadOnly = true;

            SetPlaceholderText(textBoxName, "Nhập số phòng...");
            SetPlaceholderText(textBoxPrice, "Nhập giá tiền...");

            this.AcceptButton = buttonSearch;
        }

        public void load_rooms()
        {
            positionIndex();

            sql = @"select * from rooms";
            using(ketnoi = Database_connect.connection())
            {
                ketnoi.Open();
                using(thuchien = new SQLiteCommand(sql, ketnoi))
                {
                    dataGridView1.Rows.Clear();
                    using(doc = thuchien.ExecuteReader())
                    {
                        while (doc.Read())
                        {
                            int rowIndex = dataGridView1.Rows.Add(false, doc["id"], doc["name"], doc["status"], doc["price"], doc["note"]);

                            SetStatusColor(dataGridView1.Rows[rowIndex], doc["status"].ToString());
                        }
                    }
                }
            }

            if(scrollPosition >= 0 && scrollPosition < dataGridView1.Rows.Count)
            {
                dataGridView1.FirstDisplayedScrollingRowIndex = scrollPosition;
            }

            label8.Text = countStatusRoom("Trống").ToString();
            label6.Text = countStatusRoom("Đã thuê").ToString();
            label4.Text = countStatusRoom("Đang sửa chữa").ToString();
            label3.Text = countStatusRoom("Khác").ToString();
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
                id = Int32.Parse(row.Cells[1].Value.ToString()); //lấy id từ dòng đã chọn
            }
            return id;
        }

        private void sửaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(dataGridView1.SelectedRows.Count > 0)
            {
                string id = get_id_room().ToString();

                Forms.FormEdit_one_room formEdit_Room = new Forms.FormEdit_one_room(id);

                // Đăng ký sự kiện reload lại danh sách khi form edit cập nhật
                formEdit_Room.room_updated += (s, args) => load_rooms();

                formEdit_Room.Show();
            }
        }

        private void xóaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sql = @"delete from rooms where id = @id";
            int id = get_id_room();
            thuchien = new SQLiteCommand(sql, ketnoi);
            thuchien.Parameters.AddWithValue("@id", id);
            ketnoi.Open();
            thuchien.ExecuteNonQuery();
            ketnoi.Close();
            MessageBox.Show("Xoá thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            load_rooms();
        }

        private void buttonAdd_one_room_Click(object sender, EventArgs e)
        {
            FormAdd_one_room formAdd_Room = new FormAdd_one_room();
            formAdd_Room.Show();

            // Đăng ký sự kiện reload lại danh sách khi form edit cập nhật
            formAdd_Room.room_added += (s, args) => load_rooms();
        }

        private void buttonAdd_many_room_Click(object sender, EventArgs e)
        {
            FormAdd_many_room formAdd_Many_Room = new FormAdd_many_room();
            formAdd_Many_Room.Show();

            // Đăng ký sự kiện reload lại danh sách khi form edit cập nhật
            formAdd_Many_Room.add_many_rooms += (s, args) => load_rooms();
        }

        private void buttonEdit_many_room_Click(object sender, EventArgs e)
        {
            List<int> listchecked = new List<int>();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                bool isChecked = Convert.ToBoolean(row.Cells[0].Value);
                if (isChecked)
                {
                    int id = Convert.ToInt32(row.Cells[1].Value);
                    listchecked.Add(id);
                }
            }
            if(listchecked.Count == 0) 
            {
                MessageBox.Show("Không có phòng nào được chọn", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            FormEdit_many_room formEdit_Many_Room = new FormEdit_many_room(listchecked);
            formEdit_Many_Room.Show();

            // Đăng ký sự kiện reload lại danh sách khi form edit cập nhật
            formEdit_Many_Room.update_many_room += (s, args) => load_rooms();
        }

        private void buttonDel_more_room_Click(object sender, EventArgs e)
        {
            List<int> listchecked = new List<int>();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                bool isChecked = Convert.ToBoolean(row.Cells[0].Value);
                if (isChecked)
                {
                    int id = Convert.ToInt32(row.Cells[1].Value);
                    listchecked.Add(id);
                }
            }
            if (listchecked.Count == 0)
            {
                MessageBox.Show("Không có phòng nào được chọn", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult result = MessageBox.Show("Chắc chắn xóa??", "Chú ý", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
            if(result == DialogResult.No)
            {
                return;
            }
            else
            {
                foreach (int id in listchecked)
                {
                    sql = @"delete from rooms where id = @id";
                    using (ketnoi = Database_connect.connection())
                    {
                        ketnoi.Open();
                        using (thuchien = new SQLiteCommand(sql, ketnoi))
                        {
                            thuchien.Parameters.AddWithValue("@id", id);
                            thuchien.ExecuteNonQuery();
                        }
                    }
                }
                MessageBox.Show("Xoá thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                load_rooms();
            }
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

        private void SetStatusColor(DataGridViewRow row, string status)
        {
            Color color = Color.White;
            if (status == "Trống") color = Color.LightGreen;
            else if (status == "Đã thuê") color = Color.OrangeRed;
            else if (status == "Đang sửa chữa") color = Color.Orange;

            row.Cells[3].Style.BackColor = color;
        }


        private void buttonSearch_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();

            List<string> conditions = new List<string>();
            if(textBoxName.Text != "Nhập số phòng..." && textBoxName.Text != "")
            {
                conditions.Add("name like '%' || @name || '%'");
            }
            if(comboBoxStatus.Text != "")
            {
                conditions.Add("status = @status");
            }
            if(textBoxPrice.Text != "Nhập giá tiền..." && textBoxPrice.Text != "")
            {
                conditions.Add("price between @price - 200000 and @price + 200000");
            }

            sql = @"select * from rooms";
            if (conditions.Count > 0) 
            {
                sql += " where " + string.Join(" and ", conditions);
            }

            using (ketnoi = Database_connect.connection())
            {
                ketnoi.Open();
                using (thuchien = new SQLiteCommand(sql, ketnoi))
                {
                    if (textBoxName.Text != "Nhập số phòng..." && textBoxName.Text != "")
                    {
                        thuchien.Parameters.AddWithValue("@name", textBoxName.Text);
                    }
                    if (comboBoxStatus.Text != "")
                    {
                        thuchien.Parameters.AddWithValue("@status", comboBoxStatus.Text);
                    }
                    if (textBoxPrice.Text != "Nhập giá tiền..." && textBoxPrice.Text != "")
                    {
                        thuchien.Parameters.AddWithValue("@price", textBoxPrice.Text);
                    }
                    dataGridView1.Rows.Clear();
                    using (doc = thuchien.ExecuteReader())
                    {
                        while (doc.Read())
                        {
                            int rowIndex = dataGridView1.Rows.Add(false, doc["id"], doc["name"], doc["status"], doc["price"], doc["note"]);

                            SetStatusColor(dataGridView1.Rows[rowIndex], doc["status"].ToString());
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

        private int countStatusRoom(string status)
        {
            sql = @"select count(status) as total 
                    from rooms
                    where status = @status";
            int total = 0;
            using(ketnoi = Database_connect.connection())
            {
                ketnoi.Open();
                using(thuchien = new SQLiteCommand(sql, ketnoi))
                {
                    thuchien.Parameters.AddWithValue("@status", status);
                    using (doc = thuchien.ExecuteReader())
                    {
                        while (doc.Read())
                        {
                            total = Int32.Parse(doc[0].ToString());
                        }
                    }
                }
            }
            return total;
        }
    }
}

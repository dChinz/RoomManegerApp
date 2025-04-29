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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace RoomManegerApp.Romms
{
    public partial class FormAdd_one_room : Form
    {
        public FormAdd_one_room()
        {
            InitializeComponent();
        }

        string sql;
        SQLiteConnection ketnoi;
        SQLiteCommand thuchien;
        SQLiteDataReader doc;

        private void FormAdd_room_Load(object sender, EventArgs e)
        {
            load_add_room();

            
        }

        private void load_add_room()
        {
            comboBox1.SelectedIndex = 0;
            textBox2.ReadOnly = true;
            textBox1.Text = null;
            comboBox1.SelectedIndex = 0;
            textBox3.Text = null;
            textBox4.Text = null;

            sql = @"select id, name, status, price, note from rooms order by id desc limit 1";
            int i = 0;
            using (ketnoi = Database_connect.connection())
            {
                using (thuchien = new SQLiteCommand(sql, ketnoi))
                {
                    ketnoi.Open();
                    using (doc = thuchien.ExecuteReader())
                    {
                        if (doc.Read())
                        {
                            i = Int32.Parse(doc["id"].ToString());
                            i++;

                            textBox2.Text = doc["name"].ToString() + ", " + doc["status"].ToString() + ", " + doc["price"].ToString() + ", " + doc["note"].ToString();
                        }
                    }
                }

                label2.Text = i.ToString();
            }
        }

        private void buttonCapnhat_Click(object sender, EventArgs e)
        {
            if(new Control[] {textBox1, comboBox1, textBox3, textBox4}.Any(c => string.IsNullOrWhiteSpace(c.Text)))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thiếu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string room = textBox1.Text.Trim();
            string status = comboBox1.Text.Trim();
            double price = 0;
            bool checkPrice = double.TryParse(textBox3.Text, out price);
            string note = textBox4.Text.Trim();

            if (string.IsNullOrWhiteSpace(room))
            {
                MessageBox.Show("Vui lòng nhập tên phòng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox1.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(status))
            {
                MessageBox.Show("Vui lòng chọn trạng thái!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                comboBox1.Focus();
                return;
            }
            if (!checkPrice)
            {
                MessageBox.Show("Vui lòng nhập đúng định dạng giá tiền!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox3.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(note))
            {
                MessageBox.Show("Vui lòng nhập ghi chú!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox4.Focus();
                return;
            }

            if (room.StartsWith("room ", StringComparison.OrdinalIgnoreCase))
            {
                room = "Room " + room.Substring(5).TrimStart();
            }
            else if (!room.StartsWith("Room "))
            {
                room = "Room " + room;
            }

            sql = @"insert into rooms (name, status, price, note) values(@name, @status, @price, @note)";
            using (ketnoi = Database_connect.connection())
            {
                using (thuchien = new SQLiteCommand(sql, ketnoi))
                {
                    thuchien.Parameters.AddWithValue("@name", room);
                    thuchien.Parameters.AddWithValue("@status", status);
                    thuchien.Parameters.AddWithValue("@price", price);
                    thuchien.Parameters.AddWithValue("@note", note);
                    ketnoi.Open();
                    thuchien.ExecuteNonQuery();
                }
            };
            MessageBox.Show("Thêm thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Gọi event nếu có người đăng ký
            room_added?.Invoke(this, EventArgs.Empty);

            load_add_room();
        }

        public event EventHandler room_added;
    }
}

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

namespace RoomManegerApp.Forms
{
    public partial class FormEdit_one_room : Form
    {
        private string roomId;
        public FormEdit_one_room(string id)
        {
            InitializeComponent();
            roomId = id;
        }

        string sql;
        SQLiteConnection ketnoi;
        SQLiteCommand thuchien;
        SQLiteDataReader doc;

        private void FormEdit_room_Load(object sender, EventArgs e)
        {
            sql = @"select * from rooms where id = @id";
            using (ketnoi = Database_connect.connection())
            {
                ketnoi.Open();
                using(thuchien = new SQLiteCommand(sql, ketnoi))
                {
                    thuchien.Parameters.AddWithValue("@id", roomId);
                    using(doc = thuchien.ExecuteReader())
                    {
                        if (doc.Read())
                        {
                            label2.Text = doc[0].ToString();
                            textBox1.Text = doc[1].ToString();
                            comboBox1.Text = doc[2].ToString();
                            textBox3.Text = doc[3].ToString();
                            textBox4.Text = doc[4].ToString();
                        }
                    }
                }
            }
        }

        private void buttonCapnhat_Click(object sender, EventArgs e)
        {
            if(new Control[] {textBox1, comboBox1, textBox3, textBox4}.Any(c => string.IsNullOrWhiteSpace(c.Text)))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thiếu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            double price;
            bool check = double.TryParse(textBox3.Text, out price);
            if(!check)
            {
                MessageBox.Show($"Giá trị giá phòng không hợp lệ", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            sql = @"UPDATE rooms
                    SET name = @name, status = @status, price = @price, note = @note
                    WHERE (id = @Original_id)";
            using (ketnoi = Database_connect.connection())
            {
                ketnoi.Open();
                using (thuchien = new SQLiteCommand(sql, ketnoi))
                {
                    thuchien.Parameters.AddWithValue("@Original_id", label2.Text);
                    thuchien.Parameters.AddWithValue("@name", textBox1.Text);
                    thuchien.Parameters.AddWithValue("@status", comboBox1.Text);
                    thuchien.Parameters.AddWithValue("@price", price);
                    thuchien.Parameters.AddWithValue("@note", textBox4.Text);
                    thuchien.ExecuteNonQuery();
                }
            }
            MessageBox.Show("Cập nhật thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Gọi event nếu có người đăng ký
            room_updated?.Invoke(this, EventArgs.Empty);

            this.Close();
        }

        public event EventHandler room_updated;
    }
}

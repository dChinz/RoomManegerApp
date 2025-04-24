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
            comboBox1.SelectedIndex = 0;

            sql = @"select * from rooms order by id desc limit 1";
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
                            i = Int32.Parse(doc[0].ToString());
                            i++;
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

            sql = @"insert into rooms (name, status, price, note) values(@name, @status, @price, @note)";
            using (ketnoi = Database_connect.connection())
            {
                using (thuchien = new SQLiteCommand(sql, ketnoi))
                {
                    thuchien.Parameters.AddWithValue("@name", textBox1.Text);
                    thuchien.Parameters.AddWithValue("@status", comboBox1.Text);
                    thuchien.Parameters.AddWithValue("@price", textBox3.Text);
                    thuchien.Parameters.AddWithValue("@note", textBox4.Text);
                    ketnoi.Open();
                    thuchien.ExecuteNonQuery();
                }
            };
            MessageBox.Show("Thêm thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Gọi event nếu có người đăng ký
            room_added?.Invoke(this, EventArgs.Empty);

            this.Close();
        }

        public event EventHandler room_added;
    }
}

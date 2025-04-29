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

namespace RoomManegerApp.Forms
{
    public partial class FormRoomDeatils : Form
    {
        public FormRoomDeatils()
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

        private void FormRoomDetails_Load(object sender, EventArgs e)
        {
            load_roomdetails();
        }

        public void load_roomdetails()
        {
            SetPlaceholderText(textBox1, "Nhập tên phòng...");
            dataGridView1.EditMode = DataGridViewEditMode.EditOnEnter;
            buttonSave.Visible = false;
            buttonThoat.Visible = false;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.ReadOnly = true;

            using (ketnoi = Database_connect.connection())
            {
                ketnoi.Open();

                using(var pragma = new SQLiteCommand("PRAGMA foreign_keys = ON;", ketnoi))
                {
                    pragma.ExecuteNonQuery();
                }

               sql = @"create table if not exists roomdetails(
                        id integer primary key autoincrement,
                        room_id integer,
                        items text,
                        note text,
                        foreign key (room_id) references rooms(id) on delete cascade)";
                using (thuchien = new SQLiteCommand(sql, ketnoi))
                {
                    thuchien.ExecuteNonQuery();
                }
            

                sql = @"select roomdetails.id, rooms.name, items, roomdetails.note from roomdetails
                        inner join rooms on rooms.id = roomdetails.room_id";
                using (thuchien = new SQLiteCommand(sql, ketnoi))
                {
                    using (doc = thuchien.ExecuteReader())
                    {
                        dataGridView1.Rows.Clear();
                        while (doc.Read())
                        {
                            dataGridView1.Rows.Add(doc["id"], doc["name"], doc["items"], doc["note"]);
                        }
                    }
                }
            }
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text) || textBox1.Text == "Nhập tên phòng...") 
            {
                load_roomdetails();
            }
            else
            {
                sql = @"select roomdetails.id, rooms.name, items, roomdetails.note from roomdetails 
                    inner join rooms on rooms.id = roomdetails.room_id
                    where name like '%' || @name || '%'";
                using (ketnoi = Database_connect.connection())
                {
                    ketnoi.Open();
                    using (thuchien = new SQLiteCommand(sql, ketnoi))
                    {
                        thuchien.Parameters.AddWithValue("@name", textBox1.Text);
                        using (doc = thuchien.ExecuteReader())
                        {
                            dataGridView1.Rows.Clear();
                            while (doc.Read())
                            {
                                dataGridView1.Rows.Add(doc["id"], doc["name"], doc["items"], doc["note"]);
                            }
                        }
                    }
                }
                textBox1.Text = null;
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

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            buttonUpdate.Enabled = false;

            buttonUpdate.Text = "Đang cập nhật...";
            buttonUpdate.BackColor = Color.Orange;
            buttonUpdate.ForeColor = Color.White;

            buttonSave.Visible = true;
            buttonThoat.Visible = true;

            dataGridView1.ReadOnly = false;
            dataGridView1.Columns[0].ReadOnly = true;
            dataGridView1.Columns[1].ReadOnly = true;

            MessageBox.Show("Bạn đang bật trạng thái cập nhật. Hiện tại bạn có thể sửa vào bảng.", "Thông báo");
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            using(ketnoi = Database_connect.connection())
            {
                ketnoi.Open();
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (dataGridView1.Rows[i].IsNewRow) continue;

                    int id = Convert.ToInt32(dataGridView1.Rows[i].Cells[0].Value);
                    string items = dataGridView1.Rows[i].Cells[2].Value.ToString();
                    string note = dataGridView1.Rows[i].Cells[3].Value.ToString();

                    sql = @"select items, note from roomdetails where id = @id";
                    using(thuchien = new SQLiteCommand(sql, ketnoi))
                    {
                        thuchien.Parameters.AddWithValue("@id", id);
                        using (doc = thuchien.ExecuteReader())
                        {
                            if (doc.Read())
                            {
                                string dbItems = doc["items"]?.ToString() ?? "";
                                string dbNote = doc["note"]?.ToString() ?? "";

                                if(items != dbItems || note != dbNote)
                                {
                                    sql = @"update roomdetails set items = @items, note = @note where id = @id";

                                    using(thuchien = new SQLiteCommand(sql, ketnoi))
                                    {
                                        thuchien.Parameters.AddWithValue("@id", id);
                                        thuchien.Parameters.AddWithValue("@items", items);
                                        thuchien.Parameters.AddWithValue("@note", note);
                                        thuchien.ExecuteNonQuery();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void buttonThoat_Click(object sender, EventArgs e)
        {
            dataGridView1.ReadOnly = true;
            buttonUpdate.Enabled = true;
            buttonSave.Visible = false;
            buttonThoat.Visible = false;

            buttonUpdate.Text = "Cập nhật";
            buttonUpdate.BackColor = SystemColors.ControlLight;
            buttonUpdate.ForeColor = Color.Black;
            load_roomdetails();
        }
    }
}

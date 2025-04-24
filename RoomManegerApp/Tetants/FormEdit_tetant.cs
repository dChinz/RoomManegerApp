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

namespace RoomManegerApp.Tetants
{
    public partial class FormEdit_tetant : Form
    {
        string tentant_id;
        public FormEdit_tetant(string id)
        {
            InitializeComponent();
            tentant_id = id;
        }

        string sql;
        SQLiteConnection ketnoi;
        SQLiteCommand thuchien;
        SQLiteDataReader doc;

        private void FormEdit_tetant_Load(object sender, EventArgs e)
        {
            sql = @"select * from tentants where id = @id";
            using(ketnoi = Database_connect.connection())
            {
                ketnoi.Open();
                using(thuchien = new SQLiteCommand(sql, ketnoi))
                {
                    thuchien.Parameters.AddWithValue("@id", tentant_id);
                    using (doc = thuchien.ExecuteReader())
                    {
                        if (doc.Read())
                        {
                            label2.Text = doc["id"].ToString();
                            textBox1.Text = doc["name"].ToString();
                            textBox2.Text = doc["phone"].ToString();
                            textBox3.Text = doc["id_card"].ToString();
                            comboBox1.Text = doc["gender"].ToString();
                            textBox5.Text = doc["address"].ToString();
                            textBox6.Text = doc["note"].ToString();
                        }
                    }
                }
            }
        }

        private void buttonCapnhat_Click(object sender, EventArgs e)
        {
            string name = textBox1.Text.Trim();
            string phone = textBox2.Text.Trim();
            string id_card = textBox3.Text.Trim();
            string gender = comboBox1.Text.Trim();
            string address = textBox5.Text.Trim();
            string note = textBox6.Text.Trim();

            if(!string.IsNullOrWhiteSpace(name) || !string.IsNullOrWhiteSpace(gender) || !string.IsNullOrWhiteSpace(address) || !string.IsNullOrWhiteSpace(note))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (phone.Length != 10 || !Int32.TryParse(phone, out int phoneNumber))
            {
                MessageBox.Show("Số điện thoại không đúng", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox2.Focus();
                return;
            }
            if (id_card.Length != 12 || !Int64.TryParse(id_card, out long id_cardNumber))
            {
                MessageBox.Show("Số CCCD không đúng", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox3.Focus();
                return;
            }

            sql = @"update tentants set name = @name, phone = @phone, id_card = @id_card, gender = @gender, address = @address, note = @note where id = @id";
            using(ketnoi = Database_connect.connection())
            {
                ketnoi.Open();
                using(thuchien = new SQLiteCommand(sql, ketnoi))
                {
                    thuchien.Parameters.AddWithValue("id", tentant_id);
                    thuchien.Parameters.AddWithValue("@name", name);
                    thuchien.Parameters.AddWithValue("@phone", phone);
                    thuchien.Parameters.AddWithValue("@id_card", id_card);
                    thuchien.Parameters.AddWithValue("@gender", gender);
                    thuchien.Parameters.AddWithValue("@address", address);
                    thuchien.Parameters.AddWithValue("@note", note);
                    thuchien.ExecuteNonQuery();
                }
            }
            MessageBox.Show("Sửa thành công!", "Thông bào", MessageBoxButtons.OK, MessageBoxIcon.Information);

            tentant_updateded?.Invoke(this, EventArgs.Empty);
            this.Close();

        }
        public event EventHandler tentant_updateded;
    }
}

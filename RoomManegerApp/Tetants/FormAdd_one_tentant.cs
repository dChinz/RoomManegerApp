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

namespace RoomManegerApp.Tetants
{
    public partial class FormAdd_one_tentant : Form
    {
        public FormAdd_one_tentant()
        {
            InitializeComponent();
        }

        string sql;
        SQLiteConnection ketnoi;
        SQLiteCommand thuchien;
        SQLiteDataReader doc;

        private void FormAdd_one_tentant_Load(object sender, EventArgs e)
        {
            sql = @"select * from tentants order by id desc limit 1";
            int i = 0;
            using (ketnoi = Database_connect.connection())
            {
                ketnoi.Open();
                using (thuchien = new SQLiteCommand(sql, ketnoi))
                {
                    using (doc = thuchien.ExecuteReader())
                    {
                        if (doc.Read())
                        {
                            i = Int32.Parse(doc[0].ToString());
                            i++;
                        }
                    }
                }
            }
            label2.Text = i.ToString();
        }

        private void buttonCapnhat_Click(object sender, EventArgs e)
        {
            string name = textBox1.Text.Trim();
            string phone = textBox2.Text.Trim();
            string id_card = textBox3.Text.Trim(); 
            string gender = comboBox1.Text.Trim();
            string address = textBox5.Text.Trim();
            string note = textBox6.Text.Trim();

            if(phone.Length != 10 || !Int32.TryParse(phone, out int phoneNumber))
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

            sql = "select 1 from tentants where name = @name and phone = @phone and id_card = @id_card and gender = @gender and address = @address and note = @note";
            using(ketnoi = Database_connect.connection())
            {
                ketnoi.Open();
                using(thuchien = new SQLiteCommand(sql, ketnoi))
                {
                    thuchien.Parameters.AddWithValue("@name", name);
                    thuchien.Parameters.AddWithValue("@phone", phone);
                    thuchien.Parameters.AddWithValue("@id_card", id_card);
                    thuchien.Parameters.AddWithValue("@gender", gender);
                    thuchien.Parameters.AddWithValue("@address", address);
                    thuchien.Parameters.AddWithValue("@note", note);
                    using (doc = thuchien.ExecuteReader())
                    {
                        if (doc.Read())
                        {
                            MessageBox.Show("Bản ghi đã tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
                sql = @"insert into tentants (name, phone, id_card, gender, address, note) values (@name, @phone, @id_card, @gender, @address, @note)";
                using (thuchien = new SQLiteCommand(sql, ketnoi))
                {
                    thuchien.Parameters.AddWithValue("@name", name);
                    thuchien.Parameters.AddWithValue("@phone", phone);
                    thuchien.Parameters.AddWithValue("@id_card", id_card);
                    thuchien.Parameters.AddWithValue("@gender", gender);
                    thuchien.Parameters.AddWithValue("@address", address);
                    thuchien.Parameters.AddWithValue("@note", note);
                    thuchien.ExecuteNonQuery();

                }
            }
            MessageBox.Show("Thêm thành công!", "Thông bào", MessageBoxButtons.OK);

            tentant_added?.Invoke(this, EventArgs.Empty);
            this.Close();
        }

        public event EventHandler tentant_added;
    }
}

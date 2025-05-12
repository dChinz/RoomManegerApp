using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RoomManegerApp.Tetants
{
    public partial class FormAdd_one_tenant : Form
    {
        private int id;
        private string tenantName;
        public FormAdd_one_tenant(string name)
        {
            InitializeComponent();
            tenantName = name;
            textBox1.Text = tenantName;
        }
        public FormAdd_one_tenant(int tenantId)
        {
            InitializeComponent();
            id = tenantId;
        }
        public FormAdd_one_tenant()
        {
            InitializeComponent();
        }

        string sql;
        SQLiteConnection ketnoi;
        SQLiteCommand thuchien;
        SQLiteDataReader doc;

        private void FormAdd_one_tentant_Load(object sender, EventArgs e)
        {
            if(id != 0)
            {
                load_form();   
            }
        }
        private void load_form()
        {
            sql = @"select * from tenants where id = @id";
            using (ketnoi = Database_connect.connection())
            {
                ketnoi.Open();
                using (thuchien = new SQLiteCommand(sql, ketnoi))
                {
                    thuchien.Parameters.AddWithValue("@id", id);
                    using (doc = thuchien.ExecuteReader())
                    {
                        if (doc.Read())
                        {
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

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(gender))
            {
                MessageBox.Show("Vui lòng điển đầy đủ thông tin", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (phone.Length != 10 || !Int32.TryParse(phone, out int phoneNumber))
            {
                MessageBox.Show("Số điện thoại không đúng (SDT gồm 10 số)", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox2.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(id_card))
            {
                MessageBox.Show("Số CCCD không đúng", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox3.Focus();
                return;
            }

            using(ketnoi = Database_connect.connection())
            {
                ketnoi.Open();

                if (id != 0)
                {
                    sql = @"update tenants set name = @name, phone = @phone, id_card = @id_card, gender = @gender, address = @address, note = @note where id = @id";
                    using (thuchien = new SQLiteCommand(sql, ketnoi))
                    {
                        thuchien.Parameters.AddWithValue("@id", id);
                        thuchien.Parameters.AddWithValue("@name", name);
                        thuchien.Parameters.AddWithValue("@phone", phone);
                        thuchien.Parameters.AddWithValue("@id_card", id_card);
                        thuchien.Parameters.AddWithValue("@gender", gender);
                        thuchien.Parameters.AddWithValue("@address", address);
                        thuchien.Parameters.AddWithValue("@note", note);
                        thuchien.ExecuteNonQuery();
                        MessageBox.Show("Cập nhật thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                        return;
                    }
                }

                sql = "select 1 from tenants where name = @name or phone = @phone or id_card = @id_card";
                using (thuchien = new SQLiteCommand(sql, ketnoi))
                {
                    thuchien.Parameters.AddWithValue("@name", name);
                    thuchien.Parameters.AddWithValue("@phone", phone);
                    thuchien.Parameters.AddWithValue("@id_card", id_card);
                    using (doc = thuchien.ExecuteReader())
                    {
                        if (doc.HasRows)
                        {
                            MessageBox.Show("Bản ghi đã tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                        {
                            sql = @"insert into tenants (name, phone, id_card, gender, address, note) values (@name, @phone, @id_card, @gender, @address, @note)";
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
                            MessageBox.Show("Thêm khách hàng thành công!", "Thông bào", MessageBoxButtons.OK);
                        }
                    }
                }
                
            }

            resetForm();
            if(tenantName != null)
            {
                this.Close();
            }
        }

        public event EventHandler tentant_added;

        private void buttonThoat_Click(object sender, EventArgs e)
        {
            this.Close();

            tentant_added?.Invoke(this, EventArgs.Empty);
        }

        private void resetForm()
        {
            textBox1.Text = null;
            textBox2.Text = null;
            textBox3.Text = null;
            comboBox1.SelectedIndex = -1;
            textBox5.Text = null;
            textBox6.Text = null;
        }
    }
}

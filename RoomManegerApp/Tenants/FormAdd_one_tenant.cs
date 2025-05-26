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

            this.StartPosition = FormStartPosition.CenterScreen;
        }
        public FormAdd_one_tenant(int tenantId)
        {
            InitializeComponent();
            id = tenantId;

            this.StartPosition = FormStartPosition.CenterScreen;
        }
        public FormAdd_one_tenant()
        {
            InitializeComponent();

            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void FormAdd_one_tentant_Load(object sender, EventArgs e)
        {
            if(id != 0)
            {
                load_form();   
            }
        }
        private void load_form()
        {
            try
            {
                string sql = @"select * from tenants where id = @id";
                var data = Database_connect.ExecuteReader(sql, new Dictionary<string, object> { { "@id", id } });
                foreach (var row in data)
                {
                    textBox1.Text = row["name"].ToString();
                    textBox2.Text = row["phone"].ToString();
                    textBox6.Text = row["email"].ToString();
                    textBox3.Text = row["id_card"].ToString();
                    comboBox1.Text = row["gender"].ToString();
                    textBox5.Text = row["address"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi gọi dữ liệu: " +  ex.Message,"Thông báo",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void buttonCapnhat_Click(object sender, EventArgs e)
        {
            string sql;
            string name = textBox1.Text.Trim();
            string phone = textBox2.Text.Trim();
            string email = textBox6.Text.Trim();
            string id_card = textBox3.Text.Trim(); 
            string gender = comboBox1.Text.Trim();
            string address = textBox5.Text.Trim();

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

            if (id != 0)
            {
                sql = @"update tenants set name = @name, phone = @phone, id_card = @id_card, gender = @gender, address = @address, note = @note where id = @id";
                int rowAffected = Convert.ToInt16(Database_connect.ExecuteNonQuery(sql, new Dictionary<string, object>
                    {
                        { "id", id },
                        { "name", name },
                        { "@phone",  phone },
                        { "@email", email },
                        { "@id_card", id_card},
                        { "@gender", gender},
                        { "address", address},
                    }));
                if (rowAffected > 0)
                {
                    MessageBox.Show("Cập nhật thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                    return;
                }
                else
                {
                    MessageBox.Show("Lỗi cập nhật dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            sql = "select 1 from tenants where name = @name";
            int row = Convert.ToInt16(Database_connect.ExecuteScalar(sql, new Dictionary<string, object>
                {
                    { "@name", name },
                }));
            if (row == 0)
            {
                sql = @"insert into tenants (name, phone, email, id_card, gender, address) values (@name, @phone, @email, @id_card, @gender, @address)";
                int rowAffected = Convert.ToInt16(Database_connect.ExecuteNonQuery(sql, new Dictionary<string, object>
                    {
                        { "name", name },
                        { "@phone",  phone },
                        { "@email", email },
                        { "@id_card", id_card},
                        { "@gender", gender},
                        { "address", address},
                    }));
                if (rowAffected > 0)
                {
                    MessageBox.Show("Cập nhật thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                    return;
                }
                else
                {
                    MessageBox.Show("Lỗi cập nhật dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Bản ghi đã tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
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

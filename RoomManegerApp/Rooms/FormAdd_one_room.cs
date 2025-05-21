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

        private void FormAdd_room_Load(object sender, EventArgs e)
        {
            load_add_room();
        }

        private void load_add_room()
        {
            textBox2.ReadOnly = true;
            textBox1.Text = null;
            textBox4.Text = null;
            comboBox1.SelectedIndex = 0;
            comboBoxSize.SelectedIndex = 0;

            string sql = @"select name, type, price, size from rooms order by id desc limit 1";
            var data = Database_connect.ExecuteReader(sql);
            foreach(var row in data)
            {
                textBox2.Text = row["name"].ToString() + ", " + row["type"].ToString() + ", " + row["price"].ToString() + ", " + row["size"].ToString();
            }
        }

        private void buttonCapnhat_Click(object sender, EventArgs e)
        {
            string name = textBox1.Text.Trim();
            string status = label7.Text.Trim();
            string type = comboBox1.Text.Trim();
            string size = comboBoxSize.Text.Trim();
            double price = 0;
            string note = textBox4.Text.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Vui lòng nhập tên phòng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(type))
            {
                MessageBox.Show("Vui lòng chọn loại phòng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if(type == "Standard")
            {
                price = 1300000;
            }
            else if(type == "Superior")
            {
                price = 1500000;
            }
            else if (type == "Deluxe")
            {
                price = 1800000;
            }
            else if (type == "Executive")
            {
                price = 2000000;
            }
            else if (type == "VIP")
            {
                price = 2500000;
            }

            if(size == "Đôi")
            {
                price += 100000;
            }

            string sql = @"select 1 from rooms where name = @name";
            int row = Convert.ToInt16(Database_connect.ExecuteScalar(sql, new Dictionary<string, object> { { "@name", name } }));
            if(row == 0)
            {
                sql = @"insert into rooms (name, status, type, size, price, note) values(@name, @status, @type, @size, @price, @note)";
                int rowAffected = Convert.ToInt16(Database_connect.ExecuteNonQuery(sql, new Dictionary<string, object>
                {
                    { "@name", name},
                    { "@status", status },
                    { "@type", type },
                    { "@price", price },
                    { "@note", note },
                }));
                if (rowAffected > 0)
                {
                    MessageBox.Show("Thêm thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Gọi event nếu có người đăng ký
                    room_added?.Invoke(this, EventArgs.Empty);
                    load_add_room();
                }
                else
                {
                    MessageBox.Show("Thêm thất bại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public event EventHandler room_added;
    }
}

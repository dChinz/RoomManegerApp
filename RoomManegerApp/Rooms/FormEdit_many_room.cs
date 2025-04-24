using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace RoomManegerApp.Rooms
{
    public partial class FormEdit_many_room : Form
    {
        private List<int> listId;
        public FormEdit_many_room(List<int> listchecked)
        {
            InitializeComponent();
            listId = listchecked;
        }

        string sql;
        SQLiteConnection ketnoi;
        SQLiteCommand thuchien;

        private void buttonCapnhat_Click(object sender, EventArgs e)
        {
            ketnoi = Database_connect.connection();
            ketnoi.Open();

            if(comboBox1.Text != "")
            {
                foreach (int id in listId)
                {
                    sql = "UPDATE rooms SET status = @status WHERE id = @id";
                    thuchien = new SQLiteCommand(sql, ketnoi);
                    thuchien.Parameters.AddWithValue("@status", comboBox1.Text);
                    thuchien.Parameters.AddWithValue("@id", id);
                    thuchien.ExecuteNonQuery();
                }
            }

            if(textBox3.Text != "")
            {
                string price = textBox3.Text;
                string result = price.Substring(1);
                char s = price[0];
                string a = "";
                if (s == '+')
                {
                    a = "price = price + @price";
                }
                else if(s == '-')
                {
                    a = "price = price - @price";
                }
                else
                {
                    a = "price = @price";
                }
                foreach (int id in listId)
                {
                    sql = "UPDATE rooms SET " + a + " WHERE id = @id";
                    updatePrice(price, id);
                }

            }
            ketnoi.Close();
            MessageBox.Show("Đã cập nhật nhiều phòng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Gọi event nếu có người đăng ký
            update_many_room?.Invoke(this, EventArgs.Empty);

            this.Close();
        }

        private void updatePrice(string price, int id)
        {
            thuchien = new SQLiteCommand(sql, ketnoi);
            thuchien.Parameters.AddWithValue("@price", price);
            thuchien.Parameters.AddWithValue("@id", id);
            thuchien.ExecuteNonQuery();
        }

        public event EventHandler update_many_room;
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RoomManegerApp.Services
{
    public partial class FormAddService : Form
    {
        public FormAddService()
        {
            InitializeComponent();
        }

        private int id;
        public FormAddService(int id)
        {
            InitializeComponent();
            this.id = id;
        }

        private void FormAddService_Load(object sender, EventArgs e)
        {
            load_form();
        }
        private void load_form()
        {
            if(id != 0)
            {
                string sql = @"select * from service where id = @id";
                var data = Database_connect.ExecuteReader(sql, new Dictionary<string, object> { { "@id", id } });
                foreach(var row in data)
                {
                    textBoxName.Text = row["name"].ToString();
                    textBoxNote.Text = row["note"].ToString();
                    textBoxPrice.Text = row["price"].ToString();
                    textBoxType.Text = row["type"].ToString();
                    textBoxUnit.Text = row["unit"].ToString();
                }
            }
        }

        private void buttonCreate_Click(object sender, EventArgs e)
        {
            string nameStr = textBoxName.Text;
            string typeStr = textBoxType.Text;
            string unitStr = textBoxUnit.Text;
            string priceStr = textBoxPrice.Text;
            string noteStr = textBoxNote.Text;

            if(id == 0)
            {
                string sql = @"insert into service (name, type, unit, price, note) values (@name, @type, @unit, @price, @note)";
                Database_connect.ExecuteReader(sql, new Dictionary<string, object>
                {
                    { "@name", nameStr },
                    { "@type", typeStr },
                    { "@unit", unitStr },
                    { "@price", priceStr },
                    { "@note", noteStr }
                });

                MessageBox.Show("Thêm dịch vụ thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                string sql = @"update service set name = @name, type = @type, unit = @unit, price = @price, note = @note where id = @id";
                Database_connect.ExecuteReader(sql, new Dictionary<string, object>
                {
                    { "@id", id},
                    { "@name", nameStr },
                    { "@type", typeStr },
                    { "@unit", unitStr },
                    { "@price", priceStr },
                    { "@note", noteStr }
                });

                MessageBox.Show("Thêm dịch vụ thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        
    }
}

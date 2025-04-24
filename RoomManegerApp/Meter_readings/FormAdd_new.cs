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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace RoomManegerApp.Meter_readings
{
    public partial class FormAdd_new : Form
    {
        public FormAdd_new()
        {
            InitializeComponent();
        }

        string sql;
        SQLiteConnection ketnoi;
        SQLiteCommand thuchien;
        SQLiteDataReader doc;

        private void FormAdd_new_Load(object sender, EventArgs e)
        {
            load_data();

            buttonSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right; // Gắn vào dưới và phải
            buttonSave.Left = this.ClientSize.Width - buttonSave.Width - 10; // Cách mép phải 10px
            buttonSave.Top = this.ClientSize.Height - buttonSave.Height - 10; // Cách mép dưới 10px

            dataGridView1.Columns[0].ReadOnly = true;
            dataGridView1.Columns[1].ReadOnly = true;
        }

        public void load_data()
        {
            string month = DateTime.Now.ToString("MM/yyyy");
            sql = @"select name from rooms";
            using (ketnoi = Database_connect.connection())
            {
                ketnoi.Open();
                using (thuchien = new SQLiteCommand(sql, ketnoi))
                {
                    using (doc = thuchien.ExecuteReader())
                    {
                        while (doc.Read())
                        {
                            int index = dataGridView1.Rows.Add();
                            dataGridView1.Rows[index].Cells["name"].Value = doc["name"].ToString();
                            dataGridView1.Rows[index].Cells["month"].Value = month;
                        }
                    }
                }
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            Dictionary<string, int> roomMap = new Dictionary<string, int>();

            using(ketnoi = Database_connect.connection())
            {
                ketnoi.Open();
                sql = @"select name, id from rooms";
                using(thuchien = new SQLiteCommand(sql, ketnoi))
                {
                    using (doc = thuchien.ExecuteReader())
                    {
                        while (doc.Read())
                        {
                            string name = doc["name"].ToString();
                            int id = Convert.ToInt16(doc["id"].ToString());
                            if (!roomMap.ContainsKey(name))
                                roomMap.Add(name, id);
                        }
                    }
                }

                sql = @"insert into meterreadings (room_id, month, e_number, w_number) values (@room_id, @month, @e_number, @w_number)";
                using(thuchien = new SQLiteCommand(sql, ketnoi))
                {
                    foreach(DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (row.IsNewRow) continue;

                        string name = row.Cells["name"].Value?.ToString();
                        string month = row.Cells["month"].Value?.ToString();

                        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(month) || !roomMap.ContainsKey(name))
                            continue;

                        if (!double.TryParse(row.Cells["e_number"].Value?.ToString(), out double e_number))
                        {
                            MessageBox.Show("Sai định dạng điện", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            row.Cells["e_number"].Selected = true;
                            return;
                        }

                        if (!double.TryParse(row.Cells["w_number"].Value?.ToString(), out double w_number))
                        {
                            MessageBox.Show("Sai định dạng nước", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            row.Cells["w_number"].Selected = true;
                            return;
                        }

                        thuchien.Parameters.AddWithValue("@room_id", roomMap[name]);
                        thuchien.Parameters.AddWithValue("@month", month);
                        thuchien.Parameters.AddWithValue("@e_number", e_number);
                        thuchien.Parameters.AddWithValue("@W_number", w_number);
                    }
                    thuchien.ExecuteNonQuery();
                }
            }
            MessageBox.Show("Lưu thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
    }
}

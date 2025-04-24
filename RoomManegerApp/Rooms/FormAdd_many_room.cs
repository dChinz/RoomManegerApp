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

namespace RoomManegerApp.Rooms
{
    public partial class FormAdd_many_room : Form
    {
        public FormAdd_many_room()
        {
            InitializeComponent();
        }

        string sql;
        SQLiteConnection ketnoi;
        SQLiteCommand thuchien;

        private void FormAdd_many_room_Load(object sender, EventArgs e)
        {
            DataGridViewComboBoxColumn column = new DataGridViewComboBoxColumn();
            column.HeaderText = "TrangThai";
            column.Name = "status";
            column.Items.AddRange("Trống", "Đã thuê", "Đang sửa chữa", "Khác");
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns.Insert(1, column);

            button1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right; // Gắn vào dưới và phải
            button1.Left = this.ClientSize.Width - button1.Width - 10; // Cách mép phải 10px
            button1.Top = this.ClientSize.Height - button1.Height - 10; // Cách mép dưới 10px

            this.Size = new Size(1000, 500);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ketnoi = Database_connect.connection();
            ketnoi.Open();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow) continue;
                bool err = false;
                int index = -1;
                if (row.Cells[0].Value == null || string.IsNullOrWhiteSpace(row.Cells[0].Value.ToString()))
                {
                    err = true;
                    index = 0;
                }

                if (row.Cells[1].Value == null)
                {
                    err = true;
                    index = 1;
                }

                double price;
                bool check = double.TryParse(row.Cells[2].Value.ToString(), out price);
                if (!check)
                {
                    err = true;
                    index = 1;
                }

                if (err)
                {
                    dataGridView1.ClearSelection();
                    MessageBox.Show($"Giá trị không hợp lệ ở dòng {row.Index + 1}.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    row.Cells[index].Selected = true;
                    return;
                }

                sql = @"insert into rooms (name, status, price, note) values(@name, @status, @price, @note)";
                thuchien = new SQLiteCommand(sql, ketnoi);
                thuchien.Parameters.AddWithValue("@name", row.Cells[0].Value?.ToString());
                thuchien.Parameters.AddWithValue("@status", row.Cells[1].Value?.ToString());
                thuchien.Parameters.AddWithValue("@price", price.ToString());
                thuchien.Parameters.AddWithValue("@note", row.Cells[3].Value?.ToString());

                thuchien.ExecuteNonQuery();
            }
            ketnoi.Close();

            // Gọi event nếu có người đăng ký
            add_many_rooms?.Invoke(this, EventArgs.Empty);

            MessageBox.Show("Thêm thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        public event EventHandler add_many_rooms;

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;

                var cell = dataGridView1.CurrentCell;
                int col = cell.ColumnIndex;
                int row = cell.RowIndex;

                // Nếu đang edit và là ComboBox, xác nhận chọn rồi thoát edit
                if (dataGridView1.EditingControl is ComboBox comboBox && comboBox.DroppedDown)
                {
                    comboBox.DroppedDown = false;
                    return;
                }

                // Xử lý di chuyển
                if (col < dataGridView1.ColumnCount - 1)
                {
                    dataGridView1.CurrentCell = dataGridView1.Rows[row].Cells[col + 1];
                }
                else
                {
                    // Nếu đến cuối dòng
                    if (row < dataGridView1.RowCount - 1)
                    {
                        dataGridView1.CurrentCell = dataGridView1.Rows[row + 1].Cells[0];
                    }
                    else
                    {
                        // Tự thêm dòng mới và chuyển đến ô đầu tiên của dòng đó
                        dataGridView1.Rows.Add();
                        dataGridView1.CurrentCell = dataGridView1.Rows[row + 1].Cells[0];
                    }
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dataGridView1.Columns[e.ColumnIndex] is DataGridViewComboBoxColumn)
            {
                dataGridView1.BeginEdit(true); // Chuyển sang chế độ edit

                if (dataGridView1.EditingControl is ComboBox comboBox)
                {
                    comboBox.DroppedDown = true; // Tự động xổ dropdown
                }
            }
        }

        private void dataGridView1_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Cells["status"].Value = "Trống";
        }
    }
}

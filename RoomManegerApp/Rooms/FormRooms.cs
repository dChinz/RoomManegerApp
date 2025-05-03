using RoomManegerApp.Forms;
using RoomManegerApp.Romms;
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

namespace RoomManegerApp
{
    public partial class FormRooms : Form
    {
        public FormRooms()
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

        private void FormRooms_Load(object sender, EventArgs e)
        {
            load_rooms();

            SetPlaceholderText(textBoxName, "Nhập số phòng...");
            SetPlaceholderText(textBoxPrice, "Nhập giá tiền...");

            this.AcceptButton = buttonSearch;
        }

        public void load_rooms()
        {
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.ReadOnly = true;

            positionIndex();
            dataGridView1.EditMode = DataGridViewEditMode.EditOnEnter;

            sql = @"select * from rooms";
            using(ketnoi = Database_connect.connection())
            {
                ketnoi.Open();
                using(thuchien = new SQLiteCommand(sql, ketnoi))
                {
                    dataGridView1.Rows.Clear();
                    using(doc = thuchien.ExecuteReader())
                    {
                        while (doc.Read())
                        {
                            int rowIndex = dataGridView1.Rows.Add(doc["id"], doc["name"], doc["status"], doc["type"], doc["price"], doc["note"]);

                            if (buttonSave.Visible)
                            {
                                SetStatusColor(dataGridView1.Rows[rowIndex], doc["status"].ToString());
                            }
                        }
                    }
                }
            }

            if(scrollPosition >= 0 && scrollPosition < dataGridView1.Rows.Count)
            {
                dataGridView1.FirstDisplayedScrollingRowIndex = scrollPosition;
            }

            label8.Text = countStatusRoom("Trống").ToString();
            label6.Text = countStatusRoom("Đã thuê").ToString();
            label4.Text = countStatusRoom("Đang sửa chữa").ToString();
            buttonSave.Visible = false;
            buttonExit.Visible = false;
            buttonSelect_all.Visible = false;
            buttonUn_selected_all.Visible = false;
        }

        private void dataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var hit = dataGridView1.HitTest(e.X, e.Y);
                if (hit.RowIndex >= 0)
                {
                    // Chọn dòng chuột phải vào
                    dataGridView1.ClearSelection();
                    dataGridView1.Rows[hit.RowIndex].Selected = true;

                    // Hiển thị menu tại vị trí chuột
                    contextMenuStrip1.Show(dataGridView1, e.Location);
                }
            }
        }

        public int get_id_room()
        {
            int id = -1;
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var row = dataGridView1.SelectedRows[0];
                id = Int32.Parse(row.Cells[0].Value.ToString()); //lấy id từ dòng đã chọn
            }
            return id;
        }

        private void xóaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sql = @"delete from rooms where id = @id";
            int id = get_id_room();
            thuchien = new SQLiteCommand(sql, ketnoi);
            thuchien.Parameters.AddWithValue("@id", id);
            ketnoi.Open();
            thuchien.ExecuteNonQuery();
            ketnoi.Close();
            MessageBox.Show("Xoá thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            load_rooms();
        }

        private void buttonAdd_one_room_Click(object sender, EventArgs e)
        {
            FormAdd_one_room formAdd_Room = new FormAdd_one_room();
            formAdd_Room.Show();

            // Đăng ký sự kiện reload lại danh sách khi form edit cập nhật
            formAdd_Room.room_added += (s, args) => load_rooms();
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Bạn đang bật trạng thái cập nhật. Hiện tại bạn có thể sửa luôn vào bảng.", "Thông báo");

            //DataGridViewComboBoxColumn comboColumn = new DataGridViewComboBoxColumn();
            //comboColumn.Name = "status";
            //comboColumn.HeaderText = "Tình trạng";
            //comboColumn.Items.Add("Trống");
            //comboColumn.Items.Add("Đã thuê");
            //comboColumn.Items.Add("Đang sửa chữa");
            //comboColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            //dataGridView1.Columns.RemoveAt(2);
            //dataGridView1.Columns.Insert(2, comboColumn);

            load_rooms();

            buttonUpdate.Enabled = false;
            buttonAdd.Enabled = false;
            buttonDelete.Enabled = false;

            buttonUpdate.Text = "Đang cập nhật...";
            buttonUpdate.BackColor = Color.Orange;
            buttonUpdate.ForeColor = Color.White;

            buttonSave.Visible = true;
            buttonExit.Visible = true;

            dataGridView1.ReadOnly = false;
            dataGridView1.Columns[0].ReadOnly = true;
            dataGridView1.Columns[1].ReadOnly = true;
            dataGridView1.Columns[2].ReadOnly = true;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Columns[2] is DataGridViewComboBoxColumn)
            {
                using (ketnoi = Database_connect.connection())
                {
                    ketnoi.Open();
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        if (dataGridView1.Rows[i].IsNewRow) continue;

                        int id = Convert.ToInt32(dataGridView1.Rows[i].Cells[0].Value);
                        string status = dataGridView1.Rows[i].Cells[2].Value.ToString();
                        double price = Convert.ToDouble(dataGridView1.Rows[i].Cells[3].Value.ToString());
                        string note = dataGridView1.Rows[i].Cells[4].Value.ToString();

                        sql = @"select status, price, note from rooms where id = @id";
                        using (thuchien = new SQLiteCommand(sql, ketnoi))
                        {
                            thuchien.Parameters.AddWithValue("@id", id);
                            using (doc = thuchien.ExecuteReader())
                            {
                                if (doc.Read())
                                {
                                    string dbStatus = doc["status"]?.ToString() ?? "";
                                    double dbPrice = Convert.ToDouble(doc["price"]?.ToString());
                                    string dbNote = doc["note"]?.ToString() ?? "";

                                    if (status != dbStatus || price != dbPrice || note != dbNote)
                                    {
                                        sql = @"update rooms set status = @status, price = @price, note = @note where id = @id";

                                        using (thuchien = new SQLiteCommand(sql, ketnoi))
                                        {
                                            thuchien.Parameters.AddWithValue("@id", id);
                                            thuchien.Parameters.AddWithValue("@status", status);
                                            thuchien.Parameters.AddWithValue("@price", price);
                                            thuchien.Parameters.AddWithValue("@note", note);
                                            thuchien.ExecuteNonQuery();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                MessageBox.Show("Đã lưu cập nhật", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (dataGridView1.Columns[0] is DataGridViewCheckBoxColumn)
            {
                bool hasSelected = false;
                foreach(DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells[0] is DataGridViewCheckBoxCell cell && Convert.ToBoolean(cell.Value) == true)
                    {
                        hasSelected = true;
                        break;
                    }
                }

                if (!hasSelected)
                {
                    MessageBox.Show("Vui lòng chọn ít nhất 1 dòng để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                DialogResult result = MessageBox.Show("Bạn chắc chắn muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes) 
                {
                    sql = @"delete from rooms where id = @id";
                    using (ketnoi = Database_connect.connection())
                    {
                        ketnoi.Open();
                        using (thuchien = new SQLiteCommand(sql, ketnoi))
                        {
                            foreach (DataGridViewRow row in dataGridView1.Rows)
                            {
                                DataGridViewCheckBoxCell cell = row.Cells[0] as DataGridViewCheckBoxCell;
                                if (cell != null && Convert.ToBoolean(cell.Value) == true)
                                {
                                    thuchien.Parameters.Clear();
                                    thuchien.Parameters.AddWithValue("@id", row.Cells[1].Value);
                                    thuchien.ExecuteNonQuery();
                                }
                            }

                        }
                    }
                }

                MessageBox.Show("Đã xóa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                dataGridView1.Columns.RemoveAt(0);
                buttonDelete.Text = "Xóa";
                buttonDelete.BackColor = SystemColors.ControlLight;
                buttonDelete.ForeColor = Color.Black;
                buttonUpdate.Enabled = true;
                buttonAdd.Enabled = true;
                buttonDelete.Enabled = true;
                load_rooms();
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Vào trạng thái xóa. Tích ô trước dòng cần xóa và bấm xác nhận để xóa", "Thông báo", MessageBoxButtons.OK);
            buttonUpdate.Enabled = false;
            buttonAdd.Enabled = false;
            buttonDelete.Enabled = false;

            buttonDelete.Text = "Đang xóa...";
            buttonDelete.BackColor = Color.Orange;
            buttonDelete.ForeColor = Color.White;

            buttonSelect_all.Visible = true;
            buttonUn_selected_all.Visible = true;
            buttonSave.Visible = true;
            buttonExit.Visible = true;

            DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn();
            dataGridView1.Columns.Insert(0, checkBoxColumn);

            dataGridView1.ReadOnly = false;
            dataGridView1.Columns[1].ReadOnly = true;
            dataGridView1.Columns[2].ReadOnly = true;
            dataGridView1.Columns[3].ReadOnly = true;
            dataGridView1.Columns[4].ReadOnly = true;
            dataGridView1.Columns[5].ReadOnly = true;
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

        private void SetStatusColor(DataGridViewRow row, string status)
        {
            Color color = Color.White;
            if (status == "Trống") color = Color.LightGreen;
            else if (status == "Đã thuê") color = Color.OrangeRed;
            else if (status == "Đang sửa chữa") color = Color.Orange;

            row.Cells["status"].Style.BackColor = color;
        }


        private void buttonSearch_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();

            List<string> conditions = new List<string>();
            if(textBoxName.Text != "Nhập số phòng..." && textBoxName.Text != "")
            {
                conditions.Add("name like '%' || @name || '%'");
            }
            if(comboBoxStatus.Text != "")
            {
                conditions.Add("status = @status");
            }
            if(textBoxPrice.Text != "Nhập giá tiền..." && textBoxPrice.Text != "")
            {
                conditions.Add("price between @price - 200000 and @price + 200000");
            }

            sql = @"select * from rooms";
            if (conditions.Count > 0) 
            {
                sql += " where " + string.Join(" and ", conditions);
            }

            using (ketnoi = Database_connect.connection())
            {
                ketnoi.Open();
                using (thuchien = new SQLiteCommand(sql, ketnoi))
                {
                    if (textBoxName.Text != "Nhập số phòng..." && textBoxName.Text != "")
                    {
                        thuchien.Parameters.AddWithValue("@name", textBoxName.Text);
                    }
                    if (comboBoxStatus.Text != "")
                    {
                        thuchien.Parameters.AddWithValue("@status", comboBoxStatus.Text);
                    }
                    if (textBoxPrice.Text != "Nhập giá tiền..." && textBoxPrice.Text != "")
                    {
                        thuchien.Parameters.AddWithValue("@price", textBoxPrice.Text);
                    }
                    dataGridView1.Rows.Clear();
                    using (doc = thuchien.ExecuteReader())
                    {
                        while (doc.Read())
                        {
                            int rowIndex = dataGridView1.Rows.Add(doc["id"], doc["name"], doc["status"], doc["price"], doc["note"]);

                            SetStatusColor(dataGridView1.Rows[rowIndex], doc["status"].ToString());
                        }
                    }
                }
            }
            comboBoxStatus.SelectedIndex = -1;
        }

        private int scrollPosition = 0;
        private void positionIndex()
        {
            scrollPosition = dataGridView1.FirstDisplayedScrollingRowIndex;
        }

        private int countStatusRoom(string status)
        {
            sql = @"select count(status) as total 
                    from rooms
                    where status = @status";
            int total = 0;
            using(ketnoi = Database_connect.connection())
            {
                ketnoi.Open();
                using(thuchien = new SQLiteCommand(sql, ketnoi))
                {
                    thuchien.Parameters.AddWithValue("@status", status);
                    using (doc = thuchien.ExecuteReader())
                    {
                        while (doc.Read())
                        {
                            total = Int32.Parse(doc[0].ToString());
                        }
                    }
                }
            }
            return total;
        }

        private void buttonExit_edit_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Columns[2] is DataGridViewComboBoxColumn)
            {
                DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
                column.Name = "status";
                column.HeaderText = "Tình trạng";
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                dataGridView1.Columns.RemoveAt(2);
                dataGridView1.Columns.Insert(2, column);


                buttonUpdate.Text = "Cập nhật";
                buttonUpdate.BackColor = SystemColors.ControlLight;
                buttonUpdate.ForeColor = Color.Black;
            }

            if (dataGridView1.Columns[0] is DataGridViewCheckBoxColumn)
            {
                dataGridView1.Columns.RemoveAt(0);

                buttonDelete.Text = "Xóa";
                buttonDelete.BackColor = SystemColors.ControlLight;
                buttonDelete.ForeColor = Color.Black;
            }

            buttonUpdate.Enabled = true;
            buttonAdd.Enabled = true;
            buttonDelete.Enabled = true;
            load_rooms();
        }

        private void buttonSelect_all_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Columns[0] is DataGridViewCheckBoxColumn)
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {   
                    DataGridViewCheckBoxCell cell = row.Cells[0] as DataGridViewCheckBoxCell;
                    if (cell != null)
                    {
                        cell.Value = true;
                    }
                }
            }
        }

        private void buttonUn_selected_all_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Columns[0] is DataGridViewCheckBoxColumn)
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    DataGridViewCheckBoxCell cell = row.Cells[0] as DataGridViewCheckBoxCell;
                    if (cell != null)
                    {
                        cell.Value = false;
                    }
                }
            }
        }
    }
}

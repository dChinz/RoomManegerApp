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

            SetPlaceholderText(textBoxName, "Nhập số phòng...");
            SetPlaceholderText(textBoxPrice, "Nhập giá tiền...");

            this.AcceptButton = buttonSearch;

            dataGridView1.RowHeadersVisible = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.EditMode = DataGridViewEditMode.EditOnEnter;
        }

        private async void FormRooms_Load(object sender, EventArgs e)
        {
            await load_rooms();
            loadStatusRoom();
        }

        public async Task load_rooms()
        {
            positionIndex();
            try
            {
                string sql = @"select * from rooms";

                var data = await Task.Run(() => Database_connect.ExecuteReader(sql));
                BeginInvoke(new Action(() => FillDataGirdView(data)));
            }
            catch (Exception ex) 
            {
                MessageBox.Show("lỗi khi tải dữ liệu: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            buttonSave.Visible = false;
            buttonExit.Visible = false;
            buttonSelect_all.Visible = false;
            buttonUn_selected_all.Visible = false;
        }

        private void FillDataGirdView(List<Dictionary<string, object>> data)
        {
            dataGridView1.SuspendLayout();
            dataGridView1.Rows.Clear();
            foreach (var row in data)
            {
                int rowIndex = dataGridView1.Rows.Add(row["id"], row["name"], row["status"], row["type"], row["price"], row["size"], row["note"]);
                SetStatusColor(dataGridView1.Rows[rowIndex], row["status"].ToString());
            }

            if (scrollPosition >= 0 && scrollPosition < dataGridView1.Rows.Count)
            {
                dataGridView1.FirstDisplayedScrollingRowIndex = scrollPosition;
            }
        }

        private void loadStatusRoom()
        {
            label8.Text = countStatusRoom("Trống").ToString();
            label6.Text = countStatusRoom("Đã thuê").ToString();
            label4.Text = countStatusRoom("Đang sửa chữa").ToString();
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
            if (dataGridView1.SelectedRows.Count > 0 &&
                int.TryParse(dataGridView1.SelectedRows[0].Cells[0].Value?.ToString(), out int id))
            {
                return id;
            }
            return -1;
        }

        private async void xóaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int id = get_id_room();
            string sql = @"delete from rooms where id = @id";
            DialogResult dialogResult = MessageBox.Show("Bạn có chắc chắn muốn xóa không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes) 
            {
                int rowAffected = Database_connect.ExecuteNonQuery(sql, new Dictionary<string, object> { { "@id", id } });
                if(rowAffected > 0)
                {
                    MessageBox.Show($"Xóa thành công {rowAffected} bản ghi", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    positionIndex();
                    await load_rooms();
                    loadStatusRoom();
                }
                else
                {
                    MessageBox.Show("Xoá không thành công hoặc không tìm thấy bản ghi.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                return;
            }
        }

        private void buttonAdd_one_room_Click(object sender, EventArgs e)
        {
            FormAdd_one_room formAdd_Room = new FormAdd_one_room();
            formAdd_Room.Show();

            // Đăng ký sự kiện reload lại danh sách khi form edit cập nhật
            formAdd_Room.room_added += async (s, args) =>
            {
                await load_rooms();
                loadStatusRoom();
            };
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Bạn đang bật trạng thái cập nhật. Hiện tại bạn có thể sửa tại bảng.", "Thông báo");
            ModeUpdate();
            positionIndex();
        }

        private async void buttonSave_Click(object sender, EventArgs e)
        {
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

                DialogResult result = MessageBox.Show("Bạn chắc chắn muốn xóa không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                int rowAffected = 0;
                if (result == DialogResult.Yes) 
                {
                    string sql = @"delete from rooms where id = @id";
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        var cell = row.Cells[0] as DataGridViewCheckBoxCell;
                        if (cell != null && Convert.ToBoolean(cell.Value) == true)
                        {
                            var parameters = new Dictionary<string, object> { { "@id", row.Cells[0].Value } };
                            rowAffected += Convert.ToInt32(Database_connect.ExecuteNonQuery(sql, parameters));
                        }
                    }
                }

                MessageBox.Show($"Đã xóa thành công {rowAffected}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                ModeNormal();
                await load_rooms();
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Vào trạng thái xóa. Tích ô trước dòng cần xóa và bấm xác nhận để xóa", "Thông báo", MessageBoxButtons.OK);
            ModeDelete();
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
            Color color = status switch
            {
                "Trống" => Color.LightGreen,
                "Đã thuê" => Color.OrangeRed,
                "Đang sữa chữa" => Color.Orange,
                _ => Color.White
            };

            row.Cells["status"].Style.BackColor = color;
        }


        private void buttonSearch_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();

            var conditions = new List<string>();
            var parameters = new Dictionary<string, object>();
            if (textBoxName.Text != "Nhập số phòng..." && !string.IsNullOrEmpty(textBoxName.Text))
            {
                conditions.Add("name like '%' || @name || '%'");
                parameters.Add("@name", textBoxName.Text);
            }
            if(comboBoxStatus.Text != "")
            {
                conditions.Add("status = @status");
                parameters.Add("@status", comboBoxStatus.Text);
            }
            if(textBoxPrice.Text != "Nhập giá tiền..." && !string.IsNullOrEmpty(textBoxPrice.Text))
            {
                if (Int32.TryParse(textBoxPrice.Text, out int tmp))
                {
                    conditions.Add("price between @price - 200000 and @price + 200000");
                    parameters.Add("@price", textBoxPrice.Text);
                }
                else
                {
                    MessageBox.Show("Số tiền không hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            string sql = @"select * from rooms";
            if (conditions.Count > 0) 
            {
                sql += " where " + string.Join(" and ", conditions);
            }

            try
            {
                var data = Database_connect.ExecuteReader(sql, parameters);
                FillDataGirdView(data);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            string sql = @"select count(status) as total 
                    from rooms
                    where status = @status";
            int total = Convert.ToInt32(Database_connect.ExecuteScalar(sql, new Dictionary<string, object> { { "@status", status} }));
            return total;
        }

        private async void buttonExit_edit_Click(object sender, EventArgs e)
        {
            ModeNormal();
            await load_rooms();
        }

        private void buttonSelect_all_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Columns[0] is DataGridViewCheckBoxColumn)
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {   
                    if (row.Cells[0] is DataGridViewCheckBoxCell cell)
                        cell.Value = true;
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

        private void ModeNormal()
        {
            buttonUpdate.Enabled = true;
            buttonAdd.Enabled = true;
            buttonDelete.Enabled = true;

            buttonDelete.Text = "Xóa";
            buttonDelete.BackColor = SystemColors.ControlLight;
            buttonDelete.ForeColor = Color.Black;

            buttonUpdate.Text = "Cập nhật";
            buttonUpdate.BackColor = SystemColors.ControlLight;
            buttonUpdate.ForeColor = Color.Black;

            buttonSave.Visible = false;
            buttonExit.Visible = false;
            buttonSelect_all.Visible = false;
            buttonUn_selected_all.Visible = false;

            if (dataGridView1.Columns[0] is DataGridViewCheckBoxColumn)
                dataGridView1.Columns.RemoveAt(0);

            dataGridView1.ReadOnly = true;
        }

        private void ModeUpdate()
        {
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

        private void ModeDelete()
        {
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
    }
}

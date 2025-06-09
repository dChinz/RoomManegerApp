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

            buttonSave.Visible = false;
            buttonExit.Visible = false;
        }

        private async void FormRooms_Load(object sender, EventArgs e)
        {
            await load_rooms();
            loadStatusRoom(); 
        }

        private async void ReloadData()
        {
            await load_rooms();
            loadStatusRoom();
        }

        private int currentPage = 1;
        private int pageSize = 24;
        private int totalRecords = 0;
        private int totalPages = 0;

        public async Task load_rooms()
        {
            UpdatePaginationInfo();
            positionIndex();
            try
            {
                int offset = (currentPage - 1) * pageSize;
                string sql = @"select * from rooms limit @pageSize offset @offset";
                var data = await Task.Run(() => Database_connect.ExecuteReader(sql, new Dictionary<string, object>
                {
                    { "@pageSize", pageSize},
                    { "@offset", offset},
                }));
                BeginInvoke(new Action(() => FillDataGirdView(data)));
            }
            catch (Exception ex) 
            {
                MessageBox.Show("lỗi khi tải dữ liệu: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            labelPageInfo.Text = $"Trang {currentPage}/{totalPages}";

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (!row.IsNewRow)
                    row.Cells[0].Value = false; // Cột 0 là checkbox
            }
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

        private void UpdatePaginationInfo()
        {
            string sql = "SELECT COUNT(*) FROM rooms";
            totalRecords = Convert.ToInt32(Database_connect.ExecuteScalar(sql));
            totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
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
                deleteRoom();
            }
            else
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.IsNewRow) continue;
                    if (row.Tag?.ToString() != "modified") continue;

                    if (int.TryParse(row.Cells[0].Value.ToString(), out int id))
                    {
                        string type = row.Cells[3].Value?.ToString();
                        string size = row.Cells[5].Value?.ToString();
                        string note = row.Cells[6].Value?.ToString();
                        double price = 0;

                        if (type == "Standard")
                        {
                            price = 1300000;
                        }
                        else if (type == "Superior")
                        {
                            price = 1500000;
                        }
                        else if (type == "Delexu")
                        {
                            price = 1800000;
                        }
                        else if (type == "Executive")
                        {
                            price = 2000000;
                        }

                        if (size == "Đôi")
                        {
                            price += 100000;
                        }

                        string sql = @"update rooms set type = @type, price = @price, size = @size, note = @note where id = @id";
                        await Task.Run(() => Database_connect.ExecuteNonQuery(sql, new Dictionary<string, object>
                    {
                        { "@id", id},
                        { "@type", type},
                        { "@price", price},
                        { "@size", size},
                        { "note", note}
                    }));

                        row.Tag = null;
                    }
                }
                MessageBox.Show("Đã lưu những thay đổi.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            ModeNormal();
            await load_rooms();
        }

        private void deleteRoom()
        {
            try
            {
                if (dataGridView1.Columns[0] is DataGridViewCheckBoxColumn)
                {
                    for (int i = dataGridView1.Rows.Count - 1; i >= 0; i--)
                    {
                        DataGridViewRow row = dataGridView1.Rows[i];
                        if (row.IsNewRow) continue;

                        bool isChecked = Convert.ToBoolean(row.Cells[0].Value);
                        if (isChecked)
                        {
                            int id = Convert.ToInt32(row.Cells[1].Value);
                            string sql = @"delete from rooms where id = @id";
                            Database_connect.ExecuteNonQuery(sql, new Dictionary<string, object> { { "@id", id } });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã có lỗi xảy ra " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            MessageBox.Show("Xóa phòng thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ReloadData();
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
                "Đang thuê" => Color.LightSalmon,
                "Đang sửa chữa" => Color.LightGray,
                _ => Color.White
            };

            row.Cells["status"].Style.BackColor = color;
        }


        private void buttonSearch_Click(object sender, EventArgs e)
        {
            load_search();
        }

        private void load_search()
        {
            dataGridView1.Rows.Clear();

            var conditions = new List<string>();
            var parameters = new Dictionary<string, object>();
            if (textBoxName.Text != "Nhập số phòng..." && !string.IsNullOrEmpty(textBoxName.Text))
            {
                conditions.Add("name like '%' || @name || '%'");
                parameters.Add("@name", textBoxName.Text);
            }
            if (comboBoxStatus.Text != "")
            {
                conditions.Add("status = @status");
                parameters.Add("@status", comboBoxStatus.Text);
            }
            if (textBoxPrice.Text != "Nhập giá tiền..." && !string.IsNullOrEmpty(textBoxPrice.Text))
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
            sql += " limit @pageSize offset @offset";
            parameters.Add("@pageSize", pageSize);
            parameters.Add("@offset", (currentPage - 1) * pageSize);
            try
            {
                var data = Database_connect.ExecuteReader(sql, parameters);
                FillDataGirdView(data);

                sql = @"select count(*) from rooms";
                if (conditions.Count > 0)
                {
                    sql += " where " + string.Join(" and ", conditions);
                }
                totalRecords = Convert.ToInt32(Database_connect.ExecuteScalar(sql, parameters));
                totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
                labelPageInfo.Text = $"Trang {currentPage}/{totalPages}";

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

        private void loadStatusRoom()
        {
            string sql = @"select count(status) as total 
                    from rooms
                    where status = @status";
            label8.Text = Database_connect.ExecuteScalar(sql, new Dictionary<string, object> { { "@status", "Trống"} }).ToString();
            label6.Text = Database_connect.ExecuteScalar(sql, new Dictionary<string, object> { { "@status", "Đang thuê" } }).ToString();
            label4.Text = Database_connect.ExecuteScalar(sql, new Dictionary<string, object> { { "@status", "Đang sửa chữa" } }).ToString();
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
            buttonCreate.Enabled = true;
            buttonUpdate.Enabled = true;
            buttonDelete.Enabled = true;

            buttonUpdate.Text = "Cập nhật phòng";
            buttonUpdate.BackColor = SystemColors.ControlLight;
            buttonUpdate.ForeColor = Color.Black;

            buttonDelete.Text = "Xóa phòng";
            buttonDelete.BackColor = SystemColors.ControlLight;
            buttonDelete.ForeColor = Color.Black;

            buttonSave.Visible = false;
            buttonExit.Visible = false;

            dataGridView1.ReadOnly = true;

            if (dataGridView1.Columns[0] is DataGridViewCheckBoxColumn)
            {
                dataGridView1.Columns.RemoveAt(0);
            }
        }

        private void ModeUpdate()
        {
            buttonCreate.Enabled = false;
            buttonUpdate.Enabled = false;
            buttonDelete.Enabled = false;

            buttonUpdate.Text = "Cập nhật...";
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
            DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn();
            checkBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;

            dataGridView1.Columns.Insert(0, checkBoxColumn);

            buttonCreate.Enabled = false;
            buttonUpdate.Enabled = false;
            buttonDelete.Enabled = false;

            buttonDelete.Text = "Đang xóa...";
            buttonDelete.BackColor = Color.Orange;
            buttonDelete.ForeColor = Color.White;

            buttonSave.Visible = true;
            buttonExit.Visible = true;

            dataGridView1.ReadOnly = false;
            dataGridView1.Columns[1].ReadOnly = true;
            dataGridView1.Columns[2].ReadOnly = true;
            dataGridView1.Columns[3].ReadOnly = true;
            dataGridView1.Columns[4].ReadOnly = true;
            dataGridView1.Columns[5].ReadOnly = true;
            dataGridView1.Columns[6].ReadOnly = true;
        }

        private async void btnFirst_Click(object sender, EventArgs e)
        {
            currentPage = 1;
            await load_rooms();
        }

        private async void btnPrev_Click(object sender, EventArgs e)
        {
            if(currentPage > 1)
            {
                currentPage--;
                await load_rooms();
            }
        }

        private async void btnNext_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                await load_rooms();
            }
        }

        private async void btnLast_Click(object sender, EventArgs e)
        {
            currentPage = totalPages;
            await load_rooms();
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex >= 0)
            {
                var row = dataGridView1.Rows[e.RowIndex];
                row.Tag = "modified";
            }
        }

        private void buttonCreate_Click(object sender, EventArgs e)
        {
            FormAdd_one_room formAdd_Room = new FormAdd_one_room(ReloadData);
            formAdd_Room.ShowDialog();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            ModeDelete();
            positionIndex();
        }

        private void sửaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int id = get_id_room();
            int rowIndex = dataGridView1.CurrentCell.RowIndex;
            if (dataGridView1.Rows[rowIndex].Cells[2].Value.ToString() == "Đang thuê")
            {
                MessageBox.Show("Phòng này đang được thuê. Không thể sửa lúc này.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            FormAdd_one_room form = new FormAdd_one_room(id, ReloadData);
            form.ShowDialog();
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
    }
}

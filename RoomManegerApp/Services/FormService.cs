using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RoomManegerApp.Services
{
    public partial class FormService : Form
    {
        public FormService()
        {
            InitializeComponent();
        }

        private int currentPage = 1;
        private int pageSize = 22;
        private int totalPages = 0;
        private int totalRecords = 0;

        private async void FormService_Load(object sender, EventArgs e)
        {
            await load_form();
        }
        private async Task load_form()
        {
            UpdatePaginationInfo();
            dataGridView1.Rows.Clear();
            int offset = (currentPage - 1) * pageSize;
            string sql = @"select id, name, type, unit, price, note from service limit @pageSize offset @offset";
            var data = await Task.Run(() => Database_connect.ExecuteReader(sql, new Dictionary<string, object> { 
                { "@pageSize", pageSize }, 
                { "@offset", offset }
            }));
            foreach (var row in data)
            {
                dataGridView1.Rows.Add(row["id"], row["name"], row["type"], row["unit"], row["price"], row["note"]);
            }
            labelPageInfo.Text = $"Trang {currentPage}/{totalPages}";
        }
        private void UpdatePaginationInfo()
        {
            string sql = "SELECT COUNT(*) FROM bills";
            totalRecords = Convert.ToInt32(Database_connect.ExecuteScalar(sql));
            totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
        }

        private async void btnFirst_Click(object sender, EventArgs e)
        {
            currentPage = 1;
            await load_form();
        }

        private async void btnPrev_Click(object sender, EventArgs e)
        {
            if(currentPage > 1)
            {
                currentPage--;
                await load_form();
            }
        }

        private async void btnNext_Click(object sender, EventArgs e)
        {
            if(currentPage < totalPages)
            {
                currentPage++;
                await load_form();
            }
        }

        private async void btnLast_Click(object sender, EventArgs e)
        {
            currentPage = totalPages;
            await load_form();
        }

        private int get_id()
        {
            if (dataGridView1.SelectedRows.Count > 0 && Int32.TryParse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString(), out int id))
            {
                return id;
            }
            return -1;
        }

        private void xóaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int id = get_id();
            try
            {
                string sql = @"delete from service where id = @id";
                Database_connect.ExecuteNonQuery(sql, new Dictionary<string, object> { { "@id", id } });

                MessageBox.Show("Xóa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã có lỗi xảy ra " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
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

        private void sừaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int id = get_id();
            FormAddService form = new FormAddService(id);
            form.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormAddService form = new FormAddService();
            form.ShowDialog();
        }
    }
}

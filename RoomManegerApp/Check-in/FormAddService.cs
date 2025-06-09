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

namespace RoomManegerApp.Check_in
{
    public partial class FormAddService : Form
    {
        private int idBills;
        private Action _callback;
        public FormAddService(int idBills, Action callback)
        {
            InitializeComponent();
            this.idBills = idBills;
            comboBoxService.SelectedIndex = 0;
            this._callback = callback;
        }

        private void LoadServiceByType(string type)
        {
            panelServices.Controls.Clear();
            int y = 10;

            // Lấy dịch vụ đã thêm vào hóa đơn (nếu có)
            string sqlAdded = @"SELECT service_id, number 
                        FROM serviceDetails 
                        WHERE bill_id = @id";
            var addedServices = Database_connect.ExecuteReader(sqlAdded, new Dictionary<string, object> { { "@id", idBills } });

            // Tạo dictionary lưu dịch vụ đã có: service_id => số lượng
            Dictionary<int, int> addedDict = new Dictionary<int, int>();
            foreach (var row in addedServices)
            {
                int serviceId = Convert.ToInt32(row["service_id"]);
                int number = Convert.ToInt32(row["number"]);
                addedDict[serviceId] = number;
            }

            // Lấy tất cả dịch vụ thuộc loại này (dù đã hay chưa thêm)
            string sql = "SELECT id, name, price FROM service WHERE type = @type";
            var allServices = Database_connect.ExecuteReader(sql, new Dictionary<string, object> { { "@type", type } });
            // Duyệt qua toàn bộ dịch vụ theo loại
            foreach (var row in allServices)
            {
                int id = Convert.ToInt32(row["id"]);
                string name = row["name"].ToString();
                int price = Convert.ToInt32(row["price"]);

                var lbl = new Label()
                {
                    Text = $"{name} ({price:N0}đ)",
                    Location = new Point(10, y),
                    AutoSize = true
                };

                // Gán value nếu dịch vụ đã có
                int value = addedDict.ContainsKey(id) ? addedDict[id] : 0;

                var num = new NumericUpDown()
                {
                    Name = $"num_{id}",
                    Location = new Point(200, y - 2),
                    Width = 60,
                    Minimum = 0,
                    Maximum = 100,
                    Value = value
                };

                panelServices.Controls.Add(lbl);
                panelServices.Controls.Add(num);

                y += 30;
            }
        }

        private void comboBoxService_SelectedIndexChanged(object sender, EventArgs e)
        {
            string type = comboBoxService.SelectedItem.ToString();
            LoadServiceByType(type);
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            foreach (Control control in panelServices.Controls)
            {
                if (control is NumericUpDown num && num.Value >= 0)
                {
                    int serviceId = int.Parse(num.Name.Split('_')[1]);
                    int quantity = (int)num.Value;

                    // Kiểm tra xem đã có service này trong bill chưa
                    string checkSql = @"SELECT COUNT(*) FROM serviceDetails 
                                WHERE bill_id = @bill AND service_id = @service";
                    var check = Database_connect.ExecuteScalar(checkSql, new Dictionary<string, object>
                    {
                        { "@bill", idBills },
                        { "@service", serviceId }
                    });

                    int exists = Convert.ToInt32(check);

                    if (exists > 0)
                    {
                        if (quantity > 0)
                        {
                            // Đã có → cập nhật lại số lượng
                            string updateSql = @"UPDATE serviceDetails 
                                     SET number = @number 
                                     WHERE bill_id = @bill AND service_id = @service";
                            Database_connect.ExecuteNonQuery(updateSql, new Dictionary<string, object>
                            {
                                { "@bill", idBills },
                                { "@service", serviceId },
                                { "@number", quantity }
                            });
                        }
                        else
                        {
                            string deleteSql = @"delete from serviceDetails where bill_id = @bill AND service_id = @service";
                            Database_connect.ExecuteNonQuery(deleteSql, new Dictionary<string, object>
                            {
                                { "@bill", idBills },
                                { "@service", serviceId },
                            });
                        }
                    }
                    else
                    {
                        // Chưa có → thêm mới
                        string insertSql = @"INSERT INTO serviceDetails (bill_id, service_id, number) 
                                     VALUES (@bill, @service, @number)";
                        Database_connect.ExecuteNonQuery(insertSql, new Dictionary<string, object>
                        {
                            { "@bill", idBills },
                            { "@service", serviceId },
                            { "@number", quantity }
                        });
                    }
                }
            }

            _callback?.Invoke();
            MessageBox.Show("Lưu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        private void buttonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

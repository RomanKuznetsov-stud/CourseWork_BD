
using DrinkShop.AppControllers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace DrinkShop
{
    public partial class All_Orders_Form : Form
    {
        private readonly ShopController _controller;
        private readonly string _role;
        private readonly int _userId;

        public All_Orders_Form(ShopController controller, int userId, string role)
        {
            InitializeComponent();
            _controller = controller;
            _userId = userId;
            _role = role;
        }

        private void All_Orders_Form_Load(object sender, EventArgs e)
        {
            if (_role == "User")
            {
                LoadUserOrders();
            }
            else if (_role == "Admin")
            {
                LoadAllOrders();
            }
        }

        private void LoadUserOrders()
        {
            DataTable dataTable = _controller.LoadUserOrders(_userId);

            dataGridViewOrders.DataSource = dataTable;

            dataGridViewOrders.Columns["OrderId"].Visible = false;
            dataGridViewOrders.Columns["OrderDate"].HeaderText = "Дата Замовлення";
            dataGridViewOrders.Columns["TotalAmount"].HeaderText = "Сума Замовлення";
        }

        private void LoadAllOrders()
        {
            DataTable dataTable = _controller.LoadAllOrders();

            dataGridViewOrders.DataSource = dataTable;

            dataGridViewOrders.Columns["OrderId"].HeaderText = "ID Замовлення";
            dataGridViewOrders.Columns["Username"].HeaderText = "Користувач";
            dataGridViewOrders.Columns["OrderDate"].HeaderText = "Дата Замовлення";
            dataGridViewOrders.Columns["TotalAmount"].HeaderText = "Сума Замовлення";
        }

        private void order_button_Click_1(object sender, EventArgs e)
        {
            if (dataGridViewOrders.SelectedRows.Count > 0)
            {
                int orderId = Convert.ToInt32(dataGridViewOrders.SelectedRows[0].Cells["OrderId"].Value);

                using (Order_information detailsForm = new Order_information(orderId, _controller))
                {
                    detailsForm.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("Будь ласка, виберіть замовлення для перегляду деталей.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}

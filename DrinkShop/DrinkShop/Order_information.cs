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

namespace DrinkShop
{
    public partial class Order_information : Form
    {
        private readonly int _orderId;
        private readonly ShopController _shopController;

        public Order_information(int orderId, ShopController shopController)
        {
            InitializeComponent();
            _orderId = orderId;
            _shopController = shopController;
        }

        private void Order_information_Load(object sender, EventArgs e)
        {
            LoadOrderDetails();
        }

        private void LoadOrderDetails()
        {
            try
            {
                DataTable details = _shopController.GetOrderDetails(_orderId);
                details_datagrid.DataSource = details;

                if (details_datagrid.Columns["ProductName"] != null)
                    details_datagrid.Columns["ProductName"].HeaderText = "Назва продукту";

                if (details_datagrid.Columns["Quantity"] != null)
                    details_datagrid.Columns["Quantity"].HeaderText = "Кількість";

                if (details_datagrid.Columns["Price"] != null)
                    details_datagrid.Columns["Price"].HeaderText = "Ціна";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка під час завантаження деталей замовлення: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }
}
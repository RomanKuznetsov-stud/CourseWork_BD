
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Menu;

namespace DrinkShop
{
    public partial class OrderForm : Form
    {
        private readonly ShopController _shopController;
        private readonly int _userId;
        private readonly List<CartItem> cart = new List<CartItem>();

        public OrderForm(int userId, ShopController shopController)
        {
            InitializeComponent();
            _userId = userId;
            _shopController = shopController;

            this.Load += OrderForm_Load;
        }

        private void OrderForm_Load(object sender, EventArgs e)
        {
            LoadProducts();
            LabelVisibility(false);
        }

        private void LoadProducts()
        {
            dataGridViewProducts.DataSource = _shopController.LoadBuyProducts();
            dataGridViewProducts.Columns["Id"].Visible = false;
        }

        private void buttonAddToCart_Click(object sender, EventArgs e)
        {
            LabelVisibility(true);

            if (dataGridViewProducts.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridViewProducts.SelectedRows[0];
                var productId = Convert.ToInt32(selectedRow.Cells["Id"].Value);
                var productName = selectedRow.Cells["ProductName"].Value.ToString();
                var price = Convert.ToDecimal(selectedRow.Cells["Price"].Value);

                var existingItem = cart.FirstOrDefault(c => c.ProductId == productId);
                if (existingItem != null)
                {
                    existingItem.Quantity++;
                }
                else
                {
                    cart.Add(new CartItem { ProductId = productId, ProductName = productName, Price = price, Quantity = 1 });
                }

                UpdateCartView();
            }
        }

        private void buttonCheckout_Click(object sender, EventArgs e)
        {
            if (cart.Count == 0)
            {
                MessageBox.Show("Кошик порожній!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            decimal totalAmount = cart.Sum(c => c.Quantity * c.Price);
            int orderId = _shopController.InsertOrder(_userId, totalAmount);

            foreach (var item in cart)
            {
                _shopController.InsertOrderDetails(orderId, item.ProductId, item.Quantity, item.Price);
            }

            MessageBox.Show("Замовлення успішно оформлено!", "Інформація", MessageBoxButtons.OK, MessageBoxIcon.Information);
            cart.Clear();
            UpdateCartView();
        }

        private void UpdateCartView()
        {
            dataGridViewCart.DataSource = null;
            dataGridViewCart.DataSource = cart.Select(c => new
            {
                c.ProductName,
                c.Price,
                c.Quantity,
                Total = c.Quantity * c.Price
            }).ToList();

            labelTotalAmount.Text = $"Загальна сума: {cart.Sum(c => c.Quantity * c.Price):C}";
        }

        private void LabelVisibility(bool isVisible)
        {
            labelTotalAmount.Visible = isVisible;
        }

        public class CartItem
        {
            public int ProductId { get; set; }
            public string ProductName { get; set; }
            public decimal Price { get; set; }
            public int Quantity { get; set; }
        }
    }
}




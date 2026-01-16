
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Security;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using DrinkShop.AppControllers;

namespace DrinkShop
{
    public partial class MainScreen : Form
    {

        private readonly ShopController _productController;
        private string _username;
        private string _role;
        private Account _parentForm;
        private readonly string _connectionString;

        public MainScreen(Account parentForm, string connectionString, string username, string role)
        {
            InitializeComponent();
            _productController = new ShopController(connectionString);
            _parentForm = parentForm;
            _username = username;
            _role = role;
            _connectionString = connectionString;
        }
        private void MainScreen_Load(object sender, EventArgs e)
        {
            User_label.Text = _username;
            Redact_information_button.Visible = _role == "Admin";
            List_of_users.Visible = _role == "Admin";
            discount_button.Visible = _role == "Admin";
            SetAdminControlsVisibility(false);
            UserTableVisibility(false);
            special_price.Visible = false;
            default_price.Visible = false;
            discountTextBox.Visible = false;
            top_productVisibility(false);
            LoadProducts();
            LoadUsers();
            background_drink.SendToBack();
        }

        private void LoadProducts()
        {
            try
            {
                var productsTable = _productController.LoadProducts();
                dataGridViewProducts.DataSource = productsTable;

                if (dataGridViewProducts.Columns["ProductsName"] != null)
                    dataGridViewProducts.Columns["ProductsName"].HeaderText = "Product Name";

                if (dataGridViewProducts.Columns["Price"] != null)
                    dataGridViewProducts.Columns["Price"].HeaderText = "Price";

                if (dataGridViewProducts.Columns["CountryOfOrigin"] != null)
                    dataGridViewProducts.Columns["CountryOfOrigin"].HeaderText = "Country";

                if (dataGridViewProducts.Columns["CategoryName"] != null)
                    dataGridViewProducts.Columns["CategoryName"].HeaderText = "Category";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading products: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void AddProducts_button_Click(object sender, EventArgs e)
        {
            ShopController shopController = new ShopController(_connectionString);
            using (EditProductForm editProductForm = new EditProductForm(shopController))
            {
                if (editProductForm.ShowDialog() == DialogResult.OK)
                {
                    LoadProducts();
                }
            }
        }

        private void DeleteProduct_button_Click(object sender, EventArgs e)
        {
            if (dataGridViewProducts.SelectedRows.Count > 0)
            {
                string productName = dataGridViewProducts.SelectedRows[0].Cells["ProductsName"].Value.ToString();

                if (_productController.DeleteProduct(productName))
                {
                    MessageBox.Show("Product deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadProducts();
                }
                else
                {
                    MessageBox.Show("Failed to delete product.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a product to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ChangePrice_button_Click(object sender, EventArgs e)
        {
            if (dataGridViewProducts.SelectedRows.Count > 0)
            {
                string productName = dataGridViewProducts.SelectedRows[0].Cells["ProductsName"].Value.ToString();

                using (ChangePriceForm changePriceForm = new ChangePriceForm())
                {
                    if (changePriceForm.ShowDialog() == DialogResult.OK)
                    {
                        decimal newPrice = changePriceForm.NewPrice;

                        if (_productController.ChangeProductPrice(productName, newPrice))
                        {
                            MessageBox.Show("Ціна успішно змінена.", "Інформація", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadProducts();
                        }
                        else
                        {
                            MessageBox.Show("Не вдалося змінити ціну.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Будь ласка, виберіть товар для зміни ціни.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void Logout_buton_Click(object sender, EventArgs e)
        {
            _parentForm.ResetForm();
            _parentForm.Show();
            this.Close();
        }
        private void Redact_information_button_Click(object sender, EventArgs e)
        {
            SetAdminControlsVisibility(true);
            UserTableVisibility(false);
            special_price.Visible = false;
            default_price.Visible = false;
            discountTextBox.Visible = false;
            top_productVisibility(false);
        }
        private void Home_button_Click(object sender, EventArgs e)
        {
            SetAdminControlsVisibility(false);
            UserTableVisibility(false);
            special_price.Visible = false;
            default_price.Visible = false;
            discountTextBox.Visible = false;
            dataGridViewProducts.Visible = true;
            top_productVisibility(false);
        }
        private void List_of_users_Click(object sender, EventArgs e)
        {
            SetAdminControlsVisibility(false);
            UserTableVisibility(true);
            top_productVisibility(false);
            special_price.Visible = false;
            default_price.Visible = false;
            discountTextBox.Visible = false;
            LoadUsers();
        }

        private void Buy_button_Click(object sender, EventArgs e)
        {
            int userId = _productController.GetUserIdFromDatabase(_username);
            OrderForm orderForm = new OrderForm(userId, _productController);
            UserTableVisibility(false);
            SetAdminControlsVisibility(false);
            top_productVisibility(false);
            special_price.Visible = false;
            default_price.Visible = false;
            discountTextBox.Visible = false;
            orderForm.Show();
        }

        private void LoadUsers()
        {
            try
            {
                DataTable users = _productController.LoadUsers();

                dataGridViewUsers.DataSource = users;

                if (dataGridViewUsers.Columns["Id"] != null)
                    dataGridViewUsers.Columns["Id"].HeaderText = "ID";

                if (dataGridViewUsers.Columns["Email"] != null)
                    dataGridViewUsers.Columns["Email"].HeaderText = "Email";

                if (dataGridViewUsers.Columns["Username"] != null)
                    dataGridViewUsers.Columns["Username"].HeaderText = "Username";

                if (dataGridViewUsers.Columns["Role"] != null)
                    dataGridViewUsers.Columns["Role"].HeaderText = "Role";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка під час завантаження користувачів: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Orders_button_Click_1(object sender, EventArgs e)
        {
            int userId = _productController.GetUserIdFromDatabase(_username);
            All_Orders_Form ordersForm = new All_Orders_Form(_productController, userId, _role);
            ordersForm.ShowDialog();

            UserTableVisibility(false);
            SetAdminControlsVisibility(false);
            special_price.Visible = false;
            default_price.Visible = false;
            discountTextBox.Visible = false;
            top_productVisibility(false);
        }
        private void Max_Order_Button_Click(object sender, EventArgs e)
        {
            try
            {

                DataTable dataTable = _productController.GetUsersWithMaxOrders();
                dataGridViewUsers.DataSource = dataTable;
                if (dataGridViewUsers.Columns["Id"] != null)
                    dataGridViewUsers.Columns["Id"].HeaderText = "ID";

                if (dataGridViewUsers.Columns["Username"] != null)
                    dataGridViewUsers.Columns["Username"].HeaderText = "Username";

                if (dataGridViewUsers.Columns.Contains("TotalOrders"))
                    dataGridViewUsers.Columns["TotalOrders"].HeaderText = "Total Orders";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка під час завантаження даних: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Max_Price_Button_Click(object sender, EventArgs e)
        {
            try
            {

                DataTable dataTable = _productController.GetUsersWithMaxSpent();

                dataGridViewUsers.DataSource = dataTable;

                if (dataGridViewUsers.Columns["Id"] != null)
                    dataGridViewUsers.Columns["Id"].HeaderText = "ID";

                if (dataGridViewUsers.Columns["Username"] != null)
                    dataGridViewUsers.Columns["Username"].HeaderText = "Username";

                if (dataGridViewUsers.Columns.Contains("TotalSpent"))
                    dataGridViewUsers.Columns["TotalSpent"].HeaderText = "Total Spent";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка під час завантаження даних: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void background_drink_Click(object sender, EventArgs e)
        {
            background_drink.SendToBack();
        }
        private void discount_button_Click(object sender, EventArgs e)
        {
            dataGridViewProducts.Visible = true;
            special_price.Visible = true;
            default_price.Visible = true;
            discountTextBox.Visible = true;
            UserTableVisibility(false);
            top_productVisibility(false);
        }
        private void special_price_Click(object sender, EventArgs e)
        {
            if (decimal.TryParse(discountTextBox.Text, out decimal discountPercentage))
            {
                try
                {
                    int rowsAffected = _productController.ApplyDiscount(discountPercentage);
                    MessageBox.Show($"Знижка {discountPercentage}% застосована до {rowsAffected} товарів.", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadProducts();
                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show(ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Помилка під час застосування знижки: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Будь ласка, введіть коректний відсоток знижки.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void default_price_Click(object sender, EventArgs e)
        {
            try
            {
                int rowsAffected = _productController.ResetPricesToDefault();
                MessageBox.Show($"Ціни повернені до стандартних для {rowsAffected} товарів.", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadProducts();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка під час відновлення цін: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void top_product_Click(object sender, EventArgs e)
        {
            special_price.Visible = false;
            default_price.Visible = false;
            discountTextBox.Visible = false;
            UserTableVisibility(false);
            SetAdminControlsVisibility(false);
            top_productVisibility(true);
        }



        private void top_count_button_Click(object sender, EventArgs e)
        {
            int productCount = (int)top_product_count.Value;
            decimal? maxPrice = null;
            decimal? minPrice = null;

            if (productCount <= 0)
            {
                MessageBox.Show("Будь ласка, введіть коректну кількість напоїв.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

 
            if (decimal.TryParse(max_price_textbox.Text, out decimal parsedMaxPrice))
            {
                maxPrice = parsedMaxPrice;
            }

            if (decimal.TryParse(min_price_textbox.Text, out decimal parsedMinPrice))
            {
                minPrice = parsedMinPrice;
            }

            try
            {
                DataTable dataTable = _productController.GetTopSellingProductsByPrice(productCount, maxPrice, minPrice);

                top_product_datagrid.DataSource = dataTable;

                if (top_product_datagrid.Columns["ProductName"] != null)
                    top_product_datagrid.Columns["ProductName"].HeaderText = "Назва продукту";

                if (top_product_datagrid.Columns["TotalQuantity"] != null)
                    top_product_datagrid.Columns["TotalQuantity"].HeaderText = "Кількість проданих";

                if (top_product_datagrid.Columns["Price"] != null)
                    top_product_datagrid.Columns["Price"].HeaderText = "Ціна";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка під час завантаження топ-продуктів: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void UserTableVisibility(bool isVisible)
        {
            dataGridViewUsers.Visible = isVisible;
            Max_Price_Button.Visible = isVisible;
            Max_Order_Button.Visible = isVisible;

        }
        private void SetAdminControlsVisibility(bool isVisible)
        {
            dataGridViewProducts.Visible = isVisible;
            DeleteProduct_button.Visible = isVisible;
            ChangePriсe_button.Visible = isVisible;
            AddProducts_button.Visible = isVisible;

        }
        private void top_productVisibility(bool isVisible)
        {
            top_product_datagrid.Visible = isVisible;
            top_count_button.Visible = isVisible;
            top_product_count.Visible = isVisible;
            min_price_textbox.Visible = isVisible;
            max_price_textbox.Visible = isVisible;
            L1.Visible = isVisible;
            L2.Visible = isVisible;
        }

        private void top_product_datagrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}


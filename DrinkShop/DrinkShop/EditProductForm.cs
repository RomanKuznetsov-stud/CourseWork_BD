using DrinkShop.AppControllers;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DrinkShop
{
    public partial class EditProductForm : Form
    {
        private readonly ShopController _shopController;

        public EditProductForm(ShopController shopController)
        {
            InitializeComponent();
            _shopController = shopController;
        }

        private void Save_button_Click(object sender, EventArgs e)
        {
            string name = textBoxName.Text;
            string country = textBoxCountry.Text;

            if (!decimal.TryParse(textBoxPrice.Text, out decimal price))
            {
                MessageBox.Show("Invalid price value.");
                return;
            }

            if (!int.TryParse(comboBoxCategory.SelectedValue?.ToString(), out int categoryId))
            {
                MessageBox.Show("Select a valid category.");
                return;
            }

            try
            {
                if (_shopController.AddNewProduct(name, price, country, categoryId))
                {
                    MessageBox.Show("Product added successfully.");
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show("Error adding product.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding product: {ex.Message}");
            }
        }

        private void EditProductForm_Load(object sender, EventArgs e)
        {
            LoadCategories();
        }

        private void LoadCategories()
        {
            DataTable categories = _shopController.LoadCategories();
            comboBoxCategory.DataSource = categories;
            comboBoxCategory.DisplayMember = "Name";
            comboBoxCategory.ValueMember = "Id";
        }
    }
}
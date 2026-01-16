using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using DrinkShop.AppControllers;


namespace DrinkShop
{
    public partial class Account : Form
    {
        private readonly string _connectionString = "Server=DESKTOP-7FQ25H6; Database=DrinkOnlineShop; Trusted_Connection=True;";
        private readonly AccountController _controller;

        public Account()
        {
            InitializeComponent();
            _controller = new AccountController(_connectionString);
        }
        public void ResetForm()
        {
            Email_textbox.Clear();
            password_textbox.Clear();
            user_name_textbox.Clear();
        }

        private void sign_in_label_Click(object sender, EventArgs e)
        {
            string email = Email_textbox.Text;
            string password = password_textbox.Text;

            if (_controller.Login(email, password, out string role, out string username))
            {
                MessageBox.Show($"Авторизація успішна! Роль: {role}");

                MainScreen mainscreen = new MainScreen(this, _controller.ConnectionString, username, role);
                this.Hide();
                mainscreen.Show();
            }
            else
            {
                MessageBox.Show("Невірний логін або пароль!");
            }
        }

        private void sign_up_button_Click(object sender, EventArgs e)
        {
            string email = Email_textbox.Text;
            string username = user_name_textbox.Text;
            string password = password_textbox.Text;

            if (_controller.Register(email, username, password))
            {
                MessageBox.Show("Реєстрація успішна! Тепер ви можете увійти.");
            }
            else
            {
                MessageBox.Show("Користувач із такою поштою або ім'ям уже існує.");
            }
        }

        private void password_textbox_TextChanged(object sender, EventArgs e)
        {
            password_textbox.PasswordChar = '*';
        }

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrinkShop.AppModel
{
    public class AccountModel
    {
        private readonly string _connectionString;

        public AccountModel(string connectionString)
        {
            _connectionString = connectionString;
        }

        public bool AuthenticateUser(string email, string password, out string role, out string username)
        {
            role = string.Empty;
            username = string.Empty;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT Role, Username FROM Users WHERE Email = @Email AND Password = @Password";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Email", email);
                string hashedPassword = PasswordHasher.HashPassword(password);
                command.Parameters.AddWithValue("@Password", hashedPassword);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        role = reader["Role"].ToString();
                        username = reader["Username"].ToString();
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Помилка доступу до бази даних: {ex.Message}");
                }
            }

            return false;
        }

        public bool RegisterUser(string email, string username, string password)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO Users (Email, Username, Password, Role) VALUES (@Email, @Username, @Password, 'User')";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Username", username);
                string hashedPassword = PasswordHasher.HashPassword(password);
                command.Parameters.AddWithValue("@Password", hashedPassword);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    return true;
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2627)
                        return false;

                    throw new Exception($"Помилка бази даних: {ex.Message}");
                }
            }
        }
    }
}
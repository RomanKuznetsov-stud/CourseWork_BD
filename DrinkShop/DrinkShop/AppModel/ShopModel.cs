using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrinkShop.AppModel
{
    public class ShopModel
    {
        private readonly string _connectionString;

        public ShopModel(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DataTable GetAllProducts()
        {
            string query = @"
                SELECT 
                    Products.Name AS ProductsName, 
                    Products.Price, 
                    Products.Country AS CountryOfOrigin,
                    Category.Name AS CategoryName
                FROM 
                    Products
                INNER JOIN 
                    Category 
                ON 
                    Products.CategoryId = Category.Id";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                return dataTable;
            }
        }

        public bool DeleteProduct(string productName)
        {
            string query = "DELETE FROM Products WHERE Name = @ProductName";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ProductName", productName);

                connection.Open();
                return command.ExecuteNonQuery() > 0;
            }
        }

        public bool ChangePrice(string productName, decimal newPrice)
        {
            string updateQuery = "UPDATE Products SET Price = @NewPrice, StandardPrice = @NewPrice WHERE Name = @ProductName";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(updateQuery, connection);
                command.Parameters.AddWithValue("@NewPrice", newPrice);
                command.Parameters.AddWithValue("@ProductName", productName);

                connection.Open();
                return command.ExecuteNonQuery() > 0;
            }
        }

        public DataTable GetCategories()
        {
            string query = "SELECT Id, Name FROM Category";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable categories = new DataTable();
                adapter.Fill(categories);
                return categories;
            }
        }

        public bool AddProduct(string name, decimal price, string country, int categoryId)
        {
            string query = "INSERT INTO Products (Name, Price, Country, CategoryId) VALUES (@Name, @Price, @Country, @CategoryId)";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@Price", price);
                command.Parameters.AddWithValue("@Country", country);
                command.Parameters.AddWithValue("@CategoryId", categoryId);

                connection.Open();
                return command.ExecuteNonQuery() > 0;
            }
        }

        public DataTable GetUsers()
        {
            string query = "SELECT Id, Email, Username, Role FROM Users";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                return dataTable;
            }
        }


        public DataTable LoadProducts()
        {
            string query = @"
        SELECT 
            Products.Id, 
            Products.Name AS ProductName, 
            Products.Price, 
            Products.Country, 
            Category.Name AS CategoryName
        FROM Products
        INNER JOIN Category ON Products.CategoryId = Category.Id";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                return dataTable;
            }
        }

        public int InsertOrder(int userId, decimal totalAmount)
        {
            string insertOrderQuery = "INSERT INTO Orders (UserId, TotalAmount) OUTPUT INSERTED.Id VALUES (@UserId, @TotalAmount)";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(insertOrderQuery, connection);
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@TotalAmount", totalAmount);

                connection.Open();
                return (int)command.ExecuteScalar();
            }
        }

        public void InsertOrderDetails(int orderId, int productId, int quantity, decimal price)
        {
            string insertOrderDetailsQuery = @"
        INSERT INTO OrderDetails (OrderId, ProductId, Quantity, Price) 
        VALUES (@OrderId, @ProductId, @Quantity, @Price)";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(insertOrderDetailsQuery, connection);
                command.Parameters.AddWithValue("@OrderId", orderId);
                command.Parameters.AddWithValue("@ProductId", productId);
                command.Parameters.AddWithValue("@Quantity", quantity);
                command.Parameters.AddWithValue("@Price", price);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public int GetUserIdFromDatabase(string username)
        {

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT Id FROM Users WHERE Username = @Username";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", username);

                connection.Open();
                return (int)command.ExecuteScalar();
            }
        }
        public DataTable GetUserOrders(int userId)
        {
            string query = @"
            SELECT 
                Orders.Id AS OrderId, 
                Orders.OrderDate, 
                Orders.TotalAmount
            FROM Orders
            WHERE UserId = @UserId";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                adapter.SelectCommand.Parameters.AddWithValue("@UserId", userId);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                return dataTable;
            }
        }

        public DataTable GetAllOrders()
        {
            string query = @"
            SELECT 
                Orders.Id AS OrderId, 
                Users.Username, 
                Orders.OrderDate, 
                Orders.TotalAmount
            FROM Orders
            INNER JOIN Users ON Orders.UserId = Users.Id";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                return dataTable;
            }
        }

        public DataTable GetOrderDetails(int orderId)
        {
            string query = @"
                SELECT 
                    Products.Name AS ProductName,
                    OrderDetails.Quantity,
                    OrderDetails.Price
                FROM OrderDetails
                INNER JOIN Products ON OrderDetails.ProductId = Products.Id
                WHERE OrderDetails.OrderId = @OrderId";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                adapter.SelectCommand.Parameters.AddWithValue("@OrderId", orderId);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                return dataTable;
            }
        }

        public int ApplyDiscount(decimal discountFactor)
        {
            string query = @"
                UPDATE Products
                SET Price = Price * @DiscountFactor
                WHERE Price > 100";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@DiscountFactor", discountFactor);
                connection.Open();
                return command.ExecuteNonQuery(); 
            }
        }

        public int ResetPricesToDefault()
        {
            string query = @"
                UPDATE Products
                SET Price = StandardPrice";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                return command.ExecuteNonQuery();
            }
        }

        public DataTable GetUsersWithMaxOrders()
        {
            string query = @"
            SELECT 
                Users.Id, 
                Users.Username, 
                COUNT(Orders.Id) AS TotalOrders
            FROM 
                Users
            INNER JOIN 
                Orders 
            ON 
                Users.Id = Orders.UserId
            GROUP BY 
                Users.Id, Users.Username
            ORDER BY 
                TotalOrders DESC";

            return ExecuteQuery(query);
        }

        public DataTable GetUsersWithMaxSpent()
        {
            string query = @"
            SELECT 
                Users.Id, 
                Users.Username, 
                SUM(Orders.TotalAmount) AS TotalSpent
            FROM 
                Users
            INNER JOIN 
                Orders 
            ON 
                Users.Id = Orders.UserId
            GROUP BY 
                Users.Id, Users.Username
            ORDER BY 
                TotalSpent DESC";

            return ExecuteQuery(query);
        }

        private DataTable ExecuteQuery(string query)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                return dataTable;
            }
        }



        public DataTable GetTopSellingProductsByPrice(int productCount, decimal? maxPrice, decimal? minPrice)
        {
            string query = @"
        SELECT TOP (@ProductCount)
            Products.Name AS ProductName,
            SUM(OrderDetails.Quantity) AS TotalQuantity,
            Products.Price
        FROM 
            Products
        INNER JOIN 
            OrderDetails ON Products.Id = OrderDetails.ProductId
        WHERE 
            (@MaxPrice IS NULL OR Products.Price <= @MaxPrice) AND 
            (@MinPrice IS NULL OR Products.Price >= @MinPrice)
        GROUP BY 
            Products.Name, Products.Price
        ORDER BY 
            TotalQuantity DESC";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                adapter.SelectCommand.Parameters.AddWithValue("@ProductCount", productCount);
                adapter.SelectCommand.Parameters.AddWithValue("@MaxPrice", (object)maxPrice ?? DBNull.Value);
                adapter.SelectCommand.Parameters.AddWithValue("@MinPrice", (object)minPrice ?? DBNull.Value);

                DataTable dataTable = new DataTable();
                connection.Open();
                adapter.Fill(dataTable);
                return dataTable;
            }
        }



    }

}


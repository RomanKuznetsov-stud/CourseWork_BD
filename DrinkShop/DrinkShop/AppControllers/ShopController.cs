using DrinkShop.AppModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DrinkShop.AppControllers
{
    public class ShopController
    {
        private readonly ShopModel _shopModel;

        public ShopController(string connectionString)
        {
            _shopModel = new ShopModel(connectionString);
        }

        public DataTable LoadProducts()
        {
            return _shopModel.GetAllProducts();
        }

        public bool DeleteProduct(string productName)
        {
            return _shopModel.DeleteProduct(productName);
        }

        public bool ChangeProductPrice(string productName, decimal newPrice)
        {
            return _shopModel.ChangePrice(productName, newPrice);
        }

        public DataTable LoadCategories()
        {
            return _shopModel.GetCategories();
        }

        public bool AddNewProduct(string name, decimal price, string country, int categoryId)
        {
            return _shopModel.AddProduct(name, price, country, categoryId);
        }
        public DataTable LoadUsers()
        {
            return _shopModel.GetUsers();
        }
        public DataTable LoadBuyProducts()
        {
            return _shopModel.LoadProducts();
        }

        public int InsertOrder(int userId, decimal totalAmount)
        {
            return _shopModel.InsertOrder(userId, totalAmount);
        }

        public void InsertOrderDetails(int orderId, int productId, int quantity, decimal price)
        {
            _shopModel.InsertOrderDetails(orderId, productId, quantity, price);
        }

        public int GetUserIdFromDatabase(string username)
        {
            return _shopModel.GetUserIdFromDatabase(username);
        }
        public DataTable LoadUserOrders(int userId)
        {
            return _shopModel.GetUserOrders(userId);
        }

        public DataTable LoadAllOrders()
        {
            return _shopModel.GetAllOrders();
        }
        public DataTable GetOrderDetails(int orderId)
        {
            return _shopModel.GetOrderDetails(orderId);
        }
        public int ApplyDiscount(decimal discountPercentage)
        {
            if (discountPercentage <= 0 || discountPercentage > 100)
                throw new ArgumentException("Discount percentage must be between 0 and 100.");

            decimal discountFactor = 1 - (discountPercentage / 100);
            return _shopModel.ApplyDiscount(discountFactor);
        }

        public int ResetPricesToDefault()
        {
            return _shopModel.ResetPricesToDefault();
        }

        public DataTable GetUsersWithMaxOrders()
        {
            return _shopModel.GetUsersWithMaxOrders();
        }

        public DataTable GetUsersWithMaxSpent()
        {
            return _shopModel.GetUsersWithMaxSpent();
        }


        public DataTable GetTopSellingProductsByPrice(int productCount, decimal? maxPrice, decimal? minPrice)
        {
            if (productCount <= 0)
                throw new ArgumentException("Кількість продуктів має бути більше 0.");

            return _shopModel.GetTopSellingProductsByPrice(productCount, maxPrice, minPrice);
        }


    }
}
using DrinkShop.AppModel;
using System;

namespace DrinkShop.AppControllers
{
    public class AccountController
    {
        private readonly AccountModel _model;


        public string ConnectionString { get; }


        public AccountController(string connectionString)
        {
            ConnectionString = connectionString;
            _model = new AccountModel(connectionString);
        }


        public bool Login(string email, string password, out string role, out string username)
        {
            return _model.AuthenticateUser(email, password, out role, out username);
        }

        public bool Register(string email, string username, string password)
        {
            return _model.RegisterUser(email, username, password);
        }
    }
}

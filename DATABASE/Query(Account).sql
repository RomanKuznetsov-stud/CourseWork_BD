CREATE DATABASE DrinkOnlineShop;
GO
USE DrinkOnlineShop;
CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
	Email NVARCHAR(50) NOT NULL UNIQUE CONSTRAINT Email_Format CHECK(Email LIKE '%_@__%.__%'),
	Password NVARCHAR(255) NOT NULL,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    Role NVARCHAR(20) NOT NULL CHECK (Role IN ('User', 'Admin')) 
);
INSERT INTO Users (Email, Password,Username, Role)
VALUES 
('ibragim45@gmail.com', '111000','Roman', 'User'),
('admin24db@gmail.com', '221133R','Admin', 'Admin');
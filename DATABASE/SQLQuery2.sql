CREATE TABLE Suppliers (
    Id INT PRIMARY KEY IDENTITY(1,1),  
    Name NVARCHAR(255) NOT NULL,      
    ContactName NVARCHAR(255),       
    Phone NVARCHAR(50),               
    Address NVARCHAR(255),           
    Country NVARCHAR(100)             
);

ALTER TABLE Products
ADD SupplierId INT;

ALTER TABLE Products
ADD CONSTRAINT FK_Products_Suppliers
FOREIGN KEY (SupplierId) REFERENCES Suppliers(Id);

CREATE TABLE Reviews (
    Id INT PRIMARY KEY IDENTITY(1,1),      
    ProductId INT NOT NULL,              
    UserId INT NOT NULL,                  
    Rating INT CHECK (Rating BETWEEN 1 AND 5), 
    Comment NVARCHAR(MAX),                
    ReviewDate DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_Reviews_Products FOREIGN KEY (ProductId) REFERENCES Products(Id),
    CONSTRAINT FK_Reviews_Users FOREIGN KEY (UserId) REFERENCES Users(Id)
);

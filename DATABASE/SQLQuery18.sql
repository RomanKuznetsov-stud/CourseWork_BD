CREATE TABLE Category (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL
);

CREATE TABLE Products (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Price DECIMAL(10, 2) NOT NULL,
	Country NVARCHAR(100) NOT NULL,
    CategoryId INT NOT NULL,
    FOREIGN KEY (CategoryId) REFERENCES Category(Id)
);
INSERT INTO Category (Name) 
VALUES 
('Соки'), 
('Кава'), 
('Чай'),
('Міцні напої'),
('Вина'),
('Ігристі вина'),
('Енергетики'),
('Газовані напої');


INSERT INTO Products (Name, Price, CategoryId,Country) 
VALUES
('Апельсиновий сік', 50.00, 1, 'Іспанія'),
('Яблучний сік', 45.00, 1, 'Україна'),
('Еспресо', 30.00, 2, 'Італія'),
('Капучино', 35.00, 2, 'Італія'),
('Зелений чай', 25.00, 3, 'Китай'),
('Чорний чай', 20.00, 3, 'Індія'),
('Red Bull', 40.00, 7, 'Австрія'),
('Coca-Cola', 35.00, 8, 'США'),
('Єгермейстер', 450.00, 3,'Німеччина');

Select * from Products
Select * from Category
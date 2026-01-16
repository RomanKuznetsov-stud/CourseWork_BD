ALTER TABLE Products
ADD StandardPrice DECIMAL(10, 2) NOT NULL DEFAULT 0;
UPDATE Products
SET StandardPrice = Price;

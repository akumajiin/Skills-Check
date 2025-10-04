-- Create Products table
CREATE TABLE Products (
  Id INT PRIMARY KEY,
  Name NVARCHAR(100),
  Price DECIMAL(10,2)
);

-- Sample data
INSERT INTO Products (Id, Name, Price) VALUES (1, 'Widget', 9.99);
INSERT INTO Products (Id, Name, Price) VALUES (2, 'Gadget', 19.95);
INSERT INTO Products (Id, Name, Price) VALUES (3, 'Thingamajig', 5.50);

GO

-- Stored procedure for paginated access
IF OBJECT_ID('dbo.GetProductsPaged', 'P') IS NOT NULL
    DROP PROCEDURE dbo.GetProductsPaged;
GO

CREATE PROCEDURE dbo.GetProductsPaged
    @Page INT,
    @PageSize INT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Offset INT = (@Page - 1) * @PageSize;

    SELECT Id, Name, Price
    FROM Products
    ORDER BY Id
    OFFSET @Offset ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END;
GO

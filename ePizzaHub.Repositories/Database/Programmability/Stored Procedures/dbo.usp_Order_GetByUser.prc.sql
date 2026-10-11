IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'usp_Order_GetByUser')
DROP PROCEDURE usp_Order_GetByUser
GO

CREATE OR ALTER PROCEDURE dbo.usp_Order_GetByUser
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Result set 1: orders + payment totals
    SELECT o.Id, o.CreatedDate, o.Street, o.City, o.Locality, o.ZipCode, o.PhoneNumber,
           p.Total, p.Tax, p.GrandTotal, p.Status
    FROM dbo.Orders o
    JOIN dbo.PaymentDetails p ON p.Id = o.PaymentId
    WHERE o.UserId = @UserId
    ORDER BY o.CreatedDate DESC;

    -- Result set 2: lines
    SELECT oi.OrderId, oi.ItemId, i.Name AS ItemName, oi.UnitPrice, oi.Quantity, oi.Total
    FROM dbo.OrderItems oi
    JOIN dbo.Orders o ON o.Id = oi.OrderId
    JOIN dbo.Items i  ON i.Id = oi.ItemId
    WHERE o.UserId = @UserId
    ORDER BY oi.OrderId, oi.Id;
END
GO
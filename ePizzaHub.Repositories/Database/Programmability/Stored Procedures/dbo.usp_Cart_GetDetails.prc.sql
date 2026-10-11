IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'usp_Cart_GetDetails')
DROP PROCEDURE usp_Cart_GetDetails
GO

CREATE OR ALTER PROCEDURE dbo.usp_Cart_GetDetails
    @CartId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    -- Result set 1: header + subtotal
    SELECT c.Id, c.UserId, c.CreatedDate,
           ISNULL(SUM(ci.UnitPrice * ci.Quantity), 0) AS Total
    FROM dbo.Carts c
    LEFT JOIN dbo.CartItems ci ON ci.CartId = c.Id
    WHERE c.Id = @CartId AND c.IsActive = 1
    GROUP BY c.Id, c.UserId, c.CreatedDate;

    -- Result set 2: lines
    SELECT ci.Id, ci.ItemId, ci.UnitPrice, ci.Quantity,
           i.Name AS ItemName, i.ImageUrl
    FROM dbo.CartItems ci
    JOIN dbo.Items i ON i.Id = ci.ItemId
    JOIN dbo.Carts c ON c.Id = ci.CartId
    WHERE ci.CartId = @CartId AND c.IsActive = 1
    ORDER BY ci.Id;
END
GO

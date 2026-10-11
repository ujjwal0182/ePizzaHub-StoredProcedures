IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'usp_Cart_RemoveItem')
DROP PROCEDURE usp_Cart_RemoveItem
GO

CREATE OR ALTER PROCEDURE dbo.usp_Cart_RemoveItem
    @CartId UNIQUEIDENTIFIER,
    @ItemId INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE ci
    FROM dbo.CartItems ci
    JOIN dbo.Carts c ON c.Id = ci.CartId
    WHERE ci.CartId = @CartId AND ci.ItemId = @ItemId AND c.IsActive = 1;

    SELECT @@ROWCOUNT AS RowsDeleted;
END
GO
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'usp_Cart_UpdateItemQuantity')
DROP PROCEDURE usp_Cart_UpdateItemQuantity
GO

CREATE OR ALTER PROCEDURE dbo.usp_Cart_UpdateItemQuantity
    @CartId   UNIQUEIDENTIFIER,
    @ItemId   INT,
    @Quantity INT
AS
BEGIN
    SET NOCOUNT ON;

    IF @Quantity <= 0
        THROW 50010, 'Quantity must be greater than zero. Use remove to delete the item.', 1;

    UPDATE ci
    SET ci.Quantity = @Quantity
    FROM dbo.CartItems ci
    JOIN dbo.Carts c ON c.Id = ci.CartId
    WHERE ci.CartId = @CartId AND ci.ItemId = @ItemId AND c.IsActive = 1;

    IF @@ROWCOUNT = 0
        THROW 50014, 'Cart item not found.', 1;
END
GO

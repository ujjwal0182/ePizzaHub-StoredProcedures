IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'usp_Cart_UpdateUser')
DROP PROCEDURE usp_Cart_UpdateUser
GO

CREATE OR ALTER PROCEDURE dbo.usp_Cart_UpdateUser
    @CartId UNIQUEIDENTIFIER,
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.Carts
    SET UserId = @UserId
    WHERE Id = @CartId AND IsActive = 1;

    IF @@ROWCOUNT = 0
        THROW 50013, 'Cart not found.', 1;
END
GO
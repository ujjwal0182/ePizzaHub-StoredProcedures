IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'usp_Cart_GetItemCount')
DROP PROCEDURE usp_Cart_GetItemCount
GO

CREATE OR ALTER PROCEDURE dbo.usp_Cart_GetItemCount
    @CartId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    SELECT COUNT(*) AS ItemCount
    FROM dbo.CartItems
    WHERE CartId = @CartId;
END
GO


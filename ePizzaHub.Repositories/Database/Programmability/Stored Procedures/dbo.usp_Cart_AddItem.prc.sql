IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'usp_Cart_AddItem')
DROP PROCEDURE usp_Cart_AddItem
GO

CREATE OR ALTER PROCEDURE dbo.usp_Cart_AddItem
    @CartId   UNIQUEIDENTIFIER,
    @UserId   INT,
    @ItemId   INT,
    @Quantity INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF @Quantity <= 0
        THROW 50010, 'Quantity must be greater than zero.', 1;

    -- Price client se nahi, DB se
    DECLARE @UnitPrice DECIMAL(18,2);
    SELECT @UnitPrice = UnitPrice FROM dbo.Items WHERE Id = @ItemId;

    IF @UnitPrice IS NULL
        THROW 50011, 'Item not found.', 1;

    DECLARE @IsActive BIT;

    BEGIN TRAN;

        -- Cart header: get-or-create
        SELECT @IsActive = IsActive
        FROM dbo.Carts WITH (UPDLOCK, HOLDLOCK)
        WHERE Id = @CartId;

        IF @@ROWCOUNT = 0
            INSERT dbo.Carts (Id, UserId, CreatedDate, IsActive)
            VALUES (@CartId, @UserId, SYSUTCDATETIME(), 1);
        ELSE IF @IsActive = 0
            THROW 50012, 'Cart is no longer active.', 1;

        -- Cart item: upsert
        UPDATE dbo.CartItems WITH (UPDLOCK, HOLDLOCK)
        SET Quantity = Quantity + @Quantity
        WHERE CartId = @CartId AND ItemId = @ItemId;

        IF @@ROWCOUNT = 0
            INSERT dbo.CartItems (CartId, ItemId, UnitPrice, Quantity)
            VALUES (@CartId, @ItemId, @UnitPrice, @Quantity);

    COMMIT;
END
GO

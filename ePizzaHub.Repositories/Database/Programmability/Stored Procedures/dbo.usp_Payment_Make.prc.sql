IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'usp_Payment_Make')
DROP PROCEDURE usp_Payment_Make
GO

CREATE OR ALTER PROCEDURE dbo.usp_Payment_Make
    @PaymentId     NVARCHAR(450),      -- Razorpay payment id (PaymentDetails PK)
    @TransactionId NVARCHAR(200),
    @OrderId       NVARCHAR(450),      -- Razorpay order id (Orders PK)
    @CartId        UNIQUEIDENTIFIER,
    @UserId        INT,
    @Email         NVARCHAR(256),
    @Currency      NVARCHAR(10),
    @Status        NVARCHAR(50),
    @PaidAmount    DECIMAL(18,2),
    @TaxRate       DECIMAL(5,4),       -- C# config se, e.g. 0.0500
    @Street        NVARCHAR(500),
    @City          NVARCHAR(200),
    @Locality      NVARCHAR(200),
    @ZipCode       NVARCHAR(20),
    @PhoneNumber   NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    -- Idempotency: Razorpay callback retry ho to duplicate order na bane
    IF EXISTS (SELECT 1 FROM dbo.PaymentDetails WHERE Id = @PaymentId)
    BEGIN
        SELECT Id AS OrderId FROM dbo.Orders WHERE PaymentId = @PaymentId;
        RETURN;
    END

    DECLARE @Total DECIMAL(18,2), @Tax DECIMAL(18,2), @Grand DECIMAL(18,2),
            @Now DATETIME2(7) = SYSUTCDATETIME();

    BEGIN TRAN;

        -- Cart lock: isi user ka, active hona chahiye
        IF NOT EXISTS (SELECT 1 FROM dbo.Carts WITH (UPDLOCK, HOLDLOCK)
                       WHERE Id = @CartId AND UserId = @UserId AND IsActive = 1)
            THROW 50020, 'Active cart not found for this user.', 1;

        IF NOT EXISTS (SELECT 1 FROM dbo.CartItems WHERE CartId = @CartId)
            THROW 50021, 'Cart is empty.', 1;

        -- Server-side totals, latest price se
        SELECT @Total = SUM(i.UnitPrice * ci.Quantity)
        FROM dbo.CartItems ci
        JOIN dbo.Items i ON i.Id = ci.ItemId
        WHERE ci.CartId = @CartId;

        SET @Tax   = ROUND(@Total * @TaxRate, 2);
        SET @Grand = @Total + @Tax;

        -- Razorpay amount rupee me round hota hai, isliye 1 rupee tolerance
        IF ABS(@Grand - @PaidAmount) >= 0.01
            THROW 50022, 'Paid amount does not match cart total.', 1;

        INSERT dbo.PaymentDetails
            (Id, TransactionId, Tax, Currency, Total, Email, Status,
             CartId, GrandTotal, CreatedDate, UserId)
        VALUES
            (@PaymentId, @TransactionId, @Tax, @Currency, @Total, @Email, @Status,
             @CartId, @Grand, @Now, @UserId);

        INSERT dbo.Orders
            (Id, PaymentId, Street, ZipCode, City, Locality, PhoneNumber, CreatedDate, UserId)
        VALUES
            (@OrderId, @PaymentId, @Street, @ZipCode, @City, @Locality, @PhoneNumber, @Now, @UserId);

        INSERT dbo.OrderItems (ItemId, UnitPrice, Quantity, Total, OrderId)
        SELECT ci.ItemId, i.UnitPrice, ci.Quantity, i.UnitPrice * ci.Quantity, @OrderId
        FROM dbo.CartItems ci
        JOIN dbo.Items i ON i.Id = ci.ItemId
        WHERE ci.CartId = @CartId;

        -- Cart band (purane code me ye missing tha)
        UPDATE dbo.Carts SET IsActive = 0 WHERE Id = @CartId;

    COMMIT;

    SELECT @OrderId AS OrderId;
END
GO
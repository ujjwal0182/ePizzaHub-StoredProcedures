DECLARE @uid INT = (SELECT Id FROM dbo.Users WHERE Email = N'user@gmail.com');
DECLARE @cart UNIQUEIDENTIFIER = NEWID();

EXEC dbo.usp_Cart_AddItem @cart, @uid, 1, 2; --cartid,userid,itemid,quantity
EXEC dbo.usp_Cart_AddItem @cart, @uid, 2, 1; --cartid,userid,itemid,quantity
EXEC dbo.usp_Cart_GetDetails @cart;     -- Total note kar lo, GrandTotal = Total * 1.05 --997.00

-- 1) Wrong amount: 50022 aana chahiye
EXEC dbo.usp_Payment_Make N'pay_T1', N'txn_1', N'order_T1', '8128D7E4-3542-4336-B8FE-C1761D87D2C4', 2,
     N'user@gmail.com', N'INR', N'succeeded', 1.00, 0.05,
     N'MG Road', N'Mohali', N'Phase 7', N'160055', N'9999999999';

-- 2) Sahi amount (apna GrandTotal daalo, e.g. 525.00)
EXEC dbo.usp_Payment_Make N'pay_T1', N'txn_1', N'order_T1', '8128D7E4-3542-4336-B8FE-C1761D87D2C4', 2,
     N'user@gmail.com', N'INR', N'succeeded', 1046.85, 0.05,
     N'MG Road', N'Mohali', N'Phase 7', N'160055', N'9999999999';

--DECLARE @Total DECIMAL(18,2), @Tax DECIMAL(18,2), @Grand DECIMAL(18,2)
--DECLARE @PaidAmount DECIMAL(18,2), @TaxRate  DECIMAL(5,4) 
--SELECT @Total = SUM(i.UnitPrice * ci.Quantity)
--        FROM dbo.CartItems ci
--        JOIN dbo.Items i ON i.Id = ci.ItemId
--        WHERE ci.CartId = '8128D7E4-3542-4336-B8FE-C1761D87D2C4';
--SET @TaxRate = 0.05
--SET @Tax   = ROUND(@Total * @TaxRate, 2);
--SET @Grand = @Total + @Tax;
--SET @PaidAmount = 1046.85

--SELECT @Total
--SELECT @Tax
--SELECT @Grand
--SELECT ABS(@Grand - @PaidAmount)

-- 3) Same call dobara: duplicate nahi, wahi OrderId return hona chahiye (idempotent)
EXEC dbo.usp_Payment_Make N'pay_T1', N'txn_1', N'order_T1', '8128D7E4-3542-4336-B8FE-C1761D87D2C4', 2,
     N'user@gmail.com', N'INR', N'succeeded', 1046.85, 0.05,
     N'MG Road', N'Mohali', N'Phase 7', N'160055', N'9999999999';

-- 4) Verify
SELECT IsActive FROM dbo.Carts WHERE Id = '8128D7E4-3542-4336-B8FE-C1761D87D2C4';          -- 0
SELECT COUNT(*) FROM dbo.Orders WHERE PaymentId = N'pay_T1';  -- 1
EXEC dbo.usp_Cart_GetDetails '8128D7E4-3542-4336-B8FE-C1761D87D2C4';                       -- khali
EXEC dbo.usp_Order_GetByUser 2;

-- 5) Rollback proof: ek aur cart, bad data se fail karwao
DECLARE @cart2 UNIQUEIDENTIFIER = NEWID();
EXEC dbo.usp_Cart_AddItem @cart2, 2, 1, 1; --cartid,userid,itemid,quantity

EXEC dbo.usp_Payment_Make N'pay_T2', N'txn_2', N'order_T1', '14C8BA2E-169C-42D7-ABBC-87DB973C68FF', 2,   -- order_T1 duplicate PK!
     N'user@gmail.com', N'INR', N'succeeded', 313.9500, 0.05,
     N'a', N'b', N'c', N'd', N'e';

SELECT SUM(i.UnitPrice * ci.Quantity) * 1.05 AS GrandTotal
FROM dbo.CartItems ci JOIN dbo.Items i ON i.Id = ci.ItemId
WHERE ci.CartId = '14C8BA2E-169C-42D7-ABBC-87DB973C68FF';


SELECT COUNT(*) FROM dbo.PaymentDetails WHERE Id = N'pay_T2'; -- 0 (rollback hua)

SELECT IsActive FROM dbo.Carts WHERE Id = '14C8BA2E-169C-42D7-ABBC-87DB973C68FF';             -- 1
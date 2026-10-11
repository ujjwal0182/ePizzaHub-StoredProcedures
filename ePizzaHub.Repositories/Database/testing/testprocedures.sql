DECLARE @cart UNIQUEIDENTIFIER = NEWID();

EXEC dbo.usp_Cart_AddItem @cart, 0, 1, 2;      -- cart, userid, itemid, quantity
EXEC dbo.usp_Cart_AddItem '0D82D1C0-B18B-4F9A-BE9F-2C25E202062A', 0, 1, 1;      -- same item: qty 3 hona chahiye

DECLARE @cart UNIQUEIDENTIFIER = NEWID();
EXEC dbo.usp_Cart_AddItem @cart, 0, 2, 1;      -- second item

EXEC dbo.usp_Cart_GetDetails '0D82D1C0-B18B-4F9A-BE9F-2C25E202062A';            -- 2 result sets
EXEC dbo.usp_Cart_GetItemCount '0D82D1C0-B18B-4F9A-BE9F-2C25E202062A';          -- 2

EXEC dbo.usp_Cart_GetDetails '32426546-2A0E-4335-9AB2-06E11F6EB807';            -- 2 result sets
EXEC dbo.usp_Cart_GetItemCount '32426546-2A0E-4335-9AB2-06E11F6EB807';          -- 2




EXEC dbo.usp_Cart_UpdateItemQuantity '0D82D1C0-B18B-4F9A-BE9F-2C25E202062A', 1, 5;
EXEC dbo.usp_Cart_RemoveItem '32426546-2A0E-4335-9AB2-06E11F6EB807', 2;
EXEC dbo.usp_Cart_UpdateUser '32426546-2A0E-4335-9AB2-06E11F6EB807', 2;

-- Negative tests (har ek error dena chahiye)
EXEC dbo.usp_Cart_AddItem '32426546-2A0E-4335-9AB2-06E11F6EB807', 0, 999, 1;    -- 50011

EXEC dbo.usp_Cart_AddItem '32426546-2A0E-4335-9AB2-06E11F6EB807', 0, 1, 0;      -- 50010

EXEC dbo.usp_Cart_UpdateItemQuantity '32426546-2A0E-4335-9AB2-06E11F6EB807', 777, 1;  -- 50014



-- Inactive cart test
UPDATE dbo.Carts SET IsActive = 0 WHERE Id = '32426546-2A0E-4335-9AB2-06E11F6EB807';

EXEC dbo.usp_Cart_AddItem '32426546-2A0E-4335-9AB2-06E11F6EB807', 0, 1, 1;      -- 50012

EXEC dbo.usp_Cart_GetDetails '32426546-2A0E-4335-9AB2-06E11F6EB807';            -- dono result sets khali

select * from Carts
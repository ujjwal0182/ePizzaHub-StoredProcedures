select * from sys.procedures

EXEC dbo.usp_User_GetByEmail @Email = N'admin@gmail.com';

DECLARE @id INT;
EXEC dbo.usp_User_Create N'Test3', N'test3@x.com', N'hash', N'9999999999', @id OUTPUT;
SELECT @id;

EXEC dbo.usp_User_Create N'Test2', N'test3@x.com', N'hash', N'9999999999', @id OUTPUT; -- 50001 error aana chahiye

select * from Users
select * from UserRoles


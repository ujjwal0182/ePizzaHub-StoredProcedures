IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'usp_User_Create')
DROP PROCEDURE usp_User_Create
GO

CREATE OR ALTER PROCEDURE dbo.usp_User_Create
    @Name NVARCHAR(200), 
	@Email NVARCHAR(256),
    @PasswordHash NVARCHAR(200), 
	@PhoneNumber NVARCHAR(20),
    @NewUserId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF EXISTS (SELECT 1 FROM dbo.Users WHERE Email = @Email)
    BEGIN
        ;THROW 50001, 'Email already registered.', 1;
    END

    BEGIN TRAN;
        INSERT dbo.Users (Name, Email, Password, PhoneNumber, EmailConfirmed, CreatedDate)
        VALUES (@Name, @Email, @PasswordHash, @PhoneNumber, 0, SYSUTCDATETIME());

        SET @NewUserId = SCOPE_IDENTITY();

        INSERT dbo.UserRoles (UserId, RoleId)
        SELECT @NewUserId, Id FROM dbo.Roles WHERE Name = N'User';
    COMMIT;
END
GO
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'usp_User_GetByEmail')
DROP PROCEDURE usp_User_GetByEmail
GO

CREATE OR ALTER PROCEDURE dbo.usp_User_GetByEmail
    @Email NVARCHAR(256)
AS
BEGIN
    SET NOCOUNT ON;

    -- Result set 1: user
    SELECT Id, Name, Email, Password, PhoneNumber
    FROM dbo.Users
    WHERE Email = @Email;

    -- Result set 2: roles
    SELECT r.Name
    FROM dbo.Roles r
    JOIN dbo.UserRoles ur ON ur.RoleId = r.Id
    JOIN dbo.Users u      ON u.Id = ur.UserId
    WHERE u.Email = @Email;
END
GO
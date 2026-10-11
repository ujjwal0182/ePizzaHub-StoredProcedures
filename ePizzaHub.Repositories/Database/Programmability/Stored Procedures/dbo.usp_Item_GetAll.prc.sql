IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'usp_Item_GetAll')
DROP PROCEDURE usp_Item_GetAll
GO

CREATE OR ALTER PROCEDURE dbo.usp_Item_GetAll
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Name, Description, UnitPrice, ImageUrl
    FROM dbo.Items
    ORDER BY Name;
END
GO


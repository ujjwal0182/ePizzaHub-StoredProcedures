IF NOT EXISTS(SELECT * FROM sys.databases where name = 'ePizzaHub_SP')
BEGIN
	CREATE DATABASE ePizzaHub_SP;
END
GO

USE [ePizzaHub_SP]
GO

/****** Object:  Table [dbo].[CartItems] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP TABLE IF EXISTS [CartItems]

CREATE TABLE [dbo].[CartItems](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CartId] [uniqueidentifier] NOT NULL, --A uniqueidentifier in SQL Server is a 16-byte GUID
	[ItemId] [int] NOT NULL,
	[UnitPrice] [decimal](18, 2) NOT NULL,
	[Quantity] [int] NOT NULL,
 CONSTRAINT [PK_CartItems] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Carts] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP TABLE IF EXISTS [Carts]

CREATE TABLE [dbo].[Carts](
	[Id] [uniqueidentifier] NOT NULL,
	[UserId] [int] NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Carts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Categories] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP TABLE IF EXISTS [Categories]

CREATE TABLE [dbo].[Categories](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Items] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP TABLE IF EXISTS [Items]

CREATE TABLE [dbo].[Items](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[UnitPrice] [decimal](18, 2) NOT NULL,
	[ImageUrl] [nvarchar](max) NOT NULL,
	[CategoryId] [int] NOT NULL,
	[ItemTypeId] [int] NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Items] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

/****** Object:  Table [dbo].[ItemTypes] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP TABLE IF EXISTS [ItemTypes]

CREATE TABLE [dbo].[ItemTypes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_ItemTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Logs] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP TABLE IF EXISTS [Logs]

CREATE TABLE [dbo].[Logs](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Message] [nvarchar](max) NULL,
	[MessageTemplate] [nvarchar](max) NULL,
	[Level] [nvarchar](max) NULL,
	[TimeStamp] [datetime] NULL,
	[Exception] [nvarchar](max) NULL,
	[Properties] [nvarchar](max) NULL,
 CONSTRAINT [PK_Logs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

/****** Object:  Table [dbo].[OrderItems] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP TABLE IF EXISTS [OrderItems]

CREATE TABLE [dbo].[OrderItems](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ItemId] [int] NOT NULL,
	[UnitPrice] [decimal](18, 2) NOT NULL,
	[Quantity] [int] NOT NULL,
	[Total] [decimal](18, 2) NOT NULL,
	[OrderId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_OrderItems] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Orders] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP TABLE IF EXISTS [Orders]

CREATE TABLE [dbo].[Orders](
	[Id] [nvarchar](450) NOT NULL,
	[PaymentId] [nvarchar](max) NOT NULL,
	[Street] [nvarchar](max) NOT NULL,
	[ZipCode] [nvarchar](max) NOT NULL,
	[City] [nvarchar](max) NOT NULL,
	[Locality] [nvarchar](max) NOT NULL,
	[PhoneNumber] [nvarchar](max) NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[UserId] [int] NOT NULL,
 CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

/****** Object:  Table [dbo].[PaymentDetails] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP TABLE IF EXISTS [PaymentDetails]

CREATE TABLE [dbo].[PaymentDetails](
	[Id] [nvarchar](450) NOT NULL,
	[TransactionId] [nvarchar](max) NOT NULL,
	[Tax] [decimal](18, 2) NOT NULL,
	[Currency] [nvarchar](max) NOT NULL,
	[Total] [decimal](18, 2) NOT NULL,
	[Email] [nvarchar](max) NOT NULL,
	[Status] [nvarchar](max) NOT NULL,
	[CartId] [uniqueidentifier] NOT NULL,
	[GrandTotal] [decimal](18, 2) NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[UserId] [int] NOT NULL,
 CONSTRAINT [PK_PaymentDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Roles] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP TABLE IF EXISTS [Roles]

CREATE TABLE [dbo].[Roles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

/****** Object:  Table [dbo].[UserRoles] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP TABLE IF EXISTS [UserRoles]

CREATE TABLE [dbo].[UserRoles](
	[UserId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
 CONSTRAINT [PK_UserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Users] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP TABLE IF EXISTS [Users]

CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Email] [nvarchar](max) NOT NULL,
	[Password] [nvarchar](max) NOT NULL,
	[PhoneNumber] [nvarchar](max) NOT NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

-- CartItems.CartId → Carts.Id
ALTER TABLE [dbo].[CartItems]  WITH CHECK ADD  CONSTRAINT [FK_CartItems_Carts_CartId] FOREIGN KEY([CartId])
REFERENCES [dbo].[Carts] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[CartItems] CHECK CONSTRAINT [FK_CartItems_Carts_CartId]
GO

-- CartItems.ItemId → Items.Id
ALTER TABLE [dbo].[CartItems]  WITH CHECK ADD  CONSTRAINT [FK_CartItems_Items] FOREIGN KEY([ItemId])
REFERENCES [dbo].[Items] ([Id])
GO

ALTER TABLE [dbo].[CartItems] CHECK CONSTRAINT [FK_CartItems_Items]
GO

-- Items.CategoryId → Categories.Id
ALTER TABLE [dbo].[Items]  WITH CHECK ADD  CONSTRAINT [FK_Items_Categories_CategoryId] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Categories] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Items] CHECK CONSTRAINT [FK_Items_Categories_CategoryId]
GO

-- Items.ItemTypeId → ItemTypes.Id
ALTER TABLE [dbo].[Items]  WITH CHECK ADD  CONSTRAINT [FK_Items_ItemTypes_ItemTypeId] FOREIGN KEY([ItemTypeId])
REFERENCES [dbo].[ItemTypes] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Items] CHECK CONSTRAINT [FK_Items_ItemTypes_ItemTypeId]
GO

-- OrderItems.ItemId → Items.Id
ALTER TABLE [dbo].[OrderItems]  WITH CHECK ADD  CONSTRAINT [FK_OrderItems_Items] FOREIGN KEY([ItemId])
REFERENCES [dbo].[Items] ([Id])
GO

ALTER TABLE [dbo].[OrderItems] CHECK CONSTRAINT [FK_OrderItems_Items]
GO

-- OrderItems.OrderId → Orders.Id
ALTER TABLE [dbo].[OrderItems]  WITH CHECK ADD  CONSTRAINT [FK_OrderItems_Orders_OrderId] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Orders] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[OrderItems] CHECK CONSTRAINT [FK_OrderItems_Orders_OrderId]
GO

-- Orders.UserId → Users.Id
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_Users_UserId]
GO

-- PaymentDetails.CartId → Carts.Id
ALTER TABLE [dbo].[PaymentDetails]  WITH CHECK ADD  CONSTRAINT [FK_PaymentDetails_Carts] FOREIGN KEY([CartId])
REFERENCES [dbo].[Carts] ([Id])
GO

ALTER TABLE [dbo].[PaymentDetails] CHECK CONSTRAINT [FK_PaymentDetails_Carts]
GO

-- PaymentDetails.UserId → Users.Id
ALTER TABLE [dbo].[PaymentDetails]  WITH CHECK ADD  CONSTRAINT [FK_PaymentDetails_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[PaymentDetails] CHECK CONSTRAINT [FK_PaymentDetails_Users_UserId]
GO

-- UserRoles.RoleId → Roles.Id
ALTER TABLE [dbo].[UserRoles]  WITH CHECK ADD  CONSTRAINT [FK_UserRoles_Roles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[UserRoles] CHECK CONSTRAINT [FK_UserRoles_Roles_RoleId]
GO

-- UserRoles.UserId → Users.Id
ALTER TABLE [dbo].[UserRoles]  WITH CHECK ADD  CONSTRAINT [FK_UserRoles_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[UserRoles] CHECK CONSTRAINT [FK_UserRoles_Users_UserId]
GO


/**** ADD UNIQUE INDEX ****/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'idx_uk_users_email' AND object_id = OBJECT_ID('Users'))
BEGIN
	ALTER TABLE Users ALTER COLUMN Email NVARCHAR(256);
	CREATE UNIQUE INDEX idx_uk_users_email ON dbo.Users (Email);
END
GO


IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Items_CategoryId' AND object_id = OBJECT_ID('Items'))
BEGIN
	CREATE NONCLUSTERED INDEX IX_Items_CategoryId ON Items (CategoryId)
END

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_CartItems_CartId' AND object_id = OBJECT_ID('CartItems'))
BEGIN
	CREATE NONCLUSTERED INDEX IX_CartItems_CartId ON CartItems (CartId)
END

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Orders_UserId' AND object_id = OBJECT_ID('Orders'))
BEGIN
	CREATE NONCLUSTERED INDEX IX_Orders_UserId ON Orders (UserId)
END


IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'UX_CartItems_CartId_ItemId' AND object_id = OBJECT_ID('CartItems'))
BEGIN
	CREATE UNIQUE NONCLUSTERED INDEX UX_CartItems_CartId_ItemId 
	ON CartItems (CartId, ItemId);
END


--ALTER THE length to 450 so that joins can easily run
ALTER TABLE dbo.Orders ALTER COLUMN PaymentId NVARCHAR(450) NOT NULL;

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Orders_PaymentId')
BEGIN
	CREATE INDEX IX_Orders_PaymentId ON dbo.Orders(PaymentId);
END

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Orders_PaymentDetails' AND parent_object_id = object_id('Orders'))
BEGIN
	ALTER TABLE dbo.Orders ADD CONSTRAINT FK_Orders_PaymentDetails
		FOREIGN KEY (PaymentId) REFERENCES dbo.PaymentDetails(Id);
END



--ROUGH
SELECT * FROM Orders
SELECT * FROM PaymentDetails

SELECT * FROM sys.foreign_keys where parent_object_id = OBJECT_ID('Orders')
SELECT * FROM sys.foreign_keys where parent_object_id = OBJECT_ID('PaymentDetails')

















--Categories
--    │
--    └──< Items >── ItemTypes
--          │
--          ├──< CartItems >── Carts
--          │
--          └──< OrderItems >── Orders
--                                │
--                                └──< Users

--Users
--  ├──< Orders
--  ├──< PaymentDetails >── Carts
--  └──< UserRoles >── Roles
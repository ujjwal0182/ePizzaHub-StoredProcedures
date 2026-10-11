GO
SET IDENTITY_INSERT [dbo].[Roles] ON
GO
INSERT [dbo].[Roles] ([Id], [Name]) VALUES (1, N'Admin')
GO
INSERT [dbo].[Roles] ([Id], [Name]) VALUES (2, N'User')
GO
SET IDENTITY_INSERT [dbo].[Roles] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON
GO
INSERT [dbo].[Users] ([Id], [Name], [Email], [Password], [PhoneNumber], [EmailConfirmed], [CreatedDate]) VALUES (1, N'Admin', N'admin@gmail.com', N'$2a$11$NulP7XYlUOjMELsrj/me0uO/1OIQiHnMl.DVUk7LgB5SqjyWSas5K', N'9876543210', 0, CAST(N'2021-12-21T11:03:11.457' AS DateTime))
GO
INSERT [dbo].[Users] ([Id], [Name], [Email], [Password], [PhoneNumber], [EmailConfirmed], [CreatedDate]) VALUES (2, N'User', N'user@gmail.com', N'$2a$11$oNn03spA.XrRD8shVW9Z2.72X6ljCU/S6fjOZOTybNVFLtEr6Kb5y', N'9876543210', 0, CAST(N'2021-12-21T11:05:19.160' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
INSERT [dbo].[UserRoles] ([UserId], [RoleId]) VALUES (1, 1)
GO
INSERT [dbo].[UserRoles] ([UserId], [RoleId]) VALUES (2, 2)
GO

SET IDENTITY_INSERT [dbo].[Categories] ON
GO
INSERT [dbo].[Categories] ([Id], [Name]) VALUES (1, N'Pizza')
GO
INSERT [dbo].[Categories] ([Id], [Name]) VALUES (2, N'Dessert')
GO
INSERT [dbo].[Categories] ([Id], [Name]) VALUES (3, N'Beverages')
GO
SET IDENTITY_INSERT [dbo].[Categories] OFF
GO
SET IDENTITY_INSERT [dbo].[ItemTypes] ON
GO
INSERT [dbo].[ItemTypes] ([Id], [Name]) VALUES (1, N'Veg')
GO
INSERT [dbo].[ItemTypes] ([Id], [Name]) VALUES (2, N'NonVeg')
GO
SET IDENTITY_INSERT [dbo].[ItemTypes] OFF
GO
SET IDENTITY_INSERT [dbo].[Items] ON
GO
INSERT [dbo].[Items] ([Id], [Name], [Description], [UnitPrice], [ImageUrl], [CategoryId], [ItemTypeId], CreatedDate) VALUES (1, N'Farm House', N'Delightful combination of onion, capsicum, tomato & grilled mushroom', CAST(299.00 AS Decimal(18, 2)), N'/images/farmhouse_pizza.webp', 1, 1, GETDATE())
GO
INSERT [dbo].[Items] ([Id], [Name], [Description], [UnitPrice], [ImageUrl], [CategoryId], [ItemTypeId], CreatedDate) VALUES (2, N'Pepe Paneer', N'Flavorful trio of juicy paneer, crisp capsicum with spicy red paprika', CAST(399.00 AS Decimal(18, 2)), N'/images/paneer_pizza.webp', 1, 1, GETDATE())
GO
INSERT [dbo].[Items] ([Id], [Name], [Description], [UnitPrice], [ImageUrl], [CategoryId], [ItemTypeId], CreatedDate) VALUES (3, N'Veggie Paradise', N'The awesome foursome! Golden corn, black olives, capsicum, red paprika', CAST(499.00 AS Decimal(18, 2)), N'/images/veggie_pizza.webp', 1, 1, GETDATE())
GO
INSERT [dbo].[Items] ([Id], [Name], [Description], [UnitPrice], [ImageUrl], [CategoryId], [ItemTypeId], CreatedDate) VALUES (4, N'Cheese & Corn', N'A delectable combination of sweet & juicy golden corn', CAST(399.00 AS Decimal(18, 2)), N'/images/cheese_n_corn.webp', 1, 1, GETDATE())
GO
INSERT [dbo].[Items] ([Id], [Name], [Description], [UnitPrice], [ImageUrl], [CategoryId], [ItemTypeId], CreatedDate) VALUES (5, N'Non Veg Delight', N'Chicken sausage, pepper barbecue chicken & peri-peri chicken in a fresh pan crust', CAST(499.00 AS Decimal(18, 2)), N'/images/noveg_delight.webp', 1, 1, GETDATE())
GO
INSERT [dbo].[Items] ([Id], [Name], [Description], [UnitPrice], [ImageUrl], [CategoryId], [ItemTypeId], CreatedDate) VALUES (6, N'Non Veg Supreme', N'Loaded with a delicious creamy tomato pasta topping, pepper chicken, capsicum', CAST(599.00 AS Decimal(18, 2)), N'/images/nonveg_supreme.webp', 1, 1, GETDATE())
GO
INSERT [dbo].[Items] ([Id], [Name], [Description], [UnitPrice], [ImageUrl], [CategoryId], [ItemTypeId], CreatedDate) VALUES (7, N'Choco Lava Cake', N'Chocolate lovers delight! Indulgent, gooey molten lava inside chocolate cake', CAST(99.00 AS Decimal(18, 2)), N'/images/choco_lava.webp', 2, 1, GETDATE())
GO
INSERT [dbo].[Items] ([Id], [Name], [Description], [UnitPrice], [ImageUrl], [CategoryId], [ItemTypeId], CreatedDate) VALUES (8, N'Butterscotch Cake', N'Sweet temptation! Butterscotch flavored mousse', CAST(99.00 AS Decimal(18, 2)), N'/images/butter_scotch.webp', 2, 1, GETDATE())
GO
INSERT [dbo].[Items] ([Id], [Name], [Description], [UnitPrice], [ImageUrl], [CategoryId], [ItemTypeId], CreatedDate) VALUES (9, N'Red Lava Cake', N'NULLA truly indulgent experience with sweet and rich red cake', CAST(99.00 AS Decimal(18, 2)), N'/images/red_cake.webp', 2, 1, GETDATE())
GO
INSERT [dbo].[Items] ([Id], [Name], [Description], [UnitPrice], [ImageUrl], [CategoryId], [ItemTypeId], CreatedDate) VALUES (10, N'Pepsi', N'Sparkling and Refreshing Beverage', CAST(99.00 AS Decimal(18, 2)), N'/images/pepsi.webp', 3, 1, GETDATE())
GO
INSERT [dbo].[Items] ([Id], [Name], [Description], [UnitPrice], [ImageUrl], [CategoryId], [ItemTypeId], CreatedDate) VALUES (11, N'Mirinda', N'Delicious Orange Flavoured beverage', CAST(99.00 AS Decimal(18, 2)), N'/images/mirinda.webp', 3, 1, GETDATE())
GO
INSERT [dbo].[Items] ([Id], [Name], [Description], [UnitPrice], [ImageUrl], [CategoryId], [ItemTypeId], CreatedDate) VALUES (12, N'7 Up', N'Refreshing clear drink with lemon flavor', CAST(99.00 AS Decimal(18, 2)), N'/images/7up.webp', 3, 1, GETDATE())
GO
SET IDENTITY_INSERT [dbo].[Items] OFF
GO
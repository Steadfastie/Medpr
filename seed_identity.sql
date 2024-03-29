CREATE DATABASE [ITA-Medpr-Identity];
GO
USE [ITA-Medpr-Identity]
GO
/****** Object:  Table [AspNetRoleClaims]    Script Date: 1/21/2023 7:19:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [AspNetRoleClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [uniqueidentifier] NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [AspNetRoles]    Script Date: 1/21/2023 7:19:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [AspNetRoles](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](256) NULL,
	[NormalizedName] [nvarchar](256) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [AspNetUserClaims]    Script Date: 1/21/2023 7:19:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [AspNetUserLogins]    Script Date: 1/21/2023 7:19:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [AspNetUserLogins](
	[LoginProvider] [nvarchar](450) NOT NULL,
	[ProviderKey] [nvarchar](450) NOT NULL,
	[ProviderDisplayName] [nvarchar](max) NULL,
	[UserId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [AspNetUserRoles]    Script Date: 1/21/2023 7:19:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [AspNetUserRoles](
	[UserId] [uniqueidentifier] NOT NULL,
	[RoleId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [AspNetUsers]    Script Date: 1/21/2023 7:19:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [AspNetUsers](
	[Id] [uniqueidentifier] NOT NULL,
	[UserName] [nvarchar](256) NULL,
	[NormalizedUserName] [nvarchar](256) NULL,
	[Email] [nvarchar](256) NULL,
	[NormalizedEmail] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEnd] [datetimeoffset](7) NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
 CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [AspNetUserTokens]    Script Date: 1/21/2023 7:19:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [AspNetUserTokens](
	[UserId] [uniqueidentifier] NOT NULL,
	[LoginProvider] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](450) NOT NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[LoginProvider] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20221009120756_InitialIdentity', N'6.0.8')
INSERT [__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20221009122405_AddIdentity', N'6.0.9')
GO
INSERT [AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'36237b44-3de0-45bd-0164-08daab777ee4', N'Default', N'DEFAULT', N'9402491d-ca8a-49bc-a44a-f332da8784f9')
INSERT [AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'3d4216ea-df5d-47e2-0165-08daab777ee4', N'Admin', N'ADMIN', N'0a352927-6058-4e28-b62a-89086ed5e9ba')
GO
INSERT [AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'bafef5d2-d072-4e59-7881-08daab90edc6', N'36237b44-3de0-45bd-0164-08daab777ee4')
INSERT [AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'f1e73fad-8b2d-4c7a-267b-08daac8f9f21', N'36237b44-3de0-45bd-0164-08daab777ee4')
INSERT [AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'5263faef-e3e1-4b5c-e52d-08daad131971', N'36237b44-3de0-45bd-0164-08daab777ee4')
INSERT [AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'6c351fe8-47a4-457f-0e77-08dab76bc3c4', N'36237b44-3de0-45bd-0164-08daab777ee4')
INSERT [AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'08253fcd-49e1-4829-ad1f-08dac731f8f1', N'36237b44-3de0-45bd-0164-08daab777ee4')
INSERT [AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'df30e013-ae24-4047-8b92-08dad7bd0005', N'36237b44-3de0-45bd-0164-08daab777ee4')
INSERT [AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'c15c1511-54d4-4a81-51ad-08dae1058e69', N'36237b44-3de0-45bd-0164-08daab777ee4')
INSERT [AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'dbd01bb4-69a9-4687-a8c6-08dae1c508f1', N'36237b44-3de0-45bd-0164-08daab777ee4')
INSERT [AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'e5e9a1e5-606c-4eae-0236-08dae1d07f21', N'36237b44-3de0-45bd-0164-08daab777ee4')
INSERT [AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'b7ab1335-9243-4399-a1a1-08daab78df95', N'3d4216ea-df5d-47e2-0165-08daab777ee4')
GO
INSERT [AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'b7ab1335-9243-4399-a1a1-08daab78df95', N'admin@admin.com', N'ADMIN@ADMIN.COM', NULL, NULL, 0, N'AQAAAAEAACcQAAAAEGGOunIvRJUDJI3icSLIu/1hpBI8aQmacUjzjDgH8pvl8C7vaBMbVm1JQ3f8uTho4g==', N'SKT3NZ2MNJDB7Y6NMHBYHG72OUFEXKS6', N'121f179d-fdb1-45f4-9cc2-5c41fa446aab', NULL, 0, 0, NULL, 1, 0)
INSERT [AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'bafef5d2-d072-4e59-7881-08daab90edc6', N'firstuser@gmail.com', N'FIRSTUSER@GMAIL.COM', NULL, NULL, 0, N'AQAAAAEAACcQAAAAEK5heD3DMoj9CslcGo3E5fcasPvib7ofyVIDaTWm09WqtLkgFKzBAFLKA9gEKxsQWg==', N'7PXNHZOIWE4PNPWTXYPOVPWE34KGWF73', N'c33b49e4-8f8a-4e6e-8669-d53f386875e2', NULL, 0, 0, NULL, 1, 0)
INSERT [AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'f1e73fad-8b2d-4c7a-267b-08daac8f9f21', N'seconduser@email.com', N'SECONDUSER@EMAIL.COM', N'seconduser@email.com', N'SECONDUSER@EMAIL.COM', 0, N'AQAAAAEAACcQAAAAEDxZbUEuG5j4VYaNqGXc5rmZGoUWq7ijGMcbq8SFFxpSHPdglN++LRCU/ZoN5k1nGg==', N'H6WP3SCON2PAHCIU6LMKCQZAQ5HTOS2X', N'fdd5973f-9d47-43cb-8d8a-c35ad298cf1a', NULL, 0, 0, NULL, 1, 0)
INSERT [AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'5263faef-e3e1-4b5c-e52d-08daad131971', N'thirduser@email.com', N'THIRDUSER@EMAIL.COM', NULL, NULL, 0, N'AQAAAAEAACcQAAAAEFWpP0YkwQjKJSmv8C8FjpWqLujKg09Q20IVp2wuCHhxWYIIDJg+wGY4qDO9t7NPKA==', N'VX442GUMN2MYY423OVVD4WTVZRHB4OPZ', N'a010a67d-ef38-493a-be8b-92fd3c3c0322', NULL, 0, 0, NULL, 1, 0)
INSERT [AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'6c351fe8-47a4-457f-0e77-08dab76bc3c4', N'fourthuser@email.com', N'FOURTHUSER@EMAIL.COM', N'fourthuser@email.com', N'FOURTHUSER@EMAIL.COM', 0, N'AQAAAAEAACcQAAAAEC4/M5VDhObpFzmKpBCxsWhw9zPQRSSoUshqMZGkPWHhg1yE4oE4AuQ/RN9MKwiBBg==', N'KQ4CAGKHKYK46FT7PXKIK6OP6O2WXFOT', N'8bd4e1ff-f9da-4681-9ad3-21ceea83119b', NULL, 0, 0, NULL, 1, 0)
INSERT [AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'08253fcd-49e1-4829-ad1f-08dac731f8f1', N'swagger@email.com', N'SWAGGER@EMAIL.COM', NULL, NULL, 0, N'AQAAAAEAACcQAAAAEI5g++b9jPTG6S/ocvIOrretr6jclSq6+3Yek14C451j6/20BJWDdcu9Timo/E3oag==', N'4MBR6H2YCMW5IP3VTGDPWIAD22PQFNDE', N'7a2bfbe4-c2dc-4414-a211-84f970f808e3', NULL, 0, 0, NULL, 1, 0)
INSERT [AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'df30e013-ae24-4047-8b92-08dad7bd0005', N'angular@gmail.com', N'ANGULAR@GMAIL.COM', NULL, NULL, 0, N'AQAAAAEAACcQAAAAEFDDRMIgCK2bMUM/T3PBjAooFchkxQXEBazwqh+mugmDigeu968rwqiQtyVJ2gEIhw==', N'WU2VF4OLZBQJYWWBAZP4GQ4BC4Y7W5UM', N'162c86cd-0775-4209-a86b-bb27a3885135', NULL, 0, 0, NULL, 1, 0)
INSERT [AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'c15c1511-54d4-4a81-51ad-08dae1058e69', N'angularsecond@gmail.com', N'ANGULARSECOND@GMAIL.COM', NULL, NULL, 0, N'AQAAAAEAACcQAAAAEHhVm78q/Whr0t0erHHFe4vgQ58wrnbS2j9o+QrrFSMztVlR5n2PnMNUjTDapKdEzQ==', N'4RPWHDNMD5VRP7TJTYT2HGCVP3ISOZIM', N'1c19cd32-1870-4a15-a26d-875415c7ea81', NULL, 0, 0, NULL, 1, 0)
INSERT [AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'dbd01bb4-69a9-4687-a8c6-08dae1c508f1', N'subject@gmail.com', N'SUBJECT@GMAIL.COM', NULL, NULL, 0, N'AQAAAAEAACcQAAAAEDIVD7c2gS9bG/2h00nwZ6xUmGUEhD11Uh+MufgPtMVAM6Rm5HYjfbGSomnMDuyIbw==', N'RBHVPM6GJAKZNSSTFJQPDHYIMEJEYMAA', N'738ab482-9f61-41e8-8b2b-8aa4977ff969', NULL, 0, 0, NULL, 1, 0)
INSERT [AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'e5e9a1e5-606c-4eae-0236-08dae1d07f21', N'sticker@gmail.com', N'STICKER@GMAIL.COM', NULL, NULL, 0, N'AQAAAAEAACcQAAAAENCdT7WCpyM5h/LiuFpTTCjhio7uwMbUpo42MV+cPl1slyp867yg8ivVUUieozJMbg==', N'RRNJ7PBE43FGSCWJQKKY6CIWO3ERDPQR', N'2c78e4a3-2a19-4ce1-9dc0-bcc5f36cb51f', NULL, 0, 0, NULL, 1, 0)
GO
ALTER TABLE [AspNetRoleClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [AspNetRoleClaims] CHECK CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId]
GO
ALTER TABLE [AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [AspNetUserClaims] CHECK CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId]
GO
ALTER TABLE [AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [AspNetUserLogins] CHECK CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId]
GO
ALTER TABLE [AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId]
GO
ALTER TABLE [AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId]
GO
ALTER TABLE [AspNetUserTokens]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [AspNetUserTokens] CHECK CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId]
GO

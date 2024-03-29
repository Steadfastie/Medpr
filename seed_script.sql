CREATE DATABASE [ITA-Medpr];
GO
USE [ITA-Medpr]
GO
/****** Object:  Table [Appointments]    Script Date: 1/21/2023 6:53:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Appointments](
	[Id] [uniqueidentifier] NOT NULL,
	[Date] [datetime2](7) NOT NULL,
	[Place] [nvarchar](max) NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[DoctorId] [uniqueidentifier] NOT NULL,
	[NotificationId] [nvarchar](max) NULL,
 CONSTRAINT [PK_Appointment] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [Doctors]    Script Date: 1/21/2023 6:53:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Doctors](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Experience] [int] NOT NULL,
 CONSTRAINT [PK_Doctor] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [Drugs]    Script Date: 1/21/2023 6:53:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Drugs](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[PharmGroup] [nvarchar](max) NOT NULL,
	[Price] [int] NOT NULL,
 CONSTRAINT [PK_Drug] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [Families]    Script Date: 1/21/2023 6:53:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Families](
	[Id] [uniqueidentifier] NOT NULL,
	[Surname] [nvarchar](max) NOT NULL,
	[Creator] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Family] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [FamilyMembers]    Script Date: 1/21/2023 6:53:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [FamilyMembers](
	[Id] [uniqueidentifier] NOT NULL,
	[IsAdmin] [bit] NOT NULL,
	[FamilyId] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_FamilyMember] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Prescriptions]    Script Date: 1/21/2023 6:53:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Prescriptions](
	[Id] [uniqueidentifier] NOT NULL,
	[Date] [datetime2](7) NOT NULL,
	[EndDate] [datetime2](7) NOT NULL,
	[Dose] [int] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[DrugId] [uniqueidentifier] NOT NULL,
	[DoctorId] [uniqueidentifier] NOT NULL,
	[NotificationId] [nvarchar](max) NULL,
 CONSTRAINT [PK_Prescription] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [Users]    Script Date: 1/21/2023 6:53:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Users](
	[Id] [uniqueidentifier] NOT NULL,
	[Login] [nvarchar](max) NOT NULL,
	[DateOfBirth] [datetime2](7) NULL,
	[FullName] [nvarchar](max) NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [Vaccinations]    Script Date: 1/21/2023 6:53:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Vaccinations](
	[Id] [uniqueidentifier] NOT NULL,
	[Date] [datetime2](7) NOT NULL,
	[DaysOfProtection] [int] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[VaccineId] [uniqueidentifier] NOT NULL,
	[NotificationId] [nvarchar](max) NULL,
 CONSTRAINT [PK_Vaccination] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [Vaccines]    Script Date: 1/21/2023 6:53:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Vaccines](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Reason] [nvarchar](max) NOT NULL,
	[Price] [int] NOT NULL,
 CONSTRAINT [PK_Vaccine] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [Appointments] ([Id], [Date], [Place], [UserId], [DoctorId], [NotificationId]) VALUES (N'a67a9fc5-c79e-4fc8-b2e8-1425bdbe3ac9', CAST(N'2022-12-21T00:00:00.0000000' AS DateTime2), N'Great Hospital', N'e5e9a1e5-606c-4eae-0236-08dae1d07f21', N'56e8ee78-8b18-48f9-a4ff-841622c36430', N'96')
INSERT [Appointments] ([Id], [Date], [Place], [UserId], [DoctorId], [NotificationId]) VALUES (N'd4460da6-f19c-4deb-99fd-1f434c81921b', CAST(N'2022-12-20T00:00:00.0000000' AS DateTime2), N'Sightseen', N'c15c1511-54d4-4a81-51ad-08dae1058e69', N'b97a37d8-13fb-4f71-9400-bfbb758a778a', N'85')
INSERT [Appointments] ([Id], [Date], [Place], [UserId], [DoctorId], [NotificationId]) VALUES (N'8da044fc-8f38-4472-b3ea-32221fd31443', CAST(N'2022-12-14T00:00:00.0000000' AS DateTime2), N'Closest hospital', N'dbd01bb4-69a9-4687-a8c6-08dae1c508f1', N'56e8ee78-8b18-48f9-a4ff-841622c36430', NULL)
INSERT [Appointments] ([Id], [Date], [Place], [UserId], [DoctorId], [NotificationId]) VALUES (N'a8282a84-a0ad-4d85-90e5-4391d167b88d', CAST(N'2022-12-30T21:00:00.0000000' AS DateTime2), N'Happy', N'df30e013-ae24-4047-8b92-08dad7bd0005', N'ab31d6be-3314-48a0-bbcd-d96e46cc0790', N'49')
INSERT [Appointments] ([Id], [Date], [Place], [UserId], [DoctorId], [NotificationId]) VALUES (N'0e3290dc-8e3f-4aff-9b4b-48fc7bbbeb2f', CAST(N'2022-12-15T00:00:00.0000000' AS DateTime2), N'Great Hospital', N'e5e9a1e5-606c-4eae-0236-08dae1d07f21', N'56e8ee78-8b18-48f9-a4ff-841622c36430', NULL)
INSERT [Appointments] ([Id], [Date], [Place], [UserId], [DoctorId], [NotificationId]) VALUES (N'0d93d604-ba05-40d7-8337-50372eeeb970', CAST(N'2022-10-13T00:00:00.0000000' AS DateTime2), N'Closest hospital', N'bafef5d2-d072-4e59-7881-08daab90edc6', N'269af575-f2d4-4796-8d32-c7a3c57928b4', NULL)
INSERT [Appointments] ([Id], [Date], [Place], [UserId], [DoctorId], [NotificationId]) VALUES (N'b58b6e50-2e67-4f07-bac0-6bf4e8d286f4', CAST(N'2022-10-29T00:00:00.0000000' AS DateTime2), N'Some place', N'6c351fe8-47a4-457f-0e77-08dab76bc3c4', N'b97a37d8-13fb-4f71-9400-bfbb758a778a', NULL)
INSERT [Appointments] ([Id], [Date], [Place], [UserId], [DoctorId], [NotificationId]) VALUES (N'eb8af274-7b2d-441e-bc55-867864b04fdf', CAST(N'2022-12-22T21:00:00.0000000' AS DateTime2), N'Comletely Happy', N'df30e013-ae24-4047-8b92-08dad7bd0005', N'c69c481a-4bed-430f-89b4-938478f06d7b', N'50')
INSERT [Appointments] ([Id], [Date], [Place], [UserId], [DoctorId], [NotificationId]) VALUES (N'a5a10eef-ee8d-48f6-87ff-89ddd2308e8c', CAST(N'2022-10-26T00:00:00.0000000' AS DateTime2), N'St.George Hospital', N'f1e73fad-8b2d-4c7a-267b-08daac8f9f21', N'ab31d6be-3314-48a0-bbcd-d96e46cc0790', NULL)
INSERT [Appointments] ([Id], [Date], [Place], [UserId], [DoctorId], [NotificationId]) VALUES (N'ac4e943c-c384-40c6-a14d-92173c718bd2', CAST(N'2022-12-21T00:00:00.0000000' AS DateTime2), N'Main hospital', N'dbd01bb4-69a9-4687-a8c6-08dae1c508f1', N'56e8ee78-8b18-48f9-a4ff-841622c36430', N'95')
INSERT [Appointments] ([Id], [Date], [Place], [UserId], [DoctorId], [NotificationId]) VALUES (N'85dc02ad-c7a7-4993-9e3e-d5d1c4afffd5', CAST(N'2022-10-05T00:00:00.0000000' AS DateTime2), N'St.Maria Hospital', N'5263faef-e3e1-4b5c-e52d-08daad131971', N'b97a37d8-13fb-4f71-9400-bfbb758a778a', NULL)
INSERT [Appointments] ([Id], [Date], [Place], [UserId], [DoctorId], [NotificationId]) VALUES (N'98970223-a7b7-4b6c-8fd8-f614686c438a', CAST(N'2022-12-13T21:00:00.0000000' AS DateTime2), N'St.Maria', N'df30e013-ae24-4047-8b92-08dad7bd0005', N'269af575-f2d4-4796-8d32-c7a3c57928b4', NULL)
GO
INSERT [Doctors] ([Id], [Name], [Experience]) VALUES (N'56e8ee78-8b18-48f9-a4ff-841622c36430', N'Dr.Exam', 22)
INSERT [Doctors] ([Id], [Name], [Experience]) VALUES (N'c69c481a-4bed-430f-89b4-938478f06d7b', N'Dr.Kiss', 42)
INSERT [Doctors] ([Id], [Name], [Experience]) VALUES (N'b97a37d8-13fb-4f71-9400-bfbb758a778a', N'Dr.Blackwell', 89)
INSERT [Doctors] ([Id], [Name], [Experience]) VALUES (N'269af575-f2d4-4796-8d32-c7a3c57928b4', N'Dr.Allmighty', 60)
INSERT [Doctors] ([Id], [Name], [Experience]) VALUES (N'ab31d6be-3314-48a0-bbcd-d96e46cc0790', N'Dr.Welldone', 56)
GO
INSERT [Drugs] ([Id], [Name], [PharmGroup], [Price]) VALUES (N'1777ce21-a36f-476e-8ad4-0335a1135de7', N'Amoxicillin', N'Penicillin antibiotic', 25)
INSERT [Drugs] ([Id], [Name], [PharmGroup], [Price]) VALUES (N'b39b3dd4-ce0c-4541-be0f-07a1686e968d', N'Tranexamic acid', N'Antifibrinolytic Agent', 4)
INSERT [Drugs] ([Id], [Name], [PharmGroup], [Price]) VALUES (N'a66dad4d-9727-42ac-a77d-09377fdcc9a7', N'Fluocinonide', N'Corticosteroid', 8)
INSERT [Drugs] ([Id], [Name], [PharmGroup], [Price]) VALUES (N'342c7c43-d854-436d-a4c8-12706aa99bf7', N'Lopinavir', N'Protease Inhibitor', 8)
INSERT [Drugs] ([Id], [Name], [PharmGroup], [Price]) VALUES (N'62fe42ae-4efc-491e-93b2-28e128831757', N'Vicodin', N'Pain reducer ', 55)
INSERT [Drugs] ([Id], [Name], [PharmGroup], [Price]) VALUES (N'01d77ba5-064d-456a-9559-3b6a5a60a6bc', N'Fenofibrate', N'Peroxisome', 6)
INSERT [Drugs] ([Id], [Name], [PharmGroup], [Price]) VALUES (N'a71775d4-55da-4622-94c9-4ad649a5c3a7', N'Metronidazole', N'Nitroimidazole Antimicrobial', 4)
INSERT [Drugs] ([Id], [Name], [PharmGroup], [Price]) VALUES (N'09a03df0-df41-4004-991e-6a065e89cbb8', N'Rifampin', N'Rifamycin Antibacterial', 7)
INSERT [Drugs] ([Id], [Name], [PharmGroup], [Price]) VALUES (N'585cebc9-1320-4b4b-860f-6f1bc8efd25d', N'Norethindrone', N'Progestin', 3)
INSERT [Drugs] ([Id], [Name], [PharmGroup], [Price]) VALUES (N'98be87fe-8fbc-421d-96a7-72e4a02aaac8', N'Levetiracetam', N'Anticonvulsant', 511)
INSERT [Drugs] ([Id], [Name], [PharmGroup], [Price]) VALUES (N'd8c8eb06-488d-4974-b8d0-8ee21eea13bc', N'Panacea', N'God''s creations', 999)
INSERT [Drugs] ([Id], [Name], [PharmGroup], [Price]) VALUES (N'2ffcb46c-93ad-4f2e-87fc-95486b540796', N'Risperidone', N'Atypical Antipsychotic', 91)
INSERT [Drugs] ([Id], [Name], [PharmGroup], [Price]) VALUES (N'f3a67ca8-791d-48c3-9591-bd1d93e31df6', N'Alcohol', N'God''s creations', 1)
INSERT [Drugs] ([Id], [Name], [PharmGroup], [Price]) VALUES (N'c80c0051-f65c-412c-9326-ccd9e1e8f9e3', N'Ibuprofin', N'Nonsteroidal anti-inflammatory', 5)
INSERT [Drugs] ([Id], [Name], [PharmGroup], [Price]) VALUES (N'19bf0b0d-2e28-41dc-9b2a-d2c81bf6714e', N'Aponvie', N'Aprepitant', 82)
INSERT [Drugs] ([Id], [Name], [PharmGroup], [Price]) VALUES (N'828b12b6-393b-48ea-8eb3-dbe6396469c6', N'Atenolol', N'Beta blocker', 15)
INSERT [Drugs] ([Id], [Name], [PharmGroup], [Price]) VALUES (N'b9cf2f1d-8bf9-4e38-a11c-f3595247f3e1', N'Acetylcysteine', N'Antidote', 6)
INSERT [Drugs] ([Id], [Name], [PharmGroup], [Price]) VALUES (N'57ffd310-5ddc-43cc-81f6-ff9c61ef1add', N'Fulvestrant', N'Estrogen Receptor Antagonist', 8)
GO
INSERT [Families] ([Id], [Surname], [Creator]) VALUES (N'7fddba89-8bf9-4386-ab29-203b603ec057', N'Smiths', N'bafef5d2-d072-4e59-7881-08daab90edc6')
INSERT [Families] ([Id], [Surname], [Creator]) VALUES (N'a0a7d061-6b52-4411-9b30-619c07bae54e', N'Labor Party', N'a898f8b9-d1dd-46be-96f6-08dac8cf2b05')
INSERT [Families] ([Id], [Surname], [Creator]) VALUES (N'b3dd83fd-7502-4b21-8b38-7aa3a0ee4838', N'Frontend', N'df30e013-ae24-4047-8b92-08dad7bd0005')
INSERT [Families] ([Id], [Surname], [Creator]) VALUES (N'4809715a-3104-425f-be6b-a3a5f4627fca', N'Jacksons', N'f1e73fad-8b2d-4c7a-267b-08daac8f9f21')
INSERT [Families] ([Id], [Surname], [Creator]) VALUES (N'e4d0811b-7b6c-4f53-a224-a6662db45122', N'Huff', N'6c351fe8-47a4-457f-0e77-08dab76bc3c4')
INSERT [Families] ([Id], [Surname], [Creator]) VALUES (N'cd8f7f26-6e87-498f-9828-b7a553d43bd1', N'Ray', N'5263faef-e3e1-4b5c-e52d-08daad131971')
INSERT [Families] ([Id], [Surname], [Creator]) VALUES (N'3ffda3fc-583f-426b-9cb4-d4c05050f5a0', N'Reactive', N'0652df00-47fd-48bf-a8c5-08dae1c508f1')
INSERT [Families] ([Id], [Surname], [Creator]) VALUES (N'e6545b91-0bbc-4b15-b83f-db6ed3f0c1c2', N'Typescript', N'dbd01bb4-69a9-4687-a8c6-08dae1c508f1')
GO
INSERT [FamilyMembers] ([Id], [IsAdmin], [FamilyId], [UserId]) VALUES (N'9341aa8f-9f81-4ca2-9468-32e10164ad85', 1, N'cd8f7f26-6e87-498f-9828-b7a553d43bd1', N'5263faef-e3e1-4b5c-e52d-08daad131971')
INSERT [FamilyMembers] ([Id], [IsAdmin], [FamilyId], [UserId]) VALUES (N'fd13e197-b688-4edf-8be2-38e7a5bbc270', 0, N'e4d0811b-7b6c-4f53-a224-a6662db45122', N'f1e73fad-8b2d-4c7a-267b-08daac8f9f21')
INSERT [FamilyMembers] ([Id], [IsAdmin], [FamilyId], [UserId]) VALUES (N'fdd6ab52-fcea-458e-9e11-45ae21a47e99', 1, N'4809715a-3104-425f-be6b-a3a5f4627fca', N'6c351fe8-47a4-457f-0e77-08dab76bc3c4')
INSERT [FamilyMembers] ([Id], [IsAdmin], [FamilyId], [UserId]) VALUES (N'daa04ca4-913c-44ce-9c63-520a8aa0f2ff', 0, N'b3dd83fd-7502-4b21-8b38-7aa3a0ee4838', N'c15c1511-54d4-4a81-51ad-08dae1058e69')
INSERT [FamilyMembers] ([Id], [IsAdmin], [FamilyId], [UserId]) VALUES (N'4063b876-1c96-4523-8d84-6086b7245e05', 1, N'4809715a-3104-425f-be6b-a3a5f4627fca', N'bafef5d2-d072-4e59-7881-08daab90edc6')
INSERT [FamilyMembers] ([Id], [IsAdmin], [FamilyId], [UserId]) VALUES (N'1f94ea66-5b57-43a7-b90b-80b7b44d9786', 0, N'7fddba89-8bf9-4386-ab29-203b603ec057', N'df30e013-ae24-4047-8b92-08dad7bd0005')
INSERT [FamilyMembers] ([Id], [IsAdmin], [FamilyId], [UserId]) VALUES (N'8133b052-593c-4aca-a8ed-8d87199e711f', 0, N'4809715a-3104-425f-be6b-a3a5f4627fca', N'5263faef-e3e1-4b5c-e52d-08daad131971')
INSERT [FamilyMembers] ([Id], [IsAdmin], [FamilyId], [UserId]) VALUES (N'ab053abf-91bf-4e86-b697-a875a80e9b73', 1, N'7fddba89-8bf9-4386-ab29-203b603ec057', N'bafef5d2-d072-4e59-7881-08daab90edc6')
INSERT [FamilyMembers] ([Id], [IsAdmin], [FamilyId], [UserId]) VALUES (N'db7435e6-3964-4c10-b3f6-ab46b5f4b5ec', 1, N'4809715a-3104-425f-be6b-a3a5f4627fca', N'f1e73fad-8b2d-4c7a-267b-08daac8f9f21')
INSERT [FamilyMembers] ([Id], [IsAdmin], [FamilyId], [UserId]) VALUES (N'7eee7c39-c8fb-46dc-8d68-b65f6d42ec36', 0, N'a0a7d061-6b52-4411-9b30-619c07bae54e', N'08253fcd-49e1-4829-ad1f-08dac731f8f1')
INSERT [FamilyMembers] ([Id], [IsAdmin], [FamilyId], [UserId]) VALUES (N'871363f6-6edb-4414-984f-bf59f8caf01e', 1, N'7fddba89-8bf9-4386-ab29-203b603ec057', N'f1e73fad-8b2d-4c7a-267b-08daac8f9f21')
INSERT [FamilyMembers] ([Id], [IsAdmin], [FamilyId], [UserId]) VALUES (N'a4676204-af7c-4bd3-84b2-c62f5c143a2b', 1, N'e6545b91-0bbc-4b15-b83f-db6ed3f0c1c2', N'dbd01bb4-69a9-4687-a8c6-08dae1c508f1')
INSERT [FamilyMembers] ([Id], [IsAdmin], [FamilyId], [UserId]) VALUES (N'1e3d3b6d-dbb0-44a2-8b6d-c6a59a8abf8f', 1, N'b3dd83fd-7502-4b21-8b38-7aa3a0ee4838', N'08253fcd-49e1-4829-ad1f-08dac731f8f1')
INSERT [FamilyMembers] ([Id], [IsAdmin], [FamilyId], [UserId]) VALUES (N'96247b19-791c-4033-a758-c81d2ac9d188', 0, N'7fddba89-8bf9-4386-ab29-203b603ec057', N'5263faef-e3e1-4b5c-e52d-08daad131971')
INSERT [FamilyMembers] ([Id], [IsAdmin], [FamilyId], [UserId]) VALUES (N'7a622558-c25a-429a-85f2-dce13e86491b', 1, N'e4d0811b-7b6c-4f53-a224-a6662db45122', N'6c351fe8-47a4-457f-0e77-08dab76bc3c4')
INSERT [FamilyMembers] ([Id], [IsAdmin], [FamilyId], [UserId]) VALUES (N'6afa57c2-60c0-438c-932f-fef418a99483', 1, N'b3dd83fd-7502-4b21-8b38-7aa3a0ee4838', N'df30e013-ae24-4047-8b92-08dad7bd0005')
GO
INSERT [Prescriptions] ([Id], [Date], [EndDate], [Dose], [UserId], [DrugId], [DoctorId], [NotificationId]) VALUES (N'20c66d2a-86a5-4a8b-aae4-082410dbedb7', CAST(N'2022-10-13T00:00:00.0000000' AS DateTime2), CAST(N'2022-10-14T00:00:00.0000000' AS DateTime2), 2, N'bafef5d2-d072-4e59-7881-08daab90edc6', N'f3a67ca8-791d-48c3-9591-bd1d93e31df6', N'b97a37d8-13fb-4f71-9400-bfbb758a778a', NULL)
INSERT [Prescriptions] ([Id], [Date], [EndDate], [Dose], [UserId], [DrugId], [DoctorId], [NotificationId]) VALUES (N'2ec844e3-e6d4-40b6-8ba4-1dbab3c35f42', CAST(N'2022-10-24T00:00:00.0000000' AS DateTime2), CAST(N'2022-10-25T00:00:00.0000000' AS DateTime2), 8, N'bafef5d2-d072-4e59-7881-08daab90edc6', N'd8c8eb06-488d-4974-b8d0-8ee21eea13bc', N'269af575-f2d4-4796-8d32-c7a3c57928b4', NULL)
INSERT [Prescriptions] ([Id], [Date], [EndDate], [Dose], [UserId], [DrugId], [DoctorId], [NotificationId]) VALUES (N'3f3b51c8-df9f-48e0-ab24-2dde798e7dfe', CAST(N'2023-01-04T00:00:00.0000000' AS DateTime2), CAST(N'2023-01-18T00:00:00.0000000' AS DateTime2), 3, N'e5e9a1e5-606c-4eae-0236-08dae1d07f21', N'a66dad4d-9727-42ac-a77d-09377fdcc9a7', N'c69c481a-4bed-430f-89b4-938478f06d7b', N'97')
INSERT [Prescriptions] ([Id], [Date], [EndDate], [Dose], [UserId], [DrugId], [DoctorId], [NotificationId]) VALUES (N'3d3015d7-4f8c-4864-8aab-4fc69f36c66c', CAST(N'2022-12-19T00:00:00.0000000' AS DateTime2), CAST(N'2022-12-23T00:00:00.0000000' AS DateTime2), 8, N'c15c1511-54d4-4a81-51ad-08dae1058e69', N'09a03df0-df41-4004-991e-6a065e89cbb8', N'269af575-f2d4-4796-8d32-c7a3c57928b4', N'77')
INSERT [Prescriptions] ([Id], [Date], [EndDate], [Dose], [UserId], [DrugId], [DoctorId], [NotificationId]) VALUES (N'a75344e8-3a1f-418b-b4d2-53ab6b2b1253', CAST(N'2020-06-11T00:00:00.0000000' AS DateTime2), CAST(N'2022-10-13T00:00:00.0000000' AS DateTime2), 5, N'f1e73fad-8b2d-4c7a-267b-08daac8f9f21', N'c80c0051-f65c-412c-9326-ccd9e1e8f9e3', N'b97a37d8-13fb-4f71-9400-bfbb758a778a', NULL)
INSERT [Prescriptions] ([Id], [Date], [EndDate], [Dose], [UserId], [DrugId], [DoctorId], [NotificationId]) VALUES (N'55dcef88-f338-4270-9f26-8dd8da147c84', CAST(N'2022-12-14T00:00:00.0000000' AS DateTime2), CAST(N'2022-12-28T00:00:00.0000000' AS DateTime2), 5, N'e5e9a1e5-606c-4eae-0236-08dae1d07f21', N'd8c8eb06-488d-4974-b8d0-8ee21eea13bc', N'c69c481a-4bed-430f-89b4-938478f06d7b', NULL)
INSERT [Prescriptions] ([Id], [Date], [EndDate], [Dose], [UserId], [DrugId], [DoctorId], [NotificationId]) VALUES (N'cc5d8ea1-6b24-4f52-a8d6-a6554871da77', CAST(N'2020-06-12T00:00:00.0000000' AS DateTime2), CAST(N'2020-10-12T00:00:00.0000000' AS DateTime2), 10, N'6c351fe8-47a4-457f-0e77-08dab76bc3c4', N'd8c8eb06-488d-4974-b8d0-8ee21eea13bc', N'269af575-f2d4-4796-8d32-c7a3c57928b4', NULL)
INSERT [Prescriptions] ([Id], [Date], [EndDate], [Dose], [UserId], [DrugId], [DoctorId], [NotificationId]) VALUES (N'2943d768-3b01-4425-bb2b-ad2c3930e304', CAST(N'2022-10-04T00:00:00.0000000' AS DateTime2), CAST(N'2022-10-31T00:00:00.0000000' AS DateTime2), 4, N'bafef5d2-d072-4e59-7881-08daab90edc6', N'c80c0051-f65c-412c-9326-ccd9e1e8f9e3', N'ab31d6be-3314-48a0-bbcd-d96e46cc0790', NULL)
INSERT [Prescriptions] ([Id], [Date], [EndDate], [Dose], [UserId], [DrugId], [DoctorId], [NotificationId]) VALUES (N'811623cc-8cc2-40a8-9473-c53d053255a3', CAST(N'2020-06-12T00:00:00.0000000' AS DateTime2), CAST(N'2020-08-12T00:00:00.0000000' AS DateTime2), 22, N'08253fcd-49e1-4829-ad1f-08dac731f8f1', N'd8c8eb06-488d-4974-b8d0-8ee21eea13bc', N'269af575-f2d4-4796-8d32-c7a3c57928b4', NULL)
INSERT [Prescriptions] ([Id], [Date], [EndDate], [Dose], [UserId], [DrugId], [DoctorId], [NotificationId]) VALUES (N'33e3fa34-0f54-49b0-b256-e0ee31f4e7dc', CAST(N'2023-02-08T00:00:00.0000000' AS DateTime2), CAST(N'2023-02-22T00:00:00.0000000' AS DateTime2), 15, N'df30e013-ae24-4047-8b92-08dad7bd0005', N'62fe42ae-4efc-491e-93b2-28e128831757', N'c69c481a-4bed-430f-89b4-938478f06d7b', N'73')
INSERT [Prescriptions] ([Id], [Date], [EndDate], [Dose], [UserId], [DrugId], [DoctorId], [NotificationId]) VALUES (N'91c1a2f7-b821-4bd1-b3e5-ecf720750c13', CAST(N'2022-12-15T00:00:00.0000000' AS DateTime2), CAST(N'2022-12-29T00:00:00.0000000' AS DateTime2), 8, N'df30e013-ae24-4047-8b92-08dad7bd0005', N'a66dad4d-9727-42ac-a77d-09377fdcc9a7', N'b97a37d8-13fb-4f71-9400-bfbb758a778a', N'67')
GO
INSERT [Users] ([Id], [Login], [DateOfBirth], [FullName]) VALUES (N'bafef5d2-d072-4e59-7881-08daab90edc6', N'firstuser@gmail.com', NULL, NULL)
INSERT [Users] ([Id], [Login], [DateOfBirth], [FullName]) VALUES (N'f1e73fad-8b2d-4c7a-267b-08daac8f9f21', N'seconduser@email.com', CAST(N'2007-03-21T16:12:00.0000000' AS DateTime2), N'German Shepard')
INSERT [Users] ([Id], [Login], [DateOfBirth], [FullName]) VALUES (N'5263faef-e3e1-4b5c-e52d-08daad131971', N'thirduser@email.com', NULL, NULL)
INSERT [Users] ([Id], [Login], [DateOfBirth], [FullName]) VALUES (N'6c351fe8-47a4-457f-0e77-08dab76bc3c4', N'fourthuser@email.com', NULL, N'Great Dog')
INSERT [Users] ([Id], [Login], [DateOfBirth], [FullName]) VALUES (N'08253fcd-49e1-4829-ad1f-08dac731f8f1', N'swagger@email.com', NULL, NULL)
INSERT [Users] ([Id], [Login], [DateOfBirth], [FullName]) VALUES (N'df30e013-ae24-4047-8b92-08dad7bd0005', N'angular@gmail.com', CAST(N'2016-05-10T00:00:00.0000000' AS DateTime2), N'Angular User')
INSERT [Users] ([Id], [Login], [DateOfBirth], [FullName]) VALUES (N'c15c1511-54d4-4a81-51ad-08dae1058e69', N'angularsecond@gmail.com', NULL, NULL)
INSERT [Users] ([Id], [Login], [DateOfBirth], [FullName]) VALUES (N'dbd01bb4-69a9-4687-a8c6-08dae1c508f1', N'subject@gmail.com', CAST(N'1998-07-15T00:00:00.0000000' AS DateTime2), N'Behavior Subject')
INSERT [Users] ([Id], [Login], [DateOfBirth], [FullName]) VALUES (N'e5e9a1e5-606c-4eae-0236-08dae1d07f21', N'sticker@gmail.com', NULL, N'Big Sticker')
GO
INSERT [Vaccinations] ([Id], [Date], [DaysOfProtection], [UserId], [VaccineId], [NotificationId]) VALUES (N'da9f95d0-25ab-4cba-936b-13d93636f1bc', CAST(N'2022-08-15T00:00:00.0000000' AS DateTime2), 150, N'5263faef-e3e1-4b5c-e52d-08daad131971', N'8865e724-e999-40ca-8b53-f816e7636cb9', NULL)
INSERT [Vaccinations] ([Id], [Date], [DaysOfProtection], [UserId], [VaccineId], [NotificationId]) VALUES (N'4538e800-a8f6-4c32-964f-1c0b6e2893ce', CAST(N'2022-10-12T00:00:00.0000000' AS DateTime2), 180, N'f1e73fad-8b2d-4c7a-267b-08daac8f9f21', N'2ba96243-9854-4c94-a960-5efc5c462d0f', NULL)
INSERT [Vaccinations] ([Id], [Date], [DaysOfProtection], [UserId], [VaccineId], [NotificationId]) VALUES (N'ce35777a-1dbf-42c9-a07d-3b6ea9ab4390', CAST(N'2023-01-12T00:00:00.0000000' AS DateTime2), 189, N'df30e013-ae24-4047-8b92-08dad7bd0005', N'2a3d57fd-0599-4378-9171-30354d571ee3', N'72')
INSERT [Vaccinations] ([Id], [Date], [DaysOfProtection], [UserId], [VaccineId], [NotificationId]) VALUES (N'9b59adb6-17e7-4372-b643-4d57e9241f92', CAST(N'2022-12-20T00:00:00.0000000' AS DateTime2), 11, N'c15c1511-54d4-4a81-51ad-08dae1058e69', N'2a3d57fd-0599-4378-9171-30354d571ee3', N'75')
INSERT [Vaccinations] ([Id], [Date], [DaysOfProtection], [UserId], [VaccineId], [NotificationId]) VALUES (N'0fe19e45-58a2-404a-aee3-6286460e19e3', CAST(N'2021-06-17T00:00:00.0000000' AS DateTime2), 121, N'bafef5d2-d072-4e59-7881-08daab90edc6', N'2a3d57fd-0599-4378-9171-30354d571ee3', NULL)
INSERT [Vaccinations] ([Id], [Date], [DaysOfProtection], [UserId], [VaccineId], [NotificationId]) VALUES (N'c517fc79-606b-474e-b892-6732ee112432', CAST(N'2023-01-10T00:00:00.0000000' AS DateTime2), 14, N'dbd01bb4-69a9-4687-a8c6-08dae1c508f1', N'2a3d57fd-0599-4378-9171-30354d571ee3', N'88')
INSERT [Vaccinations] ([Id], [Date], [DaysOfProtection], [UserId], [VaccineId], [NotificationId]) VALUES (N'f094a8b8-ca12-4e34-81e4-d3ad14f91683', CAST(N'2021-10-20T00:00:00.0000000' AS DateTime2), 82, N'f1e73fad-8b2d-4c7a-267b-08daac8f9f21', N'8865e724-e999-40ca-8b53-f816e7636cb9', NULL)
GO
INSERT [Vaccines] ([Id], [Name], [Reason], [Price]) VALUES (N'2a3d57fd-0599-4378-9171-30354d571ee3', N'HPV', N'Nasty', 40)
INSERT [Vaccines] ([Id], [Name], [Reason], [Price]) VALUES (N'2ba96243-9854-4c94-a960-5efc5c462d0f', N'Tetanus', N'Clostridium tetani', 20)
INSERT [Vaccines] ([Id], [Name], [Reason], [Price]) VALUES (N'cfd1aa88-9f92-4b8a-bca3-9fd18e94dd40', N'Ultima', N'Hangover', 2)
INSERT [Vaccines] ([Id], [Name], [Reason], [Price]) VALUES (N'2856e4c9-b601-4499-9376-df92c5c31c67', N'Love', N'Everything', 1)
INSERT [Vaccines] ([Id], [Name], [Reason], [Price]) VALUES (N'8865e724-e999-40ca-8b53-f816e7636cb9', N'Chickenpox', N'Varicella', 20)
GO
ALTER TABLE [Families] ADD  DEFAULT ('00000000-0000-0000-0000-000000000000') FOR [Creator]
GO
ALTER TABLE [Vaccines] ADD  DEFAULT ((0)) FOR [Price]
GO
ALTER TABLE [Appointments]  WITH CHECK ADD  CONSTRAINT [FK_Appointment_Doctor_DoctorId] FOREIGN KEY([DoctorId])
REFERENCES [Doctors] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [Appointments] CHECK CONSTRAINT [FK_Appointment_Doctor_DoctorId]
GO
ALTER TABLE [Appointments]  WITH CHECK ADD  CONSTRAINT [FK_Appointment_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [Appointments] CHECK CONSTRAINT [FK_Appointment_Users_UserId]
GO
ALTER TABLE [FamilyMembers]  WITH CHECK ADD  CONSTRAINT [FK_FamilyMember_Family_FamilyId] FOREIGN KEY([FamilyId])
REFERENCES [Families] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [FamilyMembers] CHECK CONSTRAINT [FK_FamilyMember_Family_FamilyId]
GO
ALTER TABLE [FamilyMembers]  WITH CHECK ADD  CONSTRAINT [FK_FamilyMember_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [FamilyMembers] CHECK CONSTRAINT [FK_FamilyMember_Users_UserId]
GO
ALTER TABLE [Prescriptions]  WITH CHECK ADD  CONSTRAINT [FK_Prescription_Doctor_DoctorId] FOREIGN KEY([DoctorId])
REFERENCES [Doctors] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [Prescriptions] CHECK CONSTRAINT [FK_Prescription_Doctor_DoctorId]
GO
ALTER TABLE [Prescriptions]  WITH CHECK ADD  CONSTRAINT [FK_Prescription_Drug_DrugId] FOREIGN KEY([DrugId])
REFERENCES [Drugs] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [Prescriptions] CHECK CONSTRAINT [FK_Prescription_Drug_DrugId]
GO
ALTER TABLE [Prescriptions]  WITH CHECK ADD  CONSTRAINT [FK_Prescription_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [Prescriptions] CHECK CONSTRAINT [FK_Prescription_Users_UserId]
GO
ALTER TABLE [Vaccinations]  WITH CHECK ADD  CONSTRAINT [FK_Vaccination_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [Vaccinations] CHECK CONSTRAINT [FK_Vaccination_Users_UserId]
GO
ALTER TABLE [Vaccinations]  WITH CHECK ADD  CONSTRAINT [FK_Vaccination_Vaccine_VaccineId] FOREIGN KEY([VaccineId])
REFERENCES [Vaccines] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [Vaccinations] CHECK CONSTRAINT [FK_Vaccination_Vaccine_VaccineId]
GO

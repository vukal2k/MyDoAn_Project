USE [DbProjectManagement_DEV]
GO
INSERT [dbo].[LookupStatus] ([Id], [IsProject], [IsTask], [IsActive], [Name]) VALUES (1, 1, 1, 1, N'Opened')
INSERT [dbo].[LookupStatus] ([Id], [IsProject], [IsTask], [IsActive], [Name]) VALUES (2, 0, 1, 1, N'In Progress')
INSERT [dbo].[LookupStatus] ([Id], [IsProject], [IsTask], [IsActive], [Name]) VALUES (3, 0, 1, 1, N'Resolved')
INSERT [dbo].[LookupStatus] ([Id], [IsProject], [IsTask], [IsActive], [Name]) VALUES (4, 0, 1, 1, N'ReOpened')
INSERT [dbo].[LookupStatus] ([Id], [IsProject], [IsTask], [IsActive], [Name]) VALUES (5, 0, 0, 1, N'Draft')
INSERT [dbo].[LookupStatus] ([Id], [IsProject], [IsTask], [IsActive], [Name]) VALUES (6, 0, 0, 1, N'Pending Approval')
INSERT [dbo].[LookupStatus] ([Id], [IsProject], [IsTask], [IsActive], [Name]) VALUES (7, 0, 0, 1, N'Approved')
INSERT [dbo].[LookupStatus] ([Id], [IsProject], [IsTask], [IsActive], [Name]) VALUES (8, 0, 0, 1, N'Rejected')
INSERT [dbo].[LookupStatus] ([Id], [IsProject], [IsTask], [IsActive], [Name]) VALUES (9, 0, 0, 1, N'Cancelled')
INSERT [dbo].[LookupStatus] ([Id], [IsProject], [IsTask], [IsActive], [Name]) VALUES (10, 1, 1, 1, N'Closed')
INSERT [dbo].[UserInfo] ([UserName], [FullName], [CurrentJob], [Company], [CountExperience], [TimeUnit], [IsActive]) VALUES (N'devtest', N'Dev Test', N'Dev', NULL, NULL, NULL, 1)
INSERT [dbo].[UserInfo] ([UserName], [FullName], [CurrentJob], [Company], [CountExperience], [TimeUnit], [IsActive]) VALUES (N'pmtest', N'PM Test', N'Project Manager', NULL, NULL, NULL, 1)
INSERT [dbo].[UserInfo] ([UserName], [FullName], [CurrentJob], [Company], [CountExperience], [TimeUnit], [IsActive]) VALUES (N'teamleadtest', N'Team Lead Test', N'Team Lead', NULL, NULL, NULL, 1)
INSERT [dbo].[UserInfo] ([UserName], [FullName], [CurrentJob], [Company], [CountExperience], [TimeUnit], [IsActive]) VALUES (N'testertest', N'Tester Test', N'Tester', NULL, NULL, NULL, 1)
SET IDENTITY_INSERT [dbo].[Project] ON 

INSERT [dbo].[Project] ([Id], [Name], [From], [To], [CreatedBy], [CreatedDate], [StatusId], [IsActive], [Code], [Description]) VALUES (3, N'first project', CAST(N'2019-08-13' AS Date), CAST(N'2019-08-23' AS Date), N'pmtest', CAST(N'2019-08-03' AS Date), 1, 0, N'FI', N'123')
INSERT [dbo].[Project] ([Id], [Name], [From], [To], [CreatedBy], [CreatedDate], [StatusId], [IsActive], [Code], [Description]) VALUES (4, N'seproject', CAST(N'2019-08-04' AS Date), CAST(N'2019-08-20' AS Date), N'pmtest', CAST(N'2019-08-03' AS Date), 1, 0, N'FI2', NULL)
INSERT [dbo].[Project] ([Id], [Name], [From], [To], [CreatedBy], [CreatedDate], [StatusId], [IsActive], [Code], [Description]) VALUES (5, N'first project', CAST(N'2019-08-05' AS Date), CAST(N'2019-08-22' AS Date), N'pmtest', CAST(N'2019-08-04' AS Date), 1, 0, N'fa', N'123')
SET IDENTITY_INSERT [dbo].[Project] OFF
SET IDENTITY_INSERT [dbo].[Module] ON 

INSERT [dbo].[Module] ([Id], [Title], [ProjectId], [TeamLead], [IsActive]) VALUES (1, N'Fist Module', 3, N'teamleadtest', 1)
SET IDENTITY_INSERT [dbo].[Module] OFF
SET IDENTITY_INSERT [dbo].[TaskType] ON 

INSERT [dbo].[TaskType] ([Id], [Title]) VALUES (1, N'Task')
SET IDENTITY_INSERT [dbo].[TaskType] OFF
SET IDENTITY_INSERT [dbo].[Task] ON 

INSERT [dbo].[Task] ([Id], [Title], [CreatedBy], [AssignedTo], [TaskTypeId], [ModuleId], [From], [To], [Level], [StatusId], [Description], [IsActive]) VALUES (4, N'Fist Task', N'pmtest', N'pmtest', 1, 1, CAST(N'2019-01-01 00:00:00.000' AS DateTime), CAST(N'2019-02-01 00:00:00.000' AS DateTime), 1, 10, NULL, 1)
SET IDENTITY_INSERT [dbo].[Task] OFF
SET IDENTITY_INSERT [dbo].[JobRole] ON 

INSERT [dbo].[JobRole] ([Id],[Title], [Description], [IsActive], [CanDelete]) VALUES (1, N'PM', N'Project Manager', 1, 0)
INSERT [dbo].[JobRole] ([Id],[Title], [Description], [IsActive], [CanDelete]) VALUES (2, N'Team Lead', NULL, 1, 0)
INSERT [dbo].[JobRole] ([Id], [Title], [Description], [IsActive], [CanDelete]) VALUES (3, N'Developer', NULL, 1, 1)
INSERT [dbo].[JobRole] ([Id], [Title], [Description], [IsActive], [CanDelete]) VALUES (5, N'QA', NULL, 1, 1)
INSERT [dbo].[JobRole] ([Id], [Title], [Description], [IsActive], [CanDelete]) VALUES (6, N'Tester', NULL, 1, 1)
SET IDENTITY_INSERT [dbo].[JobRole] OFF
SET IDENTITY_INSERT [dbo].[RoleInProject] ON 

INSERT [dbo].[RoleInProject] ([Id], [ProjectId], [RoleId], [UserName], [IsActive]) VALUES (1, 3, 1, N'pmtest', 1)
INSERT [dbo].[RoleInProject] ([Id], [ProjectId], [RoleId], [UserName], [IsActive]) VALUES (2, 4, 1, N'pmtest', 1)
INSERT [dbo].[RoleInProject] ([Id], [ProjectId], [RoleId], [UserName], [IsActive]) VALUES (3, 5, 1, N'pmtest', 1)
INSERT [dbo].[RoleInProject] ([Id], [ProjectId], [RoleId], [UserName], [IsActive]) VALUES (4, 5, 5, N'devtest', 1)
INSERT [dbo].[RoleInProject] ([Id], [ProjectId], [RoleId], [UserName], [IsActive]) VALUES (5, 5, 3, N'pmtest', 1)
INSERT [dbo].[RoleInProject] ([Id], [ProjectId], [RoleId], [UserName], [IsActive]) VALUES (6, 5, 6, N'teamleadtest', 1)
SET IDENTITY_INSERT [dbo].[RoleInProject] OFF
INSERT [dbo].[ResolveType] ([Id], [Title]) VALUES (1, N'Create')
SET IDENTITY_INSERT [dbo].[Solution] ON 

INSERT [dbo].[Solution] ([Id], [TaskId], [ResolveType], [Reason], [Solution], [Description], [CreatedBy], [CreatedDateTime], [ResolveType1_Id]) VALUES (1, 4, 1, N'123', N'13', N'123', N'pmtest', CAST(N'2019-08-05 02:05:16.470' AS DateTime), NULL)
SET IDENTITY_INSERT [dbo].[Solution] OFF

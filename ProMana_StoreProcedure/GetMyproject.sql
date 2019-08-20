USE [DbProjectManagement_DEV]
GO
/****** Object:  StoredProcedure [dbo].[GetUserDoNotInProject]    Script Date: 8/20/2019 11:51:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].GetMyproject
	@username VARCHAR(100)
AS
BEGIN
	SELECT DISTINCT p.*
	FROM dbo.Project p
	LEFT JOIN dbo.Module m ON p.Id = m.ProjectId AND m.IsActive=1
	INNER JOIN dbo.RoleInProject rip ON m.Id=rip.ModuleId AND rip.IsActive=1 AND rip.UserName=@username
	WHERE p.IsActive=1
END

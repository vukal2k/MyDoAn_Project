-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE GetUserDoNotInProject
	@projectId INT
AS
BEGIN
	IF @projectId<>0
	BEGIN
			SELECT ui.* FROM dbo.UserInfo ui
		LEFT JOIN dbo.RoleInProject rip
			ON ui.UserName=rip.UserName
			AND rip.IsActive = 1
		LEFT JOIN dbo.Module m
			ON rip.ModuleId=m.Id
				AND m.IsActive=1
		WHERE rip.Id is null
			  AND m.ProjectId = @projectId
			  AND m.Title <> 'Watcher'
	END;
	ELSE
	BEGIN
		SELECT * FROM dbo.UserInfo
	END;
END
GO

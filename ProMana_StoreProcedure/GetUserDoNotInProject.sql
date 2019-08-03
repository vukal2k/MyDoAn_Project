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
CREATE PROCEDURE GetUserDoNotInProject
	@projectId INT
AS
BEGIN
	SELECT * FROM dbo.UserInfo ui
	LEFT JOIN dbo.RoleInProject rip
		ON ui.UserName=rip.UserName
		AND rip.IsActive = 1
	WHERE rip.Id is null
		  AND ProjectId = @projectId
END
GO

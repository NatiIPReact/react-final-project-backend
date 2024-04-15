USE [igroup122_test2]
GO
/****** Object:  StoredProcedure [dbo].[Proj_SP_GetUserPastQuiz]    Script Date: 24/07/2023 13:19:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Returns the past quizzes of @UserID>
-- =============================================
ALTER PROCEDURE [dbo].[Proj_SP_GetUserPastQuiz]
	-- Add the parameters for the stored procedure here
	@UserID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	select * from Proj_Quiz Q inner join Proj_Question QU on Q.QuizID = QU.QuizID
	where Q.UserID = @UserID
END

USE [igroup122_test2]
GO
/****** Object:  StoredProcedure [dbo].[Proj_SP_AddUserXP]    Script Date: 24/07/2023 12:48:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Adds the requested XP to this user>
-- =============================================
ALTER PROCEDURE [dbo].[Proj_SP_AddUserXP]
	-- Add the parameters for the stored procedure here
	@UserID int,
	@XPToAdd int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	update Proj_Users
	set XP = XP + @XPToAdd
	where UserID = @UserID
END

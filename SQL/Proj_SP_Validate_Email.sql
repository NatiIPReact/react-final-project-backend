USE [igroup122_test2]
GO
/****** Object:  StoredProcedure [dbo].[Proj_SP_Validate_Email]    Script Date: 24/07/2023 13:36:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Validates user email.>
-- =============================================
ALTER PROCEDURE [dbo].[Proj_SP_Validate_Email]
	-- Add the parameters for the stored procedure here
	@UserEmail nvarchar(50),
	@UserToken varchar(16)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;
	declare @flag int
	set @flag = 0
    -- Insert statements for procedure here
	if ((select count(*) from Proj_Users where UserEmail = @UserEmail) = 0)
	set @flag = 1 -- User doesn't exist
	else if ((select count(*) from Proj_Users where UserEmail = @UserEmail and UserIsVerified = 1) <> 0)
	set @flag = 2 -- User already verified
	else if ((select count(*) from Proj_Users where UserEmail = @UserEmail and @UserToken <> UserToken) > 0)
	set @flag = 3 -- Token doesn't match
	else if (datediff(minute, (select top 1 LastTokenTime from Proj_Users where UserEmail = @UserEmail and @UserToken = UserToken), getdate()) > 30)
	set @flag = 4 -- Took too long (More than 30 minutes)
	else if ((select count(*) from Proj_Users where UserEmail = @UserEmail and @UserToken = UserToken) > 0)
	update Proj_Users
	set UserToken = '', UserIsVerified = 1
	where UserEmail = @UserEmail and @UserToken = UserToken
	return @flag
END

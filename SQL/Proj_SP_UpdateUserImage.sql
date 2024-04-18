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
CREATE PROCEDURE Proj_SP_UpdateUserImage
	-- Add the parameters for the stored procedure here
	@UserID int,
	@UserEmail nvarchar(50),
    @UserName nvarchar(50),
    @UserPassword nvarchar(100),
	@isNewEmail bit,
	@token varchar(16),
	@image VARBINARY(MAX)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @flag int
	set @flag = 0
	if (@isNewEmail = 0)
	UPDATE Proj_Users
    SET UserName = @UserName, UserPassword = @UserPassword, image = @image
    where UserID = @UserID
	else UPDATE Proj_Users
	set UserEmail = @UserEmail, UserName = @UserName, UserToken = @token, LastTokenTime = getdate(), UserIsVerified = 0, image = @image
	where UserID = @UserID
END
GO

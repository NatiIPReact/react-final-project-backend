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
-- Author:		<cgroup22>
-- Create date: <4.6.23>
-- Description:	<Handles user login, returns the user if successful>
-- =============================================
CREATE PROCEDURE Proj_SP_UserLogin
	-- Add the parameters for the stored procedure here
	@email nvarchar(50),
	@password nvarchar(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	DECLARE @returnValue int
	SELECT * FROM Proj_Users WHERE (email = @email AND [password] = @password)
	if ((SELECT COUNT(*) FROM Proj_Users WHERE email = @email and [password] = @password) >= 1)
    set @returnValue = 2 -- Success if email and password are validated correctly
    else if ((SELECT COUNT(*) FROM Proj_Users WHERE email = @email) >= 1)
    set @returnValue = 1 -- Failed with wrong password
    else set @returnValue = 0 -- User doesn't exists
    return @returnValue
END
GO

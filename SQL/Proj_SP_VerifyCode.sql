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
CREATE PROCEDURE Proj_SP_VerifyCode
	-- Add the parameters for the stored procedure here
	@email nvarchar(50),
	@code int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	if not exists (SELECT * from Proj_Users where UserEmail = @email and code = @code)
	select 0 as CodeRight
	else
	begin
	update Proj_Users
	set code = 0
	where UserEmail = @email
	select 1 as CodeRight
	end
END
GO

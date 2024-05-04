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
alter PROCEDURE Proj_SP_GetLastPositionInSong
	-- Add the parameters for the stored procedure here
	@UserID int,
	@SongID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	if exists (select * from Proj_RecentlyPlayed WHERE UserID = @UserID AND SongID = @SongID)
	SELECT COALESCE(LastPosition, 0) AS LastPosition
FROM Proj_RecentlyPlayed 
WHERE UserID = @UserID AND SongID = @SongID;
else select 0 as LastPosition
END
GO

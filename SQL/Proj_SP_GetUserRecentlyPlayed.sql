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
CREATE PROCEDURE Proj_SP_GetUserRecentlyPlayed
	-- Add the parameters for the stored procedure here
	@UserID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT G.GenreName, S.SongLength, P.PerformerID, P.PerformerImage, P.PerformerName, S.SongID, S.SongName,
	case when exists (select * from Proj_UserFavorites UF where UF.UserID = @UserID and UF.SongID = S.SongID) then 1 else 0 end as InFav
	from Proj_RecentlyPlayed RP inner join Proj_Song S on RP.SongID = S.SongID
	inner join Proj_Genre G on G.GenreID = S.GenreID inner join Proj_Performer P on P.PerformerID = S.PerformerID
	where RP.UserID = 1
	order by RP.DateTimePlayed desc
END
GO

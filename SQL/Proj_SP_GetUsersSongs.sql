USE [igroup122_test2]
GO
/****** Object:  StoredProcedure [dbo].[Proj_SP_GetUsersSongs]    Script Date: 5/3/2024 12:06:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[Proj_SP_GetUsersSongs 1]
	-- Add the parameters for the stored procedure here
	@UserID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	select S.SongID, P.PerformerName, P.PerformerID, P.PerformerImage, S.SongName, G.GenreName, S.SongLength, S.NumOfPlays, S.ReleaseYear,
	case when @UserID < 1 then 0
	when (select count(*) from Proj_UserFavorites where SongID = S.SongID and UserID = @UserID) > 0 then 1 else 0 end as InFav,
	case when exists (select * from Proj_Following F where F.UserID = @UserID and F.PerformerID = S.PerformerID) then 1
	else 0 end as FollowingArtist,
	case when exists (select UF.UserID from Proj_UserFavorites UF inner join Proj_Song S1 on UF.SongID = S1.SongID
	and UF.UserID = @UserID and S1.GenreID = S.GenreID) then 1 else 0 end as LikesGenre,
	case when exists (select * from Proj_RecentlyPlayed R where R.UserID = @UserID and R.SongID = S.SongID
	and DATEDIFF(month, R.DateTimePlayed, GETDATE()) < 1) then 1
	else 0 end as IsInRecentlyPlayed
	from Proj_Song S inner join Proj_Performer P on S.PerformerID = P.PerformerID inner join Proj_Genre G on S.GenreID = G.GenreID
END

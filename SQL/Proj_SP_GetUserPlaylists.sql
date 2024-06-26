USE [igroup122_test2]
GO
/****** Object:  StoredProcedure [dbo].[Proj_SP_GetPlaylists]    Script Date: 4/12/2024 3:01:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[Proj_SP_GetPlaylists]
	-- Add the parameters for the stored procedure here
	@UserID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT P.PlaylistID, 
       P.PlaylistName, 
       COALESCE(count(S.SongID), 0) AS SongsInPlaylist 
FROM Proj_Playlist P 
LEFT JOIN Proj_SongInPlaylist S 
ON P.PlaylistID = S.PlaylistID
where P.UserID = @UserID
GROUP BY P.PlaylistID, P.PlaylistName
	--SELECT P.PlaylistID, PlaylistName from Proj_Playlist P inner join Proj_SongInPlaylist S on P.PlaylistID = S.PlaylistID where UserID = @UserID
END
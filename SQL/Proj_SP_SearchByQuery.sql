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
CREATE PROCEDURE Proj_SP_SearchByQuery
	-- Add the parameters for the stored procedure here
	@Query nvarchar(100),
	@UserID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT S.SongID, S.SongName, S.NumOfPlays, S.SongLength, P.PerformerName, P.PerformerImage, P.PerformerID, G.GenreName,
    CASE WHEN @UserID < 1 THEN -1
        WHEN (SELECT COUNT(*) FROM Proj_UserFavorites WHERE SongID = S.SongID AND UserID = @UserID) > 0 THEN 1
        ELSE 0 END AS InFav,
    COALESCE((SELECT COUNT(distinct UF2.UserID) FROM Proj_UserFavorites UF2 WHERE UF2.SongID = S.SongID GROUP BY UF2.SongID), 0) AS UserFavorites,
    COALESCE((SELECT COUNT(distinct UF1.UserID) FROM Proj_UserFavorites UF1 INNER JOIN Proj_Song S1 ON UF1.SongID = S1.SongID
        WHERE S1.PerformerID = P.PerformerID), 0) AS ArtistFavorites
FROM Proj_Song S
INNER JOIN Proj_Performer P ON S.PerformerID = P.PerformerID
INNER JOIN Proj_Genre G ON S.GenreID = G.GenreID
WHERE S.SongName LIKE '%' + @Query + '%'
    OR P.PerformerName LIKE '%' + @Query + '%'
    OR G.GenreName LIKE '%' + @Query + '%';
END
GO

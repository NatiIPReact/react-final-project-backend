USE [igroup122_test2]
GO
/****** Object:  StoredProcedure [dbo].[Proj_SP_AdminGetMostLovedGenre]    Script Date: 24/07/2023 12:50:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Returns information about the most loved genre>
-- =============================================
ALTER PROCEDURE [dbo].[Proj_SP_AdminGetMostLovedGenre]
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT G.GenreID, G.GenreName, sum(S.NumOfPlays) as SumPlays, count(*) as NumOfSongs
	from Proj_Genre G inner join Proj_Song S on G.GenreID = S.GenreID
	group by G.GenreID, G.GenreName
	order by sum(S.NumOfPlays) desc
END

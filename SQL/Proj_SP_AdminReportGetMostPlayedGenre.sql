USE [igroup122_test2]
GO
/****** Object:  StoredProcedure [dbo].[Proj_SP_AdminReportGetMostPlayedGenre]    Script Date: 24/07/2023 12:52:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Returns the most played genre>
-- =============================================
ALTER PROCEDURE [dbo].[Proj_SP_AdminReportGetMostPlayedGenre]
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	select top(1) G.GenreName, sum(S.NumOfPlays) as TotalPlays
	from Proj_Genre G inner join Proj_Song S on G.GenreID = S.GenreID
	group by G.GenreName
	order by sum(S.NumOfPlays) desc
END

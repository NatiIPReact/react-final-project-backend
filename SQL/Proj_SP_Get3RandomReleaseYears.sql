USE [igroup122_test2]
GO
/****** Object:  StoredProcedure [dbo].[Proj_SP_Get3RandomReleaseYears]    Script Date: 24/07/2023 12:58:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Returns 3 random songs release years. Can't include @ReleaseYearToIgnore>
-- =============================================
ALTER PROCEDURE [dbo].[Proj_SP_Get3RandomReleaseYears]
	-- Add the parameters for the stored procedure here
	@ReleaseYearToIgnore int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT top(3) ReleaseYear
	from (select distinct ReleaseYear from Proj_Song where ReleaseYear <> 1990) as x
	ORDER BY CHECKSUM(NEWID());
END

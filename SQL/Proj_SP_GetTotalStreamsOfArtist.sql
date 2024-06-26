USE [igroup122_test2]
GO
/****** Object:  StoredProcedure [dbo].[Proj_SP_GetTotalStreamsOfArtist]    Script Date: 24/07/2023 13:18:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Returns the total streams of @PerformerID>
-- =============================================
ALTER PROCEDURE [dbo].[Proj_SP_GetTotalStreamsOfArtist]
	-- Add the parameters for the stored procedure here
	@PerformerID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT sum(NumOfPlays) as TotalPlays from Proj_Song Where PerformerID = @PerformerID
END

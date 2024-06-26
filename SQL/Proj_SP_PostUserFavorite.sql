USE [igroup122_test2]
GO
/****** Object:  StoredProcedure [dbo].[Proj_SP_PostUserFavorite]    Script Date: 24/07/2023 13:34:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Adds a song to the user's favorites.>
-- =============================================
ALTER PROCEDURE [dbo].[Proj_SP_PostUserFavorite]
	-- Add the parameters for the stored procedure here
	@UserID int,
	@SongID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	insert into Proj_UserFavorites(UserID, SongID) values (@UserID, @SongID)
END

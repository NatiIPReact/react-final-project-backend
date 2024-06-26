USE [igroup122_test2]
GO
/****** Object:  StoredProcedure [dbo].[Proj_SP_CreateUserPlaylist]    Script Date: 24/07/2023 12:53:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Creates a playlist for the requested user>
-- =============================================
ALTER PROCEDURE [dbo].[Proj_SP_CreateUserPlaylist]
	-- Add the parameters for the stored procedure here
	@UserID int,
	@PlaylistName nvarchar(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	insert into Proj_Playlist (UserID, PlaylistName) values (@UserID, @PlaylistName)
END

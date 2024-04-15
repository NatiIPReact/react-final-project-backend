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
CREATE PROCEDURE Proj_SP_InsertArtist
	-- Add the parameters for the stored procedure here
	@PerformerName nvarchar(100),
	@PerformerImage VARBINARY(MAX),
	@birthDate date
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @iden int
	insert into Proj_Performer(PerformerName, isABand, PerformerImage) values (@PerformerName, 0, @PerformerImage)
	set @iden = scope_identity()
	insert into Proj_Artist(ArtistID, birthDate) values (@iden, @birthDate)
END
GO

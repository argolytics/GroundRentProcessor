CREATE PROCEDURE [dbo].[spHowardCounty_CreateOrUpdateSDATScraper]
	@AccountId NCHAR(16),
    @IsGroundRent BIT,
	@PdfCount SMALLINT,
	@AllPdfsDownloaded BIT
AS
SET NOCOUNT ON;
	
BEGIN
	IF EXISTS (SELECT [AccountId] FROM dbo.[HowardCounty] 
	WHERE [AccountId] = @AccountId)
BEGIN
	UPDATE dbo.[HowardCounty] SET
	[AccountId] = @AccountId,
    [IsGroundRent] = @IsGroundRent,
	[PdfCount] = @PdfCount,
	[AllPdfsDownloaded] = @AllPdfsDownloaded
	WHERE [AccountId] = @AccountId
END
ELSE
BEGIN
	INSERT INTO dbo.[HowardCounty](
	[AccountId],
    [IsGroundRent],
	[PdfCount],
	[AllPdfsDownloaded])

	VALUES(
	@AccountId,
    @IsGroundRent,
	@PdfCount,
	@AllPdfsDownloaded)
END
END
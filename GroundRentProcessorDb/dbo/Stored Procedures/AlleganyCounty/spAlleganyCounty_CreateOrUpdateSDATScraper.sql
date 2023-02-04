CREATE PROCEDURE [dbo].[spAlleganyCounty_CreateOrUpdateSDATScraper]
	@AccountId NCHAR(16),
    @IsGroundRent BIT,
	@PdfCount SMALLINT,
	@AllPdfsDownloaded BIT
AS
SET NOCOUNT ON;
	
BEGIN
	IF EXISTS (SELECT [AccountId] FROM dbo.[AlleganyCounty] 
	WHERE [AccountId] = @AccountId)
BEGIN
	UPDATE dbo.[AlleganyCounty] SET
	[AccountId] = @AccountId,
    [IsGroundRent] = @IsGroundRent,
	[PdfCount] = @PdfCount,
	[AllPdfsDownloaded] = @AllPdfsDownloaded
	WHERE [AccountId] = @AccountId
END
ELSE
BEGIN
	INSERT INTO dbo.[AlleganyCounty](
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
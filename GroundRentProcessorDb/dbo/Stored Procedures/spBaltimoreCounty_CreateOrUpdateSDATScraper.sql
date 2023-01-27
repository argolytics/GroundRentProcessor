CREATE PROCEDURE [dbo].[spBaltimoreCounty_CreateOrUpdateSDATScraper]
	@AccountId NCHAR(16),
    @IsGroundRent BIT,
	@PdfDownloaded BIT
AS
SET NOCOUNT ON;
	
BEGIN
	IF EXISTS (SELECT [AccountId] FROM dbo.[BaltimoreCounty] 
	WHERE [AccountId] = @AccountId)
BEGIN
	UPDATE dbo.[BaltimoreCounty] SET
	[AccountId] = @AccountId,
    [IsGroundRent] = @IsGroundRent,
	[PdfDownloaded] = @PdfDownloaded
	WHERE [AccountId] = @AccountId
END
ELSE
BEGIN
	INSERT INTO dbo.[BaltimoreCounty](
	[AccountId],
    [IsGroundRent],
	[PdfDownloaded])

	VALUES(
	@AccountId,
    @IsGroundRent,
	@PdfDownloaded)
END
END
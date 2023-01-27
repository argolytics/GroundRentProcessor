CREATE PROCEDURE [dbo].[spBaltimoreCity_CreateOrUpdateSDATScraper]
	@AccountId NCHAR(16),
    @IsGroundRent BIT,
	@PdfDownloaded BIT
AS
SET NOCOUNT ON;
	
BEGIN
	IF EXISTS (SELECT [AccountId] FROM dbo.[BaltimoreCity] 
	WHERE [AccountId] = @AccountId)
BEGIN
	UPDATE dbo.[BaltimoreCity] SET
	[AccountId] = @AccountId,
    [IsGroundRent] = @IsGroundRent,
	[PdfDownloaded] = @PdfDownloaded
	WHERE [AccountId] = @AccountId
END
ELSE
BEGIN
	INSERT INTO dbo.[BaltimoreCity](
	[AccountId],
    [IsGroundRent],
	[PdfDownloaded])

	VALUES(
	@AccountId,
    @IsGroundRent,
	@PdfDownloaded)
END
END
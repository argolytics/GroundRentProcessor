CREATE PROCEDURE [dbo].[spCecilCounty_CreateOrUpdateSDATScraper]
	@AccountId NCHAR(16),
    @IsGroundRent BIT,
	@PdfDownloaded BIT
AS
SET NOCOUNT ON;
	
BEGIN
	IF EXISTS (SELECT [AccountId] FROM dbo.[CecilCounty] 
	WHERE [AccountId] = @AccountId)
BEGIN
	UPDATE dbo.[CecilCounty] SET
	[AccountId] = @AccountId,
    [IsGroundRent] = @IsGroundRent,
	[PdfDownloaded] = @PdfDownloaded
	WHERE [AccountId] = @AccountId
END
ELSE
BEGIN
	INSERT INTO dbo.[CecilCounty](
	[AccountId],
    [IsGroundRent],
	[PdfDownloaded])

	VALUES(
	@AccountId,
    @IsGroundRent,
	@PdfDownloaded)
END
END
CREATE PROCEDURE [dbo].[spMontgomeryCounty_CreateOrUpdateSDATScraper]
	@AccountId NCHAR(16),
    @IsGroundRent BIT,
	@PdfDownloaded BIT
AS
SET NOCOUNT ON;
	
BEGIN
	IF EXISTS (SELECT [AccountId] FROM dbo.[MontgomeryCounty] 
	WHERE [AccountId] = @AccountId)
BEGIN
	UPDATE dbo.[MontgomeryCounty] SET
	[AccountId] = @AccountId,
    [IsGroundRent] = @IsGroundRent,
	[PdfDownloaded] = @PdfDownloaded
	WHERE [AccountId] = @AccountId
END
ELSE
BEGIN
	INSERT INTO dbo.[MontgomeryCounty](
	[AccountId],
    [IsGroundRent],
	[PdfDownloaded])

	VALUES(
	@AccountId,
    @IsGroundRent,
	@PdfDownloaded)
END
END
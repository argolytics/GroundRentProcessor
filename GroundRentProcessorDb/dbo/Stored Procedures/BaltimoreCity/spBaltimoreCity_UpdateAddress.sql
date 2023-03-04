CREATE PROCEDURE [dbo].[spBaltimoreCity_UpdateAddress]
	@AccountId NCHAR(16),
	@County NCHAR (4),
	@AccountNumber NCHAR (16),
	@Ward NCHAR (2),
	@Section NCHAR (2),
	@Block NCHAR (5),
	@Lot NCHAR (4),
	@LandUseCode NCHAR (2),
	@YearBuilt SMALLINT,
	@IsGroundRent BIT,
	@IsRedeemed BIT,
	@PdfCount SMALLINT,
	@AllDataDownloaded BIT
AS
SET NOCOUNT ON;
BEGIN
	IF EXISTS (SELECT [Id] FROM dbo.[BaltimoreCity] 
	WHERE [AccountId] = @AccountId)
BEGIN
	UPDATE dbo.[BaltimoreCity] SET
	[AccountId] = @AccountId,
	[County] = @County,
	[AccountNumber] = @AccountNumber,
	[Ward] = @Ward,
	[Section] = @Section,
	[Block] = @Block,
	[Lot] = @Lot,
	[LandUseCode] = @LandUseCode,
	[YearBuilt] = @YearBuilt,
	[IsGroundRent] = @IsGroundRent,
	[IsRedeemed] = @IsRedeemed,
	[PdfCount] = @PdfCount,
	[AllDataDownloaded] = @AllDataDownloaded
	WHERE [AccountId] = @AccountId
END
END
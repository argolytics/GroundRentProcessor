CREATE PROCEDURE [dbo].[spBaltimoreCity_CreateAddress]
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
	@AllDataDownloaded BIT,
	@Id INT OUT
AS
SET NOCOUNT ON;
BEGIN
	INSERT INTO dbo.[BaltimoreCity](
	[AccountId],
	[County],
	[AccountNumber],
	[Ward],
	[Section],
	[Block],
	[Lot],
	[LandUseCode],
	[YearBuilt],
	[IsGroundRent],
	[IsRedeemed],
	[PdfCount],
	[AllDataDownloaded])

	VALUES(
	@AccountId,
	@County,
	@AccountNumber,
	@Ward,
	@Section,
	@Block,
	@Lot,
	@LandUseCode,
	@YearBuilt,
	@IsGroundRent,
	@IsRedeemed,
	@PdfCount,
	@AllDataDownloaded)

	SET @Id = SCOPE_IDENTITY();
END
CREATE PROCEDURE [dbo].[TestCreate]
@AccountId NCHAR(20),
	@AccountNumber NCHAR(16),
    @Ward NCHAR (2),
	@LandUseCode NCHAR (2),
	@YearBuilt SMALLINT
AS
BEGIN
SET NOCOUNT ON;
INSERT INTO dbo.[CecilCounty](
	[AccountId],
	[AccountNumber],
    [Ward],
	[LandUseCode],
	[YearBuilt])

	VALUES(
	@AccountId,
	@AccountNumber,
	@Ward,
	@LandUseCode,
	@YearBuilt)
END
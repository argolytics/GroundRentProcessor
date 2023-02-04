CREATE PROCEDURE [dbo].[spBaltimoreCity_CreateOrUpdateFile]
	@AccountId NCHAR(16),
    @Ward NCHAR (2),
	@Section  NCHAR (2),
    @Block  NCHAR (5),
    @Lot  NCHAR (4),
	@LandUseCode NCHAR (2),
	@YearBuilt SMALLINT
AS
SET NOCOUNT ON;
	
BEGIN
	IF EXISTS (SELECT [AccountId] FROM dbo.[BaltimoreCity] 
	WHERE [AccountId] = @AccountId)
BEGIN
	UPDATE dbo.[BaltimoreCity] SET
	[AccountId] = @AccountId,
    [Ward] = @Ward,
	[Section] = @Section,
	[Block] = @Block,
	[Lot] = @Lot,
	[LandUseCode] = @LandUseCode,
	[YearBuilt] = @YearBuilt
    
	WHERE [AccountId] = @AccountId
END
ELSE
BEGIN
	INSERT INTO dbo.[BaltimoreCity](
	[AccountId],
    [Ward],
	[Section],
	[Block],
	[Lot],
	[LandUseCode],
	[YearBuilt])

	VALUES(
	@AccountId,
	@Ward,
	@Section,
	@Block,
	@Lot,
	@LandUseCode,
	@YearBuilt)
END
END
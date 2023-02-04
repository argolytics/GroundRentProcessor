CREATE PROCEDURE [dbo].[spCarrollCounty_CreateOrUpdateFile]
	@AccountId NCHAR(16),
	@AccountNumber NCHAR(16),
    @Ward NCHAR (2),
	@LandUseCode NCHAR (2),
	@YearBuilt SMALLINT
AS
SET NOCOUNT ON;
	
BEGIN
	IF EXISTS (SELECT [AccountId] FROM dbo.[CarrollCounty] 
	WHERE [AccountId] = @AccountId)
BEGIN
	UPDATE dbo.[CarrollCounty] SET
	[AccountId] = @AccountId,
	[AccountNumber] = @AccountNumber,
    [Ward] = @Ward,
	[LandUseCode] = @LandUseCode,
	[YearBuilt] = @YearBuilt
    
	WHERE [AccountId] = @AccountId
END
ELSE
BEGIN
	INSERT INTO dbo.[CarrollCounty](
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
END
CREATE PROCEDURE [dbo].[spMontgomeryCounty_CreateOrUpdateSDATRedeemedFile]
@AccountId NCHAR(16),
@IsRedeemed BIT
AS
SET NOCOUNT ON;
	
BEGIN
	IF EXISTS (SELECT [AccountId] FROM dbo.[MontgomeryCounty] 
	WHERE [AccountId] = @AccountId)
BEGIN
	UPDATE dbo.[MontgomeryCounty] SET
	[AccountId] = @AccountId,
    [IsRedeemed] = @IsRedeemed
	WHERE [AccountId] = @AccountId
END
ELSE
BEGIN
	INSERT INTO dbo.[MontgomeryCounty](
	[AccountId],
    [IsGroundRent])

	VALUES(
	@AccountId,
    @IsRedeemed)
END
END
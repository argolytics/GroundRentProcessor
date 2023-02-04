CREATE PROCEDURE [dbo].[spBaltimoreCounty_CreateOrUpdateSDATRedeemedFile]
@AccountId NCHAR(16),
@IsRedeemed BIT
AS
SET NOCOUNT ON;
	
BEGIN
	IF EXISTS (SELECT [AccountId] FROM dbo.[BaltimoreCounty] 
	WHERE [AccountId] = @AccountId)
BEGIN
	UPDATE dbo.[BaltimoreCounty] SET
	[AccountId] = @AccountId,
    [IsRedeemed] = @IsRedeemed
	WHERE [AccountId] = @AccountId
END
ELSE
BEGIN
	INSERT INTO dbo.[BaltimoreCounty](
	[AccountId],
    [IsRedeemed])

	VALUES(
	@AccountId,
    @IsRedeemed)
END
END
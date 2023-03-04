CREATE PROCEDURE [dbo].[spBaltimoreCity_DeleteAddress]
	@AccountId NCHAR(16)
AS
BEGIN
	DELETE FROM dbo.[BaltimoreCity] WHERE AccountId = @AccountId;
END
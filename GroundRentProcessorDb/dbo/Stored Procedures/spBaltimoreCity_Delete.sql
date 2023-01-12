CREATE PROCEDURE [dbo].[spBaltimoreCity_Delete]
@AccountId nchar(16)
AS
BEGIN
	DELETE FROM dbo.[BaltimoreCity] WHERE AccountId = @AccountId;
END
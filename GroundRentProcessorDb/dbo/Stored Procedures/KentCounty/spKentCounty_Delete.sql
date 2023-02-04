CREATE PROCEDURE [dbo].[spKentCounty_Delete]
@AccountId nchar(16)
AS
BEGIN
	DELETE FROM dbo.[KentCounty] WHERE AccountId = @AccountId;
END
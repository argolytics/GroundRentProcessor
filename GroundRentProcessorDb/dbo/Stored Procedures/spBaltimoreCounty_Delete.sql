CREATE PROCEDURE [dbo].[spBaltimoreCounty_Delete]
@AccountId nchar(16)
AS
BEGIN
	DELETE FROM dbo.[BaltimoreCounty] WHERE AccountId = @AccountId;
END
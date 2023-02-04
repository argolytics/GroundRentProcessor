CREATE PROCEDURE [dbo].[spCalvertCounty_Delete]
@AccountId nchar(16)
AS
BEGIN
	DELETE FROM dbo.[CalvertCounty] WHERE AccountId = @AccountId;
END
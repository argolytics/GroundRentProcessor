CREATE PROCEDURE [dbo].[spCharlesCounty_Delete]
@AccountId nchar(16)
AS
BEGIN
	DELETE FROM dbo.[CharlesCounty] WHERE AccountId = @AccountId;
END
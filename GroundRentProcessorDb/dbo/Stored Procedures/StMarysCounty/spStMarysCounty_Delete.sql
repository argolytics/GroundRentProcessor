CREATE PROCEDURE [dbo].[spStMarysCounty_Delete]
@AccountId nchar(16)
AS
BEGIN
	DELETE FROM dbo.[StMarysCounty] WHERE AccountId = @AccountId;
END
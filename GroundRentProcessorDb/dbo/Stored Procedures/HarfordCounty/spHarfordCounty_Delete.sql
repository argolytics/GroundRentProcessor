CREATE PROCEDURE [dbo].[spHarfordCounty_Delete]
@AccountId nchar(16)
AS
BEGIN
	DELETE FROM dbo.[HarfordCounty] WHERE AccountId = @AccountId;
END
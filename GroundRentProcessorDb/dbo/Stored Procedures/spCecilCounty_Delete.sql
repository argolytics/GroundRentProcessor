CREATE PROCEDURE [dbo].[spCecilCounty_Delete]
@AccountId nchar(16)
AS
BEGIN
	DELETE FROM dbo.[CecilCounty] WHERE AccountId = @AccountId;
END
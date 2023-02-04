CREATE PROCEDURE [dbo].[spTalbotCounty_Delete]
@AccountId nchar(16)
AS
BEGIN
	DELETE FROM dbo.[TalbotCounty] WHERE AccountId = @AccountId;
END
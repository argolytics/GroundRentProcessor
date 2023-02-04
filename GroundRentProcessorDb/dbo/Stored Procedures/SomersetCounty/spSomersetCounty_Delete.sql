CREATE PROCEDURE [dbo].[spSomersetCounty_Delete]
@AccountId nchar(16)
AS
BEGIN
	DELETE FROM dbo.[SomersetCounty] WHERE AccountId = @AccountId;
END
CREATE PROCEDURE [dbo].[spWashingtonCounty_Delete]
@AccountId nchar(16)
AS
BEGIN
	DELETE FROM dbo.[WashingtonCounty] WHERE AccountId = @AccountId;
END
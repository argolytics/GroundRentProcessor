CREATE PROCEDURE [dbo].[spCarrollCounty_Delete]
@AccountId nchar(16)
AS
BEGIN
	DELETE FROM dbo.[CarrollCounty] WHERE AccountId = @AccountId;
END
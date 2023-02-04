CREATE PROCEDURE [dbo].[spWorcesterCounty_Delete]
@AccountId nchar(16)
AS
BEGIN
	DELETE FROM dbo.[WorcesterCounty] WHERE AccountId = @AccountId;
END
CREATE PROCEDURE [dbo].[spGarrettCounty_Delete]
@AccountId nchar(16)
AS
BEGIN
	DELETE FROM dbo.[GarrettCounty] WHERE AccountId = @AccountId;
END
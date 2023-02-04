CREATE PROCEDURE [dbo].[spCarolineCounty_Delete]
@AccountId nchar(16)
AS
BEGIN
	DELETE FROM dbo.[CarolineCounty] WHERE AccountId = @AccountId;
END
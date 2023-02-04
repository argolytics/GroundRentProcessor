CREATE PROCEDURE [dbo].[spWicomicoCounty_Delete]
@AccountId nchar(16)
AS
BEGIN
	DELETE FROM dbo.[WicomicoCounty] WHERE AccountId = @AccountId;
END
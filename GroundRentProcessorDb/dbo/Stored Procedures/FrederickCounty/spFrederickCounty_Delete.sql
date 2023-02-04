CREATE PROCEDURE [dbo].[spFrederickCounty_Delete]
@AccountId nchar(16)
AS
BEGIN
	DELETE FROM dbo.[FrederickCounty] WHERE AccountId = @AccountId;
END
CREATE PROCEDURE [dbo].[spPrinceGeorgesCounty_Delete]
@AccountId nchar(16)
AS
BEGIN
	DELETE FROM dbo.[PrinceGeorgesCounty] WHERE AccountId = @AccountId;
END
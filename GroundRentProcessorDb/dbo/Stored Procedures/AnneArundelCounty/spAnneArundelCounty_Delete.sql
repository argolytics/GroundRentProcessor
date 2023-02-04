CREATE PROCEDURE [dbo].[spAnneArundelCounty_Delete]
@AccountId nchar(16)
AS
BEGIN
	DELETE FROM dbo.[AnneArundelCounty] WHERE AccountId = @AccountId;
END
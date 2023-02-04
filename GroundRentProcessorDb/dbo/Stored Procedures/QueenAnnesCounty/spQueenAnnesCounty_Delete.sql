CREATE PROCEDURE [dbo].[spQueenAnnesCounty_Delete]
@AccountId nchar(16)
AS
BEGIN
	DELETE FROM dbo.[QueenAnnesCounty] WHERE AccountId = @AccountId;
END
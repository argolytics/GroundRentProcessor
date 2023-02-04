CREATE PROCEDURE [dbo].[spAlleganyCounty_Delete]
@AccountId nchar(16)
AS
BEGIN
	DELETE FROM dbo.[AlleganyCounty] WHERE AccountId = @AccountId;
END
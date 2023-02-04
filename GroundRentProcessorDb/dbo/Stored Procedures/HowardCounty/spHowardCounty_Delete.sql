CREATE PROCEDURE [dbo].[spHowardCounty_Delete]
@AccountId nchar(16)
AS
BEGIN
	DELETE FROM dbo.[HowardCounty] WHERE AccountId = @AccountId;
END
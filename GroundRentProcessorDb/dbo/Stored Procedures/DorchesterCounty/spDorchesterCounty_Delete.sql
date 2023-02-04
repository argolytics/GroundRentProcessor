CREATE PROCEDURE [dbo].[spDorchesterCounty_Delete]
@AccountId nchar(16)
AS
BEGIN
	DELETE FROM dbo.[DorchesterCounty] WHERE AccountId = @AccountId;
END
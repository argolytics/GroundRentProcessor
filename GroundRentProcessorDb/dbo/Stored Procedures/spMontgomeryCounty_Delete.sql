CREATE PROCEDURE [dbo].[spMontgomeryCounty_Delete]
@AccountId nchar(16)
AS
BEGIN
	DELETE FROM dbo.[MontgomeryCounty] WHERE AccountId = @AccountId;
END
CREATE PROCEDURE [dbo].[spMontgomeryCounty_ReadTopAmountWhereIsGroundRentNull]
@Amount int
AS
begin
	select top (@Amount) [AccountId], [AccountNumber], [Ward]
	
	FROM dbo.[MontgomeryCounty] where [IsGroundRent] is null
End
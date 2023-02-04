CREATE PROCEDURE [dbo].[spKentCounty_ReadTopAmountWhereIsGroundRentNull]
@Amount int
AS
begin
	select top (@Amount) [AccountId], [AccountNumber], [Ward]
	
	FROM dbo.[KentCounty] where [IsGroundRent] is null
End
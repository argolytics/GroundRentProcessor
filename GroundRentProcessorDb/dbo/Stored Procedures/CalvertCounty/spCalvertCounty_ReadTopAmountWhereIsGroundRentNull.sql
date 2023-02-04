CREATE PROCEDURE [dbo].[spCalvertCounty_ReadTopAmountWhereIsGroundRentNull]
@Amount int
AS
begin
	select top (@Amount) [AccountId], [AccountNumber], [Ward]
	
	FROM dbo.[CalvertCounty] where [IsGroundRent] is null
End
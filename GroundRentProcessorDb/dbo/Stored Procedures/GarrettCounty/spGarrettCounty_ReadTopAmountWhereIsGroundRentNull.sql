CREATE PROCEDURE [dbo].[spGarrettCounty_ReadTopAmountWhereIsGroundRentNull]
@Amount int
AS
begin
	select top (@Amount) [AccountId], [AccountNumber], [Ward]
	
	FROM dbo.[GarrettCounty] where [IsGroundRent] is null
End
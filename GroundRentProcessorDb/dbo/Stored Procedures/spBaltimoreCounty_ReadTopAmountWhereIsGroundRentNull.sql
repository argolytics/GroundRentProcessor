CREATE PROCEDURE [dbo].[spBaltimoreCounty_ReadTopAmountWhereIsGroundRentNull]
@Amount int
AS
begin
	select top (@Amount) [AccountId], [AccountNumber], [Ward]
	
	FROM dbo.[BaltimoreCounty] where [IsGroundRent] is null
End
CREATE PROCEDURE [dbo].[spCarolineCounty_ReadTopAmountWhereIsGroundRentNull]
@Amount int
AS
begin
	select top (@Amount) [AccountId], [AccountNumber], [Ward]
	
	FROM dbo.[CarolineCounty] where [IsGroundRent] is null
End
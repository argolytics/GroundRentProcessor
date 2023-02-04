CREATE PROCEDURE [dbo].[spWicomicoCounty_ReadTopAmountWhereIsGroundRentNull]
@Amount int
AS
begin
	select top (@Amount) [AccountId], [AccountNumber], [Ward]
	
	FROM dbo.[WicomicoCounty] where [IsGroundRent] is null
End
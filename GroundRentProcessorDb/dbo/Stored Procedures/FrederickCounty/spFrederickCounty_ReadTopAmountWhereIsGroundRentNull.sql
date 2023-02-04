CREATE PROCEDURE [dbo].[spFrederickCounty_ReadTopAmountWhereIsGroundRentNull]
@Amount int
AS
begin
	select top (@Amount) [AccountId], [AccountNumber], [Ward]
	
	FROM dbo.[FrederickCounty] where [IsGroundRent] is null
End
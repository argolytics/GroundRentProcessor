CREATE PROCEDURE [dbo].[spAnneArundelCounty_ReadTopAmountWhereIsGroundRentNull]
@Amount int
AS
begin
	select top (@Amount) [AccountId], [AccountNumber], [Ward]
	
	FROM dbo.[AnneArundelCounty] where [IsGroundRent] is null
End
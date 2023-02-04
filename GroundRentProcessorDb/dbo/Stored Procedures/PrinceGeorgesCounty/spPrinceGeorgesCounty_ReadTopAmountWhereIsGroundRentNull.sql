CREATE PROCEDURE [dbo].[spPrinceGeorgesCounty_ReadTopAmountWhereIsGroundRentNull]
@Amount int
AS
begin
	select top (@Amount) [AccountId], [AccountNumber], [Ward]
	
	FROM dbo.[PrinceGeorgesCounty] where [IsGroundRent] is null
End
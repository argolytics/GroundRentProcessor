CREATE PROCEDURE [dbo].[spQueenAnnesCounty_ReadTopAmountWhereIsGroundRentNull]
@Amount int
AS
begin
	select top (@Amount) [AccountId], [AccountNumber], [Ward]
	
	FROM dbo.[QueenAnnesCounty] where [IsGroundRent] is null
End
CREATE PROCEDURE [dbo].[spStMarysCounty_ReadTopAmountWhereIsGroundRentNull]
@Amount int
AS
begin
	select top (@Amount) [AccountId], [AccountNumber], [Ward]
	
	FROM dbo.[StMarysCounty] where [IsGroundRent] is null
End
CREATE PROCEDURE [dbo].[spCharlesCounty_ReadTopAmountWhereIsGroundRentNull]
@Amount int
AS
begin
	select top (@Amount) [AccountId], [AccountNumber], [Ward]
	
	FROM dbo.[CharlesCounty] where [IsGroundRent] is null
End
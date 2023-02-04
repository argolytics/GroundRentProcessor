CREATE PROCEDURE [dbo].[spAlleganyCounty_ReadTopAmountWhereIsGroundRentNull]
@Amount int
AS
begin
	select top (@Amount) [AccountId], [AccountNumber], [Ward]
	
	FROM dbo.[AlleganyCounty] where [IsGroundRent] is null
End
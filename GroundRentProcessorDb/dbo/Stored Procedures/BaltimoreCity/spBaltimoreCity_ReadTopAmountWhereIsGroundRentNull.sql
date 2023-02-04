CREATE PROCEDURE [dbo].[spBaltimoreCity_ReadTopAmountWhereIsGroundRentNull]
@Amount int
AS
begin
	select top (@Amount) [AccountId], [Ward], [Section], [Block], [Lot]
	
	FROM dbo.[BaltimoreCity] where [IsGroundRent] is null
End
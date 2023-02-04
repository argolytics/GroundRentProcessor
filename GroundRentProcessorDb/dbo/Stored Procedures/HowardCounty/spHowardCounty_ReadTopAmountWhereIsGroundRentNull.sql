CREATE PROCEDURE [dbo].[spHowardCounty_ReadTopAmountWhereIsGroundRentNull]
@Amount int
AS
begin
	select top (@Amount) [AccountId], [AccountNumber], [Ward]
	
	FROM dbo.[HowardCounty] where [IsGroundRent] is null
End
CREATE PROCEDURE [dbo].[spCarrollCounty_ReadTopAmountWhereIsGroundRentNull]
@Amount int
AS
begin
	select top (@Amount) [AccountId], [AccountNumber], [Ward]
	
	FROM dbo.[CarrollCounty] where [IsGroundRent] is null
End
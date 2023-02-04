CREATE PROCEDURE [dbo].[spSomersetCounty_ReadTopAmountWhereIsGroundRentNull]
@Amount int
AS
begin
	select top (@Amount) [AccountId], [AccountNumber], [Ward]
	
	FROM dbo.[SomersetCounty] where [IsGroundRent] is null
End
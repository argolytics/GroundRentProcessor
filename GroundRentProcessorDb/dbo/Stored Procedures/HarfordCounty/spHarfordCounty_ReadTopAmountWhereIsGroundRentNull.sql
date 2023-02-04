CREATE PROCEDURE [dbo].[spHarfordCounty_ReadTopAmountWhereIsGroundRentNull]
@Amount int
AS
begin
	select top (@Amount) [AccountId], [AccountNumber], [Ward]
	
	FROM dbo.[HarfordCounty] where [IsGroundRent] is null
End
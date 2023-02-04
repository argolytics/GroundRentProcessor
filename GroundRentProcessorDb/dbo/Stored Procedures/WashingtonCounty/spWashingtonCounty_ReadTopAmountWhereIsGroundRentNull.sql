CREATE PROCEDURE [dbo].[spWashingtonCounty_ReadTopAmountWhereIsGroundRentNull]
@Amount int
AS
begin
	select top (@Amount) [AccountId], [AccountNumber], [Ward]
	
	FROM dbo.[WashingtonCounty] where [IsGroundRent] is null
End
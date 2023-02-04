CREATE PROCEDURE [dbo].[spTalbotCounty_ReadTopAmountWhereIsGroundRentNull]
@Amount int
AS
begin
	select top (@Amount) [AccountId], [AccountNumber], [Ward]
	
	FROM dbo.[TalbotCounty] where [IsGroundRent] is null
End
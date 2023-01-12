CREATE PROCEDURE [dbo].[spCecilCounty_ReadTopAmountWhereIsGroundRentNull]
@Amount int
AS
begin
	select top (@Amount) [AccountId], [AccountNumber], [Ward]
	
	FROM dbo.[CecilCounty] where [IsGroundRent] is null
End
CREATE PROCEDURE [dbo].[spCecilCounty_ReadTopAmountWhereIsGroundRentTrue]
@Amount int
AS
begin
	select top (@Amount) [AccountId], [AccountNumber], [Ward]
	
	FROM dbo.[CecilCounty] where [IsGroundRent] = 1;
End
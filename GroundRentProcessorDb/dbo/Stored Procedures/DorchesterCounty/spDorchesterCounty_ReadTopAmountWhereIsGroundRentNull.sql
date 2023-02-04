CREATE PROCEDURE [dbo].[spDorchesterCounty_ReadTopAmountWhereIsGroundRentNull]
@Amount int
AS
begin
	select top (@Amount) [AccountId], [AccountNumber], [Ward]
	
	FROM dbo.[DorchesterCounty] where [IsGroundRent] is null
End
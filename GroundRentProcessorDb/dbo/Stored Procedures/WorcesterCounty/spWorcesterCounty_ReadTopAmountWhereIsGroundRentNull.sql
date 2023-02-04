CREATE PROCEDURE [dbo].[spWorcesterCounty_ReadTopAmountWhereIsGroundRentNull]
@Amount int
AS
begin
	select top (@Amount) [AccountId], [AccountNumber], [Ward]
	
	FROM dbo.[WorcesterCounty] where [IsGroundRent] is null
End
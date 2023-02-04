CREATE PROCEDURE [dbo].[spGarrettCounty_ReadTopAmountWhereIsGroundRentTrue]
@Amount int
AS
begin
	select top (@Amount) [AccountId], [AccountNumber], [Ward]
	
	FROM dbo.[GarrettCounty] where [IsGroundRent] = 1 and [AllPdfsDownloaded] is null;
End
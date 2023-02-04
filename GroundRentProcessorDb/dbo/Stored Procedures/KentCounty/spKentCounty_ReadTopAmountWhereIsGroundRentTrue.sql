CREATE PROCEDURE [dbo].[spKentCounty_ReadTopAmountWhereIsGroundRentTrue]
@Amount int
AS
begin
	select top (@Amount) [AccountId], [AccountNumber], [Ward]
	
	FROM dbo.[KentCounty] where [IsGroundRent] = 1 and [AllPdfsDownloaded] is null;
End
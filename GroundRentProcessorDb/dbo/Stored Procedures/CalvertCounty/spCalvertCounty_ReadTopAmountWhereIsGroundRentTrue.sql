CREATE PROCEDURE [dbo].[spCalvertCounty_ReadTopAmountWhereIsGroundRentTrue]
@Amount int
AS
begin
	select top (@Amount) [AccountId], [AccountNumber], [Ward]
	
	FROM dbo.[CalvertCounty] where [IsGroundRent] = 1 and [AllPdfsDownloaded] is null;
End
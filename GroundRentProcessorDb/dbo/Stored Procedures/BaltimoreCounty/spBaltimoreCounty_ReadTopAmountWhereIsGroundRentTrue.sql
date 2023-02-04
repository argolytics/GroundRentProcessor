CREATE PROCEDURE [dbo].[spBaltimoreCounty_ReadTopAmountWhereIsGroundRentTrue]
@Amount int
AS
begin
	select top (@Amount) [AccountId], [AccountNumber], [Ward]
	
	FROM dbo.[BaltimoreCounty] where [IsGroundRent] = 1 and [AllPdfsDownloaded] is null;
End
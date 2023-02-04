CREATE PROCEDURE [dbo].[spCarolineCounty_ReadTopAmountWhereIsGroundRentTrue]
@Amount int
AS
begin
	select top (@Amount) [AccountId], [AccountNumber], [Ward]
	
	FROM dbo.[CarolineCounty] where [IsGroundRent] = 1 and [AllPdfsDownloaded] is null;
End
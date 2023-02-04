CREATE PROCEDURE [dbo].[spHarfordCounty_ReadTopAmountWhereIsGroundRentTrue]
@Amount int
AS
begin
	select top (@Amount) [AccountId], [AccountNumber], [Ward]
	
	FROM dbo.[HarfordCounty] where [IsGroundRent] = 1 and [AllPdfsDownloaded] is null;
End
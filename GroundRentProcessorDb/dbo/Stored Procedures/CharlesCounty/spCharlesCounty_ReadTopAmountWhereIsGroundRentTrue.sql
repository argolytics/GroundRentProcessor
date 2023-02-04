CREATE PROCEDURE [dbo].[spCharlesCounty_ReadTopAmountWhereIsGroundRentTrue]
@Amount int
AS
begin
	select top (@Amount) [AccountId], [AccountNumber], [Ward]
	
	FROM dbo.[CharlesCounty] where [IsGroundRent] = 1 and [AllPdfsDownloaded] is null;
End
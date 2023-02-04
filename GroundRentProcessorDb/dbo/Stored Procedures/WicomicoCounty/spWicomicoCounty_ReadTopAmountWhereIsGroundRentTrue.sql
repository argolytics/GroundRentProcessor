CREATE PROCEDURE [dbo].[spWicomicoCounty_ReadTopAmountWhereIsGroundRentTrue]
@Amount int
AS
begin
	select top (@Amount) [AccountId], [AccountNumber], [Ward]
	
	FROM dbo.[WicomicoCounty] where [IsGroundRent] = 1 and [AllPdfsDownloaded] is null;
End
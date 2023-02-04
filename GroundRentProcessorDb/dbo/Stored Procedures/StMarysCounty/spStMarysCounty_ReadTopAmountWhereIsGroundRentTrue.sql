CREATE PROCEDURE [dbo].[spStMarysCounty_ReadTopAmountWhereIsGroundRentTrue]
@Amount int
AS
begin
	select top (@Amount) [AccountId], [AccountNumber], [Ward]
	
	FROM dbo.[StMarysCounty] where [IsGroundRent] = 1 and [AllPdfsDownloaded] is null;
End
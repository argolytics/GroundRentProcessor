CREATE PROCEDURE [dbo].[spTalbotCounty_ReadTopAmountWhereIsGroundRentTrue]
@Amount int
AS
begin
	select top (@Amount) [AccountId], [AccountNumber], [Ward]
	
	FROM dbo.[TalbotCounty] where [IsGroundRent] = 1 and [AllPdfsDownloaded] is null;
End
CREATE PROCEDURE [dbo].[spAnneArundelCounty_ReadTopAmountWhereIsGroundRentTrue]
@Amount int
AS
begin
	select top (@Amount) [AccountId], [AccountNumber], [Ward]
	
	FROM dbo.[AnneArundelCounty] where [IsGroundRent] = 1 and [AllPdfsDownloaded] is null;
End
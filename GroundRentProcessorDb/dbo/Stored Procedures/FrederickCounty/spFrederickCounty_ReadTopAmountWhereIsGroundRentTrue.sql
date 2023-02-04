CREATE PROCEDURE [dbo].[spFrederickCounty_ReadTopAmountWhereIsGroundRentTrue]
@Amount int
AS
begin
	select top (@Amount) [AccountId], [AccountNumber], [Ward]
	
	FROM dbo.[FrederickCounty] where [IsGroundRent] = 1 and [AllPdfsDownloaded] is null;
End
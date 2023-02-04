CREATE PROCEDURE [dbo].[spAlleganyCounty_ReadTopAmountWhereIsGroundRentTrue]
@Amount int
AS
begin
	select top (@Amount) [AccountId], [AccountNumber], [Ward]
	
	FROM dbo.[AlleganyCounty] where [IsGroundRent] = 1 and [AllPdfsDownloaded] is null;
End
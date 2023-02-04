CREATE PROCEDURE [dbo].[spPrinceGeorgesCounty_ReadTopAmountWhereIsGroundRentTrue]
@Amount int
AS
begin
	select top (@Amount) [AccountId], [AccountNumber], [Ward]
	
	FROM dbo.[PrinceGeorgesCounty] where [IsGroundRent] = 1 and [AllPdfsDownloaded] is null;
End
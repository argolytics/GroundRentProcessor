CREATE PROCEDURE [dbo].[spQueenAnnesCounty_ReadTopAmountWhereIsGroundRentTrue]
@Amount int
AS
begin
	select top (@Amount) [AccountId], [AccountNumber], [Ward]
	
	FROM dbo.[QueenAnnesCounty] where [IsGroundRent] = 1 and [AllPdfsDownloaded] is null;
End
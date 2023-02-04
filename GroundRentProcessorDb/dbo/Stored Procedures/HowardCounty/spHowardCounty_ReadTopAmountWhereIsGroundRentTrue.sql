CREATE PROCEDURE [dbo].[spHowardCounty_ReadTopAmountWhereIsGroundRentTrue]
@Amount int
AS
begin
	select top (@Amount) [AccountId], [AccountNumber], [Ward]
	
	FROM dbo.[HowardCounty] where [IsGroundRent] = 1 and [AllPdfsDownloaded] is null;
End
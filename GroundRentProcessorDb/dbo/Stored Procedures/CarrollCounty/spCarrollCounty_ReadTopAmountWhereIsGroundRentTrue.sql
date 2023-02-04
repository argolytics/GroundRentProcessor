CREATE PROCEDURE [dbo].[spCarrollCounty_ReadTopAmountWhereIsGroundRentTrue]
@Amount int
AS
begin
	select top (@Amount) [AccountId], [AccountNumber], [Ward]
	
	FROM dbo.[CarrollCounty] where [IsGroundRent] = 1 and [AllPdfsDownloaded] is null;
End
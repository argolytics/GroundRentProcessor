CREATE PROCEDURE [dbo].[spWorcesterCounty_ReadTopAmountWhereIsGroundRentTrue]
@Amount int
AS
begin
	select top (@Amount) [AccountId], [AccountNumber], [Ward]
	
	FROM dbo.[WorcesterCounty] where [IsGroundRent] = 1 and [AllPdfsDownloaded] is null;
End
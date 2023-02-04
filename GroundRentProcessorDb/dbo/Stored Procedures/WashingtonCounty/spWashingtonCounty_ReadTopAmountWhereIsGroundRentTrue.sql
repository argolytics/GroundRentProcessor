CREATE PROCEDURE [dbo].[spWashingtonCounty_ReadTopAmountWhereIsGroundRentTrue]
@Amount int
AS
begin
	select top (@Amount) [AccountId], [AccountNumber], [Ward]
	
	FROM dbo.[WashingtonCounty] where [IsGroundRent] = 1 and [AllPdfsDownloaded] is null;
End
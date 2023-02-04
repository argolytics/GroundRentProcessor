CREATE PROCEDURE [dbo].[spBaltimoreCity_ReadTopAmountWhereIsGroundRentTrue]
@Amount int
AS
begin
	select top (@Amount) [AccountId], [Ward], [Section], [Block], [Lot]
	
	FROM dbo.[BaltimoreCity] where [IsGroundRent] = 1 and [AllPdfsDownloaded] is null;
End
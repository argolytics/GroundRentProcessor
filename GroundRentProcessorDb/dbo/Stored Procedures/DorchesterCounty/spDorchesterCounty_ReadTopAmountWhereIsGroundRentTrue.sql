CREATE PROCEDURE [dbo].[spDorchesterCounty_ReadTopAmountWhereIsGroundRentTrue]
@Amount int
AS
begin
	select top (@Amount) [AccountId], [AccountNumber], [Ward]
	
	FROM dbo.[DorchesterCounty] where [IsGroundRent] = 1 and [AllPdfsDownloaded] is null;
End
CREATE PROCEDURE [dbo].[spMontgomeryCounty_ReadTopAmountWhereIsGroundRentTrue]
@Amount int
AS
begin
	select top (@Amount) [AccountId], [AccountNumber], [Ward]
	
	FROM dbo.[MontgomeryCounty] where [IsGroundRent] = 1 and [PdfDownloaded] is null;
End
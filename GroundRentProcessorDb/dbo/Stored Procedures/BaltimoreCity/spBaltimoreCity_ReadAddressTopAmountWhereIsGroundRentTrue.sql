CREATE PROCEDURE [dbo].[spBaltimoreCity_ReadAddressTopAmountWhereIsGroundRentTrue]
@Amount INT
AS
begin
	select top (@Amount)
	[Id],
	[AccountId],
	[County],
	[AccountNumber],
	[Ward],
	[Section],
	[Block],
	[Lot],
	[LandUseCode],
	[YearBuilt],
	[IsGroundRent],
	[IsRedeemed],
	[PdfCount],
	[AllDataDownloaded]
	
	FROM dbo.[BaltimoreCity] where [IsGroundRent] = 1 and [AllDataDownloaded] is null;
End
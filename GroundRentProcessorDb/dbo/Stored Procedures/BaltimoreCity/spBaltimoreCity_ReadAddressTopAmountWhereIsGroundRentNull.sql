CREATE PROCEDURE [dbo].[spBaltimoreCity_ReadAddressTopAmountWhereIsGroundRentNull]
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
	
	FROM dbo.[BaltimoreCity] where [IsGroundRent] is null
End
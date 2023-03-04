CREATE PROCEDURE [dbo].[spBaltimoreCity_UpdateGroundRentPdf]
	@AccountId NCHAR(16), 
    @AddressId INT,
    @AcknowledgementNumber NVARCHAR(32), 
    @DocumentFiledType NVARCHAR(16), 
    @DateTimeFiled SMALLDATETIME, 
    @PdfPageCount NVARCHAR(3), 
    @Book NVARCHAR(10), 
    @Page NVARCHAR(10), 
    @ClerkInitials NVARCHAR(10), 
    @YearRecorded SMALLINT

AS
SET NOCOUNT ON;
BEGIN
	IF EXISTS (SELECT [Id] FROM dbo.[BaltimoreCityGroundRentPdf] 
    WHERE [AccountId] = @AccountId AND [DateTimeFiled] = @DateTimeFiled)
BEGIN
	UPDATE dbo.[BaltimoreCityGroundRentPdf] SET
	[AccountId] = @AccountId, 
    [AddressId] = @AddressId, 
    [DocumentFiledType] = @DocumentFiledType, 
    [AcknowledgementNumber] = @AcknowledgementNumber, 
    [DateTimeFiled] = @DateTimeFiled, 
    [PdfPageCount] = @PdfPageCount, 
    [Book] = @Book, 
    [Page] = @Page, 
    [ClerkInitials] = @ClerkInitials, 
    [YearRecorded] = @YearRecorded
	WHERE [AccountId] = @AccountId AND [DateTimeFiled] = @DateTimeFiled
END
END
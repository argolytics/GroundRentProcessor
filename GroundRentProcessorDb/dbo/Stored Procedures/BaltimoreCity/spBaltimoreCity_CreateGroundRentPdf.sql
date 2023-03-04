CREATE PROCEDURE [dbo].[spBaltimoreCity_CreateGroundRentPdf]
    @AddressId INT,
    @AccountId NCHAR(16), 
    @AcknowledgementNumber NVARCHAR(32), 
    @DocumentFiledType NVARCHAR(16), 
    @DateTimeFiled SMALLDATETIME, 
    @PdfPageCount NVARCHAR(3), 
    @Book NVARCHAR(10), 
    @Page NVARCHAR(10), 
    @ClerkInitials NVARCHAR(10), 
    @YearRecorded SMALLINT,
    @Id INT OUT
AS
SET NOCOUNT ON;
BEGIN
	INSERT INTO dbo.[BaltimoreCityGroundRentPdf](
	[AddressId],
    [AccountId], 
    [DocumentFiledType], 
    [AcknowledgementNumber], 
    [DateTimeFiled], 
    [PdfPageCount], 
    [Book], 
    [Page], 
    [ClerkInitials], 
    [YearRecorded])

	VALUES(
    @AddressId,
	@AccountId, 
    @DocumentFiledType, 
    @AcknowledgementNumber, 
    @DateTimeFiled, 
    @PdfPageCount, 
    @Book, 
    @Page, 
    @ClerkInitials, 
    @YearRecorded)

    SET @Id = SCOPE_IDENTITY();
END
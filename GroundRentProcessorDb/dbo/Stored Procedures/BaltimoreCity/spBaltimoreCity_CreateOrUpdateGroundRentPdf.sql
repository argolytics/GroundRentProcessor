CREATE PROCEDURE [dbo].[spBaltimoreCity_CreateOrUpdateGroundRentPdf]
	@AccountId NCHAR(16) NULL, 
    @DocumentFiledType NCHAR(16) NULL, 
    @AcknowledgementNumber NCHAR(32) NULL, 
    @DateTimeFiled SMALLDATETIME NULL, 
    @DateTimeFiledString NVARCHAR(24) NULL, 
    @PdfPageCount NVARCHAR(3) NULL, 
    @Book NVARCHAR(10) NULL, 
    @Page NVARCHAR(10) NULL, 
    @ClerkInitials NVARCHAR(10) NULL, 
    @YearRecorded SMALLINT NULL,
	@Id INT OUTPUT
AS
SET NOCOUNT ON;
	
BEGIN
	IF EXISTS (SELECT [AccountId] FROM dbo.[BaltimoreCityGroundRentPdf] 
	WHERE [Id] = @Id)
BEGIN
	UPDATE dbo.[BaltimoreCityGroundRentPdf] SET
	[AccountId] = @AccountId, 
    [DocumentFiledType] = @DocumentFiledType, 
    [AcknowledgementNumber] = @AcknowledgementNumber, 
    [DateTimeFiled] = @DateTimeFiled, 
    [DateTimeFiledString] = @DateTimeFiledString, 
    [PdfPageCount] = @PdfPageCount, 
    [Book] = @Book, 
    [Page] = @Page, 
    [ClerkInitials] = @ClerkInitials, 
    [YearRecorded] = @YearRecorded
	WHERE [AcknowledgementNumber] = @AcknowledgementNumber
END
ELSE
BEGIN
	INSERT INTO dbo.[BaltimoreCityGroundRentPdf](
	[AccountId], 
    [DocumentFiledType], 
    [AcknowledgementNumber], 
    [DateTimeFiled], 
    [DateTimeFiledString], 
    [PdfPageCount], 
    [Book], 
    [Page], 
    [ClerkInitials], 
    [YearRecorded])

	VALUES(
	@AccountId, 
    @DocumentFiledType, 
    @AcknowledgementNumber, 
    @DateTimeFiled, 
    @DateTimeFiledString, 
    @PdfPageCount, 
    @Book, 
    @Page, 
    @ClerkInitials, 
    @YearRecorded)

    SET @Id = SCOPE_IDENTITY();
END
END
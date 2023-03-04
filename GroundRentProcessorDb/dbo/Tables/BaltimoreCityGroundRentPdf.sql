CREATE TABLE [dbo].[BaltimoreCityGroundRentPdf]
(
    [Id] INT IDENTITY(1,1) NOT NULL,
    [AddressId] INT NULL,
    [AcknowledgementNumber] NVARCHAR(32) NULL,
    [AccountId] NCHAR(16) NULL,
    [DocumentFiledType] NVARCHAR(16) NULL, 
    [DateTimeFiled] SMALLDATETIME NULL,  
    [PdfPageCount] NVARCHAR(3) NULL, 
    [Book] NVARCHAR(10) NULL, 
    [Page] NVARCHAR(10) NULL, 
    [ClerkInitials] NVARCHAR(10) NULL, 
    [YearRecorded] SMALLINT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_BaltimoreCity_BaltimoreCityGroundRentPdf] FOREIGN KEY ([AddressId]) REFERENCES [dbo].[BaltimoreCity] ([Id]) ON DELETE CASCADE
)

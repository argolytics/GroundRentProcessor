CREATE TABLE [dbo].[BaltimoreCityGroundRentPdf]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [AccountId] NCHAR(16) NULL, 
    [DocumentFiledType] NCHAR(16) NULL, 
    [AcknowledgementNumber] NCHAR(32) NULL, 
    [DateTimeFiled] SMALLDATETIME NULL, 
    [DateTimeFiledString] NVARCHAR(24) NULL, 
    [PageAmount] NVARCHAR(3) NULL, 
    [Book] NVARCHAR(10) NULL, 
    [Page] NVARCHAR(10) NULL, 
    [ClerkInitials] NVARCHAR(10) NULL, 
    [YearRecorded] SMALLINT NULL,
    CONSTRAINT [FK_BaltimoreCity_BaltimoreCityGroundRentPdf] FOREIGN KEY ([AccountId]) REFERENCES [dbo].[BaltimoreCity] ([AccountId])
)

﻿CREATE TABLE [dbo].[BACI] (
    [Id] INT IDENTITY(1,1) NOT NULL,
    [AccountId]     NCHAR(16) NULL,
    [County]        NCHAR (4) NULL,
    [AccountNumber] NCHAR (16) NULL,
    [Ward]          NCHAR (2)  NULL,
    [Section]       NCHAR (2)  NULL,
    [Block]         NCHAR (5)  NULL,
    [Lot]           NCHAR (4)  NULL,
    [LandUseCode]   NCHAR (2)  NULL,
    [YearBuilt]     SMALLINT   NULL,
    [IsGroundRent]  BIT        NULL,
    [IsRedeemed]    BIT        NULL,
    [PdfCount]      SMALLINT   NULL,
    [AllDataDownloaded] BIT    NULL
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


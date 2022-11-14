CREATE TABLE [dbo].[Languages] (
    [LanguageId]        INT           IDENTITY (1, 1) NOT NULL,
    [Name]              NVARCHAR (50) NOT NULL,
    [LanguageCode]      NVARCHAR (10) NOT NULL,
    [Tolerance] FLOAT (53)    NULL,
    [ToleranceType] INT           NULL,
    [Description] varchar(1000) null,
    [IsDefault]  BIT            NOT NULL DEFAULT 0,
    [CreatedAt]         DATETIME2 (7) NOT NULL,
    [UpdatedAt]         DATETIME2 (7) NULL,
    [CreatedBy]         INT           NOT NULL,
    [UpdatedBy]         INT           NOT NULL,
    CONSTRAINT [PK_Languages] PRIMARY KEY CLUSTERED ([LanguageId] ASC)
);


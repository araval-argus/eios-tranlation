CREATE TABLE [dbo].[Labels] (
    [LabelId]               INT             IDENTITY (1, 1) NOT NULL,
    [ResourceId]            NVARCHAR (1000) NOT NULL,
    [FK_LabelGroupId]       INT             NOT NULL,
    [FK_LanguageId]         INT             NOT NULL,
    [FK_BaseLabelId]        INT             NULL,
    [LabelValue]            NVARCHAR (1000) NULL,
    [LabelType]             INT             NOT NULL,
    [LabelDescription]      NVARCHAR (MAX)  NULL,
    [LabelSnapshotPath]     NVARCHAR (MAX)  NULL,
    [MachineTranslation]    NVARCHAR (1000) NULL,
    [Scope]                 NVARCHAR (50)   NULL,
    [TranslationStatus]     INT             NOT NULL,
    [Version]               INT             NOT NULL,
    [IsActive]              BIT             NOT NULL,
    [FK_PrevVersionLabelId] INT             NULL,
    [CreatedAt]             DATETIME2 (7)   NOT NULL,
    [UpdatedAt]             DATETIME2 (7)   NULL,
    [CreatedBy]             INT             NOT NULL,
    [UpdatedBy]             INT             NOT NULL,
    CONSTRAINT [PK_Labels] PRIMARY KEY CLUSTERED ([LabelId] ASC),
    CONSTRAINT [FK_Labels_LabelGroups] FOREIGN KEY ([FK_LabelGroupId]) REFERENCES [dbo].[LabelGroups] ([LabelGroupId]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_Labels_Labels] FOREIGN KEY ([FK_PrevVersionLabelId]) REFERENCES [dbo].[Labels] ([LabelId]),
    --CONSTRAINT [FK_Labels_Labels2] FOREIGN KEY ([FK_BaseLabelId]) REFERENCES [dbo].[Labels] ([LabelId])  ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_Labels_Languages] FOREIGN KEY ([FK_LanguageId]) REFERENCES [dbo].[Languages] ([LanguageId]) ON DELETE CASCADE ON UPDATE CASCADE
);




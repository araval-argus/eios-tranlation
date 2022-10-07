CREATE TABLE [dbo].[LabelGroups] (
    [LabelGroupId]          INT            IDENTITY (1, 1) NOT NULL,
    [GroupName]             NVARCHAR (100) NOT NULL,
    [FK_ParentLableGroupId] INT            NULL,
    [CreatedAt]             DATETIME2 (7)  NOT NULL,
    [UpdatedAt]             DATETIME2 (7)  NULL,
    [CreatedBy]             INT            NOT NULL,
    [UpdatedBy]             INT            NOT NULL,
    CONSTRAINT [PK_LabelGroups] PRIMARY KEY CLUSTERED ([LabelGroupId] ASC),
    CONSTRAINT [FK_LabelGroups_LabelGroups1] FOREIGN KEY ([FK_ParentLableGroupId]) REFERENCES [dbo].[LabelGroups] ([LabelGroupId])
);


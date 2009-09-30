CREATE TABLE [dbo].[Tokens]
(
[TokenID] [int] NOT NULL IDENTITY(1, 1),
[CampaignID] [int] NOT NULL,
[Code] [varchar] (10) COLLATE Latin1_General_CI_AS NOT NULL,
[AllowedRedemptions] [int] NOT NULL CONSTRAINT [DF_Tokens_AllowedRedemptions] DEFAULT ((1)),
[FileName] [varchar] (100) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_Tokens_FileName] DEFAULT (''),
[Text] [varchar] (100) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_Tokens_Text] DEFAULT ('')
)


GO
ALTER TABLE [dbo].[Tokens] ADD CONSTRAINT [CX_Token] UNIQUE NONCLUSTERED  ([Code])



GO

ALTER TABLE [dbo].[Tokens] ADD CONSTRAINT [PK_Tokens] PRIMARY KEY CLUSTERED  ([TokenID]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Tokens] ADD CONSTRAINT [FK_Tokens_Campaigns] FOREIGN KEY ([CampaignID]) REFERENCES [dbo].[Campaigns] ([CampaignID])
GO


-- PLEASE NOTE THAT BELOW IS THE SCRIPTED MIGRATIONS : YOU CAN COMMIT THIS OR U CAN JUST RUN UPDATE DATABASE COMMAND :) 
-- THIS CODE WAS GENERATED USING THIS COMMAND : script-migration "20241110112335_RemovedPlayerEntity"







BEGIN TRANSACTION;
GO

CREATE TABLE [Players] (
    [AccountId] uniqueidentifier NOT NULL,
    [Username] nvarchar(100) NOT NULL,
    [CountryCode] nvarchar(2) NULL,
    [CreatedDate] datetime2 NOT NULL,
    [LastModifiedDate] datetime2 NOT NULL,
    CONSTRAINT [PK_Players] PRIMARY KEY ([AccountId])
);
GO

CREATE TABLE [Providers] (
    [ProviderId] uniqueidentifier NOT NULL,
    [ProviderName] nvarchar(100) NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    [LastModifiedDate] datetime2 NOT NULL,
    CONSTRAINT [PK_Providers] PRIMARY KEY ([ProviderId])
);
GO

CREATE TABLE [PlayerStats] (
    [AccountId] uniqueidentifier NOT NULL,
    [TotalAmount] decimal(18,4) NOT NULL,
    [WagerCount] int NOT NULL,
    [LastWagerDateTime] datetime2 NULL,
    [LastCalculatedDateTime] datetime2 NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    [LastModifiedDate] datetime2 NOT NULL,
    CONSTRAINT [PK_PlayerStats] PRIMARY KEY ([AccountId]),
    CONSTRAINT [FK_PlayerStats_Players_AccountId] FOREIGN KEY ([AccountId]) REFERENCES [Players] ([AccountId]) ON DELETE CASCADE
);
GO

CREATE TABLE [Games] (
    [GameId] uniqueidentifier NOT NULL,
    [ProviderId] uniqueidentifier NOT NULL,
    [GameName] nvarchar(100) NOT NULL,
    [Theme] nvarchar(50) NULL,
    [CreatedDate] datetime2 NOT NULL,
    [LastModifiedDate] datetime2 NOT NULL,
    CONSTRAINT [PK_Games] PRIMARY KEY ([GameId]),
    CONSTRAINT [FK_Games_Providers_ProviderId] FOREIGN KEY ([ProviderId]) REFERENCES [Providers] ([ProviderId]) ON DELETE NO ACTION
);
GO

CREATE TABLE [CasinoWagers] (
    [WagerId] uniqueidentifier NOT NULL,
    [AccountId] uniqueidentifier NOT NULL,
    [GameId] uniqueidentifier NOT NULL,
    [TransactionId] uniqueidentifier NOT NULL,
    [BrandId] uniqueidentifier NOT NULL,
    [ExternalReferenceId] uniqueidentifier NULL,
    [TransactionTypeId] uniqueidentifier NULL,
    [Amount] decimal(18,4) NOT NULL,
    [NumberOfBets] int NOT NULL,
    [SessionData] nvarchar(max) NULL,
    [Duration] bigint NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    [LastModifiedDate] datetime2 NOT NULL,
    CONSTRAINT [PK_CasinoWagers] PRIMARY KEY ([WagerId]),
    CONSTRAINT [FK_CasinoWagers_Games_GameId] FOREIGN KEY ([GameId]) REFERENCES [Games] ([GameId]) ON DELETE NO ACTION,
    CONSTRAINT [FK_CasinoWagers_Players_AccountId] FOREIGN KEY ([AccountId]) REFERENCES [Players] ([AccountId]) ON DELETE NO ACTION
);
GO

CREATE INDEX [IX_CasinoWagers_AccountId_CreatedDate] ON [CasinoWagers] ([AccountId], [CreatedDate]);
GO

CREATE INDEX [IX_CasinoWagers_Amount] ON [CasinoWagers] ([Amount]);
GO

CREATE INDEX [IX_CasinoWagers_GameId] ON [CasinoWagers] ([GameId]);
GO

CREATE INDEX [IX_CasinoWagers_TransactionId] ON [CasinoWagers] ([TransactionId]);
GO

CREATE UNIQUE INDEX [IX_Games_ProviderId_GameName] ON [Games] ([ProviderId], [GameName]);
GO

CREATE UNIQUE INDEX [IX_Players_Username] ON [Players] ([Username]);
GO

CREATE INDEX [IX_PlayerStats_TotalAmount] ON [PlayerStats] ([TotalAmount]);
GO

CREATE UNIQUE INDEX [IX_Providers_ProviderName] ON [Providers] ([ProviderName]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20241110114726_AddedEntitiesMapping', N'8.0.10');
GO

COMMIT;

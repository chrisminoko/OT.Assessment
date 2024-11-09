IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Players] (
    [Id] uniqueidentifier NOT NULL,
    [Username] nvarchar(450) NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    [LastModifiedDate] datetime2 NOT NULL,
    CONSTRAINT [PK_Players] PRIMARY KEY ([Id])
);
GO

CREATE UNIQUE INDEX [IX_Players_Username] ON [Players] ([Username]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20241109103755_InitialCreate', N'8.0.10');
GO

COMMIT;
GO


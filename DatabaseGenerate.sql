
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



-- ========================Added a new collumn on [CasinoWagers] table ==========
BEGIN TRANSACTION;
GO

ALTER TABLE [CasinoWagers] ADD [GameName] nvarchar(max) NOT NULL DEFAULT N'';
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20241111065905_AddedGameNameColoumn', N'8.0.10');
GO

COMMIT;
GO
-- ========================Added a new collumn on [CasinoWagers] table ==========

-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Chris Minoko
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetPlayerCasinoWagers]
	-- Add the parameters for the stored procedure here
    @PlayerId UNIQUEIDENTIFIER,
    @PageSize INT = 10,
    @Page INT = 1,
    @TotalRecords INT OUTPUT,
    @TotalPages INT OUTPUT
AS
BEGIN
	
	SET NOCOUNT ON;
	IF @PageSize <= 0
        SET @PageSize = 10;
        
    IF @Page <= 0
        SET @Page = 1;

	SELECT @TotalRecords = COUNT(1)
    FROM CasinoWagers cw
    WHERE cw.AccountId = @PlayerId;
  
   SET @TotalPages = CEILING(CAST(@TotalRecords AS FLOAT) / @PageSize);

       ;WITH PaginatedWagers AS (
        SELECT 
            cw.WagerId,
            g.GameName AS Game,
            p.ProviderName AS Provider,
            cw.Amount,
            cw.CreatedDate ,
            ROW_NUMBER() OVER (ORDER BY cw.CreatedDate DESC) AS RowNum
        FROM CasinoWagers cw
        INNER JOIN Games g ON cw.GameId = g.GameId
        INNER JOIN Providers p ON g.ProviderId = p.ProviderId
        WHERE cw.AccountId = @PlayerId
    )
    SELECT 
        WagerId,
        Game,
        Provider,
        Amount,
        CreatedDate
    FROM PaginatedWagers
    WHERE RowNum BETWEEN ((@Page - 1) * @PageSize + 1) AND (@Page * @PageSize)
    ORDER BY CreatedDate DESC;

END
GO



CREATE PROCEDURE [dbo].[sp_GetTopSpenders]
	-- Add the parameters for the stored procedure here
  @Count INT = 10
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	 IF @Count <= 0
        SET @Count = 10;
    -- Insert statements for procedure here
	 SELECT TOP (@Count)
        p.AccountId,
        p.Username,
        ps.TotalAmount as TotalAmountSpend
    FROM PlayerStats ps
    INNER JOIN Players p ON ps.AccountId = p.AccountId
    ORDER BY ps.TotalAmount DESC;
END
GO


CREATE PROCEDURE [dbo].[sp_UpdatePlayerStats]
	-- Add the parameters for the stored procedure here
  @AccountId UNIQUEIDENTIFIER,
  @Amount DECIMAL(18,4)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	MERGE PlayerStats AS target
    USING (SELECT @AccountId AS AccountId, @Amount AS Amount) AS source
    ON target.AccountId = source.AccountId
    WHEN MATCHED THEN
        UPDATE SET 
            TotalAmount = target.TotalAmount + source.Amount,
            WagerCount = target.WagerCount + 1,
            LastWagerDateTime = GETUTCDATE(),
            LastCalculatedDateTime = GETUTCDATE()
    WHEN NOT MATCHED THEN
        INSERT (AccountId, TotalAmount, WagerCount, LastWagerDateTime, LastCalculatedDateTime,CreatedDate,LastModifiedDate)
        VALUES (source.AccountId, source.Amount, 1, GETUTCDATE(), GETUTCDATE(),GETUTCDATE(),GETUTCDATE());
END
GO
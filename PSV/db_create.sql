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

CREATE TABLE [Client] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [Address] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [NIP] nvarchar(max) NULL,
    [Email] nvarchar(max) NULL,
    CONSTRAINT [Client_pk] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Location] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    CONSTRAINT [Location_pk] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Operator] (
    [Id] int NOT NULL IDENTITY,
    [FirstName] nvarchar(max) NOT NULL,
    [LastName] nvarchar(max) NOT NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [IdLocation] int NOT NULL,
    CONSTRAINT [Operator_pk] PRIMARY KEY ([Id]),
    CONSTRAINT [Operator_Locations] FOREIGN KEY ([IdLocation]) REFERENCES [Location] ([Id])
);
GO

CREATE TABLE [Cut] (
    [Id] int NOT NULL IDENTITY,
    [From] datetime2 NULL,
    [To] datetime2 NULL,
    [IsPresent] bit NOT NULL,
    [IdOperator] int NULL,
    CONSTRAINT [Cut_pk] PRIMARY KEY ([Id]),
    CONSTRAINT [Operator_Cut] FOREIGN KEY ([IdOperator]) REFERENCES [Operator] ([Id]) ON DELETE SET NULL
);
GO

CREATE TABLE [Milling] (
    [Id] int NOT NULL IDENTITY,
    [From] datetime2 NULL,
    [To] datetime2 NULL,
    [IsPresent] bit NOT NULL,
    [IdOperator] int NULL,
    CONSTRAINT [Milling_pk] PRIMARY KEY ([Id]),
    CONSTRAINT [Operator_Milling] FOREIGN KEY ([IdOperator]) REFERENCES [Operator] ([Id]) ON DELETE SET NULL
);
GO

CREATE TABLE [Wrapping] (
    [Id] int NOT NULL IDENTITY,
    [From] datetime2 NULL,
    [To] datetime2 NULL,
    [IsPresent] bit NOT NULL,
    [IdOperator] int NULL,
    CONSTRAINT [Wrapping_pk] PRIMARY KEY ([Id]),
    CONSTRAINT [Operator_Wrapping] FOREIGN KEY ([IdOperator]) REFERENCES [Operator] ([Id]) ON DELETE SET NULL
);
GO

CREATE TABLE [Order] (
    [Id] int NOT NULL IDENTITY,
    [OrderNumber] nvarchar(max) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [IdClient] int NULL,
    [QrCode] nvarchar(max) NULL,
    [BarCode] nvarchar(max) NULL,
    [EdgeCodeProvided] nvarchar(max) NULL,
    [EdgeCodeUsed] nvarchar(max) NULL,
    [Photos] nvarchar(max) NULL,
    [IdCut] int NOT NULL,
    [IdMilling] int NOT NULL,
    [IdWrapping] int NOT NULL,
    [IdLocation] int NOT NULL,
    CONSTRAINT [Order_pk] PRIMARY KEY ([Id]),
    CONSTRAINT [Client_Orders] FOREIGN KEY ([IdClient]) REFERENCES [Client] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [Order_Cut] FOREIGN KEY ([IdCut]) REFERENCES [Cut] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [Order_Location] FOREIGN KEY ([IdLocation]) REFERENCES [Location] ([Id]),
    CONSTRAINT [Order_Milling] FOREIGN KEY ([IdMilling]) REFERENCES [Milling] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [Order_Wrapping] FOREIGN KEY ([IdWrapping]) REFERENCES [Wrapping] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [Comment] (
    [Id] int NOT NULL IDENTITY,
    [Content] nvarchar(max) NOT NULL,
    [Time] datetime2 NOT NULL,
    [Source] nvarchar(max) NOT NULL,
    [IdOperator] int NULL,
    [IdOrder] int NOT NULL,
    CONSTRAINT [Comment_pk] PRIMARY KEY ([Id]),
    CONSTRAINT [Operator_Comment] FOREIGN KEY ([IdOperator]) REFERENCES [Operator] ([Id]) ON DELETE SET NULL,
    CONSTRAINT [Order_Comment] FOREIGN KEY ([IdOrder]) REFERENCES [Order] ([Id]) ON DELETE CASCADE
);
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Location]'))
    SET IDENTITY_INSERT [Location] ON;
INSERT INTO [Location] ([Id], [Name])
VALUES (1, N'Przasnysz'),
(2, N'Jednorożec');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Location]'))
    SET IDENTITY_INSERT [Location] OFF;
GO

CREATE INDEX [IX_Comment_IdOperator] ON [Comment] ([IdOperator]);
GO

CREATE INDEX [IX_Comment_IdOrder] ON [Comment] ([IdOrder]);
GO

CREATE INDEX [IX_Cut_IdOperator] ON [Cut] ([IdOperator]);
GO

CREATE INDEX [IX_Milling_IdOperator] ON [Milling] ([IdOperator]);
GO

CREATE INDEX [IX_Operator_IdLocation] ON [Operator] ([IdLocation]);
GO

CREATE INDEX [IX_Order_IdClient] ON [Order] ([IdClient]);
GO

CREATE UNIQUE INDEX [IX_Order_IdCut] ON [Order] ([IdCut]);
GO

CREATE INDEX [IX_Order_IdLocation] ON [Order] ([IdLocation]);
GO

CREATE UNIQUE INDEX [IX_Order_IdMilling] ON [Order] ([IdMilling]);
GO

CREATE UNIQUE INDEX [IX_Order_IdWrapping] ON [Order] ([IdWrapping]);
GO

CREATE INDEX [IX_Wrapping_IdOperator] ON [Wrapping] ([IdOperator]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240725202726_Initialize', N'7.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Order] ADD [OrderFile] nvarchar(max) NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240807181305_OrderFileField', N'7.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Order] ADD [OrderName] nvarchar(max) NOT NULL DEFAULT N'';
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240818164217_AddOrderNameColumn', N'7.0.5');
GO

COMMIT;
GO


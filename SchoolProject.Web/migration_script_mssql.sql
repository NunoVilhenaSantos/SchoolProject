IF OBJECT_ID(N'[_MyMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [_MyMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK__MyMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO


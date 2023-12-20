CREATE TABLE [TypeUser] (
    [TypeName] varchar(30) NOT NULL,
    CONSTRAINT [PK__TypeUser__D4E7DFA99DB99A9D] PRIMARY KEY ([TypeName])
);
GO


CREATE TABLE [ClientUser] (
    [Id] int NOT NULL IDENTITY,
    [Password] varchar(255) NOT NULL,
    [Email] varchar(100) NOT NULL,
    [RestartAccount] bit NULL DEFAULT (((0))),
    [ConfirmAccount] bit NULL DEFAULT (((0))),
    [Token] varchar(6) NULL,
    [FirstName] varchar(60) NOT NULL,
    [LastName] varchar(60) NOT NULL,
    [BadConduct] int NULL,
    [Banned] AS (case when [BadConduct]>(7) then (1) else (0) end),
    [TypeUserId] varchar(30) NULL,
    CONSTRAINT [PK__ClientUs__3214EC07627DAAB3] PRIMARY KEY ([Id]),
    CONSTRAINT [FK__ClientUse__TypeU__3C69FB99] FOREIGN KEY ([TypeUserId]) REFERENCES [TypeUser] ([TypeName])
);
GO


CREATE TABLE [FeedBack] (
    [Id] int NOT NULL IDENTITY,
    [ClientUserId] int NULL,
    [Message] varchar(200) NULL,
    CONSTRAINT [PK__FeedBack__3214EC075FD8F0AA] PRIMARY KEY ([Id]),
    CONSTRAINT [FK__FeedBack__Client__3F466844] FOREIGN KEY ([ClientUserId]) REFERENCES [ClientUser] ([Id])
);
GO


CREATE INDEX [IX_ClientUser_TypeUserId] ON [ClientUser] ([TypeUserId]);
GO


CREATE UNIQUE INDEX [UQ__ClientUs__A9D10534C8A10572] ON [ClientUser] ([Email]);
GO


CREATE INDEX [IX_FeedBack_ClientUserId] ON [FeedBack] ([ClientUserId]);
GO



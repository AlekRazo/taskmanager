---- CATALOGOS
CREATE TABLE [dbo].[priority](
[id] SMALLINT NOT NULL,
[priority] VARCHAR (20) NOT NULL,
CONSTRAINT [PK_priority] PRIMARY KEY([id])
);

CREATE TABLE [dbo].[status](
[id] SMALLINT NOT NULL,
[status] VARCHAR (20) NOT NULL
CONSTRAINT [PK_status] PRIMARY KEY([id])
);

INSERT INTO [dbo].[priority]([id], [priority]) VALUES(1,'Alta');
INSERT INTO [dbo].[priority]([id], [priority]) VALUES(2,'Media');
INSERT INTO [dbo].[priority]([id], [priority]) VALUES(3,'Baja');

INSERT INTO [dbo].[status]([id], [status]) VALUES(1, 'Pendiente');
INSERT INTO [dbo].[status]([id], [status]) VALUES(2, 'En progreso');
INSERT INTO [dbo].[status]([id], [status]) VALUES(3, 'Terminada');

GO

---- TABLAS DE NEGOCIO
CREATE TABLE [dbo].[user](
	[id] INT IDENTITY(1,1) NOT NULL,
	[username] VARCHAR(50) NOT NULL,
	[password] VARCHAR(255) NOT NULL,
	[created_at] DATETIME2 CONSTRAINT [DF_user_createdAt] DEFAULT CURRENT_TIMESTAMP,
	CONSTRAINT [PK_user] PRIMARY KEY(id)
)

CREATE TABLE [dbo].[task](
	[id] INT IDENTITY(1,1) NOT NULL,
	[title] VARCHAR(50) NOT NULL,
	[description] VARCHAR(500),
	[priority_id] SMALLINT NOT NULL,
	[create_date] DATE NOT NULL,
	[finish_date] DATE,
	[limit_date] DATE,
	[status_id] SMALLINT NOT NULL,
	[user_id] INT,
	[created_at] DATETIME2 CONSTRAINT [DF_task_createdAt] DEFAULT CURRENT_TIMESTAMP,
	CONSTRAINT [PK_task] PRIMARY KEY([id]),
	CONSTRAINT [FK_task_user] FOREIGN KEY([user_id]) REFERENCES [user](id),
	CONSTRAINT [FK_task_status] FOREIGN KEY([status_id]) REFERENCES [status](id),
	CONSTRAINT [FK_task_priority] FOREIGN KEY([priority_id]) REFERENCES [priority](id)
);

CREATE TABLE [dbo].[audit](
	[id] INT IDENTITY(1,1) NOT NULL,
	[task_id] INT NOT NULL,
	[record] VARCHAR(255),
	[created_at] DATETIME2 CONSTRAINT [DF_audit_createdAt] DEFAULT CURRENT_TIMESTAMP,
	CONSTRAINT [PK_audit] PRIMARY KEY([id])
);

GO

---- STORED PROCEDURE ----
CREATE PROCEDURE sp_GetPendingTasks @idUser INT
AS
BEGIN
	SELECT [u].[username], 
	COUNT(CASE WHEN t.[status_id] = 1 THEN 1 END) AS [total_pendientes],
	COUNT(CASE WHEN t.[limit_date] < CONVERT(DATE, GETDATE()) THEN 1 END) AS [total_vencidas]
	FROM [dbo].[task] t
	INNER JOIN [dbo].[user] u ON t.[user_id] = u.[id]
	WHERE u.[id] = @idUser
	GROUP BY u.[username]
END;

---- TRIGGER ----
GO

CREATE TRIGGER [dbo].[TRG_during_update_task] ON [dbo].[task]
AFTER UPDATE AS
BEGIN
	INSERT INTO [dbo].[audit] ([task_id], [record])
	SELECT 
        [i].[id],
        'Status changed from ' + [dst].[status] + 'to ' + [ist].[status]
    FROM INSERTED i
    INNER JOIN DELETED d ON [i].[id] = [d].[id]
	INNER JOIN [dbo].[status] ist ON i.[status_id] = [ist].[id]
	INNER JOIN [dbo].[status] dst ON [d].[status_id] = [dst].[id]
    WHERE [i].[status_id] <> [d].[status_id]; -- Only log if the status actually changed
END;

GO
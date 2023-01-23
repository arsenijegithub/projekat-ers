CREATE TABLE [dbo].[Data]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Type] NCHAR(11) NULL, 
    [Code] INT NULL, 
    [Timestamp] INT NULL, 
    [Value] NCHAR(11) NULL, 
    [Worktime] FLOAT NULL, 
    [Configuration] NCHAR(11) NULL
)

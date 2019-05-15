CREATE TABLE [dbo].[FileInformation]
(
	[FileInformationId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [FileName] NVARCHAR(255) NULL, 
    [PercentSaved] INT NULL
)

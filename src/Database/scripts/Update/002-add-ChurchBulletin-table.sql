IF OBJECT_ID(N'dbo.ChurchBulletinItem', N'U') IS NOT NULL
BEGIN
  PRINT 'Table Exists'
END
ELSE
CREATE TABLE [dbo].ChurchBulletinItem
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(50) NULL, 
    [Place] NVARCHAR(50) NULL, 
    [Date] DATETIME2 NULL
)

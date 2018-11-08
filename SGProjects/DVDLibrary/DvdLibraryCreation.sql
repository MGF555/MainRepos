USE master
GO

WHILE EXISTS(select NULL from sys.databases where name='DvdLibrary')
BEGIN
    DECLARE @SQL varchar(max)
    SELECT @SQL = COALESCE(@SQL,'') + 'Kill ' + Convert(varchar, SPId) + ';'
    FROM MASTER..SysProcesses
    WHERE DBId = DB_ID(N'DvdLibrary') AND SPId <> @@SPId
    EXEC(@SQL)
    DROP DATABASE [DvdLibrary]
END
GO

CREATE DATABASE DvdLibrary
GO

USE DvdLibrary
GO

IF EXISTS(SELECT * FROM sys.tables WHERE name='Rating')
	DROP TABLE Ratings
GO
IF EXISTS(SELECT * FROM sys.tables WHERE name='Director')
	DROP TABLE Directors
GO
IF EXISTS(SELECT * FROM sys.tables WHERE name='Dvd')
	DROP TABLE Dvds
GO

CREATE TABLE Ratings(
	RatingId INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	RatingName varchar(5) NOT NULL
)

CREATE TABLE Directors(
	DirectorId INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	DirectorName nvarchar(30) NOT NULL
)

CREATE TABLE Dvds(
	DvdId INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	Title nvarchar(30) NOT NULL,
	ReleaseYear INT NOT NULL,
	DirectorId INT FOREIGN KEY REFERENCES Director(DirectorId) NOT NULL,
	RatingId INT FOREIGN KEY REFERENCES Rating(RatingId),
	Notes nvarchar(50)
)
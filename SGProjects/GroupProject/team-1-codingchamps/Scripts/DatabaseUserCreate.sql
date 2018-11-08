USE master;
GO

IF EXISTS (SELECT 'true' FROM sys.sysdatabases WHERE name = 'AIBlog')
BEGIN
    DROP DATABASE AIBlog;
END;

IF EXISTS (SELECT 'true' FROM sys.syslogins WHERE name = 'bloguser')
BEGIN
	DROP LOGIN bloguser;
END;


CREATE LOGIN bloguser
WITH
  PASSWORD = 'password123'
GO


CREATE DATABASE AIBlog;
GO

USE AIBlog;
GO

IF EXISTS (SELECT 'true' FROM sys.database_principals WHERE name = 'bloguser')
BEGIN
DROP USER bloguser
END


CREATE USER bloguser
FOR LOGIN bloguser
GO

EXEC sys.sp_addrolemember @rolename = 'db_owner',  -- sysname
                          @membername = 'bloguser' -- sysname





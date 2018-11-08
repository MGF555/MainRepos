USE DvdLibrary
GO

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'GetById')
		DROP PROCEDURE GetById
GO

CREATE PROCEDURE GetById (
	@DvdId int
) AS
BEGIN
	SELECT DvdId, Title, ReleaseYear, DirectorName, RatingName, Notes
	FROM Dvds
		INNER JOIN Directors ON Directors.DirectorId = Dvds.DirectorId
		INNER JOIN Ratings ON Ratings.RatingId = Dvds.RatingId
	WHERE DvdId = @DvdId
END
GO

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'GetAll')
		DROP PROCEDURE GetAll
GO

CREATE PROCEDURE GetAll AS
BEGIN
	SELECT DvdId, Title, ReleaseYear, DirectorName, RatingName, Notes
	FROM Dvds
		INNER JOIN Directors ON Directors.DirectorId = Dvds.DirectorId
		INNER JOIN Ratings ON Ratings.RatingId = Dvds.RatingId
END
GO

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'GetDirectors')
		DROP PROCEDURE GetDirectors
GO

CREATE PROCEDURE GetDirectors AS
BEGIN
	SELECT DirectorId, DirectorName
	FROM Directors
END
GO

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'GetRatings')
		DROP PROCEDURE GetRatings
GO

CREATE PROCEDURE GetRatings AS
BEGIN
	SELECT RatingId, RatingName
	FROM Ratings
END
GO

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'GetByTitle')
		DROP PROCEDURE GetByTitle
GO

CREATE PROCEDURE GetByTitle (
	@Title nvarchar(30)
) AS
BEGIN
	SELECT DvdId, Title, ReleaseYear, DirectorName, RatingName, Notes
	FROM Dvds
		INNER JOIN Directors ON Directors.DirectorId = Dvds.DirectorId
		INNER JOIN Ratings ON Dvds.RatingId = Ratings.RatingId
	WHERE Title LIKE @Title
END
GO

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'GetByYear')
		DROP PROCEDURE GetByYear
GO

CREATE PROCEDURE GetByYear (
	@ReleaseYear int
) AS
BEGIN
	SELECT DvdId, Title, ReleaseYear, DirectorName, RatingName, Notes
	FROM Dvds
		INNER JOIN Directors ON Directors.DirectorId = Dvds.DirectorId
		INNER JOIN Ratings ON Dvds.RatingId = Ratings.RatingId
	WHERE ReleaseYear = @ReleaseYear
END
GO

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'GetByDirector')
		DROP PROCEDURE GetByDirector
GO

CREATE PROCEDURE GetByDirector (
	@DirectorName varchar(30)
) AS
BEGIN
	SELECT DvdId, Title, ReleaseYear, DirectorName, RatingName, Notes
	FROM Dvds
		INNER JOIN Directors ON Directors.DirectorId = Dvds.DirectorId
		INNER JOIN Ratings ON Dvds.RatingId = Ratings.RatingId
	WHERE DirectorName = @DirectorName
END
GO

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'GetByRating')
		DROP PROCEDURE GetByRating
GO

CREATE PROCEDURE GetByRating (
	@RatingName varchar(5)
) AS
BEGIN
	SELECT DvdId, Title, ReleaseYear, DirectorName, RatingName, Notes
	FROM Dvds
		INNER JOIN Directors ON Directors.DirectorId = Dvds.DirectorId
		INNER JOIN Ratings ON Dvds.RatingId = Ratings.RatingId
	WHERE RatingName = @RatingName
END
GO

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'DvdAdd')
		DROP PROCEDURE DvdAdd
GO

CREATE PROCEDURE DvdAdd (
	@DvdId int output,
	@Title nvarchar(30),
	@ReleaseYear int,
	@DirectorId int,
	@RatingId varchar(5),
	@Notes nvarchar(50)
) AS
BEGIN 
	INSERT INTO Dvds(Title, ReleaseYear, DirectorId, RatingId, Notes)
	VALUES(@Title, @ReleaseYear, @DirectorId, @RatingId, @Notes);

	SET @DvdId = SCOPE_IDENTITY();
END

GO

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'DirectorAdd')
		DROP PROCEDURE DirectorAdd
GO

CREATE PROCEDURE DirectorAdd (
	@DirectorId int output,
	@DirectorName nvarchar(30)
) AS
BEGIN 
	INSERT INTO Directors(DirectorName)
	VALUES(@DirectorName);

	SET @DirectorId = SCOPE_IDENTITY();
END

GO

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'DvdUpdate')
		DROP PROCEDURE DvdUpdate
GO

CREATE PROCEDURE DvdUpdate (
	@DvdId int output,
	@Title nvarchar(30),
	@ReleaseYear int,
	@DirectorId nvarchar(30),
	@RatingId int,
	@Notes nvarchar(50)
) AS
BEGIN 
	UPDATE Dvds SET
		Title = @Title,
		ReleaseYear = @ReleaseYear,
		DirectorId = @DirectorId,
		RatingId = @RatingId,
		Notes = @Notes
	WHERE DvdId = @DvdId
END

GO

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'DvdDelete')
		DROP PROCEDURE DvdDelete
GO

CREATE PROCEDURE DvdDelete (
	@DvdId int
) AS
BEGIN
	BEGIN TRANSACTION

	DELETE FROM Dvds WHERE DvdId = @DvdId;

	COMMIT TRANSACTION
END
GO
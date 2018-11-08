USE DvdLibrary
GO

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'DvdLibrarySampleData')
		DROP PROCEDURE DvdLibrarySampleData
GO

CREATE PROCEDURE DvdLibrarySampleData AS
BEGIN
	DELETE FROM Dvds;
	DELETE FROM Ratings;
	DELETE FROM Directors;

	SET IDENTITY_INSERT Ratings ON;

	INSERT INTO Ratings(RatingId, RatingName)
	VALUES(1, 'G'),
	(2, 'PG'),
	(3, 'PG-13'),
	(4, 'R'),
	(5, 'NR');

	SET IDENTITY_INSERT Ratings OFF;

	SET IDENTITY_INSERT Directors ON;

	INSERT INTO Directors(DirectorId, DirectorName)
	VALUES(1, 'John'),
	(2, 'Jacob'),
	(3, 'Jingleheimer'),
	(4, 'Schmidt');

	SET IDENTITY_INSERT Directors OFF;

	SET IDENTITY_INSERT Dvds ON;

	INSERT INTO Dvds(DvdId, Title, ReleaseYear, DirectorId, RatingId, Notes)
	VALUES (1, 'That one movie', 1995, 3, 1, 'It was ok'),
	(2, 'That old movie', 1444, 1, 3, 'Movie did not age well'),
	(3, 'That other movie', 2030, 2, 4, 'A ways off'),
	(4, 'A movie', 2017, 4, 4, 'Eh, no comment');

	SET IDENTITY_INSERT Dvds OFF;
END
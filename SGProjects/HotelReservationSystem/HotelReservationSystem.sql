USE MASTER
GO

WHILE EXISTS(select NULL from sys.databases where name='HotelReservationSystem')
BEGIN
    DECLARE @SQL varchar(max)
    SELECT @SQL = COALESCE(@SQL,'') + 'Kill ' + Convert(varchar, SPId) + ';'
    FROM MASTER..SysProcesses
    WHERE DBId = DB_ID(N'HotelReservationSystem') AND SPId <> @@SPId
    EXEC(@SQL)
    DROP DATABASE [HotelReservationSystem]
END
GO

CREATE DATABASE HotelReservationSystem
GO

USE HotelReservationSystem
GO 

CREATE TABLE Amenities(
	AmenitiesID INT IDENTITY (1,1) PRIMARY KEY,
	Amenitie NVARCHAR(50),
	Rate DECIMAL (19, 4)
)

CREATE TABLE AddOnRate(
	AddOnRateID INT IDENTITY (1,1) PRIMARY KEY,
	Rate DECIMAL(19, 4) NOT NULL,
	StartDate DATE NOT NULL,
	EndDate DATE NOT NULL
)

CREATE TABLE AddOns(
	AddOnsID INT IDENTITY (1,1) PRIMARY KEY,
	AddOn NVARCHAR(50),
	Rate DECIMAL (19, 4),
	AddonRateID INT FOREIGN KEY REFERENCES AddOnRate(AddonRateID) NULL
)

CREATE TABLE RoomRate(
	RoomRateID INT IDENTITY (1,1) PRIMARY KEY,
	Rate DECIMAL (19,4) NOT NULL,
	StartDate DATE NOT NULL,
	EndDate DATE NOT NULL,
)

CREATE TABLE RoomType(
	RoomTypeID INT IDENTITY(1, 1) PRIMARY KEY,
	RoomType NVARCHAR(30) NOT NULL,
	RoomRateID INT FOREIGN KEY REFERENCES RoomRate(RoomRateID) NULL
)

CREATE TABLE Room(
	RoomID INT IDENTITY PRIMARY KEY NOT NULL,
	FloorNumber INT NOT NULL,
	OccupancyLimit INT NOT NULL,
)

CREATE TABLE Guest(
	GuestID INT IDENTITY (1,1) PRIMARY KEY,
	GuestName NVARCHAR(50) NOT NULL,
	GuestAge INT NOT NULL
)

CREATE TABLE Customer(
	CustomerID INT IDENTITY (1,1) PRIMARY KEY,
	CustomerName NVARCHAR(50) NOT NULL,
	Phone NVARCHAR(12) NOT NULL,
	Email NVARCHAR(50) NOT NULL
)

CREATE TABLE PromotionCodes(
	PromotionCodeID INT IDENTITY (1,1) PRIMARY KEY,
	DiscountDollars DECIMAL (19,4) NULL,
	DiscountPercent DECIMAL (19,4) NULL,
	StartDate DATE NULL,
	EndDate DATE NULL
)

CREATE TABLE Bill(
	BillID INT IDENTITY (1,1) PRIMARY KEY,
	Tax DECIMAL (19,4) NOT NULL, 
	Total DECIMAL (19,4) NOT NULL,
	PromotionalCodesID INT FOREIGN KEY REFERENCES PromotionCodes(PromotionCodeID) NULL
)

CREATE TABLE Reservation(
	ReservationID INT IDENTITY (1,1) PRIMARY KEY,
	CustomerID INT FOREIGN KEY REFERENCES Customer(CustomerID) NOT NULL,
	BillID INT FOREIGN KEY REFERENCES Bill(BillID) NOT NULL,
	StartDate DATE NOT NULL,
	EndDate DATE NOT NULL
)

CREATE TABLE RoomTypeRoom(
	RoomID INT NOT NULL,
	RoomTypeID INT NOT NULL,
	CONSTRAINT PK_RoomTypeRoom
		PRIMARY KEY (RoomID, RoomTypeID),
	CONSTRAINT FK_Room_RoomType
		FOREIGN KEY (RoomID)
		REFERENCES Room(RoomID),
	CONSTRAINT FK_RoomType_Room
		FOREIGN KEY (RoomTypeID)
		REFERENCES RoomType(RoomTypeID),
)

CREATE TABLE RoomAmenities(
	RoomID INT NOT NULL,
	AmenitiesID INT NOT NULL,
	CONSTRAINT PK_RoomAmenities
		PRIMARY KEY (RoomID, AmenitiesID),
	CONSTRAINT FK_Room_Ameneties
		FOREIGN KEY (RoomID)
		REFERENCES Room(RoomID),
	CONSTRAINT FK_Amenities_Room
		FOREIGN KEY (AmenitiesID)
		REFERENCES Amenities(AmenitiesID),
)

CREATE TABLE ReservationRoom(
	RoomID INT NOT NULL,
	ReservationID INT NOT NULL,
	CONSTRAINT PK_ReservationRoom
		PRIMARY KEY (ReservationID, RoomID),
	CONSTRAINT FK_Reservation_Room
		FOREIGN KEY (ReservationID)
		REFERENCES Reservation(ReservationID),
	CONSTRAINT FK_Room_Reservation
		FOREIGN KEY (RoomID)
		REFERENCES Room(RoomID),
)

CREATE TABLE BillAddOns(
	BillID INT NOT NULL,
	AddOnsID INT NOT NULL,
	CONSTRAINT PK_BillAddOns
		PRIMARY KEY (BillID, AddOnsID),
	CONSTRAINT FK_Bill_AddOns
		FOREIGN KEY (BillID)
		REFERENCES Bill(BillID),
	CONSTRAINT FK_AddOns_Bills
		FOREIGN KEY (AddOnsID)
		REFERENCES AddOns(AddOnsID),
)

CREATE TABLE GuestReservation(
	GuestID INT NOT NULL,
	ReservationID INT NOT NULL,
	CONSTRAINT PK_GuestReservation
		PRIMARY KEY (GuestID, ReservationID),
	CONSTRAINT FK_Guest_Reservation
		FOREIGN KEY (GuestID)
		REFERENCES Guest(GuestID),
	CONSTRAINT FK_Reservation_Gueset
		FOREIGN KEY (ReservationID)
		REFERENCES Reservation(ReservationID),
	)
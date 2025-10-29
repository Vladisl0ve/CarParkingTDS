-- 1. Create database
CREATE DATABASE TDS_ParkingDB;
GO

-- 2. Create login
CREATE LOGIN ParkingSa WITH PASSWORD = 'ExampleStrongPassword123!';
GO

-- 3. Switch to the database
USE TDS_ParkingDB;
GO

-- 4. Create user
CREATE USER ParkingSa FOR LOGIN ParkingSa;
GO

-- 5. Grant roles (edit tables, read/write)
EXEC sp_addrolemember 'db_ddladmin', 'ParkingSa';
EXEC sp_addrolemember 'db_datareader', 'ParkingSa';
EXEC sp_addrolemember 'db_datawriter', 'ParkingSa';
GO

-- 6. Create table TDS_PARKING_SPACE
CREATE TABLE TDS_PARKING_SPACES
(
    ID INT IDENTITY(1,1) PRIMARY KEY,
    SPACE_NUMBER INT NOT NULL,
    IS_OCCUPIED BIT NOT NULL,
    VEHICLE_TYPE INT NULL,
    VEHICLE_REG NVARCHAR(50) NULL,
    TIME_IN DATETIME2 NULL
);

-- 7. Fill table with empty data
INSERT INTO TDS_PARKING_SPACES (SPACE_NUMBER, IS_OCCUPIED, VEHICLE_TYPE, VEHICLE_REG, TIME_IN) VALUES
(1, 0, NULL, NULL, NULL),
(2, 0, NULL, NULL, NULL),
(3, 0, NULL, NULL, NULL),
(4, 0, NULL, NULL, NULL),
(5, 0, NULL, NULL, NULL),
(6, 0, NULL, NULL, NULL),
(7, 0, NULL, NULL, NULL),
(8, 0, NULL, NULL, NULL),
(9, 0, NULL, NULL, NULL),
(10, 0, NULL, NULL, NULL);
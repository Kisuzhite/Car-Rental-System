IF DB_ID(N'CarRentalDB') IS NULL
BEGIN
    CREATE DATABASE CarRentalDB;
END;

USE CarRentalDB;

IF OBJECT_ID(N'dbo.Cars', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Cars (
        CarId INT IDENTITY(1,1) PRIMARY KEY,
        PlateNumber NVARCHAR(20) NOT NULL,
        Brand NVARCHAR(50) NOT NULL,
        Model NVARCHAR(50) NOT NULL,
        [Year] INT NOT NULL,
        Color NVARCHAR(30) NOT NULL,
        DailyRate DECIMAL(10,2) NOT NULL,
        Status NVARCHAR(20) NOT NULL,
        ImagePath NVARCHAR(500) NULL
    );
END;

IF COL_LENGTH(N'dbo.Cars', N'ImagePath') IS NULL
BEGIN
    ALTER TABLE dbo.Cars ADD ImagePath NVARCHAR(500) NULL;
END;

IF OBJECT_ID(N'dbo.Customers', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Customers (
        CustomerId INT IDENTITY(1,1) PRIMARY KEY,
        FullName NVARCHAR(100) NOT NULL,
        ContactNumber NVARCHAR(30) NOT NULL,
        LicenseNumber NVARCHAR(50) NOT NULL,
        Address NVARCHAR(200) NOT NULL
    );
END;

IF OBJECT_ID(N'dbo.Rentals', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Rentals (
        RentalId INT IDENTITY(1,1) PRIMARY KEY,
        CustomerId INT NOT NULL,
        CarId INT NOT NULL,
        RentalDate DATE NOT NULL,
        ExpectedReturnDate DATE NOT NULL,
        ActualReturnDate DATE NULL,
        DailyRate DECIMAL(10,2) NOT NULL,
        TotalAmount DECIMAL(10,2) NOT NULL,
        Status NVARCHAR(20) NOT NULL,
        CONSTRAINT FK_Rentals_Customers FOREIGN KEY (CustomerId) REFERENCES dbo.Customers(CustomerId),
        CONSTRAINT FK_Rentals_Cars FOREIGN KEY (CarId) REFERENCES dbo.Cars(CarId)
    );
END;

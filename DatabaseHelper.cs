using Microsoft.Data.SqlClient;

namespace Car_Rental_System
{
    internal static class DatabaseHelper
    {
        public const string DatabaseName = "CarRentalDB";

        public static readonly string ConnectionString =
            $@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog={DatabaseName};Integrated Security=True;TrustServerCertificate=True";

        private static readonly string MasterConnectionString =
            @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;TrustServerCertificate=True";

        public static void EnsureDatabase()
        {
            using SqlConnection masterConnection = new(MasterConnectionString);
            masterConnection.Open();

            using SqlCommand createDatabaseCommand = new(
                $"IF DB_ID(N'{DatabaseName}') IS NULL CREATE DATABASE [{DatabaseName}];",
                masterConnection);
            createDatabaseCommand.ExecuteNonQuery();

            using SqlConnection databaseConnection = new(ConnectionString);
            databaseConnection.Open();

            using SqlCommand createTablesCommand = new(GetCreateTablesSql(), databaseConnection);
            createTablesCommand.ExecuteNonQuery();
        }

        public static SqlConnection OpenConnection()
        {
            EnsureDatabase();

            SqlConnection connection = new(ConnectionString);
            connection.Open();
            return connection;
        }

        public static object ExecuteScalar(SqlConnection connection, string query)
        {
            using SqlCommand command = new(query, connection);
            return command.ExecuteScalar() ?? 0;
        }

        private static string GetCreateTablesSql()
        {
            return @"
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
END;";
        }
    }
}

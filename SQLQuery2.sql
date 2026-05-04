USE CarRentalDB;

DECLARE @Search NVARCHAR(200) = N'';
DECLARE @SearchText NVARCHAR(202) = N'%' + @Search + N'%';

SELECT
    CustomerId,
    FullName AS [Full Name],
    ContactNumber AS [Contact Number],
    LicenseNumber AS [License Number],
    Address
FROM dbo.Customers
WHERE
    @Search = N''
    OR FullName LIKE @SearchText
    OR ContactNumber LIKE @SearchText
    OR LicenseNumber LIKE @SearchText
ORDER BY CustomerId DESC;

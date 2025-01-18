-- Populate the Item table with 300 records
DECLARE @Counter INT = 1;

WHILE @Counter <= 300
BEGIN
    INSERT INTO Item (Guid, Name, Description, Price, SKU, CreatedOn, LastModification)
    VALUES (
        NEWID(), 
        CONCAT('Item-', @Counter), 
        CONCAT('Description for Item-', @Counter), 
        CAST(RAND() * 1000 + 1 AS DECIMAL(18, 2)), 
        CONCAT('SKU-', 
               SUBSTRING(CONVERT(VARCHAR(36), NEWID()), 1, 3), '-', 
               SUBSTRING(CONVERT(VARCHAR(36), NEWID()), 1, 3), '-', 
               SUBSTRING(CONVERT(VARCHAR(36), NEWID()), 1, 3)), 
        GETDATE(), 
        NULL 
    );
    SET @Counter = @Counter + 1;
END;

INSERT INTO Stock (Guid, QuantityType, Quantity, Location, ItemId)
SELECT 
    NEWID(), 
    ABS(CHECKSUM(NEWID())) % 3, 
    ABS(CHECKSUM(NEWID())) % 100 + 1, 
    CONCAT('Location-', SUBSTRING(CONVERT(VARCHAR(36), NEWID()), 1, 3)), 
    Id
FROM Item;

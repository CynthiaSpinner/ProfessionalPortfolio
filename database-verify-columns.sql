-- Verification script to check existing columns in HeroTemplates and HomePages tables

-- Check HeroTemplates table columns
SELECT 
    COLUMN_NAME,
    DATA_TYPE,
    IS_NULLABLE,
    COLUMN_DEFAULT
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'HeroTemplates' 
ORDER BY ORDINAL_POSITION;

-- Check HomePages table columns
SELECT 
    COLUMN_NAME,
    DATA_TYPE,
    IS_NULLABLE,
    COLUMN_DEFAULT
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'HomePages' 
ORDER BY ORDINAL_POSITION; 
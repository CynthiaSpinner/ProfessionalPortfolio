-- Check current database structure for features-related content
-- Run this on your Azure SQL database to see what we have

-- Check all tables in the database
SELECT 
    TABLE_SCHEMA,
    TABLE_NAME,
    TABLE_TYPE
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_TYPE = 'BASE TABLE'
ORDER BY TABLE_SCHEMA, TABLE_NAME;

-- Check all columns that might be related to features
SELECT 
    TABLE_SCHEMA,
    TABLE_NAME,
    COLUMN_NAME,
    DATA_TYPE,
    IS_NULLABLE,
    CHARACTER_MAXIMUM_LENGTH
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE COLUMN_NAME LIKE '%feature%' 
   OR COLUMN_NAME LIKE '%skill%' 
   OR COLUMN_NAME LIKE '%technology%'
   OR COLUMN_NAME LIKE '%section%'
   OR COLUMN_NAME LIKE '%title%'
   OR COLUMN_NAME LIKE '%subtitle%'
ORDER BY TABLE_NAME, COLUMN_NAME;

-- Check HomePage table structure (likely to have features content)
SELECT 
    COLUMN_NAME,
    DATA_TYPE,
    IS_NULLABLE,
    CHARACTER_MAXIMUM_LENGTH
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'HomePages'
ORDER BY ORDINAL_POSITION;

-- Check if FeaturesSections table already exists
SELECT COUNT(*) as FeaturesSectionsTableExists
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_NAME = 'FeaturesSections';

-- Sample data from HomePages to see what features content might be there
SELECT TOP 5 * FROM HomePages;

-- Check for any JSON columns that might contain features data
SELECT 
    TABLE_NAME,
    COLUMN_NAME,
    DATA_TYPE
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE DATA_TYPE = 'nvarchar' 
  AND CHARACTER_MAXIMUM_LENGTH = -1
  AND (COLUMN_NAME LIKE '%json%' OR COLUMN_NAME LIKE '%data%' OR COLUMN_NAME LIKE '%content%'); 
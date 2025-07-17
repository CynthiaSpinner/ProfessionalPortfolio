-- Check FeaturesSections table structure
SELECT TABLE_NAME, COLUMN_NAME, DATA_TYPE, IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'FeaturesSections'
ORDER BY ORDINAL_POSITION;

-- Check if table has data
SELECT COUNT(*) as RecordCount FROM FeaturesSections;

-- Show sample data if it exists
SELECT TOP 5 * FROM FeaturesSections; 
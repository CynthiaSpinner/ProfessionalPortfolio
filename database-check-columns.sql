-- Check if columns exist in HeroTemplates table
SELECT 
    COLUMN_NAME,
    DATA_TYPE,
    IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'HeroTemplates'
ORDER BY COLUMN_NAME

-- Also check the table structure
EXEC sp_help 'HeroTemplates' 
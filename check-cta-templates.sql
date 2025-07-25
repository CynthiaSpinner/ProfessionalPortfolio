-- Check if CTATemplates table exists and has data
SELECT COUNT(*) as TemplateCount FROM CTATemplates;

-- See the table structure
SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE 
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'CTATemplates' 
ORDER BY ORDINAL_POSITION;

-- See sample data
SELECT TOP 5 * FROM CTATemplates; 
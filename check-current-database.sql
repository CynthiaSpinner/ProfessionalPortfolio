-- Check current database structure for features-related content
-- Run this on your Azure SQL database to see what we have

-- Check all tables
SELECT 
    TABLE_SCHEMA,
    TABLE_NAME,
    TABLE_TYPE
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_TYPE = 'BASE TABLE'
ORDER BY TABLE_SCHEMA, TABLE_NAME;

-- Check if FeaturesSections table exists
SELECT 
    CASE 
        WHEN EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'FeaturesSections') 
        THEN 'EXISTS' 
        ELSE 'NOT EXISTS' 
    END as FeaturesSectionsTableStatus;

-- Check HomePages table structure for features-related columns
SELECT 
    COLUMN_NAME,
    DATA_TYPE,
    IS_NULLABLE,
    CHARACTER_MAXIMUM_LENGTH
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'HomePages'
  AND (COLUMN_NAME LIKE '%skill%' 
       OR COLUMN_NAME LIKE '%feature%' 
       OR COLUMN_NAME LIKE '%section%')
ORDER BY ORDINAL_POSITION;

-- Check what's in SkillsCategoriesJson and FeaturedProjectsJson
SELECT TOP 3 
    Id,
    SkillsSectionTitle,
    SkillsSectionSubtitle,
    LEN(SkillsCategoriesJson) as SkillsJsonLength,
    LEN(FeaturedProjectsJson) as ProjectsJsonLength,
    LEFT(SkillsCategoriesJson, 200) as SkillsJsonPreview,
    LEFT(FeaturedProjectsJson, 200) as ProjectsJsonPreview
FROM HomePages 
WHERE SkillsCategoriesJson IS NOT NULL OR FeaturedProjectsJson IS NOT NULL;

-- Check if there are any other tables with features content
SELECT 
    TABLE_NAME,
    COLUMN_NAME,
    DATA_TYPE
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE COLUMN_NAME LIKE '%feature%' 
   OR COLUMN_NAME LIKE '%skill%'
   OR COLUMN_NAME LIKE '%technology%'
ORDER BY TABLE_NAME, COLUMN_NAME;

-- Check for any duplicate or obsolete tables
SELECT 
    TABLE_NAME,
    COUNT(*) as ColumnCount
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME IN ('HomePages', 'FeaturesSections', 'SkillsCategories', 'Projects')
GROUP BY TABLE_NAME
ORDER BY TABLE_NAME; 
-- Check the JSON content in existing columns to understand current structure
-- Run this on your Azure SQL database

-- Check SkillsCategoriesJson in HomePages
SELECT TOP 3 
    Id,
    SkillsCategoriesJson,
    LEN(SkillsCategoriesJson) as JsonLength
FROM HomePages 
WHERE SkillsCategoriesJson IS NOT NULL AND SkillsCategoriesJson != '';

-- Check FeaturedProjectsJson in HomePages
SELECT TOP 3 
    Id,
    FeaturedProjectsJson,
    LEN(FeaturedProjectsJson) as JsonLength
FROM HomePages 
WHERE FeaturedProjectsJson IS NOT NULL AND FeaturedProjectsJson != '';

-- Check SkillsCategoriesJson in HeroTemplates
SELECT TOP 3 
    Id,
    Name,
    SkillsCategoriesJson,
    LEN(SkillsCategoriesJson) as JsonLength
FROM HeroTemplates 
WHERE SkillsCategoriesJson IS NOT NULL AND SkillsCategoriesJson != '';

-- Check if there are any non-JSON columns that might be related to features
SELECT 
    TABLE_NAME,
    COLUMN_NAME,
    DATA_TYPE,
    IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME IN ('HomePages', 'HeroTemplates')
  AND (COLUMN_NAME LIKE '%skill%' 
       OR COLUMN_NAME LIKE '%feature%' 
       OR COLUMN_NAME LIKE '%technology%'
       OR COLUMN_NAME LIKE '%section%')
  AND DATA_TYPE != 'nvarchar' OR CHARACTER_MAXIMUM_LENGTH != -1
ORDER BY TABLE_NAME, COLUMN_NAME;

-- Check the full structure of HomePages table
SELECT 
    COLUMN_NAME,
    DATA_TYPE,
    IS_NULLABLE,
    CHARACTER_MAXIMUM_LENGTH
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'HomePages'
ORDER BY ORDINAL_POSITION; 
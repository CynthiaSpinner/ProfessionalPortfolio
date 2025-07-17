-- Check the actual content of SkillsCategoriesJson to see current features structure
-- Run this on your Azure SQL database

-- Get a sample of the SkillsCategoriesJson content
SELECT TOP 1 
    Id,
    SkillsSectionTitle,
    SkillsSectionSubtitle,
    SkillsCategoriesJson,
    LEN(SkillsCategoriesJson) as JsonLength
FROM HomePages 
WHERE SkillsCategoriesJson IS NOT NULL AND SkillsCategoriesJson != '';

-- Check if there are multiple HomePage records
SELECT 
    COUNT(*) as TotalRecords,
    COUNT(CASE WHEN SkillsCategoriesJson IS NOT NULL AND SkillsCategoriesJson != '' THEN 1 END) as RecordsWithSkills
FROM HomePages;

-- Get the first 1000 characters of SkillsCategoriesJson to see the structure
SELECT TOP 1 
    Id,
    SkillsSectionTitle,
    SkillsSectionSubtitle,
    LEFT(SkillsCategoriesJson, 1000) as SkillsJsonPreview
FROM HomePages 
WHERE SkillsCategoriesJson IS NOT NULL AND SkillsCategoriesJson != ''; 
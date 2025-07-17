-- Simple script to add missing columns to Projects and SkillsCategories tables

-- Add missing columns to Projects table
ALTER TABLE Projects ADD TechnologiesJson NVARCHAR(MAX) DEFAULT '[]';
ALTER TABLE Projects ADD CompletionDate DATETIME2 NOT NULL DEFAULT GETDATE();

-- Add missing columns to SkillsCategories table  
ALTER TABLE SkillsCategories ADD SkillsJson NVARCHAR(MAX) DEFAULT '[]';
ALTER TABLE SkillsCategories ADD DisplayOrder INT NOT NULL DEFAULT 0;
ALTER TABLE SkillsCategories ADD IsActive BIT NOT NULL DEFAULT 1;

-- Show the updated table structure
SELECT TABLE_NAME, COLUMN_NAME, DATA_TYPE, IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME IN ('Projects', 'SkillsCategories')
ORDER BY TABLE_NAME, ORDINAL_POSITION; 
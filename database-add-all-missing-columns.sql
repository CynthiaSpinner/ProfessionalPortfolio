-- Comprehensive script to add all missing columns to HeroTemplates and HomePages tables
-- This script checks if columns exist before adding them to avoid errors
-- For Azure SQL Database - no USE statement needed

PRINT 'Adding missing columns to HeroTemplates table...';

-- Add missing columns to HeroTemplates table
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'HeroTemplates' AND COLUMN_NAME = 'CTABackgroundImageUrl')
BEGIN
    ALTER TABLE HeroTemplates ADD CTABackgroundImageUrl NVARCHAR(500) NULL;
    PRINT 'Added CTABackgroundImageUrl column to HeroTemplates';
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'HeroTemplates' AND COLUMN_NAME = 'CTAButtonText')
BEGIN
    ALTER TABLE HeroTemplates ADD CTAButtonText NVARCHAR(100) NULL;
    PRINT 'Added CTAButtonText column to HeroTemplates';
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'HeroTemplates' AND COLUMN_NAME = 'CTAButtonUrl')
BEGIN
    ALTER TABLE HeroTemplates ADD CTAButtonUrl NVARCHAR(500) NULL;
    PRINT 'Added CTAButtonUrl column to HeroTemplates';
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'HeroTemplates' AND COLUMN_NAME = 'CTASubtitle')
BEGIN
    ALTER TABLE HeroTemplates ADD CTASubtitle NVARCHAR(200) NULL;
    PRINT 'Added CTASubtitle column to HeroTemplates';
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'HeroTemplates' AND COLUMN_NAME = 'CTATextColor')
BEGIN
    ALTER TABLE HeroTemplates ADD CTATextColor NVARCHAR(20) NULL;
    PRINT 'Added CTATextColor column to HeroTemplates';
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'HeroTemplates' AND COLUMN_NAME = 'CTATitle')
BEGIN
    ALTER TABLE HeroTemplates ADD CTATitle NVARCHAR(200) NULL;
    PRINT 'Added CTATitle column to HeroTemplates';
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'HeroTemplates' AND COLUMN_NAME = 'FeaturedProjectsJson')
BEGIN
    ALTER TABLE HeroTemplates ADD FeaturedProjectsJson NVARCHAR(MAX) NULL;
    PRINT 'Added FeaturedProjectsJson column to HeroTemplates';
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'HeroTemplates' AND COLUMN_NAME = 'FeaturedProjectsSubtitle')
BEGIN
    ALTER TABLE HeroTemplates ADD FeaturedProjectsSubtitle NVARCHAR(200) NULL;
    PRINT 'Added FeaturedProjectsSubtitle column to HeroTemplates';
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'HeroTemplates' AND COLUMN_NAME = 'FeaturedProjectsTitle')
BEGIN
    ALTER TABLE HeroTemplates ADD FeaturedProjectsTitle NVARCHAR(200) NULL;
    PRINT 'Added FeaturedProjectsTitle column to HeroTemplates';
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'HeroTemplates' AND COLUMN_NAME = 'HeaderBackgroundStyle')
BEGIN
    ALTER TABLE HeroTemplates ADD HeaderBackgroundStyle NVARCHAR(50) NULL;
    PRINT 'Added HeaderBackgroundStyle column to HeroTemplates';
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'HeroTemplates' AND COLUMN_NAME = 'HeaderButtonColor')
BEGIN
    ALTER TABLE HeroTemplates ADD HeaderButtonColor NVARCHAR(20) NULL;
    PRINT 'Added HeaderButtonColor column to HeroTemplates';
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'HeroTemplates' AND COLUMN_NAME = 'HeaderButtonTextColor')
BEGIN
    ALTER TABLE HeroTemplates ADD HeaderButtonTextColor NVARCHAR(20) NULL;
    PRINT 'Added HeaderButtonTextColor column to HeroTemplates';
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'HeroTemplates' AND COLUMN_NAME = 'HeaderSecondaryButtonText')
BEGIN
    ALTER TABLE HeroTemplates ADD HeaderSecondaryButtonText NVARCHAR(100) NULL;
    PRINT 'Added HeaderSecondaryButtonText column to HeroTemplates';
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'HeroTemplates' AND COLUMN_NAME = 'HeaderSecondaryButtonUrl')
BEGIN
    ALTER TABLE HeroTemplates ADD HeaderSecondaryButtonUrl NVARCHAR(500) NULL;
    PRINT 'Added HeaderSecondaryButtonUrl column to HeroTemplates';
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'HeroTemplates' AND COLUMN_NAME = 'HeaderTextColor')
BEGIN
    ALTER TABLE HeroTemplates ADD HeaderTextColor NVARCHAR(20) NULL;
    PRINT 'Added HeaderTextColor column to HeroTemplates';
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'HeroTemplates' AND COLUMN_NAME = 'ShowHeaderPrimaryButton')
BEGIN
    ALTER TABLE HeroTemplates ADD ShowHeaderPrimaryButton BIT NULL;
    PRINT 'Added ShowHeaderPrimaryButton column to HeroTemplates';
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'HeroTemplates' AND COLUMN_NAME = 'ShowHeaderSecondaryButton')
BEGIN
    ALTER TABLE HeroTemplates ADD ShowHeaderSecondaryButton BIT NULL;
    PRINT 'Added ShowHeaderSecondaryButton column to HeroTemplates';
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'HeroTemplates' AND COLUMN_NAME = 'SkillsCategoriesJson')
BEGIN
    ALTER TABLE HeroTemplates ADD SkillsCategoriesJson NVARCHAR(MAX) NULL;
    PRINT 'Added SkillsCategoriesJson column to HeroTemplates';
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'HeroTemplates' AND COLUMN_NAME = 'SkillsSectionSubtitle')
BEGIN
    ALTER TABLE HeroTemplates ADD SkillsSectionSubtitle NVARCHAR(200) NULL;
    PRINT 'Added SkillsSectionSubtitle column to HeroTemplates';
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'HeroTemplates' AND COLUMN_NAME = 'SkillsSectionTitle')
BEGIN
    ALTER TABLE HeroTemplates ADD SkillsSectionTitle NVARCHAR(200) NULL;
    PRINT 'Added SkillsSectionTitle column to HeroTemplates';
END

PRINT 'Adding missing columns to HomePages table...';

-- Add missing columns to HomePages table
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'HomePages' AND COLUMN_NAME = 'CTABackgroundImageUrl')
BEGIN
    ALTER TABLE HomePages ADD CTABackgroundImageUrl NVARCHAR(500) NULL;
    PRINT 'Added CTABackgroundImageUrl column to HomePages';
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'HomePages' AND COLUMN_NAME = 'CTAButtonText')
BEGIN
    ALTER TABLE HomePages ADD CTAButtonText NVARCHAR(50) NULL;
    PRINT 'Added CTAButtonText column to HomePages';
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'HomePages' AND COLUMN_NAME = 'CTAButtonUrl')
BEGIN
    ALTER TABLE HomePages ADD CTAButtonUrl NVARCHAR(200) NULL;
    PRINT 'Added CTAButtonUrl column to HomePages';
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'HomePages' AND COLUMN_NAME = 'CTASubtitle')
BEGIN
    ALTER TABLE HomePages ADD CTASubtitle NVARCHAR(500) NULL;
    PRINT 'Added CTASubtitle column to HomePages';
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'HomePages' AND COLUMN_NAME = 'CTATextColor')
BEGIN
    ALTER TABLE HomePages ADD CTATextColor NVARCHAR(50) NULL;
    PRINT 'Added CTATextColor column to HomePages';
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'HomePages' AND COLUMN_NAME = 'CTATitle')
BEGIN
    ALTER TABLE HomePages ADD CTATitle NVARCHAR(100) NULL;
    PRINT 'Added CTATitle column to HomePages';
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'HomePages' AND COLUMN_NAME = 'FeaturedProjectsJson')
BEGIN
    ALTER TABLE HomePages ADD FeaturedProjectsJson NVARCHAR(MAX) NOT NULL DEFAULT '[]';
    PRINT 'Added FeaturedProjectsJson column to HomePages';
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'HomePages' AND COLUMN_NAME = 'FeaturedProjectsSubtitle')
BEGIN
    ALTER TABLE HomePages ADD FeaturedProjectsSubtitle NVARCHAR(500) NULL;
    PRINT 'Added FeaturedProjectsSubtitle column to HomePages';
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'HomePages' AND COLUMN_NAME = 'FeaturedProjectsTitle')
BEGIN
    ALTER TABLE HomePages ADD FeaturedProjectsTitle NVARCHAR(100) NULL;
    PRINT 'Added FeaturedProjectsTitle column to HomePages';
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'HomePages' AND COLUMN_NAME = 'HeaderBackgroundStyle')
BEGIN
    ALTER TABLE HomePages ADD HeaderBackgroundStyle NVARCHAR(500) NULL;
    PRINT 'Added HeaderBackgroundStyle column to HomePages';
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'HomePages' AND COLUMN_NAME = 'HeaderButtonColor')
BEGIN
    ALTER TABLE HomePages ADD HeaderButtonColor NVARCHAR(50) NULL;
    PRINT 'Added HeaderButtonColor column to HomePages';
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'HomePages' AND COLUMN_NAME = 'HeaderButtonTextColor')
BEGIN
    ALTER TABLE HomePages ADD HeaderButtonTextColor NVARCHAR(50) NULL;
    PRINT 'Added HeaderButtonTextColor column to HomePages';
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'HomePages' AND COLUMN_NAME = 'HeaderSecondaryButtonText')
BEGIN
    ALTER TABLE HomePages ADD HeaderSecondaryButtonText NVARCHAR(50) NULL;
    PRINT 'Added HeaderSecondaryButtonText column to HomePages';
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'HomePages' AND COLUMN_NAME = 'HeaderSecondaryButtonUrl')
BEGIN
    ALTER TABLE HomePages ADD HeaderSecondaryButtonUrl NVARCHAR(200) NULL;
    PRINT 'Added HeaderSecondaryButtonUrl column to HomePages';
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'HomePages' AND COLUMN_NAME = 'HeaderTextColor')
BEGIN
    ALTER TABLE HomePages ADD HeaderTextColor NVARCHAR(50) NULL;
    PRINT 'Added HeaderTextColor column to HomePages';
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'HomePages' AND COLUMN_NAME = 'ShowHeaderPrimaryButton')
BEGIN
    ALTER TABLE HomePages ADD ShowHeaderPrimaryButton BIT NOT NULL DEFAULT 1;
    PRINT 'Added ShowHeaderPrimaryButton column to HomePages';
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'HomePages' AND COLUMN_NAME = 'ShowHeaderSecondaryButton')
BEGIN
    ALTER TABLE HomePages ADD ShowHeaderSecondaryButton BIT NOT NULL DEFAULT 1;
    PRINT 'Added ShowHeaderSecondaryButton column to HomePages';
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'HomePages' AND COLUMN_NAME = 'SkillsCategoriesJson')
BEGIN
    ALTER TABLE HomePages ADD SkillsCategoriesJson NVARCHAR(MAX) NOT NULL DEFAULT '[]';
    PRINT 'Added SkillsCategoriesJson column to HomePages';
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'HomePages' AND COLUMN_NAME = 'SkillsSectionSubtitle')
BEGIN
    ALTER TABLE HomePages ADD SkillsSectionSubtitle NVARCHAR(500) NULL;
    PRINT 'Added SkillsSectionSubtitle column to HomePages';
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'HomePages' AND COLUMN_NAME = 'SkillsSectionTitle')
BEGIN
    ALTER TABLE HomePages ADD SkillsSectionTitle NVARCHAR(100) NULL;
    PRINT 'Added SkillsSectionTitle column to HomePages';
END

PRINT 'All missing columns have been added successfully!';
PRINT 'Database schema should now match the Entity Framework models.'; 
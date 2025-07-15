-- First, let's see what columns actually exist
PRINT 'Current columns in HeroTemplates table:'
SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE 
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'HeroTemplates'
ORDER BY COLUMN_NAME

-- Now add missing columns with proper error handling
PRINT 'Adding missing columns...'

-- CTA columns
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'HeroTemplates' AND COLUMN_NAME = 'CTABackgroundImageUrl')
BEGIN
    ALTER TABLE HeroTemplates ADD CTABackgroundImageUrl NVARCHAR(500) NULL
    PRINT 'Added CTABackgroundImageUrl'
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'HeroTemplates' AND COLUMN_NAME = 'CTAButtonText')
BEGIN
    ALTER TABLE HeroTemplates ADD CTAButtonText NVARCHAR(100) NULL
    PRINT 'Added CTAButtonText'
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'HeroTemplates' AND COLUMN_NAME = 'CTAButtonUrl')
BEGIN
    ALTER TABLE HeroTemplates ADD CTAButtonUrl NVARCHAR(500) NULL
    PRINT 'Added CTAButtonUrl'
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'HeroTemplates' AND COLUMN_NAME = 'CTASubtitle')
BEGIN
    ALTER TABLE HeroTemplates ADD CTASubtitle NVARCHAR(200) NULL
    PRINT 'Added CTASubtitle'
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'HeroTemplates' AND COLUMN_NAME = 'CTATextColor')
BEGIN
    ALTER TABLE HeroTemplates ADD CTATextColor NVARCHAR(20) NULL
    PRINT 'Added CTATextColor'
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'HeroTemplates' AND COLUMN_NAME = 'CTATitle')
BEGIN
    ALTER TABLE HeroTemplates ADD CTATitle NVARCHAR(200) NULL
    PRINT 'Added CTATitle'
END

-- Featured Projects columns
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'HeroTemplates' AND COLUMN_NAME = 'FeaturedProjectsJson')
BEGIN
    ALTER TABLE HeroTemplates ADD FeaturedProjectsJson NVARCHAR(MAX) NULL
    PRINT 'Added FeaturedProjectsJson'
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'HeroTemplates' AND COLUMN_NAME = 'FeaturedProjectsSubtitle')
BEGIN
    ALTER TABLE HeroTemplates ADD FeaturedProjectsSubtitle NVARCHAR(200) NULL
    PRINT 'Added FeaturedProjectsSubtitle'
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'HeroTemplates' AND COLUMN_NAME = 'FeaturedProjectsTitle')
BEGIN
    ALTER TABLE HeroTemplates ADD FeaturedProjectsTitle NVARCHAR(200) NULL
    PRINT 'Added FeaturedProjectsTitle'
END

-- Header styling columns
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'HeroTemplates' AND COLUMN_NAME = 'HeaderBackgroundStyle')
BEGIN
    ALTER TABLE HeroTemplates ADD HeaderBackgroundStyle NVARCHAR(50) NULL
    PRINT 'Added HeaderBackgroundStyle'
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'HeroTemplates' AND COLUMN_NAME = 'HeaderButtonColor')
BEGIN
    ALTER TABLE HeroTemplates ADD HeaderButtonColor NVARCHAR(20) NULL
    PRINT 'Added HeaderButtonColor'
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'HeroTemplates' AND COLUMN_NAME = 'HeaderButtonTextColor')
BEGIN
    ALTER TABLE HeroTemplates ADD HeaderButtonTextColor NVARCHAR(20) NULL
    PRINT 'Added HeaderButtonTextColor'
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'HeroTemplates' AND COLUMN_NAME = 'HeaderSecondaryButtonText')
BEGIN
    ALTER TABLE HeroTemplates ADD HeaderSecondaryButtonText NVARCHAR(100) NULL
    PRINT 'Added HeaderSecondaryButtonText'
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'HeroTemplates' AND COLUMN_NAME = 'HeaderSecondaryButtonUrl')
BEGIN
    ALTER TABLE HeroTemplates ADD HeaderSecondaryButtonUrl NVARCHAR(500) NULL
    PRINT 'Added HeaderSecondaryButtonUrl'
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'HeroTemplates' AND COLUMN_NAME = 'HeaderTextColor')
BEGIN
    ALTER TABLE HeroTemplates ADD HeaderTextColor NVARCHAR(20) NULL
    PRINT 'Added HeaderTextColor'
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'HeroTemplates' AND COLUMN_NAME = 'ShowHeaderPrimaryButton')
BEGIN
    ALTER TABLE HeroTemplates ADD ShowHeaderPrimaryButton BIT NULL
    PRINT 'Added ShowHeaderPrimaryButton'
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'HeroTemplates' AND COLUMN_NAME = 'ShowHeaderSecondaryButton')
BEGIN
    ALTER TABLE HeroTemplates ADD ShowHeaderSecondaryButton BIT NULL
    PRINT 'Added ShowHeaderSecondaryButton'
END

-- Skills section columns
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'HeroTemplates' AND COLUMN_NAME = 'SkillsCategoriesJson')
BEGIN
    ALTER TABLE HeroTemplates ADD SkillsCategoriesJson NVARCHAR(MAX) NULL
    PRINT 'Added SkillsCategoriesJson'
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'HeroTemplates' AND COLUMN_NAME = 'SkillsSectionSubtitle')
BEGIN
    ALTER TABLE HeroTemplates ADD SkillsSectionSubtitle NVARCHAR(200) NULL
    PRINT 'Added SkillsSectionSubtitle'
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'HeroTemplates' AND COLUMN_NAME = 'SkillsSectionTitle')
BEGIN
    ALTER TABLE HeroTemplates ADD SkillsSectionTitle NVARCHAR(200) NULL
    PRINT 'Added SkillsSectionTitle'
END

PRINT 'Column addition complete!' 
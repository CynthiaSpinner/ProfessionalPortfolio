-- Add all missing columns to HeroTemplates table
-- These columns are expected by the Entity Framework model

-- CTA (Call to Action) columns
ALTER TABLE HeroTemplates ADD CTABackgroundImageUrl NVARCHAR(500) NULL
ALTER TABLE HeroTemplates ADD CTAButtonText NVARCHAR(100) NULL
ALTER TABLE HeroTemplates ADD CTAButtonUrl NVARCHAR(500) NULL
ALTER TABLE HeroTemplates ADD CTASubtitle NVARCHAR(200) NULL
ALTER TABLE HeroTemplates ADD CTATextColor NVARCHAR(20) NULL
ALTER TABLE HeroTemplates ADD CTATitle NVARCHAR(200) NULL

-- Featured Projects columns
ALTER TABLE HeroTemplates ADD FeaturedProjectsJson NVARCHAR(MAX) NULL
ALTER TABLE HeroTemplates ADD FeaturedProjectsSubtitle NVARCHAR(200) NULL
ALTER TABLE HeroTemplates ADD FeaturedProjectsTitle NVARCHAR(200) NULL

-- Header styling columns
ALTER TABLE HeroTemplates ADD HeaderBackgroundStyle NVARCHAR(50) NULL
ALTER TABLE HeroTemplates ADD HeaderButtonColor NVARCHAR(20) NULL
ALTER TABLE HeroTemplates ADD HeaderButtonTextColor NVARCHAR(20) NULL
ALTER TABLE HeroTemplates ADD HeaderSecondaryButtonText NVARCHAR(100) NULL
ALTER TABLE HeroTemplates ADD HeaderSecondaryButtonUrl NVARCHAR(500) NULL
ALTER TABLE HeroTemplates ADD HeaderTextColor NVARCHAR(20) NULL
ALTER TABLE HeroTemplates ADD ShowHeaderPrimaryButton BIT NULL
ALTER TABLE HeroTemplates ADD ShowHeaderSecondaryButton BIT NULL

-- Skills section columns
ALTER TABLE HeroTemplates ADD SkillsCategoriesJson NVARCHAR(MAX) NULL
ALTER TABLE HeroTemplates ADD SkillsSectionSubtitle NVARCHAR(200) NULL
ALTER TABLE HeroTemplates ADD SkillsSectionTitle NVARCHAR(200) NULL

PRINT 'All missing columns have been added to HeroTemplates table' 
-- Apply HeroTemplate Migration Script
-- This script adds all the missing columns to the HeroTemplates table
-- Based on migration: 20250715213153_UpdateHeroTemplateColumns

-- First, make HeaderOverlayOpacity nullable
ALTER TABLE HeroTemplates ALTER COLUMN HeaderOverlayOpacity REAL NULL;

-- Add all the missing columns
ALTER TABLE HeroTemplates ADD CTABackgroundImageUrl NVARCHAR(500) NULL;
ALTER TABLE HeroTemplates ADD CTAButtonText NVARCHAR(100) NULL;
ALTER TABLE HeroTemplates ADD CTAButtonUrl NVARCHAR(500) NULL;
ALTER TABLE HeroTemplates ADD CTASubtitle NVARCHAR(200) NULL;
ALTER TABLE HeroTemplates ADD CTATextColor NVARCHAR(20) NULL;
ALTER TABLE HeroTemplates ADD CTATitle NVARCHAR(200) NULL;
ALTER TABLE HeroTemplates ADD FeaturedProjectsJson NVARCHAR(MAX) NULL;
ALTER TABLE HeroTemplates ADD FeaturedProjectsSubtitle NVARCHAR(200) NULL;
ALTER TABLE HeroTemplates ADD FeaturedProjectsTitle NVARCHAR(200) NULL;
ALTER TABLE HeroTemplates ADD HeaderBackgroundStyle NVARCHAR(50) NULL;
ALTER TABLE HeroTemplates ADD HeaderButtonColor NVARCHAR(20) NULL;
ALTER TABLE HeroTemplates ADD HeaderButtonTextColor NVARCHAR(20) NULL;
ALTER TABLE HeroTemplates ADD HeaderSecondaryButtonText NVARCHAR(100) NULL;
ALTER TABLE HeroTemplates ADD HeaderSecondaryButtonUrl NVARCHAR(500) NULL;
ALTER TABLE HeroTemplates ADD HeaderTextColor NVARCHAR(20) NULL;
ALTER TABLE HeroTemplates ADD IsActive BIT NULL;
ALTER TABLE HeroTemplates ADD Name NVARCHAR(100) NOT NULL DEFAULT '';
ALTER TABLE HeroTemplates ADD ShowHeaderPrimaryButton BIT NULL;
ALTER TABLE HeroTemplates ADD ShowHeaderSecondaryButton BIT NULL;
ALTER TABLE HeroTemplates ADD SkillsCategoriesJson NVARCHAR(MAX) NULL;
ALTER TABLE HeroTemplates ADD SkillsSectionSubtitle NVARCHAR(200) NULL;
ALTER TABLE HeroTemplates ADD SkillsSectionTitle NVARCHAR(200) NULL;
ALTER TABLE HeroTemplates ADD Template NVARCHAR(500) NOT NULL DEFAULT '';

PRINT 'Migration applied successfully! All missing columns have been added to HeroTemplates table.'; 
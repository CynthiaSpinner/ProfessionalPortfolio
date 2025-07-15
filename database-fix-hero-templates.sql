-- Fix HeroTemplates Table Script for Cynthia Portfolio
-- This script adds the missing columns to the HeroTemplates table

-- Add missing columns to HeroTemplates table
IF COLUMNPROPERTY(OBJECT_ID('HeroTemplates'), 'Nickname', 'ColumnId') IS NULL
BEGIN
    ALTER TABLE HeroTemplates ADD Nickname NVARCHAR(100) NOT NULL DEFAULT 'Template';
    PRINT 'Added Nickname column to HeroTemplates table';
END
ELSE
    PRINT 'Nickname column already exists in HeroTemplates table';

IF COLUMNPROPERTY(OBJECT_ID('HeroTemplates'), 'HeaderTitle', 'ColumnId') IS NULL
BEGIN
    ALTER TABLE HeroTemplates ADD HeaderTitle NVARCHAR(200) NOT NULL DEFAULT 'Hero Title';
    PRINT 'Added HeaderTitle column to HeroTemplates table';
END
ELSE
    PRINT 'HeaderTitle column already exists in HeroTemplates table';

IF COLUMNPROPERTY(OBJECT_ID('HeroTemplates'), 'HeaderSubtitle', 'ColumnId') IS NULL
BEGIN
    ALTER TABLE HeroTemplates ADD HeaderSubtitle NVARCHAR(500) NOT NULL DEFAULT 'Hero Subtitle';
    PRINT 'Added HeaderSubtitle column to HeroTemplates table';
END
ELSE
    PRINT 'HeaderSubtitle column already exists in HeroTemplates table';

IF COLUMNPROPERTY(OBJECT_ID('HeroTemplates'), 'HeaderDescription', 'ColumnId') IS NULL
BEGIN
    ALTER TABLE HeroTemplates ADD HeaderDescription NVARCHAR(MAX) NOT NULL DEFAULT '';
    PRINT 'Added HeaderDescription column to HeroTemplates table';
END
ELSE
    PRINT 'HeaderDescription column already exists in HeroTemplates table';

IF COLUMNPROPERTY(OBJECT_ID('HeroTemplates'), 'HeaderBackgroundImageUrl', 'ColumnId') IS NULL
BEGIN
    ALTER TABLE HeroTemplates ADD HeaderBackgroundImageUrl NVARCHAR(500) NOT NULL DEFAULT '';
    PRINT 'Added HeaderBackgroundImageUrl column to HeroTemplates table';
END
ELSE
    PRINT 'HeaderBackgroundImageUrl column already exists in HeroTemplates table';

IF COLUMNPROPERTY(OBJECT_ID('HeroTemplates'), 'HeaderBackgroundVideoUrl', 'ColumnId') IS NULL
BEGIN
    ALTER TABLE HeroTemplates ADD HeaderBackgroundVideoUrl NVARCHAR(500) NOT NULL DEFAULT '';
    PRINT 'Added HeaderBackgroundVideoUrl column to HeroTemplates table';
END
ELSE
    PRINT 'HeaderBackgroundVideoUrl column already exists in HeroTemplates table';

IF COLUMNPROPERTY(OBJECT_ID('HeroTemplates'), 'HeaderPrimaryButtonText', 'ColumnId') IS NULL
BEGIN
    ALTER TABLE HeroTemplates ADD HeaderPrimaryButtonText NVARCHAR(100) NOT NULL DEFAULT '';
    PRINT 'Added HeaderPrimaryButtonText column to HeroTemplates table';
END
ELSE
    PRINT 'HeaderPrimaryButtonText column already exists in HeroTemplates table';

IF COLUMNPROPERTY(OBJECT_ID('HeroTemplates'), 'HeaderPrimaryButtonUrl', 'ColumnId') IS NULL
BEGIN
    ALTER TABLE HeroTemplates ADD HeaderPrimaryButtonUrl NVARCHAR(200) NOT NULL DEFAULT '';
    PRINT 'Added HeaderPrimaryButtonUrl column to HeroTemplates table';
END
ELSE
    PRINT 'HeaderPrimaryButtonUrl column already exists in HeroTemplates table';

IF COLUMNPROPERTY(OBJECT_ID('HeroTemplates'), 'HeaderOverlayColor', 'ColumnId') IS NULL
BEGIN
    ALTER TABLE HeroTemplates ADD HeaderOverlayColor NVARCHAR(7) NOT NULL DEFAULT '#000000';
    PRINT 'Added HeaderOverlayColor column to HeroTemplates table';
END
ELSE
    PRINT 'HeaderOverlayColor column already exists in HeroTemplates table';

IF COLUMNPROPERTY(OBJECT_ID('HeroTemplates'), 'HeaderOverlayOpacity', 'ColumnId') IS NULL
BEGIN
    ALTER TABLE HeroTemplates ADD HeaderOverlayOpacity FLOAT NOT NULL DEFAULT 0.5;
    PRINT 'Added HeaderOverlayOpacity column to HeroTemplates table';
END
ELSE
    PRINT 'HeaderOverlayOpacity column already exists in HeroTemplates table';

IF COLUMNPROPERTY(OBJECT_ID('HeroTemplates'), 'CreatedAt', 'ColumnId') IS NULL
BEGIN
    ALTER TABLE HeroTemplates ADD CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE();
    PRINT 'Added CreatedAt column to HeroTemplates table';
END
ELSE
    PRINT 'CreatedAt column already exists in HeroTemplates table';

PRINT '';
PRINT 'HeroTemplates table schema fix completed!';
PRINT 'All required columns should now exist.'; 
-- Fix HomePages Table Script for Cynthia Portfolio
-- This script checks and fixes the HomePages table schema

-- Check if HomePages table exists
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'HomePages')
BEGIN
    PRINT 'HomePages table does not exist. Creating it...';
    
    CREATE TABLE HomePages (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        HeaderTitle NVARCHAR(100) NOT NULL DEFAULT 'Welcome to My Portfolio',
        HeaderSubtitle NVARCHAR(500) NOT NULL DEFAULT 'I am a passionate software engineer specializing in full-stack development.',
        HeaderDescription NVARCHAR(1000) NULL,
        HeaderBackgroundImageUrl NVARCHAR(500) NULL,
        HeaderBackgroundVideoUrl NVARCHAR(500) NULL,
        HeaderBackgroundStyle NVARCHAR(500) NULL,
        HeaderPrimaryButtonText NVARCHAR(50) NULL,
        HeaderPrimaryButtonUrl NVARCHAR(200) NULL,
        HeaderSecondaryButtonText NVARCHAR(50) NULL,
        HeaderSecondaryButtonUrl NVARCHAR(200) NULL,
        ShowHeaderPrimaryButton BIT NOT NULL DEFAULT 1,
        ShowHeaderSecondaryButton BIT NOT NULL DEFAULT 1,
        HeaderOverlayColor NVARCHAR(50) NULL,
        HeaderOverlayOpacity FLOAT NULL,
        HeaderTextColor NVARCHAR(50) NULL,
        HeaderButtonColor NVARCHAR(50) NULL,
        HeaderButtonTextColor NVARCHAR(50) NULL,
        SkillsSectionTitle NVARCHAR(100) NULL,
        SkillsSectionSubtitle NVARCHAR(500) NULL,
        SkillsCategoriesJson NVARCHAR(MAX) NOT NULL DEFAULT '[]',
        FeaturedProjectsTitle NVARCHAR(100) NULL,
        FeaturedProjectsSubtitle NVARCHAR(500) NULL,
        FeaturedProjectsJson NVARCHAR(MAX) NOT NULL DEFAULT '[]',
        CTATitle NVARCHAR(100) NULL,
        CTASubtitle NVARCHAR(500) NULL,
        CTAButtonText NVARCHAR(50) NULL,
        CTAButtonUrl NVARCHAR(200) NULL,
        CTABackgroundImageUrl NVARCHAR(500) NULL,
        CTATextColor NVARCHAR(50) NULL,
        DisplayOrder INT NOT NULL DEFAULT 0,
        IsActive BIT NOT NULL DEFAULT 1,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt DATETIME2 NULL
    );
    
    PRINT 'HomePages table created successfully.';
END
ELSE
BEGIN
    PRINT 'HomePages table exists. Checking for missing columns...';
    
    -- Add missing columns if they don't exist
    IF COLUMNPROPERTY(OBJECT_ID('HomePages'), 'HeaderTitle', 'ColumnId') IS NULL
    BEGIN
        ALTER TABLE HomePages ADD HeaderTitle NVARCHAR(100) NOT NULL DEFAULT 'Welcome to My Portfolio';
        PRINT 'Added HeaderTitle column to HomePages table';
    END

    IF COLUMNPROPERTY(OBJECT_ID('HomePages'), 'HeaderSubtitle', 'ColumnId') IS NULL
    BEGIN
        ALTER TABLE HomePages ADD HeaderSubtitle NVARCHAR(500) NOT NULL DEFAULT 'I am a passionate software engineer specializing in full-stack development.';
        PRINT 'Added HeaderSubtitle column to HomePages table';
    END

    IF COLUMNPROPERTY(OBJECT_ID('HomePages'), 'HeaderDescription', 'ColumnId') IS NULL
    BEGIN
        ALTER TABLE HomePages ADD HeaderDescription NVARCHAR(1000) NULL;
        PRINT 'Added HeaderDescription column to HomePages table';
    END

    IF COLUMNPROPERTY(OBJECT_ID('HomePages'), 'HeaderBackgroundImageUrl', 'ColumnId') IS NULL
    BEGIN
        ALTER TABLE HomePages ADD HeaderBackgroundImageUrl NVARCHAR(500) NULL;
        PRINT 'Added HeaderBackgroundImageUrl column to HomePages table';
    END

    IF COLUMNPROPERTY(OBJECT_ID('HomePages'), 'HeaderBackgroundVideoUrl', 'ColumnId') IS NULL
    BEGIN
        ALTER TABLE HomePages ADD HeaderBackgroundVideoUrl NVARCHAR(500) NULL;
        PRINT 'Added HeaderBackgroundVideoUrl column to HomePages table';
    END

    IF COLUMNPROPERTY(OBJECT_ID('HomePages'), 'HeaderPrimaryButtonText', 'ColumnId') IS NULL
    BEGIN
        ALTER TABLE HomePages ADD HeaderPrimaryButtonText NVARCHAR(50) NULL;
        PRINT 'Added HeaderPrimaryButtonText column to HomePages table';
    END

    IF COLUMNPROPERTY(OBJECT_ID('HomePages'), 'HeaderPrimaryButtonUrl', 'ColumnId') IS NULL
    BEGIN
        ALTER TABLE HomePages ADD HeaderPrimaryButtonUrl NVARCHAR(200) NULL;
        PRINT 'Added HeaderPrimaryButtonUrl column to HomePages table';
    END

    IF COLUMNPROPERTY(OBJECT_ID('HomePages'), 'HeaderOverlayColor', 'ColumnId') IS NULL
    BEGIN
        ALTER TABLE HomePages ADD HeaderOverlayColor NVARCHAR(50) NULL;
        PRINT 'Added HeaderOverlayColor column to HomePages table';
    END

    IF COLUMNPROPERTY(OBJECT_ID('HomePages'), 'HeaderOverlayOpacity', 'ColumnId') IS NULL
    BEGIN
        ALTER TABLE HomePages ADD HeaderOverlayOpacity FLOAT NULL;
        PRINT 'Added HeaderOverlayOpacity column to HomePages table';
    END

    IF COLUMNPROPERTY(OBJECT_ID('HomePages'), 'IsActive', 'ColumnId') IS NULL
    BEGIN
        ALTER TABLE HomePages ADD IsActive BIT NOT NULL DEFAULT 1;
        PRINT 'Added IsActive column to HomePages table';
    END

    IF COLUMNPROPERTY(OBJECT_ID('HomePages'), 'CreatedAt', 'ColumnId') IS NULL
    BEGIN
        ALTER TABLE HomePages ADD CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE();
        PRINT 'Added CreatedAt column to HomePages table';
    END

    IF COLUMNPROPERTY(OBJECT_ID('HomePages'), 'UpdatedAt', 'ColumnId') IS NULL
    BEGIN
        ALTER TABLE HomePages ADD UpdatedAt DATETIME2 NULL;
        PRINT 'Added UpdatedAt column to HomePages table';
    END
END

-- Insert a default home page if none exists
IF NOT EXISTS (SELECT * FROM HomePages)
BEGIN
    INSERT INTO HomePages (
        HeaderTitle, 
        HeaderSubtitle, 
        HeaderDescription, 
        HeaderPrimaryButtonText, 
        HeaderPrimaryButtonUrl, 
        HeaderOverlayColor, 
        HeaderOverlayOpacity, 
        IsActive, 
        CreatedAt
    ) VALUES (
        'Welcome to CodeSpinner & Design',
        'Senior Software Engineer & Designer',
        'Passionate about creating exceptional digital experiences through innovative code and thoughtful design.',
        'View Projects',
        '/projects',
        '#000000',
        0.5,
        1,
        GETUTCDATE()
    );
    PRINT 'Default home page created.';
END

PRINT '';
PRINT 'HomePages table schema check completed!'; 
-- Create FeaturesSections table in Azure SQL Database
-- Run this script directly on your Azure SQL database

-- Check if table already exists
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'FeaturesSections')
BEGIN
    -- Create the FeaturesSections table
    CREATE TABLE [dbo].[FeaturesSections] (
        [Id] int IDENTITY(1,1) NOT NULL,
        [SectionTitle] nvarchar(200) NOT NULL,
        [SectionSubtitle] nvarchar(500) NULL,
        [Feature1Title] nvarchar(100) NOT NULL,
        [Feature1Subtitle] nvarchar(200) NOT NULL,
        [Feature1Description] nvarchar(500) NULL,
        [Feature1Icon] nvarchar(100) NULL,
        [Feature1Link] nvarchar(200) NULL,
        [Feature2Title] nvarchar(100) NOT NULL,
        [Feature2Subtitle] nvarchar(200) NOT NULL,
        [Feature2Description] nvarchar(500) NULL,
        [Feature2Icon] nvarchar(100) NULL,
        [Feature2Link] nvarchar(200) NULL,
        [Feature3Title] nvarchar(100) NOT NULL,
        [Feature3Subtitle] nvarchar(200) NOT NULL,
        [Feature3Description] nvarchar(500) NULL,
        [Feature3Icon] nvarchar(100) NULL,
        [Feature3Link] nvarchar(200) NULL,
        [IsActive] bit NOT NULL DEFAULT 1,
        [DisplayOrder] int NOT NULL DEFAULT 1,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NOT NULL,
        CONSTRAINT [PK_FeaturesSections] PRIMARY KEY ([Id])
    );
    
    PRINT 'FeaturesSections table created successfully.';
END
ELSE
BEGIN
    -- Table exists, check if we need to add missing columns
    PRINT 'FeaturesSections table already exists. Checking for missing columns...';
    
    -- Add SectionSubtitle if it doesn't exist
    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'FeaturesSections' AND COLUMN_NAME = 'SectionSubtitle')
    BEGIN
        ALTER TABLE [dbo].[FeaturesSections] ADD [SectionSubtitle] nvarchar(500) NULL;
        PRINT 'Added SectionSubtitle column.';
    END
    
    -- Add Feature1Description if it doesn't exist
    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'FeaturesSections' AND COLUMN_NAME = 'Feature1Description')
    BEGIN
        ALTER TABLE [dbo].[FeaturesSections] ADD [Feature1Description] nvarchar(500) NULL;
        PRINT 'Added Feature1Description column.';
    END
    
    -- Add Feature1Icon if it doesn't exist
    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'FeaturesSections' AND COLUMN_NAME = 'Feature1Icon')
    BEGIN
        ALTER TABLE [dbo].[FeaturesSections] ADD [Feature1Icon] nvarchar(100) NULL;
        PRINT 'Added Feature1Icon column.';
    END
    
    -- Add Feature1Link if it doesn't exist
    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'FeaturesSections' AND COLUMN_NAME = 'Feature1Link')
    BEGIN
        ALTER TABLE [dbo].[FeaturesSections] ADD [Feature1Link] nvarchar(200) NULL;
        PRINT 'Added Feature1Link column.';
    END
    
    -- Add Feature2Description if it doesn't exist
    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'FeaturesSections' AND COLUMN_NAME = 'Feature2Description')
    BEGIN
        ALTER TABLE [dbo].[FeaturesSections] ADD [Feature2Description] nvarchar(500) NULL;
        PRINT 'Added Feature2Description column.';
    END
    
    -- Add Feature2Icon if it doesn't exist
    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'FeaturesSections' AND COLUMN_NAME = 'Feature2Icon')
    BEGIN
        ALTER TABLE [dbo].[FeaturesSections] ADD [Feature2Icon] nvarchar(100) NULL;
        PRINT 'Added Feature2Icon column.';
    END
    
    -- Add Feature2Link if it doesn't exist
    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'FeaturesSections' AND COLUMN_NAME = 'Feature2Link')
    BEGIN
        ALTER TABLE [dbo].[FeaturesSections] ADD [Feature2Link] nvarchar(200) NULL;
        PRINT 'Added Feature2Link column.';
    END
    
    -- Add Feature3Description if it doesn't exist
    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'FeaturesSections' AND COLUMN_NAME = 'Feature3Description')
    BEGIN
        ALTER TABLE [dbo].[FeaturesSections] ADD [Feature3Description] nvarchar(500) NULL;
        PRINT 'Added Feature3Description column.';
    END
    
    -- Add Feature3Icon if it doesn't exist
    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'FeaturesSections' AND COLUMN_NAME = 'Feature3Icon')
    BEGIN
        ALTER TABLE [dbo].[FeaturesSections] ADD [Feature3Icon] nvarchar(100) NULL;
        PRINT 'Added Feature3Icon column.';
    END
    
    -- Add Feature3Link if it doesn't exist
    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'FeaturesSections' AND COLUMN_NAME = 'Feature3Link')
    BEGIN
        ALTER TABLE [dbo].[FeaturesSections] ADD [Feature3Link] nvarchar(200) NULL;
        PRINT 'Added Feature3Link column.';
    END
    
    -- Add IsActive if it doesn't exist
    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'FeaturesSections' AND COLUMN_NAME = 'IsActive')
    BEGIN
        ALTER TABLE [dbo].[FeaturesSections] ADD [IsActive] bit NOT NULL DEFAULT 1;
        PRINT 'Added IsActive column.';
    END
    
    -- Add DisplayOrder if it doesn't exist
    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'FeaturesSections' AND COLUMN_NAME = 'DisplayOrder')
    BEGIN
        ALTER TABLE [dbo].[FeaturesSections] ADD [DisplayOrder] int NOT NULL DEFAULT 1;
        PRINT 'Added DisplayOrder column.';
    END
END

-- Insert default data if table is empty
IF NOT EXISTS (SELECT * FROM [dbo].[FeaturesSections])
BEGIN
    INSERT INTO [dbo].[FeaturesSections] (
        [SectionTitle],
        [SectionSubtitle],
        [Feature1Title],
        [Feature1Subtitle],
        [Feature1Description],
        [Feature1Icon],
        [Feature1Link],
        [Feature2Title],
        [Feature2Subtitle],
        [Feature2Description],
        [Feature2Icon],
        [Feature2Link],
        [Feature3Title],
        [Feature3Subtitle],
        [Feature3Description],
        [Feature3Icon],
        [Feature3Link],
        [IsActive],
        [DisplayOrder],
        [CreatedAt],
        [UpdatedAt]
    ) VALUES (
        'Key Skills & Technologies',
        'Explore my expertise across different domains',
        'Frontend Development',
        'React, JavaScript, HTML5, CSS3, Bootstrap',
        'Building responsive and interactive user interfaces with modern frameworks and best practices.',
        'fas fa-code',
        '/projects?category=frontend',
        'Backend Development',
        '.NET Core, C#, RESTful APIs, SQL Server',
        'Creating robust server-side applications and APIs with enterprise-grade technologies.',
        'fas fa-server',
        '/projects?category=backend',
        'Design & Tools',
        'Adobe Creative Suite, UI/UX Design, Git, Docker',
        'Crafting beautiful designs and managing development workflows with professional tools.',
        'fas fa-palette',
        '/projects?category=design',
        1,
        1,
        GETUTCDATE(),
        GETUTCDATE()
    );
    
    PRINT 'Default features data inserted successfully.';
END

-- Verify the table structure
SELECT 
    COLUMN_NAME,
    DATA_TYPE,
    IS_NULLABLE,
    CHARACTER_MAXIMUM_LENGTH
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'FeaturesSections'
ORDER BY ORDINAL_POSITION;

-- Show the data
SELECT * FROM [dbo].[FeaturesSections]; 
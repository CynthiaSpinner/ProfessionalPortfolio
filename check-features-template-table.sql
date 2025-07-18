-- Check if FeaturesTemplates table exists
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FeaturesTemplates]') AND type in (N'U'))
BEGIN
    PRINT 'FeaturesTemplates table does not exist. Creating it...';
    
    CREATE TABLE [dbo].[FeaturesTemplates](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [Nickname] [nvarchar](100) NOT NULL,
        [SectionTitle] [nvarchar](100) NOT NULL,
        [SectionSubtitle] [nvarchar](200) NOT NULL,
        [SectionDescription] [nvarchar](500) NOT NULL,
        [Feature1Title] [nvarchar](100) NOT NULL,
        [Feature1Subtitle] [nvarchar](200) NOT NULL,
        [Feature1Description] [nvarchar](500) NOT NULL,
        [Feature1Icon] [nvarchar](50) NOT NULL,
        [Feature1Link] [nvarchar](200) NOT NULL,
        [Feature2Title] [nvarchar](100) NOT NULL,
        [Feature2Subtitle] [nvarchar](200) NOT NULL,
        [Feature2Description] [nvarchar](500) NOT NULL,
        [Feature2Icon] [nvarchar](50) NOT NULL,
        [Feature2Link] [nvarchar](200) NOT NULL,
        [Feature3Title] [nvarchar](100) NOT NULL,
        [Feature3Subtitle] [nvarchar](200) NOT NULL,
        [Feature3Description] [nvarchar](500) NOT NULL,
        [Feature3Icon] [nvarchar](50) NOT NULL,
        [Feature3Link] [nvarchar](200) NOT NULL,
        [CreatedAt] [datetime2](7) NOT NULL,
        [UpdatedAt] [datetime2](7) NULL,
        CONSTRAINT [PK_FeaturesTemplates] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
    
    PRINT 'FeaturesTemplates table created successfully.';
    
    -- Insert default template
    INSERT INTO [dbo].[FeaturesTemplates] (
        [Nickname], [SectionTitle], [SectionSubtitle], [SectionDescription],
        [Feature1Title], [Feature1Subtitle], [Feature1Description], [Feature1Icon], [Feature1Link],
        [Feature2Title], [Feature2Subtitle], [Feature2Description], [Feature2Icon], [Feature2Link],
        [Feature3Title], [Feature3Subtitle], [Feature3Description], [Feature3Icon], [Feature3Link],
        [CreatedAt]
    ) VALUES (
        'Default Features', 'Key Skills & Technologies', 'Explore my expertise across different domains', '',
        'Frontend Development', 'React, JavaScript, HTML5, CSS3, Bootstrap', 'Building responsive and interactive user interfaces with modern frameworks and best practices.', 'fas fa-code', '/projects?category=frontend',
        'Backend Development', '.NET Core, C#, RESTful APIs, SQL Server', 'Creating robust server-side applications and APIs with enterprise-grade technologies.', 'fas fa-server', '/projects?category=backend',
        'Design & Tools', 'Adobe Creative Suite, UI/UX Design, Git, Docker', 'Crafting beautiful designs and managing development workflows with professional tools.', 'fas fa-palette', '/projects?category=design',
        GETUTCDATE()
    );
    
    PRINT 'Default FeaturesTemplates record inserted successfully.';
END
ELSE
BEGIN
    PRINT 'FeaturesTemplates table already exists.';
    
    -- Check if table has any data
    DECLARE @RowCount INT;
    SELECT @RowCount = COUNT(*) FROM [dbo].[FeaturesTemplates];
    PRINT 'FeaturesTemplates table has ' + CAST(@RowCount AS VARCHAR) + ' records.';
    
    -- Show sample data
    SELECT TOP 3 [Id], [Nickname], [SectionTitle], [CreatedAt], [UpdatedAt] FROM [dbo].[FeaturesTemplates] ORDER BY [CreatedAt] DESC;
END 
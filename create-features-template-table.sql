-- Create FeaturesTemplate table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FeaturesTemplates]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[FeaturesTemplates] (
        [Id] int IDENTITY(1,1) NOT NULL,
        [Nickname] nvarchar(100) NOT NULL,
        [SectionTitle] nvarchar(100) NOT NULL,
        [SectionSubtitle] nvarchar(200) NULL,
        [SectionDescription] nvarchar(500) NULL,
        [Feature1Title] nvarchar(100) NOT NULL,
        [Feature1Subtitle] nvarchar(200) NULL,
        [Feature1Description] nvarchar(500) NULL,
        [Feature1Icon] nvarchar(50) NULL,
        [Feature1Link] nvarchar(200) NULL,
        [Feature2Title] nvarchar(100) NOT NULL,
        [Feature2Subtitle] nvarchar(200) NULL,
        [Feature2Description] nvarchar(500) NULL,
        [Feature2Icon] nvarchar(50) NULL,
        [Feature2Link] nvarchar(200) NULL,
        [Feature3Title] nvarchar(100) NOT NULL,
        [Feature3Subtitle] nvarchar(200) NULL,
        [Feature3Description] nvarchar(500) NULL,
        [Feature3Icon] nvarchar(50) NULL,
        [Feature3Link] nvarchar(200) NULL,
        [CreatedAt] datetime2 NOT NULL DEFAULT GETUTCDATE(),
        [UpdatedAt] datetime2 NULL,
        CONSTRAINT [PK_FeaturesTemplates] PRIMARY KEY ([Id])
    );
    PRINT 'FeaturesTemplates table created successfully.';
END
ELSE
BEGIN
    PRINT 'FeaturesTemplates table already exists.';
END

-- Insert default template
IF NOT EXISTS (SELECT * FROM [dbo].[FeaturesTemplates] WHERE [Nickname] = 'Default Skills Template')
BEGIN
    INSERT INTO [dbo].[FeaturesTemplates] (
        [Nickname],
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
        [CreatedAt]
    ) VALUES (
        'Default Skills Template',
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
        GETUTCDATE()
    );
    PRINT 'Default FeaturesTemplate inserted successfully.';
END
ELSE
BEGIN
    PRINT 'Default FeaturesTemplate already exists.';
END

-- Create index for better performance
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_FeaturesTemplates_Nickname')
BEGIN
    CREATE INDEX [IX_FeaturesTemplates_Nickname] ON [dbo].[FeaturesTemplates] ([Nickname]);
    PRINT 'Index on FeaturesTemplates.Nickname created successfully.';
END
ELSE
BEGIN
    PRINT 'Index on FeaturesTemplates.Nickname already exists.';
END 
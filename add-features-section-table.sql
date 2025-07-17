-- Add FeaturesSections table to Azure SQL Database
-- Run this script directly on your Azure SQL database

CREATE TABLE [dbo].[FeaturesSections] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SectionTitle] nvarchar(200) NOT NULL,
    [Feature1Title] nvarchar(100) NOT NULL,
    [Feature1Subtitle] nvarchar(200) NOT NULL,
    [Feature2Title] nvarchar(100) NOT NULL,
    [Feature2Subtitle] nvarchar(200) NOT NULL,
    [Feature3Title] nvarchar(100) NOT NULL,
    [Feature3Subtitle] nvarchar(200) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_FeaturesSections] PRIMARY KEY ([Id])
);

-- Insert default data
INSERT INTO [dbo].[FeaturesSections] (
    [SectionTitle],
    [Feature1Title],
    [Feature1Subtitle],
    [Feature2Title],
    [Feature2Subtitle],
    [Feature3Title],
    [Feature3Subtitle],
    [CreatedAt],
    [UpdatedAt]
) VALUES (
    'Key Skills & Technologies',
    'Frontend Development',
    'React, JavaScript, HTML5, CSS3, Bootstrap',
    'Backend Development',
    '.NET Core, C#, RESTful APIs, MySQL',
    'Design & Tools',
    'Adobe Creative Suite, UI/UX Design, Git, Docker',
    GETUTCDATE(),
    GETUTCDATE()
);

-- Verify the table was created
SELECT * FROM [dbo].[FeaturesSections]; 
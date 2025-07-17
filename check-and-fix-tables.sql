-- Check and fix Projects and SkillsCategories tables
-- This script will create the tables if they don't exist or fix their structure

-- Check if Projects table exists
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Projects')
BEGIN
    PRINT 'Creating Projects table...'
    CREATE TABLE Projects (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Title NVARCHAR(MAX) NOT NULL,
        Description NVARCHAR(MAX) NOT NULL,
        ImageUrl NVARCHAR(MAX) NOT NULL,
        ProjectUrl NVARCHAR(MAX) NOT NULL,
        GithubUrl NVARCHAR(MAX) NOT NULL,
        TechnologiesJson NVARCHAR(MAX) DEFAULT '[]',
        CompletionDate DATETIME2 NOT NULL
    )
    PRINT 'Projects table created successfully'
END
ELSE
BEGIN
    PRINT 'Projects table already exists'
    
    -- Check if required columns exist
    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Projects' AND COLUMN_NAME = 'TechnologiesJson')
    BEGIN
        PRINT 'Adding TechnologiesJson column to Projects table...'
        ALTER TABLE Projects ADD TechnologiesJson NVARCHAR(MAX) DEFAULT '[]'
    END
    
    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Projects' AND COLUMN_NAME = 'CompletionDate')
    BEGIN
        PRINT 'Adding CompletionDate column to Projects table...'
        ALTER TABLE Projects ADD CompletionDate DATETIME2 NOT NULL DEFAULT GETDATE()
    END
END

-- Check if SkillsCategories table exists
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'SkillsCategories')
BEGIN
    PRINT 'Creating SkillsCategories table...'
    CREATE TABLE SkillsCategories (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Title NVARCHAR(100) NOT NULL,
        Description NVARCHAR(500),
        ImagePath NVARCHAR(MAX),
        SkillsJson NVARCHAR(MAX) DEFAULT '[]',
        DisplayOrder INT NOT NULL DEFAULT 0,
        IsActive BIT NOT NULL DEFAULT 1
    )
    PRINT 'SkillsCategories table created successfully'
END
ELSE
BEGIN
    PRINT 'SkillsCategories table already exists'
    
    -- Check if required columns exist
    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'SkillsCategories' AND COLUMN_NAME = 'SkillsJson')
    BEGIN
        PRINT 'Adding SkillsJson column to SkillsCategories table...'
        ALTER TABLE SkillsCategories ADD SkillsJson NVARCHAR(MAX) DEFAULT '[]'
    END
    
    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'SkillsCategories' AND COLUMN_NAME = 'DisplayOrder')
    BEGIN
        PRINT 'Adding DisplayOrder column to SkillsCategories table...'
        ALTER TABLE SkillsCategories ADD DisplayOrder INT NOT NULL DEFAULT 0
    END
    
    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'SkillsCategories' AND COLUMN_NAME = 'IsActive')
    BEGIN
        PRINT 'Adding IsActive column to SkillsCategories table...'
        ALTER TABLE SkillsCategories ADD IsActive BIT NOT NULL DEFAULT 1
    END
END

-- Insert sample data if tables are empty
IF NOT EXISTS (SELECT * FROM Projects)
BEGIN
    PRINT 'Inserting sample projects...'
    INSERT INTO Projects (Title, Description, ImageUrl, ProjectUrl, GithubUrl, TechnologiesJson, CompletionDate)
    VALUES 
    ('Portfolio Website', 'A modern portfolio website built with React and .NET Core', '/images/portfolio.jpg', 'https://portfolio.example.com', 'https://github.com/example/portfolio', '["React", "C#", ".NET Core", "SQL Server"]', '2024-01-15'),
    ('E-commerce Platform', 'Full-stack e-commerce solution with payment integration', '/images/ecommerce.jpg', 'https://ecommerce.example.com', 'https://github.com/example/ecommerce', '["React", "Node.js", "MongoDB", "Stripe"]', '2024-02-20'),
    ('Task Management App', 'Collaborative task management application', '/images/taskapp.jpg', 'https://taskapp.example.com', 'https://github.com/example/taskapp', '["Vue.js", "Express", "PostgreSQL", "Socket.io"]', '2024-03-10')
END

IF NOT EXISTS (SELECT * FROM SkillsCategories)
BEGIN
    PRINT 'Inserting sample skills categories...'
    INSERT INTO SkillsCategories (Title, Description, ImagePath, SkillsJson, DisplayOrder, IsActive)
    VALUES 
    ('Frontend Development', 'Modern frontend frameworks and technologies', '/images/frontend.jpg', '["React", "Vue.js", "JavaScript", "TypeScript", "HTML5", "CSS3", "Bootstrap", "Tailwind CSS"]', 1, 1),
    ('Backend Development', 'Server-side development and APIs', '/images/backend.jpg', '["C#", ".NET Core", "Node.js", "Express", "REST APIs", "GraphQL", "Microservices"]', 2, 1),
    ('Database & Cloud', 'Database management and cloud services', '/images/database.jpg', '["SQL Server", "PostgreSQL", "MongoDB", "Azure", "AWS", "Docker", "Kubernetes"]', 3, 1),
    ('Design & Tools', 'Design tools and development utilities', '/images/design.jpg', '["Adobe Creative Suite", "Figma", "Git", "VS Code", "Postman", "Jira"]', 4, 1)
END

-- Verify tables and data
PRINT 'Verifying tables and data...'
SELECT 'Projects' as TableName, COUNT(*) as RecordCount FROM Projects
UNION ALL
SELECT 'SkillsCategories' as TableName, COUNT(*) as RecordCount FROM SkillsCategories

PRINT 'Database check and fix completed successfully!' 
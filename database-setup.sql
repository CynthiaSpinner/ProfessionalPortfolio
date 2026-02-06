-- Create the database if it doesn't exist
-- (Create the database on your SQL Server or host first if needed)

-- Create tables based on your Entity Framework models
CREATE TABLE [dbo].[Admins] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [Username] NVARCHAR(100) NOT NULL UNIQUE,
    [PasswordHash] NVARCHAR(255) NOT NULL,
    [Email] NVARCHAR(255),
    [CreatedAt] DATETIME2 DEFAULT GETDATE(),
    [LastLogin] DATETIME2
);

CREATE TABLE [dbo].[HomePages] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [Title] NVARCHAR(255) NOT NULL,
    [Subtitle] NVARCHAR(500),
    [HeroImage] NVARCHAR(500),
    [AboutText] NVARCHAR(MAX),
    [ContactEmail] NVARCHAR(255),
    [ContactPhone] NVARCHAR(50),
    [CreatedAt] DATETIME2 DEFAULT GETDATE(),
    [UpdatedAt] DATETIME2 DEFAULT GETDATE()
);

CREATE TABLE [dbo].[Projects] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [Title] NVARCHAR(255) NOT NULL,
    [Description] NVARCHAR(MAX),
    [ImageUrl] NVARCHAR(500),
    [ProjectUrl] NVARCHAR(500),
    [GitHubUrl] NVARCHAR(500),
    [Technologies] NVARCHAR(500),
    [Category] NVARCHAR(100),
    [Featured] BIT DEFAULT 0,
    [CreatedAt] DATETIME2 DEFAULT GETDATE(),
    [UpdatedAt] DATETIME2 DEFAULT GETDATE()
);

CREATE TABLE [dbo].[SkillsCategories] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [Name] NVARCHAR(100) NOT NULL,
    [Description] NVARCHAR(500),
    [Icon] NVARCHAR(100),
    [Order] INT DEFAULT 0,
    [CreatedAt] DATETIME2 DEFAULT GETDATE()
);

CREATE TABLE [dbo].[Videos] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [Title] NVARCHAR(255) NOT NULL,
    [Description] NVARCHAR(MAX),
    [FileName] NVARCHAR(255) NOT NULL,
    [FilePath] NVARCHAR(500) NOT NULL,
    [FileSize] BIGINT,
    [Duration] INT,
    [UploadDate] DATETIME2 DEFAULT GETDATE(),
    [IsPublic] BIT DEFAULT 1
);

CREATE TABLE [dbo].[HeroTemplates] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [Name] NVARCHAR(100) NOT NULL,
    [Template] NVARCHAR(MAX) NOT NULL,
    [IsActive] BIT DEFAULT 0,
    [CreatedAt] DATETIME2 DEFAULT GETDATE()
);

-- Insert default data
INSERT INTO [dbo].[Admins] ([Username], [PasswordHash], [Email]) 
VALUES ('admin', 'YOUR_HASHED_PASSWORD', 'admin@example.com');

INSERT INTO [dbo].[HomePages] ([Title], [Subtitle], [AboutText], [ContactEmail]) 
VALUES ('Cynthia Portfolio', 'Welcome to my portfolio', 'About me text here...', 'contact@example.com');

INSERT INTO [dbo].[SkillsCategories] ([Name], [Description], [Order]) 
VALUES 
('Frontend', 'Frontend development skills', 1),
('Backend', 'Backend development skills', 2),
('Database', 'Database and data management', 3),
('DevOps', 'DevOps and deployment', 4);

-- Insert a default hero template
INSERT INTO [dbo].[HeroTemplates] ([Name], [Template], [IsActive]) 
VALUES ('Default', '{"title": "Welcome", "subtitle": "My Portfolio"}', 1); 
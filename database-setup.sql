-- Database Setup Script for Cynthia Portfolio
-- This script creates the missing tables and indexes needed for the portfolio application
-- Run this script first to set up the database schema

-- Create Abouts table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Abouts')
BEGIN
    CREATE TABLE Abouts (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Name NVARCHAR(MAX) NOT NULL,
        Title NVARCHAR(MAX) NOT NULL,
        Biography NVARCHAR(MAX) NOT NULL,
        ProfileImageUrl NVARCHAR(MAX) NOT NULL,
        Email NVARCHAR(MAX) NOT NULL,
        LinkedInUrl NVARCHAR(MAX) NOT NULL,
        GithubUrl NVARCHAR(MAX) NOT NULL,
        InterestsJson NVARCHAR(MAX) NOT NULL DEFAULT '[]'
    );
    PRINT 'Created Abouts table';
END
ELSE
    PRINT 'Abouts table already exists';

-- Create Education table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Education')
BEGIN
    CREATE TABLE Education (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Institution NVARCHAR(MAX) NOT NULL,
        Degree NVARCHAR(MAX) NOT NULL,
        Field NVARCHAR(MAX) NOT NULL,
        StartDate DATETIME2 NOT NULL,
        EndDate DATETIME2 NULL,
        Description NVARCHAR(MAX) NOT NULL,
        AboutId INT NULL
    );
    PRINT 'Created Education table';
END
ELSE
    PRINT 'Education table already exists';

-- Create WorkExperience table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'WorkExperience')
BEGIN
    CREATE TABLE WorkExperience (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Company NVARCHAR(MAX) NOT NULL,
        Position NVARCHAR(MAX) NOT NULL,
        StartDate DATETIME2 NOT NULL,
        EndDate DATETIME2 NULL,
        Description NVARCHAR(MAX) NOT NULL,
        AchievementsJson NVARCHAR(MAX) NOT NULL DEFAULT '[]',
        AboutId INT NULL
    );
    PRINT 'Created WorkExperience table';
END
ELSE
    PRINT 'WorkExperience table already exists';

-- Add foreign key constraints
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Education_Abouts_AboutId')
BEGIN
    ALTER TABLE Education ADD CONSTRAINT FK_Education_Abouts_AboutId 
    FOREIGN KEY (AboutId) REFERENCES Abouts(Id);
    PRINT 'Added FK_Education_Abouts_AboutId constraint';
END

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_WorkExperience_Abouts_AboutId')
BEGIN
    ALTER TABLE WorkExperience ADD CONSTRAINT FK_WorkExperience_Abouts_AboutId 
    FOREIGN KEY (AboutId) REFERENCES Abouts(Id);
    PRINT 'Added FK_WorkExperience_Abouts_AboutId constraint';
END

-- Create indexes
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Education_AboutId' AND object_id = OBJECT_ID('Education'))
BEGIN
    CREATE INDEX IX_Education_AboutId ON Education(AboutId);
    PRINT 'Created IX_Education_AboutId index';
END

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_WorkExperience_AboutId' AND object_id = OBJECT_ID('WorkExperience'))
BEGIN
    CREATE INDEX IX_WorkExperience_AboutId ON WorkExperience(AboutId);
    PRINT 'Created IX_WorkExperience_AboutId index';
END

PRINT 'All missing tables and relationships have been created!'; 
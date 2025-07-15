-- Database Indexes Script for Cynthia Portfolio
-- This script adds missing indexes to existing tables for better performance
-- Run this script after the setup script

-- Add missing indexes to Admins table
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Admins_Email' AND object_id = OBJECT_ID('Admins'))
BEGIN
    CREATE UNIQUE INDEX IX_Admins_Email ON Admins(Email);
    PRINT 'Created IX_Admins_Email unique index';
END
ELSE
    PRINT 'IX_Admins_Email index already exists';

-- Add missing indexes to HomePages table
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_HomePages_IsActive' AND object_id = OBJECT_ID('HomePages'))
BEGIN
    CREATE INDEX IX_HomePages_IsActive ON HomePages(IsActive);
    PRINT 'Created IX_HomePages_IsActive index';
END
ELSE
    PRINT 'IX_HomePages_IsActive index already exists';

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_HomePages_DisplayOrder' AND object_id = OBJECT_ID('HomePages'))
BEGIN
    CREATE INDEX IX_HomePages_DisplayOrder ON HomePages(DisplayOrder);
    PRINT 'Created IX_HomePages_DisplayOrder index';
END
ELSE
    PRINT 'IX_HomePages_DisplayOrder index already exists';

-- Add missing indexes to SkillsCategories table
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_SkillsCategories_IsActive' AND object_id = OBJECT_ID('SkillsCategories'))
BEGIN
    CREATE INDEX IX_SkillsCategories_IsActive ON SkillsCategories(IsActive);
    PRINT 'Created IX_SkillsCategories_IsActive index';
END
ELSE
    PRINT 'IX_SkillsCategories_IsActive index already exists';

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_SkillsCategories_DisplayOrder' AND object_id = OBJECT_ID('SkillsCategories'))
BEGIN
    CREATE INDEX IX_SkillsCategories_DisplayOrder ON SkillsCategories(DisplayOrder);
    PRINT 'Created IX_SkillsCategories_DisplayOrder index';
END
ELSE
    PRINT 'IX_SkillsCategories_DisplayOrder index already exists';

PRINT 'All missing indexes have been added!'; 
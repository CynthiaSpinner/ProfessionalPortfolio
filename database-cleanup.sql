-- Database Cleanup Script for Cynthia Portfolio
-- This script removes problematic columns and verifies the database matches the Entity Framework model
-- Run this script last to clean up any remaining issues

-- Remove the HomePageId column from Projects table if it exists
IF COLUMNPROPERTY(OBJECT_ID('Projects'), 'HomePageId', 'ColumnId') IS NOT NULL
BEGIN
    ALTER TABLE Projects DROP COLUMN HomePageId;
    PRINT 'Removed HomePageId column from Projects table';
END
ELSE
    PRINT 'HomePageId column does not exist in Projects table';

-- Drop the Projects HomePageId index if it exists
IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Projects_HomePageId' AND object_id = OBJECT_ID('Projects'))
BEGIN
    DROP INDEX IX_Projects_HomePageId ON Projects;
    PRINT 'Removed IX_Projects_HomePageId index';
END
ELSE
    PRINT 'IX_Projects_HomePageId index does not exist';

-- Drop the foreign key constraint if it exists
IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Projects_HomePages_HomePageId')
BEGIN
    ALTER TABLE Projects DROP CONSTRAINT FK_Projects_HomePages_HomePageId;
    PRINT 'Removed FK_Projects_HomePages_HomePageId constraint';
END
ELSE
    PRINT 'FK_Projects_HomePages_HomePageId constraint does not exist';

-- Verify all required tables exist
PRINT '';
PRINT '=== VERIFYING REQUIRED TABLES ===';
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Abouts')
    PRINT '✓ Abouts table exists';
ELSE
    PRINT '✗ Abouts table missing';

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Education')
    PRINT '✓ Education table exists';
ELSE
    PRINT '✗ Education table missing';

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'WorkExperience')
    PRINT '✓ WorkExperience table exists';
ELSE
    PRINT '✗ WorkExperience table missing';

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'HomePages')
    PRINT '✓ HomePages table exists';
ELSE
    PRINT '✗ HomePages table missing';

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Projects')
    PRINT '✓ Projects table exists';
ELSE
    PRINT '✗ Projects table missing';

PRINT '';
PRINT '=== DATABASE CLEANUP COMPLETED ===';
PRINT 'Your database should now match your Entity Framework model.';
PRINT 'Try running your application to see if the errors are resolved.'; 
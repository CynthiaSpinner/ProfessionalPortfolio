-- Simple Data Type Fix Script for Cynthia Portfolio
-- This script fixes the DOUBLE to FLOAT conversion issue

-- Fix HeaderOverlayOpacity in HeroTemplates table
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('HeroTemplates') AND name = 'HeaderOverlayOpacity')
BEGIN
    PRINT 'Fixing HeaderOverlayOpacity in HeroTemplates table...';
    
    -- Drop the column and recreate it with correct type
    ALTER TABLE HeroTemplates DROP COLUMN HeaderOverlayOpacity;
    ALTER TABLE HeroTemplates ADD HeaderOverlayOpacity FLOAT NOT NULL DEFAULT 0.5;
    
    PRINT 'HeaderOverlayOpacity in HeroTemplates fixed to FLOAT type';
END

-- Fix HeaderOverlayOpacity in HomePages table
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('HomePages') AND name = 'HeaderOverlayOpacity')
BEGIN
    PRINT 'Fixing HeaderOverlayOpacity in HomePages table...';
    
    -- Drop the column and recreate it with correct type
    ALTER TABLE HomePages DROP COLUMN HeaderOverlayOpacity;
    ALTER TABLE HomePages ADD HeaderOverlayOpacity FLOAT NULL;
    
    PRINT 'HeaderOverlayOpacity in HomePages fixed to FLOAT type';
END

PRINT '';
PRINT 'Data type fixes completed!';
PRINT 'Note: Column values have been reset to defaults.'; 
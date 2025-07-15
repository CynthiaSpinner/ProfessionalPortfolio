-- Fix Data Types Script for Cynthia Portfolio
-- This script fixes the data type mismatch between DOUBLE and FLOAT

-- Fix HeaderOverlayOpacity data type in HeroTemplates table
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('HeroTemplates') AND name = 'HeaderOverlayOpacity')
BEGIN
    -- Check current data type
    DECLARE @currentType NVARCHAR(50);
    SELECT @currentType = DATA_TYPE 
    FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_NAME = 'HeroTemplates' AND COLUMN_NAME = 'HeaderOverlayOpacity';
    
    PRINT 'Current HeaderOverlayOpacity data type: ' + @currentType;
    
    -- If it's DOUBLE, convert to FLOAT
    IF @currentType = 'DOUBLE' OR @currentType = 'FLOAT'
    BEGIN
        -- Create a temporary column with the correct type
        ALTER TABLE HeroTemplates ADD HeaderOverlayOpacity_Fixed FLOAT;
        
        -- Copy data
        UPDATE HeroTemplates SET HeaderOverlayOpacity_Fixed = CAST(HeaderOverlayOpacity AS FLOAT);
        
        -- Drop the old column
        ALTER TABLE HeroTemplates DROP COLUMN HeaderOverlayOpacity;
        
        -- Rename the new column
        EXEC sp_rename 'HeroTemplates.HeaderOverlayOpacity_Fixed', 'HeaderOverlayOpacity', 'COLUMN';
        
        PRINT 'Fixed HeaderOverlayOpacity data type from ' + @currentType + ' to FLOAT';
    END
    ELSE
    BEGIN
        PRINT 'HeaderOverlayOpacity data type is already correct: ' + @currentType;
    END
END

-- Fix HeaderOverlayOpacity data type in HomePages table
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('HomePages') AND name = 'HeaderOverlayOpacity')
BEGIN
    -- Check current data type
    DECLARE @homePageType NVARCHAR(50);
    SELECT @homePageType = DATA_TYPE 
    FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_NAME = 'HomePages' AND COLUMN_NAME = 'HeaderOverlayOpacity';
    
    PRINT 'Current HomePages HeaderOverlayOpacity data type: ' + @homePageType;
    
    -- If it's DOUBLE, convert to FLOAT
    IF @homePageType = 'DOUBLE' OR @homePageType = 'FLOAT'
    BEGIN
        -- Create a temporary column with the correct type
        ALTER TABLE HomePages ADD HeaderOverlayOpacity_Fixed FLOAT;
        
        -- Copy data
        UPDATE HomePages SET HeaderOverlayOpacity_Fixed = CAST(HeaderOverlayOpacity AS FLOAT);
        
        -- Drop the old column
        ALTER TABLE HomePages DROP COLUMN HeaderOverlayOpacity;
        
        -- Rename the new column
        EXEC sp_rename 'HomePages.HeaderOverlayOpacity_Fixed', 'HeaderOverlayOpacity', 'COLUMN';
        
        PRINT 'Fixed HomePages HeaderOverlayOpacity data type from ' + @homePageType + ' to FLOAT';
    END
    ELSE
    BEGIN
        PRINT 'HomePages HeaderOverlayOpacity data type is already correct: ' + @homePageType;
    END
END

PRINT '';
PRINT 'Data type fixes completed!'; 
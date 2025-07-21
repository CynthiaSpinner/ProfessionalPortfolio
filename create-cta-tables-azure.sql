-- CTA Tables Creation Script for Azure Database
-- This script checks for existing tables and creates them if they don't exist

-- Check if CTATemplates table exists
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'CTATemplates')
BEGIN
    CREATE TABLE CTATemplates (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Nickname NVARCHAR(100) NOT NULL,
        Title NVARCHAR(200) NOT NULL,
        Subtitle NVARCHAR(500) NOT NULL,
        ButtonText NVARCHAR(100) NOT NULL,
        ButtonLink NVARCHAR(500) NOT NULL,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt DATETIME2 NULL
    );
    
    PRINT 'CTATemplates table created successfully';
END
ELSE
BEGIN
    PRINT 'CTATemplates table already exists';
    
    -- Check and add missing columns if table exists
    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'CTATemplates' AND COLUMN_NAME = 'Nickname')
    BEGIN
        ALTER TABLE CTATemplates ADD Nickname NVARCHAR(100) NOT NULL DEFAULT '';
        PRINT 'Added Nickname column to CTATemplates';
    END
    
    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'CTATemplates' AND COLUMN_NAME = 'Title')
    BEGIN
        ALTER TABLE CTATemplates ADD Title NVARCHAR(200) NOT NULL DEFAULT '';
        PRINT 'Added Title column to CTATemplates';
    END
    
    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'CTATemplates' AND COLUMN_NAME = 'Subtitle')
    BEGIN
        ALTER TABLE CTATemplates ADD Subtitle NVARCHAR(500) NOT NULL DEFAULT '';
        PRINT 'Added Subtitle column to CTATemplates';
    END
    
    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'CTATemplates' AND COLUMN_NAME = 'ButtonText')
    BEGIN
        ALTER TABLE CTATemplates ADD ButtonText NVARCHAR(100) NOT NULL DEFAULT '';
        PRINT 'Added ButtonText column to CTATemplates';
    END
    
    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'CTATemplates' AND COLUMN_NAME = 'ButtonLink')
    BEGIN
        ALTER TABLE CTATemplates ADD ButtonLink NVARCHAR(500) NOT NULL DEFAULT '';
        PRINT 'Added ButtonLink column to CTATemplates';
    END
    
    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'CTATemplates' AND COLUMN_NAME = 'CreatedAt')
    BEGIN
        ALTER TABLE CTATemplates ADD CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE();
        PRINT 'Added CreatedAt column to CTATemplates';
    END
    
    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'CTATemplates' AND COLUMN_NAME = 'UpdatedAt')
    BEGIN
        ALTER TABLE CTATemplates ADD UpdatedAt DATETIME2 NULL;
        PRINT 'Added UpdatedAt column to CTATemplates';
    END
END

-- Check if CTASections table exists
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'CTASections')
BEGIN
    CREATE TABLE CTASections (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Title NVARCHAR(200) NOT NULL,
        Subtitle NVARCHAR(500) NOT NULL,
        ButtonText NVARCHAR(100) NOT NULL,
        ButtonLink NVARCHAR(500) NOT NULL,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt DATETIME2 NULL
    );
    
    PRINT 'CTASections table created successfully';
END
ELSE
BEGIN
    PRINT 'CTASections table already exists';
    
    -- Check and add missing columns if table exists
    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'CTASections' AND COLUMN_NAME = 'Title')
    BEGIN
        ALTER TABLE CTASections ADD Title NVARCHAR(200) NOT NULL DEFAULT '';
        PRINT 'Added Title column to CTASections';
    END
    
    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'CTASections' AND COLUMN_NAME = 'Subtitle')
    BEGIN
        ALTER TABLE CTASections ADD Subtitle NVARCHAR(500) NOT NULL DEFAULT '';
        PRINT 'Added Subtitle column to CTASections';
    END
    
    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'CTASections' AND COLUMN_NAME = 'ButtonText')
    BEGIN
        ALTER TABLE CTASections ADD ButtonText NVARCHAR(100) NOT NULL DEFAULT '';
        PRINT 'Added ButtonText column to CTASections';
    END
    
    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'CTASections' AND COLUMN_NAME = 'ButtonLink')
    BEGIN
        ALTER TABLE CTASections ADD ButtonLink NVARCHAR(500) NOT NULL DEFAULT '';
        PRINT 'Added ButtonLink column to CTASections';
    END
    
    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'CTASections' AND COLUMN_NAME = 'CreatedAt')
    BEGIN
        ALTER TABLE CTASections ADD CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE();
        PRINT 'Added CreatedAt column to CTASections';
    END
    
    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'CTASections' AND COLUMN_NAME = 'UpdatedAt')
    BEGIN
        ALTER TABLE CTASections ADD UpdatedAt DATETIME2 NULL;
        PRINT 'Added UpdatedAt column to CTASections';
    END
END

-- Insert default CTA template if none exists
IF NOT EXISTS (SELECT * FROM CTATemplates)
BEGIN
    INSERT INTO CTATemplates (Nickname, Title, Subtitle, ButtonText, ButtonLink, CreatedAt)
    VALUES (
        'Default CTA',
        'Ready to Start Your Project?',
        'Let''s work together to bring your ideas to life. I''m here to help you create something amazing.',
        'Get In Touch',
        '/contact',
        GETUTCDATE()
    );
    PRINT 'Default CTA template inserted';
END

-- Insert default CTA section if none exists
IF NOT EXISTS (SELECT * FROM CTASections)
BEGIN
    INSERT INTO CTASections (Title, Subtitle, ButtonText, ButtonLink, CreatedAt)
    VALUES (
        'Ready to Start Your Project?',
        'Let''s work together to bring your ideas to life. I''m here to help you create something amazing.',
        'Get In Touch',
        '/contact',
        GETUTCDATE()
    );
    PRINT 'Default CTA section inserted';
END

PRINT 'CTA tables setup completed successfully!'; 
-- Check if CTATemplates table exists and has data
SELECT 'CTATemplates table exists' as status, COUNT(*) as template_count 
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_NAME = 'CTATemplates';

-- Check if there are any CTA templates
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'CTATemplates')
BEGIN
    SELECT 'CTA Templates in database:' as status, COUNT(*) as count FROM CTATemplates;
    
    -- Show existing templates
    SELECT Id, Nickname, Title, Subtitle, ButtonText, ButtonLink, CreatedAt, UpdatedAt 
    FROM CTATemplates 
    ORDER BY CreatedAt DESC;
    
    -- Add a test template if none exist
    IF NOT EXISTS (SELECT 1 FROM CTATemplates)
    BEGIN
        INSERT INTO CTATemplates (Nickname, Title, Subtitle, ButtonText, ButtonLink, CreatedAt, UpdatedAt)
        VALUES (
            'Default CTA',
            'Ready to Get Started?',
            'Let''s work together to bring your vision to life',
            'Contact Me',
            '/contact',
            GETUTCDATE(),
            GETUTCDATE()
        );
        
        SELECT 'Added test CTA template' as status;
    END
END
ELSE
BEGIN
    SELECT 'CTATemplates table does not exist' as status;
END 
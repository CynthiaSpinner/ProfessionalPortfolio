-- Drop default constraints first
IF EXISTS (SELECT * FROM sys.default_constraints WHERE name = 'DF__HeroTempl__Heade__31B762FC')
BEGIN
    ALTER TABLE HeroTemplates DROP CONSTRAINT DF__HeroTempl__Heade__31B762FC
END

-- Now we can modify the column to be nullable float (REAL in SQL Server)
ALTER TABLE HeroTemplates ALTER COLUMN HeaderOverlayOpacity REAL NULL

-- Don't add the default constraint back since you want it nullable
-- ALTER TABLE HeroTemplates ADD CONSTRAINT DF_HeroTemplate_HeaderOverlayOpacity DEFAULT 0.5 FOR HeaderOverlayOpacity

-- Check for other constraints that might need to be dropped
IF EXISTS (SELECT * FROM sys.default_constraints WHERE name LIKE 'DF__HeroTempl__%')
BEGIN
    PRINT 'Found other default constraints on HeroTemplates table:'
    SELECT name, parent_object_id FROM sys.default_constraints WHERE parent_object_id = OBJECT_ID('HeroTemplates')
END 
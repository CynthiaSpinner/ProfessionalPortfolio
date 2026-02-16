-- Run in DBeaver against your Render PostgreSQL database.
-- Lists all tables and columns and flags any that are NOT PascalCase.
-- PascalCase = starts with uppercase letter, no underscores (e.g. "Title", "DisplayOrder").

-- 1) Tables: flag if name is not PascalCase (lowercase start or contains underscore)
SELECT
    t.table_schema,
    t.table_name AS current_table_name,
    CASE
        WHEN t.table_name ~ '^[a-z]' OR t.table_name ~ '_' THEN 'NOT PascalCase – fix needed'
        ELSE 'OK'
    END AS table_naming
FROM information_schema.tables t
WHERE t.table_schema = 'public'
  AND t.table_type = 'BASE TABLE'
ORDER BY t.table_name;

-- 2) Columns: flag if name is not PascalCase
SELECT
    c.table_schema,
    c.table_name,
    c.column_name AS current_column_name,
    CASE
        WHEN c.column_name ~ '^[a-z]' OR c.column_name ~ '_' THEN 'NOT PascalCase – fix needed'
        ELSE 'OK'
    END AS column_naming
FROM information_schema.columns c
WHERE c.table_schema = 'public'
ORDER BY c.table_name, c.ordinal_position;

-- 3) Only show rows that need fixing (handy for a short list)
SELECT
    c.table_name,
    c.column_name AS current_column_name
FROM information_schema.columns c
WHERE c.table_schema = 'public'
  AND (c.column_name ~ '^[a-z]' OR c.column_name ~ '_')
ORDER BY c.table_name, c.ordinal_position;

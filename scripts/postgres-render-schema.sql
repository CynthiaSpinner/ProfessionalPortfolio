-- Run this in DBeaver against your Render PostgreSQL database.
-- Creates tables if they don't exist. Safe to run multiple times.

-- SkillsCategories (columns match SkillsCategory model)
CREATE TABLE IF NOT EXISTS "SkillsCategories" (
    "Id"             SERIAL PRIMARY KEY,
    "Title"          VARCHAR(100) NOT NULL,
    "Description"   VARCHAR(500) NOT NULL DEFAULT '',
    "ImagePath"      TEXT,
    "SkillsJson"     JSONB NOT NULL DEFAULT '[]',
    "DisplayOrder"   INTEGER NOT NULL DEFAULT 0,
    "IsActive"       BOOLEAN NOT NULL DEFAULT true
);

CREATE INDEX IF NOT EXISTS "IX_SkillsCategories_DisplayOrder" ON "SkillsCategories" ("DisplayOrder");
CREATE INDEX IF NOT EXISTS "IX_SkillsCategories_IsActive" ON "SkillsCategories" ("IsActive");

-- SiteSettings (single-row nav toggles)
CREATE TABLE IF NOT EXISTS "SiteSettings" (
    "Id"                   SERIAL PRIMARY KEY,
    "ShowGraphicDesignLink" BOOLEAN NOT NULL DEFAULT true,
    "ShowDesignLink"        BOOLEAN NOT NULL DEFAULT true,
    "UpdatedAt"             TIMESTAMP WITH TIME ZONE
);

-- Optional: insert one row for SiteSettings if table is empty
INSERT INTO "SiteSettings" ("ShowGraphicDesignLink", "ShowDesignLink")
SELECT true, true
WHERE NOT EXISTS (SELECT 1 FROM "SiteSettings" LIMIT 1);

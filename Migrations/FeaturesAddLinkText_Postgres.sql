-- Manual script only (no EF migrations). Run in DBeaver against your Render Postgres.
-- 1) Adds link text columns (editable "Learn more" text per feature).
-- 2) Removes icon columns from both features tables.
-- If your tables use snake_case, replace "FeaturesSections" with "features_sections" and "FeaturesTemplates" with "features_templates".

-- ========== FeaturesSections ==========
-- Add link text columns
ALTER TABLE "FeaturesSections" ADD COLUMN IF NOT EXISTS "Feature1LinkText" VARCHAR(100) NULL;
ALTER TABLE "FeaturesSections" ADD COLUMN IF NOT EXISTS "Feature2LinkText" VARCHAR(100) NULL;
ALTER TABLE "FeaturesSections" ADD COLUMN IF NOT EXISTS "Feature3LinkText" VARCHAR(100) NULL;

-- Remove icon columns
ALTER TABLE "FeaturesSections" DROP COLUMN IF EXISTS "Feature1Icon";
ALTER TABLE "FeaturesSections" DROP COLUMN IF EXISTS "Feature2Icon";
ALTER TABLE "FeaturesSections" DROP COLUMN IF EXISTS "Feature3Icon";

-- ========== FeaturesTemplates ==========
-- Add link text columns
ALTER TABLE "FeaturesTemplates" ADD COLUMN IF NOT EXISTS "Feature1LinkText" VARCHAR(100) NULL;
ALTER TABLE "FeaturesTemplates" ADD COLUMN IF NOT EXISTS "Feature2LinkText" VARCHAR(100) NULL;
ALTER TABLE "FeaturesTemplates" ADD COLUMN IF NOT EXISTS "Feature3LinkText" VARCHAR(100) NULL;

-- Remove icon columns
ALTER TABLE "FeaturesTemplates" DROP COLUMN IF EXISTS "Feature1Icon";
ALTER TABLE "FeaturesTemplates" DROP COLUMN IF EXISTS "Feature2Icon";
ALTER TABLE "FeaturesTemplates" DROP COLUMN IF EXISTS "Feature3Icon";

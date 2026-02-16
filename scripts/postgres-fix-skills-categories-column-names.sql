-- Run once in DBeaver against your Render PostgreSQL database.
-- Fixes: column "Title" of relation "SkillsCategories" does not exist
-- The app model uses "Title"; if your table has "Name", rename it.
--
-- First run: postgres-check-naming.sql to see which tables/columns need fixing.
-- Convention: all table and column names = PascalCase (see postgres-naming-convention.sql).

-- SkillsCategories: model expects "Title", not "Name"
ALTER TABLE "SkillsCategories" RENAME COLUMN "Name" TO "Title";

-- If your table was created with lowercase columns instead, use these (skip if already PascalCase):
-- ALTER TABLE "SkillsCategories" RENAME COLUMN id TO "Id";
-- ALTER TABLE "SkillsCategories" RENAME COLUMN title TO "Title";
-- ALTER TABLE "SkillsCategories" RENAME COLUMN description TO "Description";
-- ALTER TABLE "SkillsCategories" RENAME COLUMN imagepath TO "ImagePath";
-- ALTER TABLE "SkillsCategories" RENAME COLUMN skillsjson TO "SkillsJson";
-- ALTER TABLE "SkillsCategories" RENAME COLUMN displayorder TO "DisplayOrder";
-- ALTER TABLE "SkillsCategories" RENAME COLUMN isactive TO "IsActive";

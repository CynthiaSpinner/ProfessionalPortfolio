-- Run this ONCE in your Render Postgres DB (psql or any SQL client).
-- Adds the two columns needed for hero background image upload. Table name: "HomePages".

ALTER TABLE "HomePages"
  ADD COLUMN IF NOT EXISTS "HeaderBackgroundImageData" bytea,
  ADD COLUMN IF NOT EXISTS "HeaderBackgroundImageContentType" character varying(100);

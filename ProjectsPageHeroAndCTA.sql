-- Run this on your PostgreSQL database (e.g. Render) to add Projects page hero and CTA tables.
-- Id is serial (auto-increment). Adjust if your schema uses different naming.

CREATE TABLE IF NOT EXISTS "ProjectsPageHeroes" (
    "Id" serial PRIMARY KEY,
    "Title" varchar(200) NOT NULL,
    "Subtitle" varchar(500),
    "ButtonText" varchar(100),
    "ButtonUrl" varchar(500),
    "CreatedAt" timestamp with time zone NOT NULL DEFAULT (NOW() AT TIME ZONE 'UTC'),
    "UpdatedAt" timestamp with time zone
);

CREATE TABLE IF NOT EXISTS "ProjectsPageCTAs" (
    "Id" serial PRIMARY KEY,
    "Title" varchar(200) NOT NULL,
    "Subtitle" varchar(500) NOT NULL,
    "ButtonText" varchar(100) NOT NULL,
    "ButtonLink" varchar(500) NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL DEFAULT (NOW() AT TIME ZONE 'UTC'),
    "UpdatedAt" timestamp with time zone
);

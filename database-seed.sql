-- Initial data for Portfolio Database (PostgreSQL)
-- This script populates the database with sample content

-- Insert Admin User (password is 'admin123' hashed with BCrypt)
INSERT INTO "Admins" ("Username", "Email", "PasswordHash", "CreatedAt", "IsActive")
VALUES ('admin', 'admin@portfolio.com', '$2a$12$LQv3c1yqBWVHxkd0LHAkCOYz6TtxMQJqhN8/LewdBPj3bp.gUjjW6', NOW(), true)
ON CONFLICT ("Username") DO NOTHING;

-- Insert About Information
INSERT INTO "Abouts" ("Name", "Title", "Biography", "ProfileImageUrl", "Email", "LinkedInUrl", "GithubUrl", "InterestsJson")
VALUES (
    'Cynthia Spinner',
    'Full-Stack Developer & Designer',
    'Passionate full-stack developer with expertise in modern web technologies, creative design, and problem-solving. I love building beautiful, functional applications that provide exceptional user experiences.',
    '/images/profile.jpg',
    'cynthia@portfolio.com',
    'https://linkedin.com/in/cynthiaspinner',
    'https://github.com/CynthiaSpinner',
    '["Web Development", "UI/UX Design", "3D Modeling", "Animation", "Game Development", "Creative Problem Solving"]'::jsonb
)
ON CONFLICT ("Id") DO NOTHING;

-- Insert Sample Projects (matching actual model schema)
INSERT INTO "Projects" (
    "Title", 
    "Description", 
    "ImageUrl", 
    "ProjectUrl", 
    "GitHubUrl", 
    "TechnologiesJson", 
    "CompletionDate"
)
VALUES 
(
    'Professional Portfolio Website',
    'A modern, responsive portfolio website built with ASP.NET Core and React. Features real-time updates, admin CMS, and beautiful UI/UX design.',
    '/images/portfolio-project.jpg',
    'https://codespinner.netlify.app',
    'https://github.com/CynthiaSpinner/ProfessionalPortfolio',
    '["ASP.NET Core", "React", "PostgreSQL", "WebSockets", "Bootstrap", "Entity Framework"]'::jsonb,
    NOW()
),
(
    'E-Commerce Platform',
    'Full-featured e-commerce platform with shopping cart, payment processing, and inventory management.',
    '/images/ecommerce-project.jpg',
    'https://example-ecommerce.com',
    'https://github.com/CynthiaSpinner/ECommerce',
    '["React", "Node.js", "MongoDB", "Stripe API", "Redux", "Express.js"]'::jsonb,
    NOW() - INTERVAL '30 days'
),
(
    '3D Game Environment',
    'Interactive 3D game environment created in Blender with custom animations and realistic lighting.',
    '/images/3d-game-project.jpg',
    'https://example-game.com',
    'https://github.com/CynthiaSpinner/3DGame',
    '["Blender", "Unity", "C#", "3D Modeling", "Animation", "Lighting"]'::jsonb,
    NOW() - INTERVAL '60 days'
)
ON CONFLICT ("Id") DO NOTHING;

-- Insert HomePage Configuration (matching actual model schema)
INSERT INTO "HomePages" (
    "HeaderTitle", 
    "HeaderSubtitle", 
    "HeaderDescription",
    "HeaderBackgroundImageUrl",
    "HeaderBackgroundVideoUrl",
    "HeaderPrimaryButtonText",
    "HeaderPrimaryButtonUrl",
    "HeaderSecondaryButtonText",
    "HeaderSecondaryButtonUrl",
    "ShowHeaderPrimaryButton",
    "ShowHeaderSecondaryButton",
    "HeaderOverlayColor",
    "HeaderOverlayOpacity",
    "HeaderTextColor",
    "HeaderButtonColor",
    "HeaderButtonTextColor",
    "SkillsSectionTitle",
    "SkillsSectionSubtitle",
    "SkillsCategoriesJson",
    "FeaturedProjectsTitle",
    "FeaturedProjectsSubtitle",
    "FeaturedProjectsJson",
    "CTATitle",
    "CTASubtitle",
    "CTAButtonText",
    "CTAButtonUrl",
    "CTABackgroundImageUrl",
    "CTATextColor",
    "DisplayOrder",
    "IsActive",
    "CreatedAt"
)
VALUES (
    'Welcome to My Portfolio',
    'Full-Stack Developer & Creative Designer',
    'I create beautiful, functional web applications and immersive digital experiences.',
    '/images/hero-background.jpg',
    '',
    'View My Work',
    '/projects',
    'Contact Me',
    '/contact',
    true,
    true,
    '#000000',
    0.4,
    '#ffffff',
    '#007bff',
    '#ffffff',
    'My Skills',
    'Technologies and tools I work with',
    '[{"name": "Frontend Development", "skills": ["React", "JavaScript", "TypeScript", "HTML5", "CSS3", "Bootstrap"]}, {"name": "Backend Development", "skills": ["ASP.NET Core", "C#", "Node.js", "RESTful APIs", "WebSockets", "Entity Framework"]}, {"name": "Database & Cloud", "skills": ["PostgreSQL", "SQL Server", "MongoDB", "Docker"]}, {"name": "Design & 3D", "skills": ["Blender", "3D Modeling", "Animation", "UI/UX Design"]}]'::jsonb,
    'Featured Projects',
    'Some of my recent work',
    '[1, 2, 3]'::jsonb,
    'Ready to Start Your Project?',
    'Let''s work together to bring your ideas to life',
    'Get In Touch',
    '/contact',
    '/images/cta-background.jpg',
    '#ffffff',
    1,
    true,
    NOW()
)
ON CONFLICT ("Id") DO NOTHING;

-- Insert Hero Template (matching actual migration schema)
INSERT INTO "HeroTemplates" (
    "Name",
    "Title",
    "Subtitle",
    "Description",
    "BackgroundImageUrl",
    "BackgroundVideoUrl",
    "PrimaryButtonText",
    "PrimaryButtonUrl",
    "SecondaryButtonText",
    "SecondaryButtonUrl",
    "OverlayColor",
    "OverlayOpacity",
    "IsActive",
    "CreatedAt",
    "LastModified"
)
VALUES (
    'Default Hero Template',
    'Cynthia Spinner',
    'Full-Stack Developer & Designer',
    'Creating innovative web solutions and beautiful digital experiences',
    '/images/hero-bg.jpg',
    '',
    'Explore My Work',
    '/projects',
    'Contact Me',
    '/contact',
    '#000000',
    0.3,
    true,
    NOW(),
    NOW()
)
ON CONFLICT ("Id") DO NOTHING;

-- Insert Skills Categories (matching actual model schema)
INSERT INTO "SkillsCategories" (
    "Name",
    "Description", 
    "ImagePath",
    "SkillsJson",
    "DisplayOrder",
    "IsActive"
)
VALUES 
(
    'Frontend Development',
    'Building responsive and interactive user interfaces',
    '/images/frontend.jpg',
    '["React", "JavaScript", "TypeScript", "HTML5", "CSS3", "Bootstrap", "Responsive Design"]'::jsonb,
    1,
    true
),
(
    'Backend Development',
    'Creating robust server-side applications and APIs',
    '/images/backend.jpg',
    '["ASP.NET Core", "C#", "Node.js", "RESTful APIs", "WebSockets", "Entity Framework"]'::jsonb,
    2,
    true
),
(
    'Database & Cloud',
    'Managing data and cloud infrastructure',
    '/images/database.jpg',
    '["PostgreSQL", "SQL Server", "MongoDB", "Docker"]'::jsonb,
    3,
    true
),
(
    'Design & 3D',
    'Crafting beautiful designs and 3D experiences',
    '/images/design.jpg',
    '["Blender", "3D Modeling", "Animation", "UI/UX Design"]'::jsonb,
    4,
    true
)
ON CONFLICT ("Id") DO NOTHING;

-- Insert Features Section (matching actual model schema)
INSERT INTO "FeaturesSections" (
    "SectionTitle",
    "SectionSubtitle",
    "Feature1Title",
    "Feature1Subtitle",
    "Feature1Description",
    "Feature1Icon",
    "Feature1Link",
    "Feature2Title",
    "Feature2Subtitle",
    "Feature2Description",
    "Feature2Icon",
    "Feature2Link",
    "Feature3Title",
    "Feature3Subtitle",
    "Feature3Description",
    "Feature3Icon",
    "Feature3Link",
    "IsActive",
    "DisplayOrder",
    "CreatedAt",
    "UpdatedAt"
)
VALUES (
    'Key Skills & Technologies',
    'Explore my expertise across different domains',
    'Frontend Development',
    'React, JavaScript, HTML5, CSS3, Bootstrap',
    'Building responsive and interactive user interfaces with modern frameworks and best practices.',
    'fas fa-code',
    '/projects?category=frontend',
    'Backend Development',
    '.NET Core, C#, RESTful APIs, MySQL',
    'Creating robust server-side applications and APIs with enterprise-grade technologies.',
    'fas fa-server',
    '/projects?category=backend',
    'Design & Tools',
    'Adobe Creative Suite, UI/UX Design, Git, Docker',
    'Crafting beautiful designs and managing development workflows with professional tools.',
    'fas fa-palette',
    '/projects?category=design',
    true,
    1,
    NOW(),
    NOW()
)
ON CONFLICT ("Id") DO NOTHING;

-- Insert Features Templates (matching actual model schema)
INSERT INTO "FeaturesTemplates" (
    "Nickname",
    "SectionTitle",
    "SectionSubtitle",
    "SectionDescription",
    "Feature1Title",
    "Feature1Subtitle",
    "Feature1Description",
    "Feature1Icon",
    "Feature1Link",
    "Feature2Title",
    "Feature2Subtitle",
    "Feature2Description",
    "Feature2Icon",
    "Feature2Link",
    "Feature3Title",
    "Feature3Subtitle",
    "Feature3Description",
    "Feature3Icon",
    "Feature3Link",
    "CreatedAt",
    "UpdatedAt"
)
VALUES (
    'Default Features Template',
    'Key Skills & Technologies',
    'Explore my expertise across different domains',
    'Comprehensive overview of my technical skills and capabilities',
    'Frontend Development',
    'React, JavaScript, HTML5, CSS3, Bootstrap',
    'Building responsive and interactive user interfaces with modern frameworks and best practices.',
    'fas fa-code',
    '/projects?category=frontend',
    'Backend Development',
    '.NET Core, C#, RESTful APIs, MySQL',
    'Creating robust server-side applications and APIs with enterprise-grade technologies.',
    'fas fa-server',
    '/projects?category=backend',
    'Design & Tools',
    'Adobe Creative Suite, UI/UX Design, Git, Docker',
    'Crafting beautiful designs and managing development workflows with professional tools.',
    'fas fa-palette',
    '/projects?category=design',
    NOW(),
    NOW()
)
ON CONFLICT ("Id") DO NOTHING;

-- Verify data insertion
SELECT 'Data insertion completed successfully' as status;

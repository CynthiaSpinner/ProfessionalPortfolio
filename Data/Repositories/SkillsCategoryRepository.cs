using Microsoft.EntityFrameworkCore;
using Portfolio.Models;
using Portfolio.Models.Portfolio;

namespace Portfolio.Data.Repositories
{
    public class SkillsCategoryRepository : ISkillsCategoryRepository
    {
        private readonly PortfolioContext _context;

        public SkillsCategoryRepository(PortfolioContext context)
        {
            _context = context;
        }

        public async Task<List<SkillsCategory>> GetAllOrderedAsync()
        {
            return await _context.SkillsCategories
                .OrderBy(x => x.DisplayOrder)
                .ThenBy(x => x.Id)
                .ToListAsync();
        }

        public async Task<SkillsCategory?> GetByIdAsync(int id)
        {
            return await _context.SkillsCategories.FindAsync(id);
        }

        public async Task<SkillsCategory?> GetByTitleAsync(string title)
        {
            return await _context.SkillsCategories
                .FirstOrDefaultAsync(c => c.Title == title);
        }

        public async Task<SkillsCategory> AddAsync(SkillsCategory category)
        {
            _context.SkillsCategories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task UpdateAsync(SkillsCategory category)
        {
            _context.Entry(category).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(SkillsCategory category)
        {
            _context.SkillsCategories.Remove(category);
            await _context.SaveChangesAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task LoadDefaultCategoriesIfMissingAsync()
        {
            var defaults = GetDefaultCategories();
            foreach (var d in defaults)
            {
                var existing = await GetByTitleAsync(d.Title);
                if (existing != null) continue;
                _context.SkillsCategories.Add(d);
            }
            await _context.SaveChangesAsync();
        }

        private static List<SkillsCategory> GetDefaultCategories()
        {
            return new List<SkillsCategory>
            {
                new SkillsCategory { Title = "Frontend", Description = "Frontend technologies and libraries", DisplayOrder = 1, IsActive = true, Skills = new List<string> { "React.js", "JavaScript", "TypeScript", "HTML5", "CSS3/SCSS", "TailwindCSS", "Bootstrap", "jQuery", "NextJS", "Context API", "React Router", "React Query", "Redux", "Ant Design", "Formik" } },
                new SkillsCategory { Title = "Backend", Description = "Backend frameworks and APIs", DisplayOrder = 2, IsActive = true, Skills = new List<string> { "C#", "ASP.NET MVC", ".NET Core", "Node.js", "Express.js", "Sequelize ORM", "Dapper", "Entity Framework", "Sanity CMS", "RESTful APIs", "JSON", "Microservices", "Socket.io" } },
                new SkillsCategory { Title = "Databases", Description = "Databases and data management", DisplayOrder = 3, IsActive = true, Skills = new List<string> { "MySQL", "SQL Server", "Schema Design", "Query Optimization", "Stored Procedures", "Database Migration", "Indexing", "Performance Tuning" } },
                new SkillsCategory { Title = "Tools", Description = "Development and deployment tools", DisplayOrder = 4, IsActive = true, Skills = new List<string> { "Git/GitHub", "Azure", "Vercel", "Render", "Netlify", "Docker", "Figma", "Postman", "Swagger", "Jira", "Command Line", "File Upload", "CI/CD", "Auth0", "ILogger", "Serilog" } },
                new SkillsCategory { Title = "Practices", Description = "Methodologies and design practices", DisplayOrder = 5, IsActive = true, Skills = new List<string> { "Responsive Web Design", "UI/UX Design", "Test-Driven Development", "Agile/Scrum", "CI/CD", "API Design", "Dependency Injection", "Repository Pattern", "Clean Architecture", "Distributed Systems" } }
            };
        }
    }
}

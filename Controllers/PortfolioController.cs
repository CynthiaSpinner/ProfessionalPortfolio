using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.Models;
using Portfolio.Models.Portfolio;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Portfolio.Controllers
{
    public class PortfolioController : Controller
    {
        private readonly PortfolioContext _context;

        public PortfolioController(PortfolioContext context)
        {
            _context = context;
        }

        // GET: Portfolio/Projects
        public async Task<IActionResult> Projects()
        {
            var projects = await _context.Projects.ToListAsync();
            return View(projects);
        }

        // GET: Portfolio/Skills
        public async Task<IActionResult> Skills()
        {
            var skillsCategories = await _context.SkillsCategories.ToListAsync();
            return View(skillsCategories);
        }

        // GET: Portfolio/About
        public async Task<IActionResult> About()
        {
            var about = await _context.Abouts.FirstOrDefaultAsync();
            return View(about);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tekor.Data;
using Microsoft.EntityFrameworkCore;

namespace Tekor.Controllers
{
    [Route("[controller]/[action]")]
    public class GoalsController : Controller
    {
        private ApplicationDbContext _context;
        public GoalsController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            List<Goal> test = _context.Goal.Include(x=> x.Reward).ToList();
            List<object> goalItems = test.Select(x => new { x.Description ,x.Reward.Name }).ToList<object>();

            //List<string> test = new List<string> { "siker", "siker2", "siker3" };
            return Ok(new { goalItems = goalItems});
        }
    }
}
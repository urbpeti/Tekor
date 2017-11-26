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
        public async Task<IActionResult> GetList([FromQuery]string token)
        {
            if (!await _context.UserAcount.AnyAsync(x => x.UserToken == token)) {
                return Unauthorized();
            }
            List<Goal> test = _context.Goal.Include(x=> x.Reward).ToList();
            List<object> goalItems = test.Select(x => new { x.Description ,x.Name, x.ID }).ToList<object>();

            return Ok(new { goalItems = goalItems});
        }

        [HttpGet]
        public async Task<IActionResult> GetFinishedList([FromQuery]string token)
        {
            if (!await _context.UserAcount.AnyAsync(x => x.UserToken == token))
            {
                return Unauthorized();
            }
            List<ActualGoalState> test = _context.ActualGoalState.Include(x => x.Goal)
                                                                 .Include(x => x.User)
                                                                 .Where(x => x.User.UserToken == token && x.IsFinished)
                                                                 .ToList();
            List<object> goalItems = test.Select(x => new { x.Goal.Description, x.Goal.Name, x.Goal.ID }).ToList<object>();

            return Ok(new { goalItems = goalItems });
        }
    }
}
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
    public class ActiveGoalsController : Controller
    {
        private ApplicationDbContext _context;
        public ActiveGoalsController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetGoal([FromQuery]string usertoken,[FromQuery] string goalID)
        {
            if (!await _context.UserAcount.AnyAsync(x => x.UserToken == usertoken))
            {
                return Unauthorized();
            }
            ActualGoalState actualGoalState = await _context.ActualGoalState
                                                        .Include(x => x.Goal)
                                                        .Include(x => x.User)
                                                        .FirstOrDefaultAsync(x => x.Goal.ID == goalID && x.User.UserToken == usertoken);
            if (actualGoalState == null)
            {
                actualGoalState = new ActualGoalState
                {
                    ActualValue = 0,
                    Goal = await _context.Goal.FirstAsync(x => x.ID == goalID),
                    User = await _context.UserAcount.FirstAsync(x => x.UserToken == usertoken)
                };
                await _context.ActualGoalState.AddAsync(actualGoalState);
                await _context.SaveChangesAsync();
            }

            return Ok(new { sdf = actualGoalState });
        }

    }
}
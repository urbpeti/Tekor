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
                                                        .Include(x => x.Goal.Reward)
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

            string rewardCode = string.Empty;
            if (actualGoalState.IsFinished)
            {
                rewardCode = actualGoalState.Goal.Reward.CuponCode;
            }

            object result = new { ID = actualGoalState.ID,
                                  RewardName = actualGoalState.Goal.Name,
                                  Description = actualGoalState.Goal.Description,
                                  GoalValue = actualGoalState.Goal.GoalValue,
                                  ActualValue = actualGoalState.ActualValue,
                                  UserToken = actualGoalState.User.UserToken,
                                  CuponCode = rewardCode };

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddProgress([FromQuery]string usertoken, [FromQuery] string goalID)
        {
            if (!await _context.UserAcount.AnyAsync(x => x.UserToken == usertoken))
            {
                return Unauthorized();
            }
            ActualGoalState actualGoalState = await _context.ActualGoalState
                                                        .Include(x => x.Goal.Reward)
                                                        .Include(x => x.User)
                                                        .FirstOrDefaultAsync(x => x.Goal.ID == goalID && x.User.UserToken == usertoken);

            if (actualGoalState == null)
            {
                return BadRequest();
            }

            actualGoalState.ActualValue += 1;

            string rewardCode = string.Empty;
            if (actualGoalState.ActualValue == actualGoalState.Goal.GoalValue)
            {
                actualGoalState.IsFinished = true;
                rewardCode = actualGoalState.Goal.Reward.CuponCode;
            }
            await _context.SaveChangesAsync();

            return Ok(rewardCode);
        }

    }
}
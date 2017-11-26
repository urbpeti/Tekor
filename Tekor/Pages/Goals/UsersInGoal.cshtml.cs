using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Tekor.Data;
using Microsoft.EntityFrameworkCore;

namespace Tekor.Pages.Goals
{

    public class UsersInGoalModel : PageModel
    {
        private readonly Tekor.Data.ApplicationDbContext _context;
        private readonly UserManager<CompanyAccount> _userManager;

        public UsersInGoalModel(Tekor.Data.ApplicationDbContext context,
                          UserManager<CompanyAccount> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private string companyId { get; set; }
        public IList<ActualGoalState> ActualGoals { get; set; }
        public async Task<IActionResult> OnGet(string id)
        {
            companyId = _userManager.GetUserId(User);
            ActualGoals = await _context.ActualGoalState.Include(x=>x.Goal)
                                    .Where(x => x.Goal.ID == id && x.Goal.OwnerId == companyId)
                                    .Include(x => x.Goal.Reward)
                                    .Include(x=>x.User).ToListAsync();
            if (ActualGoals == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
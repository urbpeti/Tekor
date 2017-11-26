using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Tekor.Data;
using Microsoft.AspNetCore.Identity;

namespace Tekor.Pages.Goals
{
    public class DeleteModel : PageModel
    {
        private readonly Tekor.Data.ApplicationDbContext _context;
        private readonly UserManager<CompanyAccount> _userManager;

        public DeleteModel(Tekor.Data.ApplicationDbContext context,
                           UserManager<CompanyAccount> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public Goal Goal { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyId = _userManager.GetUserId(User);
            Goal = await _context.Goal.Include(x=> x.Reward).SingleOrDefaultAsync(m => m.ID == id && m.OwnerId == companyId);

            if (Goal == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var companyId = _userManager.GetUserId(User);
            Goal = await _context.Goal.Include(x => x.Reward).FirstOrDefaultAsync(x=> x.ID == id && x.OwnerId == companyId);
            
            if (Goal != null)
            {
                _context.Remove(Goal.Reward);
                var actualGoals = _context.ActualGoalState.Include(x=>x.Goal).Where(x=> x.Goal.ID == id && x.Goal.OwnerId == companyId);
                _context.RemoveRange(actualGoals);
                _context.Goal.Remove(Goal);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}

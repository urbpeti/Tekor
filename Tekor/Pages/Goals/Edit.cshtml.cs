using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tekor.Data;
using Microsoft.AspNetCore.Identity;

namespace Tekor.Pages.Goals
{
    public class EditModel : PageModel
    {
        private readonly Tekor.Data.ApplicationDbContext _context;
        private readonly UserManager<CompanyAccount> _userManager;

        public EditModel(Tekor.Data.ApplicationDbContext context,
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
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var companyId = _userManager.GetUserId(User);
            Goal goalToUpdate = await _context.Goal.Include(x => x.Reward).SingleOrDefaultAsync(m => m.ID == id && m.OwnerId == companyId);

            if (await TryUpdateModelAsync<Goal>(
                goalToUpdate,
                "goal",
                g => g.Reward, g => g.Description, g => g.GoalValue, g => g.ActualGoalStates, g=>g.OwnerId))
            {
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            return Page();
        }

        private bool GoalExists(string id)
        {
            return _context.Goal.Any(e => e.ID == id);
        }
    }
}

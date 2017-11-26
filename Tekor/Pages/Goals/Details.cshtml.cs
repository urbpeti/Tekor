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
    public class DetailsModel : PageModel
    {
        private readonly Tekor.Data.ApplicationDbContext _context;
        private readonly UserManager<CompanyAccount> _userManager;

        public DetailsModel(Tekor.Data.ApplicationDbContext context,
                            UserManager<CompanyAccount> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

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
    }
}

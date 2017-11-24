using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Tekor.Data;
using Microsoft.AspNetCore.Identity;

namespace Tekor.Pages.Goals
{
    public class CreateModel : PageModel
    {
        private readonly Tekor.Data.ApplicationDbContext _context;
        private readonly UserManager<CompanyAccount> _userManager;
        public CreateModel(Tekor.Data.ApplicationDbContext context,
                           UserManager<CompanyAccount> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Goal Goal { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var companyId = _userManager.GetUserId(User);
            Goal.OwnerId = companyId;
            _context.Goal.Add(Goal);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
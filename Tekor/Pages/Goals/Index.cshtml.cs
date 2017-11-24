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
    public class IndexModel : PageModel
    {
        private readonly Tekor.Data.ApplicationDbContext _context;
        private readonly UserManager<CompanyAccount> _userManager;

        public IndexModel(Tekor.Data.ApplicationDbContext context,
                          UserManager<CompanyAccount> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<Goal> Goal { get;set; }
        private string companyId { get; set; }


        public async Task OnGetAsync()
        {
            companyId = _userManager.GetUserId(User);
            Goal = await _context.Goal.Where(x=>x.OwnerId == companyId).Include(x=> x.Reward).ToListAsync();
        }
    }
}

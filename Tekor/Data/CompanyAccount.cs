using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Tekor.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class CompanyAccount : IdentityUser
    {
        public IList<Goal> Goals { get; set; }
    }
}

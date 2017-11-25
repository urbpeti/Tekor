using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tekor.Data;

namespace Tekor.Controllers
{
    [Route("[controller]/[action]")]
    public class LoginController : Controller
    {
        private ApplicationDbContext _context;
        public LoginController(ApplicationDbContext context)
        {
            _context = context;
        }

        public class UserData {
            public string token;
            public string email;
        }

        [HttpPost]
        public async Task<IActionResult> IsLoggedIn([FromBody] UserData userData)
        {
            var user = _context.UserAcount.FirstOrDefault(x => x.Email == userData.email);
            if (user == null) {
                return Unauthorized();
            }
            if (user.UserToken != userData.token) {
                return Unauthorized();
            }
            return Ok();
        }
    }
}
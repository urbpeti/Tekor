using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tekor.Data;

namespace Tekor.Controllers
{
    [Route("[controller]/[action]")]
    public class RegistrationController : Controller
    {
        private ApplicationDbContext _context;
        public RegistrationController(ApplicationDbContext context)
        {
            _context = context;
        }
        public class UserData
        {
            public string email;
            public string token;
        }

        [HttpPost]
        public async Task<IActionResult> Registration([FromBody] UserData userData)
        {
            var user = _context.UserAcount.FirstOrDefault(x => x.Email == userData.email);
            if (user != null)
            {
                return BadRequest(new { message = "Already used username" });
            }
            var newUser = new UserAccount { Email = userData.email, UserToken = userData.token };
            _context.UserAcount.Add(newUser);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
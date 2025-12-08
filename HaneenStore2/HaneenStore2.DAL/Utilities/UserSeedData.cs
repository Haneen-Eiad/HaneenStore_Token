using HaneenStore2.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaneenStore2.DAL.Utilities
{
    public class UserSeedData : ISeedData
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserSeedData(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task DataSeed()
        {
            if (!await _userManager.Users.AnyAsync())
            {
                var User1 = new ApplicationUser
                {
                    UserName = "Haneen1",
                    Email = "haneen@gmail.com",
                    FullName = "Haneen Shtaya",
                    EmailConfirmed = true,
                };
                var User2 = new ApplicationUser
                {
                    UserName = "Raga1",
                    Email = "Ragahd@gmail.com",
                    FullName = "Ragad Shtaya",
                    EmailConfirmed = true,
                };
                var User3 = new ApplicationUser
                {
                    UserName = "Ran2",
                    Email = "Raneen@gmail.com",
                    FullName = "Raneen Shtaya",
                    EmailConfirmed = true,
                };

                await _userManager.CreateAsync(User1, "P@ssw0rd1");
                await _userManager.CreateAsync(User2, "P@ssw0rd1");
                await _userManager.CreateAsync(User3, "P@ssw0rd1");

                await _userManager.AddToRoleAsync(User1, "SuperAdmin");
                await _userManager.AddToRoleAsync(User2, "Admin");
                await _userManager.AddToRoleAsync(User3, "User");

            }
           
        }
    }
}

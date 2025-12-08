using HaneenStore2.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaneenStore2.DAL.Data
{
   
    public class ApplicationDbContext :IdentityDbContext <ApplicationUser>
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryTranslation> Translations { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>().ToTable("User");
            builder.Entity<IdentityRole>().ToTable("Role");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRole");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaim");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserToken");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogin");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaim");




        }


    }
}

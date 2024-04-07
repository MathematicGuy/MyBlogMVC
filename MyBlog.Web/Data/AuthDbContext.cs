using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MyBlog.Web.Data
{
    public class AuthDbContext : IdentityDbContext
    {   
        // When there're more than 1 DbContext.
        // use DbContextOptions<AuthDbContext>
        // to make sure we're using the right DbContext (not BloggieDbContext)
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Seed Roles (User, Admin, SuperAdmin)
            var adminRoleId = "73a13894-0991-4aa4-81fa-cc7bd36fe248";
            var superAdminRoleId = "42d0062d-ff45-4aec-beb8-5f68fc8195a8";
            var userRoleId = "4499da9c-da78-4aa5-84f4-449857beeb33";

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name="Admin",
                    NormalizedName="Admin",
                    Id = adminRoleId,
                    ConcurrencyStamp = adminRoleId,
                },

                new IdentityRole
                {
                    Name="superAdmin",
                    NormalizedName="superAdmin",
                    Id = superAdminRoleId,
                    ConcurrencyStamp = superAdminRoleId,
                },

                new IdentityRole
                {
                    Name="User",
                    NormalizedName="User",
                    Id = userRoleId,
                    ConcurrencyStamp = userRoleId,
                }

            };
            builder.Entity<IdentityRole>().HasData(roles);


            var superAdminId = "c3675926-f9cf-4afe-9bb9-343750eb1680";
            // Seed SuperAdmin User
            var superAdminUser = new IdentityUser
            {
                UserName = "superadmin@myblog.com",
                Email = "superadmin@myblog.com".ToUpper(),
                NormalizedUserName = "superadmin@myblog.com".ToUpper(),
                Id = superAdminId,
            };

            superAdminUser.PasswordHash = new PasswordHasher<IdentityUser>()
                .HashPassword(superAdminUser, "sukurmom");

            builder.Entity<IdentityUser>().HasData(superAdminUser);

            // Add all roles to SuperAdmin User (Admin, SuperAdmin, User)
            // IdentityUserRole need a Key of type <string>
            var superAdminRoles = new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string>
                {
                    RoleId = adminRoleId,
                    UserId = superAdminId,
                },

                new IdentityUserRole<string>
                {
                    RoleId = superAdminRoleId,
                    UserId = superAdminId,
                },

                new IdentityUserRole<string>
                {
                    RoleId = userRoleId,
                    UserId = superAdminId,
                },
            };

            builder.Entity<IdentityUserRole<string>>().HasData(superAdminRoles);
        }
    }
}

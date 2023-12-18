using Project_DAW.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

// PASUL 4 - useri si roluri

namespace Project_DAW.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService
                <DbContextOptions<ApplicationDbContext>>()))
            {
                // Verificam daca in baza de date exista cel putin un rol
                // insemnand ca a fost rulat codul 
                // De aceea facem return pentru a nu insera rolurile inca o data
                // Acesta metoda trebuie sa se execute o singura data 
                if (context.Roles.Any())
                {
                    return;   // baza de date contine deja roluri
                }

                // CREAREA ROLURILOR IN BD
                // daca nu contine roluri, acestea se vor crea
                context.Roles.AddRange(
                    new IdentityRole { Id = "2c5e174e-3b0e-446f-86af-483d56fd7210", Name = "Admin", NormalizedName = "Admin".ToUpper() },
                    new IdentityRole { Id = "2c5e174e-3b0e-446f-86af-483d56fd7211", Name = "Moderator", NormalizedName = "Moderator".ToUpper() },
                    new IdentityRole { Id = "2c5e174e-3b0e-446f-86af-483d56fd7212", Name = "User", NormalizedName = "User".ToUpper() },
                     new IdentityRole { Id = "2c5e174e-3b0e-446f-86af-483d56fd7213", Name = "Admitere", NormalizedName = "Admitere".ToUpper() },
                     new IdentityRole { Id = "2c5e174e-3b0e-446f-86af-483d56fd7214", Name = "Licenta", NormalizedName = "Licenta".ToUpper() },
                     new IdentityRole { Id = "2c5e174e-3b0e-446f-86af-483d56fd7215", Name = "Master", NormalizedName = "Master".ToUpper() }

                );

                // o noua instanta pe care o vom utiliza pentru crearea parolelor utilizatorilor
                // parolele sunt de tip hash
                var hasher = new PasswordHasher<ApplicationUser>();

                // CREAREA USERILOR IN BD
                // Se creeaza cate un user pentru fiecare rol
                context.Users.AddRange(
                    new ApplicationUser
                    {
                        Id = "8e445865-a24d-4543-a6c6-9443d048cdb0", // primary key
                        UserName = "admin@test.com",
                        EmailConfirmed = true,
                        NormalizedEmail = "ADMIN@TEST.COM",
                        FirstName = "Admin",
                        LastName = "Test",
                        JoinDate = DateTime.Now,
                        Email = "admin@test.com",
                        NormalizedUserName = "ADMIN@TEST.COM",
                        PasswordHash = hasher.HashPassword(null, "Admin1!")
                    },
                    new ApplicationUser
                    {
                        Id = "8e445865-a24d-4543-a6c6-9443d048cdb1", // primary key
                        UserName = "mod@test.com",
                        EmailConfirmed = true,
                        FirstName = "Moderator",
                        LastName = "Test",
                        JoinDate = DateTime.Now,
                        NormalizedEmail = "MOD@TEST.COM",
                        Email = "mod@test.com",
                        NormalizedUserName = "MOD@TEST.COM",
                        PasswordHash = hasher.HashPassword(null, "Moderator1!")
                    },
                    new ApplicationUser
                    {
                        Id = "8e445865-a24d-4543-a6c6-9443d048cdb2", // primary key
                        UserName = "user@test.com",
                        EmailConfirmed = true,
                        NormalizedEmail = "USER@TEST.COM",
                        FirstName = "User",
                        LastName = "Test",
                        JoinDate = DateTime.Now,
                        Email = "user@test.com",
                        NormalizedUserName = "USER@TEST.COM",
                        PasswordHash = hasher.HashPassword(null, "User1!")
                    },
                    new ApplicationUser
                    {
                        Id = "8e445865-a24d-4543-a6c6-9443d048cdb3", // primary key
                        UserName = "usera@test.com",
                        EmailConfirmed = true,
                        NormalizedEmail = "USERA@TEST.COM",
                        FirstName = "Admitere",
                        LastName = "Test",
                        JoinDate = DateTime.Now,
                        Email = "usera@test.com",
                        NormalizedUserName = "USERA@TEST.COM",
                        PasswordHash = hasher.HashPassword(null, "User2!")
                    },
                    new ApplicationUser
                    {
                        Id = "8e445865-a24d-4543-a6c6-9443d048cdb4", // primary key
                        UserName = "userl@test.com",
                        EmailConfirmed = true,
                        NormalizedEmail = "USERL@TEST.COM",
                        FirstName = "Licenta",
                        LastName = "Test",
                        JoinDate = DateTime.Now,
                        Email = "userl@test.com",
                        NormalizedUserName = "USERL@TEST.COM",
                        PasswordHash = hasher.HashPassword(null, "User3!")
                    }, 
                    new ApplicationUser
                    {
                        Id = "8e445865-a24d-4543-a6c6-9443d048cdb5", // primary key
                        UserName = "userm@test.com",
                        EmailConfirmed = true,
                        NormalizedEmail = "USERM@TEST.COM",
                        FirstName = "Master",
                        LastName = "Test",
                        JoinDate = DateTime.Now,
                        Email = "userm@test.com",
                        NormalizedUserName = "USERM@TEST.COM",
                        PasswordHash = hasher.HashPassword(null, "User4!")
                    }
                );

                // ASOCIEREA USER-ROLE
                context.UserRoles.AddRange(
                    new IdentityUserRole<string>
                    {
                        RoleId = "2c5e174e-3b0e-446f-86af-483d56fd7210",
                        UserId = "8e445865-a24d-4543-a6c6-9443d048cdb0"
                    },
                    new IdentityUserRole<string>
                    {
                        RoleId = "2c5e174e-3b0e-446f-86af-483d56fd7211",
                        UserId = "8e445865-a24d-4543-a6c6-9443d048cdb1"
                    },
                    new IdentityUserRole<string>
                    {
                        RoleId = "2c5e174e-3b0e-446f-86af-483d56fd7212",
                        UserId = "8e445865-a24d-4543-a6c6-9443d048cdb2"
                    },
                     new IdentityUserRole<string>
                     {
                         RoleId = "2c5e174e-3b0e-446f-86af-483d56fd7213",
                         UserId = "8e445865-a24d-4543-a6c6-9443d048cdb3"
                     },
                      new IdentityUserRole<string>
                      {
                          RoleId = "2c5e174e-3b0e-446f-86af-483d56fd7214",
                          UserId = "8e445865-a24d-4543-a6c6-9443d048cdb4"
                      },
                       new IdentityUserRole<string>
                       {
                           RoleId = "2c5e174e-3b0e-446f-86af-483d56fd7215",
                           UserId = "8e445865-a24d-4543-a6c6-9443d048cdb5"
                       }
                );
                context.SaveChanges();

            }
        
        }
        
    }
}
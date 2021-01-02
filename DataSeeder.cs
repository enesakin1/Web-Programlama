using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using myiotprojects.Areas.Identity.Data;
using myiotprojects.Data;
using myiotprojects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace myiotprojects
{
    public class DataSeeder
    {
        private AuthDbContext _context;
        public DataSeeder(AuthDbContext context)
        {
            _context = context;
        }

        public async Task SeedSuperUser()
        {
            var roleStore = new RoleStore<IdentityRole>(_context);
            var userStore = new UserStore<AppUser>(_context);


            var user = new AppUser
            {
                UserName = "b171210397@sakarya.edu.tr",
                Email = "b171210397@sakarya.edu.tr",
                NormalizedEmail = "B171210397@SAKARYA.EDU.TR",
                NormalizedUserName = "B171210397@SAKARYA.EDU.TR",
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString(),
                Nickname = "admin",
                ProfileImageUrl = "/images/user/user.png",
                Created = DateTime.Now
            };

            var hasher = new PasswordHasher<AppUser>();
            var hashedPassword = hasher.HashPassword(user, "123");
            user.PasswordHash = hashedPassword;

            var randomUser = new AppUser
            {
                UserName = "randomuser@sakarya.edu.tr",
                Email = "randomuser@sakarya.edu.tr",
                NormalizedEmail = "RANDOMUSER@SAKARYA.EDU.TR",
                NormalizedUserName = "RANDOMUSER@SAKARYA.EDU.TR",
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString(),
                Nickname = "randomuser",
                ProfileImageUrl = "/images/user/user.png",
                Created = DateTime.Now
            };

            hashedPassword = hasher.HashPassword(randomUser, "123");
            randomUser.PasswordHash = hashedPassword;

            var hasAdminRole = _context.Roles.Any(roles => roles.Name == "Admin");
            if (!hasAdminRole)
            {
               await roleStore.CreateAsync(new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" });
            }

            var hasSuperUser = _context.Users.Any(u => u.NormalizedUserName == user.UserName);

            var post = new Post
            {
                User = user,
                Title = "Örnek Konu Başlığı",
                Content = "Örnek Konu İçeriği. Burası richtextbox ile oluşturulabilir. Daha iyi konu içeriklerini görmek istiyorsanız database backup dosyasını yükleyin.",
                Created = DateTime.Now
            };

            var reply = new PostReply
            {
                Content = "Örnek cevap. Burası richtextbox ile oluşturulabilir. Daha iyi konu içeriklerini görmek istiyorsanız database backup dosyasını yükleyin.",
                Created = DateTime.Now,
                Post = post,
                User = user
            };

            if(!hasSuperUser)
            {
                await userStore.CreateAsync(user);
                await userStore.AddToRoleAsync(user, "ADMIN");
                await userStore.CreateAsync(randomUser);
                _context.Add(post);
                _context.Add(reply);;
            }

            await _context.SaveChangesAsync();
        }

    }
}

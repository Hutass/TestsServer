using Microsoft.AspNetCore.Identity;
using WebLab2.Models;

namespace WebLab2.Data
{
    public class IdentitySeed
    {
        /// <summary>
        /// Инициализация изначаальных значений, если таковые отсутствуют
        /// </summary>
        /// <param name="serviceProvider">для получения контекста базы данных</param>
        /// <returns></returns>
        public static async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            // Создание ролей администратора и пользователя
            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }
            if (await roleManager.FindByNameAsync("moderator") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("moderator"));
            }
            if (await roleManager.FindByNameAsync("user") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("user"));
            }
            // Создание Администратора
            string adminEmail = "admin@mail.com";
            string adminPassword = "Qwe123@";
            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                User admin = new User { Email = adminEmail, UserName = adminEmail };
                IdentityResult result = await userManager.CreateAsync(admin, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }
            // Создание Модератора
            string moderatorEmail = "moderator@mail.com";
            string moderatorPassword = "Qwe123@";
            if (await userManager.FindByNameAsync(moderatorEmail) == null)
            {
                User moderator = new User { Email = moderatorEmail, UserName = moderatorEmail };
                IdentityResult result = await userManager.CreateAsync(moderator, moderatorPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(moderator, "moderator");
                }
            }
            // Создание Пользователя
            string userEmail = "user@mail.com";
            string userPassword = "Qwe123@";
            if (await userManager.FindByNameAsync(userEmail) == null)
            {
                User user = new User { Email = userEmail, UserName = userEmail };
                IdentityResult result = await userManager.CreateAsync(user, userPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "user");
                }
            }
        }
    }
}

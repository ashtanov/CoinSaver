using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoinSaver.Models.Data
{
    public static class Initalize
    {
        public static async void Run(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetService(typeof(CoinSaverContext)) as CoinSaverContext;
            var userManager = serviceProvider.GetService(typeof(UserManager<CSUser>)) as UserManager<CSUser>;

            CSRole[] roles = new CSRole[] {
                new CSRole { Name =  "User", Description = "Обычный пользователь" , NormalizedName = "USER"},
                new CSRole { Name =  "Administrator", Description = "Администратор", NormalizedName = "ADMINISTRATOR" }
            };

            foreach (var role in roles)
            {
                var roleStore = new RoleStore<CSRole>(context);

                if (!context.Roles.Any(r => r.NormalizedName == role.NormalizedName))
                {
                    await roleStore.CreateAsync(role);
                }
            }
            foreach (var user in userManager.Users)
            {
                if (!userManager.IsInRoleAsync(user, roles[0].Name).Result)
                {
                    await userManager.AddToRoleAsync(user, roles[0].Name);
                }
            }


        }
    }
}

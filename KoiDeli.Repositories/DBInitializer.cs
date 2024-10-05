using KoiDeli.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Repositories
{
    public class DBInitializer
    {
        public static async Task Initialize(KoiDeliDbContext context)
        {

            if (!context.Roles.Any())
            {
                var roles = new List<Role>
                {
                    new Role { Name = "User" },
                    new Role { Name = "Admin" }
                };

                foreach (var role in roles)
                {
                    await context.Roles.AddAsync(role);
                }

                await context.SaveChangesAsync();
            }
        }
    }
}

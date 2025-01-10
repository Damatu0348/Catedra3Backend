using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Src.Models;
using Bogus;
using Microsoft.AspNetCore.Identity;

namespace api.Src.Data
{
    public class DataSeeder
    {
        private static Random random = new Random();

        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<ApplicationDBContext>();
                var userManager = services.GetRequiredService<UserManager<Usuario>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                var faker = new Faker();

                List<IdentityRole> roles = new List<IdentityRole>
                {
                    new IdentityRole
                    {
                        Id = "1",
                        Name = "User",
                        NormalizedName = "USER",
                    },
                };

                foreach (var role in roles)
                {
                    if (!context.Roles.Any(r => r.Name == role.Name))
                    {
                        await roleManager.CreateAsync(role);
                        context.SaveChanges();
                    }
                }

                if (!userManager.Users.Any())
                {
                    for (int i = 0; i < 10; i++)
                    {
                        var usuarioSedder = new Usuario
                        {
                            Email = faker.Person.Email,
                            UserName = faker.Person.Email,
                        };

                        var createUser = await userManager.CreateAsync(
                            usuarioSedder,
                            "Password1234"
                        );
                        if (!createUser.Succeeded)
                        {
                            foreach (var error in createUser.Errors)
                            {
                                Console.WriteLine($"Error al crear usuario: {error.Description}");
                            }
                            throw new Exception("Error al crear el usuario");
                        }

                        var roleResult = await userManager.AddToRoleAsync(usuarioSedder, "User");
                        if (!roleResult.Succeeded)
                        {
                            foreach (var error in roleResult.Errors)
                            {
                                Console.WriteLine($"Error al asignar rol: {error.Description}");
                            }
                            throw new Exception("Error al asignar rol al usuario");
                        }

                        Console.WriteLine($"Usuario {usuarioSedder.UserName} creado exitosamente");
                    }
                }
            }
        }
    }
}

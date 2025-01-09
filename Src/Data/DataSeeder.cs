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

        public static async void Initialize(IServiceProvider serviceProvider)
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
                    }
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
                        faker = new Faker();

                        var usuarioSedder = new Usuario
                        {
                            UserName = faker.Person.UserName,
                            Email = faker.Person.Email,
                            Password = GenerarContraseñaAleatoria(),
                        };

                        var createUser = await userManager.CreateAsync(usuarioSedder);
                        if (!createUser.Succeeded)
                        {
                            throw new Exception("Error al crear el usuario");
                        }

                        var roleResult = userManager.AddToRoleAsync(usuarioSedder, "User");

                        if (roleResult.Result.Succeeded)
                        {
                            Console.WriteLine(
                                $"Usuario {usuarioSedder.UserName} creado exitosamente"
                            );
                            context.SaveChanges();
                        }
                        else
                        {
                            throw new Exception("Error al asignar rol al usuario");
                        }
                    }
                    context.SaveChanges();
                }
                context.SaveChanges();
            }
        }
    

    private static string GenerarContraseñaAleatoria()
        {
            var random = new Random();
            const string caracteres =
                "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            int longitudContraseña = random.Next(8, 21); // Generar una longitud entre 8 y 20 caracteres
            return new string(
                Enumerable
                    .Repeat(caracteres, longitudContraseña)
                    .Select(s => s[random.Next(s.Length)])
                    .ToArray()
            );
        }
    }
}

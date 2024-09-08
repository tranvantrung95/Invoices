using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebAPI.Data
{
    public class SeedData
    {
        public static void Initialize(InvoicikaDbContext context, ILogger<SeedData> logger)
        {
            // Ensure the database is created
            context.Database.EnsureCreated();

            try
            {
                // Check if any roles already exist
                if (!context.Roles.Any())
                {
                    // Add Roles
                    var adminRole = new Role
                    {
                        RoleId = Guid.NewGuid(),
                        RoleName = "Admin",
                        CreationDate = DateTime.UtcNow
                    };

                    var employeeRole = new Role
                    {
                        RoleId = Guid.NewGuid(),
                        RoleName = "Employee",
                        CreationDate = DateTime.UtcNow
                    };

                    context.Roles.AddRange(adminRole, employeeRole);
                    logger.LogInformation("Roles added to the database.");

                    // Add Users
                    var users = new List<User>
                    {
                        new User
                        {
                            Username = "admin1",
                            EmailAddress = "admin1@example.com",
                            PasswordHash = "hashedpassword1",
                            RoleId = adminRole.RoleId,
                            CreationDate = DateTime.UtcNow
                        },
                        new User
                        {
                            Username = "admin2",
                            EmailAddress = "admin2@example.com",
                            PasswordHash = "hashedpassword2",
                            RoleId = adminRole.RoleId,
                            CreationDate = DateTime.UtcNow
                        },
                        new User
                        {
                            Username = "famous_star1",
                            EmailAddress = "star1@example.com",
                            PasswordHash = "hashedpassword3",
                            RoleId = adminRole.RoleId,
                            CreationDate = DateTime.UtcNow
                        },
                        new User
                        {
                            Username = "famous_star2",
                            EmailAddress = "star2@example.com",
                            PasswordHash = "hashedpassword4",
                            RoleId = employeeRole.RoleId,
                            CreationDate = DateTime.UtcNow
                        }
                    };

                    context.Users.AddRange(users);
                    logger.LogInformation("Users added to the database.");

                    // Add Items
                    var items = new List<Item>();
                    for (int i = 1; i <= 25; i++)
                    {
                        items.Add(new Item
                        {
                            Name = $"Feminine Item {i}",
                            Description = $"Description for Feminine Item {i}",
                            PurchasePrice = 10.00m + i,
                            SalePrice = 20.00m + i,
                            Quantity = 100,
                            UserId = users[0].UserId, // Example user
                            CreationDate = DateTime.UtcNow
                        });
                    }

                    context.Items.AddRange(items);
                    logger.LogInformation("Items added to the database.");

                    // Save changes
                    context.SaveChanges();
                    logger.LogInformation("Database seeding completed.");
                }
                else
                {
                    logger.LogInformation("Roles already exist, skipping role seeding.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding the database.");
            }
        }
    }
}

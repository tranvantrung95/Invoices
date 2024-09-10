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
                            Role_id = adminRole.RoleId,
                            CreationDate = DateTime.UtcNow
                        },
                        new User
                        {
                            Username = "admin2",
                            EmailAddress = "admin2@example.com",
                            PasswordHash = "hashedpassword2",
                            Role_id = adminRole.RoleId,
                            CreationDate = DateTime.UtcNow
                        },
                        new User
                        {
                            Username = "famous_star1",
                            EmailAddress = "star1@example.com",
                            PasswordHash = "hashedpassword3",
                            Role_id = adminRole.RoleId,
                            CreationDate = DateTime.UtcNow
                        },
                        new User
                        {
                            Username = "famous_star2",
                            EmailAddress = "star2@example.com",
                            PasswordHash = "hashedpassword4",
                            Role_id = employeeRole.RoleId,
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
                            User_id = users[0].UserId, // Example user
                            CreationDate = DateTime.UtcNow
                        });
                    }

                    context.Items.AddRange(items);
                    logger.LogInformation("Items added to the database.");

                    // Add Customers
                    var customers = new List<Customer>
                    {
                        new Customer
                        {
                            Name = "Jenna Jameson",
                            Address = "1234 Elm St, Austin, TX 73301",
                            PhoneNumber = "512-555-1234",
                            Email = "jenna.jameson@example.com",
                            CreationDate = DateTime.UtcNow,
                            UpdateDate = DateTime.UtcNow
                        },
                        new Customer
                        {
                            Name = "Sasha Grey",
                            Address = "5678 Oak Dr, Phoenix, AZ 85001",
                            PhoneNumber = "602-555-5678",
                            Email = "sasha.grey@example.com",
                            CreationDate = DateTime.UtcNow,
                            UpdateDate = DateTime.UtcNow
                        },
                        new Customer
                        {
                            Name = "Max Müller",
                            Address = "Kaiserstraße 1, 10115 Berlin, Germany",
                            PhoneNumber = "+49 30 12345678",
                            Email = "max.mueller@example.com",
                            CreationDate = DateTime.UtcNow,
                            UpdateDate = DateTime.UtcNow
                        },
                        new Customer
                        {
                            Name = "Lukas Schmidt",
                            Address = "Schillerstraße 20, 80336 München, Germany",
                            PhoneNumber = "+49 89 23456789",
                            Email = "lukas.schmidt@example.com",
                            CreationDate = DateTime.UtcNow,
                            UpdateDate = DateTime.UtcNow
                        },
                        new Customer
                        {
                            Name = "Riley Reid",
                            Address = "9101 Maple Ave, Denver, CO 80201",
                            PhoneNumber = "303-555-9101",
                            Email = "riley.reid@example.com",
                            CreationDate = DateTime.UtcNow,
                            UpdateDate = DateTime.UtcNow
                        },
                        new Customer
                        {
                            Name = "Mia Martinez",
                            Address = "2345 Pine St, Las Vegas, NV 89101",
                            PhoneNumber = "702-555-2345",
                            Email = "mia.martinez@example.com",
                            CreationDate = DateTime.UtcNow,
                            UpdateDate = DateTime.UtcNow
                        },
                        new Customer
                        {
                            Name = "Felix Braun",
                            Address = "Hauptstraße 42, 20457 Hamburg, Germany",
                            PhoneNumber = "+49 40 34567890",
                            Email = "felix.braun@example.com",
                            CreationDate = DateTime.UtcNow,
                            UpdateDate = DateTime.UtcNow
                        },
                        new Customer
                        {
                            Name = "Julian Weber",
                            Address = "Friedrichstraße 77, 04109 Leipzig, Germany",
                            PhoneNumber = "+49 341 45678901",
                            Email = "julian.weber@example.com",
                            CreationDate = DateTime.UtcNow,
                            UpdateDate = DateTime.UtcNow
                        },
                        new Customer
                        {
                            Name = "Abella Danger",
                            Address = "6789 Cedar Ln, Seattle, WA 98101",
                            PhoneNumber = "206-555-6789",
                            Email = "abella.danger@example.com",
                            CreationDate = DateTime.UtcNow,
                            UpdateDate = DateTime.UtcNow
                        },
                        new Customer
                        {
                            Name = "Jordana James",
                            Address = "3456 Birch Rd, Orlando, FL 32801",
                            PhoneNumber = "407-555-3456",
                            Email = "jordana.james@example.com",
                            CreationDate = DateTime.UtcNow,
                            UpdateDate = DateTime.UtcNow
                        },
                        new Customer
                        {
                            Name = "Paul Schneider",
                            Address = "Berliner Allee 10, 40468 Düsseldorf, Germany",
                            PhoneNumber = "+49 211 56789012",
                            Email = "paul.schneider@example.com",
                            CreationDate = DateTime.UtcNow,
                            UpdateDate = DateTime.UtcNow
                        }
                    };

                    context.Customers.AddRange(customers);
                    logger.LogInformation("Customers added to the database.");

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

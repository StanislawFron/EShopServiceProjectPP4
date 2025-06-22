using Microsoft.EntityFrameworkCore;
using User.Domain.Models.Entities;
using User.Domain.Repositories;
using System.Security.Cryptography;
using System.Text;

namespace User.Application.Services
{
    public class ClientSeeder : IClientSeeder
    {
        private readonly DataContext _context;

        public ClientSeeder(DataContext context)
        {
            _context = context;
        }

        public async Task SeedClientsAsync()
        {
            if (await _context.Clients.AnyAsync())
            {
                return;
            }

            var users = new List<User.Domain.Models.Entities.User>
            {
                new User.Domain.Models.Entities.User { Username = "jan.kowalski", Email = "jan.kowalski@example.com", PasswordHash = HashPassword("Password1!") },
                new User.Domain.Models.Entities.User { Username = "anna.nowak", Email = "anna.nowak@example.com", PasswordHash = HashPassword("Password1!") },
                new User.Domain.Models.Entities.User { Username = "piotr.wisniewski", Email = "piotr.wisniewski@example.com", PasswordHash = HashPassword("Password1!") },
                new User.Domain.Models.Entities.User { Username = "maria.wojcik", Email = "maria.wojcik@example.com", PasswordHash = HashPassword("Password1!") },
                new User.Domain.Models.Entities.User { Username = "tomasz.kowalczyk", Email = "tomasz.kowalczyk@example.com", PasswordHash = HashPassword("Password1!") },
                new User.Domain.Models.Entities.User { Username = "katarzyna.kaminska", Email = "katarzyna.kaminska@example.com", PasswordHash = HashPassword("Password1!") },
                new User.Domain.Models.Entities.User { Username = "marek.lewandowski", Email = "marek.lewandowski@example.com", PasswordHash = HashPassword("Password1!") },
                new User.Domain.Models.Entities.User { Username = "agnieszka.zielinska", Email = "agnieszka.zielinska@example.com", PasswordHash = HashPassword("Password1!") },
                new User.Domain.Models.Entities.User { Username = "robert.dabrowski", Email = "robert.dabrowski@example.com", PasswordHash = HashPassword("Password1!") },
                new User.Domain.Models.Entities.User { Username = "ewa.kozlowska", Email = "ewa.kozlowska@example.com", PasswordHash = HashPassword("Password1!") }
            };
            await _context.Users.AddRangeAsync(users);
            await _context.SaveChangesAsync();

            var clients = new List<Client>
            {
                new Client
                {
                    Name = "Jan Kowalski",
                    NIP = "1234567890",
                    PhoneNumber = "+48 123 456 789",
                    Address = new Address
                    {
                        Street = "ul. Marszałkowska",
                        StreetNumber = "123",
                        City = "Warszawa",
                        Country = "Polska"
                    },
                    UserId = users[0].Id,
                    User = users[0]
                },
                new Client
                {
                    Name = "Anna Nowak",
                    NIP = "0987654321",
                    PhoneNumber = "+48 987 654 321",
                    Address = new Address
                    {
                        Street = "ul. Krakowska",
                        StreetNumber = "45",
                        City = "Kraków",
                        Country = "Polska"
                    },
                    UserId = users[1].Id,
                    User = users[1]
                },
                new Client
                {
                    Name = "Piotr Wiśniewski",
                    NIP = "1122334455",
                    PhoneNumber = "+48 111 222 333",
                    Address = new Address
                    {
                        Street = "ul. Długa",
                        StreetNumber = "67",
                        City = "Gdańsk",
                        Country = "Polska"
                    },
                    UserId = users[2].Id,
                    User = users[2]
                },
                new Client
                {
                    Name = "Maria Wójcik",
                    NIP = "5544332211",
                    PhoneNumber = "+48 444 555 666",
                    Address = new Address
                    {
                        Street = "ul. Szeroka",
                        StreetNumber = "89",
                        City = "Wrocław",
                        Country = "Polska"
                    },
                    UserId = users[3].Id,
                    User = users[3]
                },
                new Client
                {
                    Name = "Tomasz Kowalczyk",
                    NIP = "6677889900",
                    PhoneNumber = "+48 777 888 999",
                    Address = new Address
                    {
                        Street = "ul. Nowa",
                        StreetNumber = "12",
                        City = "Poznań",
                        Country = "Polska"
                    },
                    UserId = users[4].Id,
                    User = users[4]
                },
                new Client
                {
                    Name = "Katarzyna Kamińska",
                    NIP = "0011223344",
                    PhoneNumber = "+48 000 111 222",
                    Address = new Address
                    {
                        Street = "ul. Stara",
                        StreetNumber = "34",
                        City = "Łódź",
                        Country = "Polska"
                    },
                    UserId = users[5].Id,
                    User = users[5]
                },
                new Client
                {
                    Name = "Marek Lewandowski",
                    NIP = "5566778899",
                    PhoneNumber = "+48 333 444 555",
                    Address = new Address
                    {
                        Street = "ul. Zielona",
                        StreetNumber = "56",
                        City = "Szczecin",
                        Country = "Polska"
                    },
                    UserId = users[6].Id,
                    User = users[6]
                },
                new Client
                {
                    Name = "Agnieszka Zielińska",
                    NIP = "9988776655",
                    PhoneNumber = "+48 666 777 888",
                    Address = new Address
                    {
                        Street = "ul. Niebieska",
                        StreetNumber = "78",
                        City = "Katowice",
                        Country = "Polska"
                    },
                    UserId = users[7].Id,
                    User = users[7]
                },
                new Client
                {
                    Name = "Robert Dąbrowski",
                    NIP = "4433221100",
                    PhoneNumber = "+48 999 000 111",
                    Address = new Address
                    {
                        Street = "ul. Czerwona",
                        StreetNumber = "90",
                        City = "Lublin",
                        Country = "Polska"
                    },
                    UserId = users[8].Id,
                    User = users[8]
                },
                new Client
                {
                    Name = "Ewa Kozłowska",
                    NIP = "1122334455",
                    PhoneNumber = "+48 222 333 444",
                    Address = new Address
                    {
                        Street = "ul. Żółta",
                        StreetNumber = "11",
                        City = "Białystok",
                        Country = "Polska"
                    },
                    UserId = users[9].Id,
                    User = users[9]
                }
            };

            await _context.Clients.AddRangeAsync(clients);
            await _context.SaveChangesAsync();

            // Dodaj role jeśli nie istnieją
            var adminRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Admin");
            if (adminRole == null)
            {
                adminRole = new Role { Name = "Admin" };
                _context.Roles.Add(adminRole);
            }
            var userRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Użytkownik");
            if (userRole == null)
            {
                userRole = new Role { Name = "Użytkownik" };
                _context.Roles.Add(userRole);
            }
            await _context.SaveChangesAsync();

            // Przypisz rolę Admin użytkownikowi o id 1
            var user1 = await _context.Users.FirstOrDefaultAsync(u => u.Id == 1);
            if (user1 != null)
            {
                if (user1.Roles == null)
                    user1.Roles = new List<Role>();
                if (!user1.Roles.Any(r => r.Name == "Admin"))
                    user1.Roles.Add(adminRole);
                _context.Users.Update(user1);
                await _context.SaveChangesAsync();
            }
        }

        private string HashPassword(string password)
        {
            // Prosty hash, tylko do seedowania!
            using (var sha = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
} 
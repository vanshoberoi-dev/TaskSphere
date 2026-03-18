using Bogus;
using EFCore.BulkExtensions;
using Isopoh.Cryptography.Argon2;
using Microsoft.EntityFrameworkCore;
using TS.Model.Data;
using TS.Model.Entities;
using TS.Model.Entities.Auth;

namespace TS.Infrastructure.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            // 1. ROLES
            if (!await context.Roles.AnyAsync())
            {
                var roles = new List<RoleEntity> { new() { Role = "Admin" }, new() { Role = "User" } };
                await context.BulkInsertAsync(roles);
            }

            var adminRoleId = (await context.Roles.FirstAsync(r => r.Role == "Admin")).Id;
            var userRoleId = (await context.Roles.FirstAsync(r => r.Role == "User")).Id;

            // 2. USERS (600k)
            const int totalUserTarget = 600_000;
            int currentUserCount = await context.Users.CountAsync();

            if (currentUserCount < totalUserTarget)
            {
                int remaining = totalUserTarget - currentUserCount;
                string hashedPassword = Argon2.Hash("am i visible ?");

                var userFaker = new Faker<UserEntity>()
                    .RuleFor(u => u.Name, f => f.Name.FullName())
                    .RuleFor(u => u.Email, (f, u) => $"{f.Internet.UserName(u.Name).ToLower()}{f.UniqueIndex}@company.com")
                    .RuleFor(u => u.PasswordHash, _ => hashedPassword)
                    .RuleFor(u => u.RoleId, f => f.Random.WeightedRandom(new[] { adminRoleId, userRoleId }, new[] { 1.0f, 200.0f }));

                const int batchSize = 20000;
                for (int i = 0; i < remaining; i += batchSize)
                {
                    var users = userFaker.Generate(Math.Min(batchSize, remaining - i));
                    await context.BulkInsertAsync(users);
                    Console.WriteLine($"Users: {currentUserCount + i + users.Count}/{totalUserTarget}");
                }
            }

            // 3. TASKS (100k)
            const int totalTaskTarget = 100_000;
            if (await context.Tasks.CountAsync() < totalTaskTarget)
            {
                var adminIds = await context.Users.Where(u => u.RoleId == adminRoleId).Select(u => u.Id).Take(2000).ToListAsync();
                var taskFaker = new Faker<TaskEntity>()
                    .RuleFor(t => t.Title, f => f.Commerce.ProductName())
                    .RuleFor(t => t.Description, f => f.Lorem.Sentence())
                    .RuleFor(t => t.Status, f => f.PickRandom<TS.Contract.Enums.TaskStatus>())
                    .RuleFor(t => t.DueDate, f => f.Date.Future())
                    .RuleFor(t => t.CreatedById, f => f.PickRandom(adminIds));

                for (int i = 0; i < 10; i++)
                {
                    await context.BulkInsertAsync(taskFaker.Generate(10000));
                    Console.WriteLine($"Tasks: {(i + 1) * 10000}/100000");
                }
            }

            // 4. COMMENTS (300k - Stress Test)
            const int totalCommentTarget = 300_000;
            if (await context.Comments.CountAsync() < totalCommentTarget)
            {
                Console.WriteLine("Preparing IDs for Comment seeding...");
                // Pull a pool of valid Task and User IDs to link comments to
                var taskIds = await context.Tasks.Select(t => t.Id).Take(10000).ToListAsync();
                var userIds = await context.Users.Select(u => u.Id).Take(10000).ToListAsync();

                var commentFaker = new Faker<CommentEntity>()
                    .RuleFor(c => c.Comment, f => f.Lorem.Sentence())
                    .RuleFor(c => c.TaskId, f => f.PickRandom(taskIds))
                    .RuleFor(c => c.UserId, f => f.PickRandom(userIds))
                    .RuleFor(c => c.CreatedOn, f => f.Date.Past(1));

                for (int i = 0; i < 30; i++) // 30 batches of 10k = 300k
                {
                    var comments = commentFaker.Generate(10000);
                    await context.BulkInsertAsync(comments);
                    Console.WriteLine($"Comments: {(i + 1) * 10000}/{totalCommentTarget}");
                }
            }
        }
    }
}
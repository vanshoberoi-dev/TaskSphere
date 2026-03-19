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
            // 1. ROLES (Existing logic is fine)
            if (!await context.Roles.AnyAsync())
            {
                var roles = new List<RoleEntity> { new() { Role = "Admin" }, new() { Role = "User" } };
                await context.BulkInsertAsync(roles);
            }
            var adminRoleId = (await context.Roles.FirstAsync(r => r.Role == "Admin")).Id;
            var userRoleId = (await context.Roles.FirstAsync(r => r.Role == "User")).Id;

            // 2. USERS (1.2M)
            const int totalUserTarget = 2_400_000;
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
                    var countToGenerate = Math.Min(batchSize, remaining - i);
                    await context.BulkInsertAsync(userFaker.Generate(countToGenerate));
                    // Corrected Log: Shows the actual count now in the DB  
                    Console.WriteLine($"Users: {currentUserCount + i + countToGenerate}/{totalUserTarget}");
                }
            }

            // 3. TASKS (600k)
            const int totalTaskTarget = 600_000;
            int currentTaskCount = await context.Tasks.CountAsync();

            if (currentTaskCount < totalTaskTarget)
            {
                int remainingTasks = totalTaskTarget - currentTaskCount;
                var adminIds = await context.Users.Where(u => u.RoleId == adminRoleId).Select(u => u.Id).Take(2000).ToListAsync();
                var taskFaker = new Faker<TaskEntity>()
                    .RuleFor(t => t.Title, f => f.Commerce.ProductName())
                    .RuleFor(t => t.Description, f => f.Lorem.Sentence())
                    .RuleFor(t => t.Status, f => f.PickRandom<TS.Contract.Enums.TaskStatus>())
                    .RuleFor(t => t.DueDate, f => f.Date.Future())
                    .RuleFor(t => t.CreatedById, f => f.PickRandom(adminIds));

                const int taskBatchSize = 10000;
                // FIX: Loop based on 'remainingTasks', not a hardcoded '10'
                for (int i = 0; i < remainingTasks; i += taskBatchSize)
                {
                    var countToGenerate = Math.Min(taskBatchSize, remainingTasks - i);
                    await context.BulkInsertAsync(taskFaker.Generate(countToGenerate));
                    Console.WriteLine($"Tasks: {currentTaskCount + i + countToGenerate}/{totalTaskTarget}");
                }
            }

            // 4. COMMENTS (4.8M)
            const int totalCommentTarget = 4_800_000;
            int currentCommentCount = await context.Comments.CountAsync();

            if (currentCommentCount < totalCommentTarget)
            {
                int remainingComments = totalCommentTarget - currentCommentCount;
                Console.WriteLine($"Preparing IDs. Current: {currentCommentCount}. Remaining: {remainingComments}");

                var taskIds = await context.Tasks.Select(t => t.Id).Take(10000).ToListAsync();
                var userIds = await context.Users.Select(u => u.Id).Take(10000).ToListAsync();

                var commentFaker = new Faker<CommentEntity>()
                    .RuleFor(c => c.Comment, f => f.Lorem.Sentence())
                    .RuleFor(c => c.TaskId, f => f.PickRandom(taskIds))
                    .RuleFor(c => c.UserId, f => f.PickRandom(userIds))
                    .RuleFor(c => c.CreatedOn, f => f.Date.Past(1));

                const int commentBatchSize = 20000;
                // FIX: Loop based on 'remainingComments', not hardcoded '30'
                for (int i = 0; i < remainingComments; i += commentBatchSize)
                {
                    var countToGenerate = Math.Min(commentBatchSize, remainingComments - i);
                    await context.BulkInsertAsync(commentFaker.Generate(countToGenerate));
                    Console.WriteLine($"Comments: {currentCommentCount + i + countToGenerate}/{totalCommentTarget}");
                }
            }
        }
    }
}
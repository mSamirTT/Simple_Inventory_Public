using StarterApp.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using StarterApp.Core.Areas.Categories.Entities;
using StarterApp.Core.Areas.Products.Entities;
 using System;
using Microsoft.EntityFrameworkCore;
using StarterApp.Core.Areas.Issues.Entities;
using StarterApp.Core.Areas.Supplies.Entities;
using System.Collections.Generic;

namespace StarterApp.Infrastructure.Persistence
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedDefaultUserAsync(UserManager<ApplicationUser> userManager)
        {
            var defaultUser = new ApplicationUser { UserName = "administrator@localhost", Email = "administrator@localhost" };

            if (userManager.Users.All(u => u.UserName != defaultUser.UserName))
            {
                await userManager.CreateAsync(defaultUser, "Administrator1!");
            }
        }

        public static async Task SeedSampleDataAsync(ApplicationDbContext context)
        {
            // Seed, if necessary
            if (!context.Categories.Any())
            {
                for (int i = 0; i < 4; i++)
                {
                    context.Categories.Add(new Category("Category_" + (i+1),
                        "https://static6.depositphotos.com/1112859/621/i/950/depositphotos_6219942-stock-photo-search-of-data-isolated-3d.jpg"));
                }
            }
            await context.SaveChangesAsync();

            if (!context.Products.Any())
            {
                (await context.Categories.ToListAsync()).ForEach(category =>
                {
                    for (int i = 0; i < 3; i++)
                    {
                        context.Products.Add(new Product("Product_" + ((category.Id * 10) + i+1),
                            "Description for product_" + ((category.Id * 10) + i+1),
                            new Random().Next(1, 1000),
                            "https://previews.123rf.com/images/aquir/aquir1311/aquir131100316/23569861-sample-grunge-red-round-stamp.jpg",
                            category.Id
                            ));
                    }
                });
            }
            await context.SaveChangesAsync();

            if (!context.SupplyHeaders.Any() && !context.IssueHeaders.Any())
            {
                var productList = await context.Products.ToListAsync();
                for (int i = 0; i < 20; i++)
                {
                    var supplyDetails = new List<SupplyDetail>();
                    var issueDetails = new List<IssueDetail>();
                    for (int j = 0; j < new Random().Next(1, productList.Count); j++)
                    {
                        supplyDetails.Add(new SupplyDetail(productList[j].Id, new Random().Next(1, 10)));
                        issueDetails.Add(new IssueDetail(productList[j].Id, new Random().Next(1, 5)));
                    }

                    var supplyHeader = new SupplyHeader(i + 1, "Lorem Ipsum is simply dummy text of the printing and typesetting #" + (i + 1), DateTime.Now.AddDays(-i - 1), supplyDetails);
                    var issueHeader = new IssueHeader(i + 1, "Lorem Ipsum is simply dummy text of the printing and typesetting #" + (i + 1), DateTime.Now.AddDays(-i - 1), issueDetails);

                    supplyHeader.ClearDomainEvents();
                    issueHeader.ClearDomainEvents();

                    context.SupplyHeaders.Add(supplyHeader);
                    context.IssueHeaders.Add(issueHeader);
                }
                await context.SaveChangesAsync();
            }
        }
    }
}

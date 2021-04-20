using Bogus;
using Lms.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Data.Data
{
    public class SeedData
    {
        public static async Task InitAsync(IServiceProvider services)
        {
            using (var db = new ApplicationDbContext(services.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                var fake = new Faker("sv");
                var courses = new List<Course>();

                for (int i = 0; i < 20; i++)
                {
                    var noOfModules = fake.Random.Int(3, 10);
                    var course = new Course
                    {
                        Title = fake.Company.CatchPhrase(),
                        StartDate = DateTime.Now.AddDays(fake.Random.Int(-2, 2)),
                        Modules = new List<Module>()
                    };
                    for (int m = 0; m < noOfModules; m++)
                    {
                        course.Modules.Add(new Module
                        {
                            Title = fake.Company.Bs(),
                            StartDate = DateTime.Now.AddDays(fake.Random.Int(10, 20))
                        }); ;
                    }
                    courses.Add(course);
                }
                await db.AddRangeAsync(courses);
                await db.SaveChangesAsync();
            }
        }
    }
}

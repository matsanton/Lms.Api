using Lms.Core.Entities;
using Lms.Core.Repositories;
using Lms.Data.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Data.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly ApplicationDbContext db;
        public CourseRepository(ApplicationDbContext context)
        {
            db = context;
        }
        public async Task<IEnumerable<Course>> GetAllCourses(bool includeModules)
        {
            return includeModules ? await db.Courses
                                        .Include(c => c.Modules)
                                        .ToListAsync() :
                                    await db.Courses.ToListAsync();
        }
        public Task<Course> GetCourse(int? id)
        {
            return Task.FromResult<Course>(db.Courses.FirstOrDefault(c => c.Id == id));
        }
        public async Task<bool> SaveAsync()
        {
            return (await db.SaveChangesAsync()) >= 0;
        }
        public async Task AddAsync<T>(T added)
        {
            await db.AddAsync(added);
        }
        public void Remove<T>(T removed)
        {
            db.Remove(removed);
        }
    }



}

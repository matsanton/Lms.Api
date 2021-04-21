using Lms.Core.Entities;
using Lms.Core.Repositories;
using Lms.Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Data.Repositories
{
    public class ModuleRepository : IModuleRepository
    {
        private readonly ApplicationDbContext db;

        public ModuleRepository(ApplicationDbContext context)
        {
            db = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task AddAsync<T>(T added)
        {
            await db.AddAsync(added);
        }
        public void Remove<T>(T removed)
        {
            db.Remove(removed);
        }
        public Task<IEnumerable<Module>> GetModulesForCourse(int? id)
        {
            return Task.FromResult<IEnumerable<Module>>(db.Modules.Where(m => m.CourseId == id));
        }

        public Task<Module> GetModuleForCourse(int? id)
        {
            return Task.FromResult<Module>(db.Modules.FirstOrDefault(m => m.CourseId == id));
        }

        public async Task<bool> SaveAsync()
        {
            return (await db.SaveChangesAsync()) >= 0;
        }
    }
}


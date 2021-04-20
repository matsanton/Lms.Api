using Lms.Core.Repositories;
using Lms.Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Data.Repositories
{
    public class UoW : IUoW
    {
        private readonly ApplicationDbContext db;
        public ICourseRepository CourseRepository { get; set; }
        public IModuleRepository ModuleRepository { get; set; }

        public UoW(ApplicationDbContext context)
        {
            db = context;
            CourseRepository = new CourseRepository(db);
            ModuleRepository = new ModuleRepository(db);
        }

        public async Task CompleteAsync()
        {
            await db.SaveChangesAsync();
        }
    }
}

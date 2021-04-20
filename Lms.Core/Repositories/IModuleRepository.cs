using Lms.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Core.Repositories
{
    public interface IModuleRepository
    {
        Task<IEnumerable<Module>> GetAllModulesForCourse(int? id);
        Task<Module> GetModuleForCourse(int? id);
        Task<bool> SaveAsync();
        Task AddAsync<T>(T added);
        //void Remove<T>(T removed);
    }
}

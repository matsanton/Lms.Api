using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lms.Core.Entities;
using Lms.Data.Data;
using Lms.Data.Repositories;
using Lms.Core.Repositories;
using AutoMapper;
using Lms.Core.Dto;

namespace Lms.Api.Controllers
{
    [ApiController]
    [Route("api/courses/{courseId}/modules")]
    public class ModulesController : ControllerBase
    {
        #region Private fields
        private readonly IUoW uow;
        private readonly IMapper mapper;
        #endregion

        public ModulesController(IUoW uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ModuleDto>>> GetModulesForCourse(int courseId)
        {
            var course = await uow.CourseRepository.GetCourse(courseId);
            if (course == null)
            {
                return NotFound();
            }
        
            var modulesForCourse = await uow.ModuleRepository.GetModulesForCourse(courseId);

            if (modulesForCourse == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<IEnumerable<ModuleDto>>(modulesForCourse));
        }

          
        /*

                [HttpPut("{id}")]
                public async Task<IActionResult> PutModule(int id, Module @module)
                {
                    if (id != @module.Id)
                    {
                        return BadRequest();
                    }

                    _context.Entry(@module).State = EntityState.Modified;

                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ModuleExists(id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }

                    return NoContent();
                }


                [HttpPost]
                public async Task<ActionResult<Module>> PostModule(Module @module)
                {
                    _context.Modules.Add(@module);
                    await _context.SaveChangesAsync();

                    return CreatedAtAction("GetModule", new { id = @module.Id }, @module);
                }


                [HttpDelete("{id}")]
                public async Task<IActionResult> DeleteModule(int id)
                {
                    var @module = await _context.Modules.FindAsync(id);
                    if (@module == null)
                    {
                        return NotFound();
                    }

                    _context.Modules.Remove(@module);
                    await _context.SaveChangesAsync();

                    return NoContent();
                }
        */


    }
}

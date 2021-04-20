using AutoMapper;
using Lms.Core.Entities;
using Lms.Core.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lms.Api.Controllers
{
    [ApiController]
    [Route("api/courses")]
    public class CoursesController : ControllerBase
    {
        private readonly IUoW uow;
        private readonly IMapper mapper;

        public CoursesController(IUoW uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourses(bool includeModules = false)
        { 
            var result = await uow.CourseRepository.GetAllCoursesAsync(includeModules);
            return Ok(result);
        }

        [HttpGet("{courseId}")]
        public async Task<ActionResult<Course>> GetCourse(int courseId)
        {
            var course = await uow.CourseRepository.GetCourse(courseId);

            if (course == null)
            {
                return NotFound();
            }

            return Ok(course);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourse(int id, Course course)
        {
           // if (id != course.Id)
           // {
           //     return BadRequest();
           // }

           //db.Entry(course).State = EntityState.Modified;

           // try
           // {
           //     await _context.SaveChangesAsync();
           // }
           // catch (DbUpdateConcurrencyException)
           // {
           //     if (!CourseExists(id))
           //     {
           //         return NotFound();
           //     }
           //     else
           //     {
           //         throw;
           //     }
           // }

            return NoContent();
        }
        /*
        // POST: api/Courses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Course>> PostCourse(Course course)
        {
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCourse", new { id = course.Id }, course);
        }

        // DELETE: api/Courses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            //var course = await _context.Courses.FindAsync(id);
            //if (course == null)
            //{
            //    return NotFound();
            //}

            //_context.Courses.Remove(course);
            //await _context.SaveChangesAsync();

            //return NoContent();
        }

        private bool CourseExists(int id)
        {
            //return _context.Courses.Any(e => e.Id == id);
        }
         */
    }

}

using AutoMapper;
using Lms.Core.Entities;
using Lms.Core.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lms.Core.Dto;
using Microsoft.AspNetCore.JsonPatch;

namespace Lms.Api.Controllers
{
    [ApiController]
    [Route("api/courses")]
    public class CoursesController : ControllerBase
    {
        #region Private fields
        private readonly IUoW uow;
        private readonly IMapper mapper;

        #endregion
        public CoursesController(IUoW uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        // GET: api/courses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseDto>>> GetAllCourses(bool includeModules = false)
        {
            var courses = await uow.CourseRepository.GetAllCourses(includeModules);
            var coursesDto = mapper.Map<IEnumerable<CourseDto>>(courses);
            return Ok(coursesDto);
        }

        /// <summary>
        /// Get a cource by its id
        /// </summary>
        /// <param name="courseId">The id of the course you want to get</param> 
        /// <returns>An course with Title, StarDate and EndDate fields</returns>
        [HttpGet("{courseId}")]
        public async Task<ActionResult<CourseDto>> GetCourse(int courseId)
        {
            var course = await uow.CourseRepository.GetCourse(courseId);

            if (course == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<CourseDto>(course));
        }

        [HttpPost]
        public async Task<ActionResult<CourseDto>> CreateCourse(CourseDto dto)
        {

            var course = mapper.Map<Course>(dto);
            await uow.CourseRepository.AddAsync(course);
            if (await uow.CourseRepository.SaveAsync())
            {
                var model = mapper.Map<CourseDto>(course);
                return CreatedAtAction("GetCourse", new { id = course.Id }, model);
                // The framework's default is to delet the Async from the action name.
                // the can be overridden in the startup class by adding an option " opt.SuppressAsyncSuffixInActionNames = false; " so the Add controller method will look like:
                /*
                 * services.AddControllers(opt =>
                        {
                            opt.ReturnHttpNotAcceptable = true;
                            opt.SuppressAsyncSuffixInActionNames = false;
                        })
                 * */
            }
            else
            {
                return StatusCode(500);
            }

        }

        /*
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourse(int id, Course course)
        {
            if (id != course.Id)
            {
                return BadRequest();
            }

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

        [HttpPatch("{courseId}")]
        public async Task<ActionResult<CourseDto>> PatchCourse(int courseId, JsonPatchDocument<CourseDto> patchDocument)
        {
            // Kolla om courseId finns, annars returnera NotFound()
            var course = new Course();
            // Ta fram kursen som ska patchas med uow

            var model = mapper.Map<CourseDto>(course);

            patchDocument.ApplyTo(model, ModelState);

            // Försök validera modellen returnera BadRequest om det inte går

            mapper.Map(model, course);

            if (await uow.CourseRepository.SaveAsync())
            {
                return Ok(mapper.Map<CourseDto>(course));
            }
            else
            {
                return StatusCode(500);
            }
        }
    }
}

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

        // GET: api/Courses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseDto>>> GetAllCourses(bool includeModules = false)
        {
            var courses = await uow.CourseRepository.GetAllCourses(includeModules);
            var coursesDto = mapper.Map<IEnumerable<CourseDto>>(courses);
            return Ok(coursesDto);
        }

        /// <summary>
        /// Get a cource by its id
        /// GET: api/Courses/5
        /// </summary>
        /// <param name="courseId">The id of the course you want to get</param> 
        /// <returns>An course with Title, StarDate and EndDate fields</returns>
        [HttpGet("{courseId}", Name = "GetCourse")]
        public async Task<ActionResult<CourseDto>> GetCourse(int courseId)
        {
            var course = await uow.CourseRepository.GetCourse(courseId);

            if (course == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<CourseDto>(course));
        }

        // POST: api/Courses
        [HttpPost]
        public async Task<ActionResult<CourseDto>> CreateCourse(CourseDto coursesDto)
        {
            var course = mapper.Map<Course>(coursesDto);
            await uow.CourseRepository.AddAsync(course);
            await uow.CompleteAsync();
            return CreatedAtAction("GetCourse", new { courseId = course.Id }, course);
        }

        // PUT: api/Courses/5
        [HttpPut("{courseId}")]
        public async Task<IActionResult> UpdateCourse(int courseId, CourseDto courseDto)
        {
            var course = await uow.CourseRepository.GetCourse(courseId);

            if (course == null)
            {
                return NotFound();
            }

            mapper.Map(courseDto, course);

            await uow.CompleteAsync();
            return Ok(courseDto);
        }

        [HttpPatch("{courseId}")]
        public async Task<ActionResult<CourseDto>> PatchCourse(int courseId, JsonPatchDocument<CourseDto> patchDocument)
        {
            var course = await uow.CourseRepository.GetCourse(courseId);
            if (course == null)
            {
                return NotFound();
            }

            var courseDto = mapper.Map<CourseDto>(course);

            patchDocument.ApplyTo(courseDto, ModelState);
            if (!TryValidateModel(courseDto))
            {
                return BadRequest();
            }

            mapper.Map(courseDto, course);

            if (await uow.CourseRepository.SaveAsync())
            {
                return Ok(mapper.Map<CourseDto>(course));
            }
            else
            {
                return StatusCode(500);
            }
        }

        // DELETE: api/Courses/5
        [HttpDelete("{courseId}")]
        public async Task<IActionResult> DeleteCourse(int courseId)
        {
            var course = await uow.CourseRepository.GetCourse(courseId);
            if (course == null)
            {
                return NotFound();
            }

            uow.CourseRepository.Remove(course);
            await uow.CompleteAsync();

            return NoContent();
        }

    }
}

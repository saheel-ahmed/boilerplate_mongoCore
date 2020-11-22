using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDbDemo.Models;
using MongoDbDemo.Models.Services;

namespace MongoDbDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly StudentService _studentService;
        private readonly CourseService _courseService;

        public StudentsController(StudentService service, CourseService courseService)
        {
            _studentService = service;
            _courseService = courseService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetAll()
        {
            var students = await _studentService.GetAllAsync();
            return Ok(students);
        }

        [HttpGet("{id}", Name = "GetById")]
        public async Task<ActionResult<Student>> GetById(string id)
        {
            var student = await _studentService.GetByIdAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            if (student.Courses.Count > 0)
            {
                var tempList = new List<Course>();
                foreach (var courseId in student.Courses)
                {
                    var course = await _courseService.GetByIdAsync(courseId);
                    if (course != null)
                        tempList.Add(course);
                }
                student.CourseList = tempList;
            }
            return Ok(student);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Student student)
        {
            student.CreatedOn = DateTime.Now;
            student.UpdatedOn = DateTime.Now;
            await _studentService.CreateAsync(student);
            return Ok(student);
        }

        [HttpPut("{id}", Name = "Update")]
        public async Task<IActionResult> Update(string id, Student updatedStudent)
        {
            updatedStudent.UpdatedOn = DateTime.Now;
            var queriedStudent = await _studentService.GetByIdAsync(id);
            if (queriedStudent == null)
            {
                return NotFound();
            }
            updatedStudent.Id = queriedStudent.Id;
            await _studentService.UpdateAsync(id, updatedStudent);
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var student = await _studentService.GetByIdAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            await _studentService.DeleteAsync(id);
            return NoContent();
        }
    }
}
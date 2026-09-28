using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentRosterDbApi.Data;
using StudentRosterDbApi.Models;

namespace StudentRosterDbApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        // Inject the AppDbContext through the constructor
        public StudentsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/students
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetAllStudents()
        {
            return await _context.Students.ToListAsync(); // Returns 200 OK
        }

        // GET: api/students/course/BSCS
        [HttpGet("course/{courseName}")]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudentsByCourse(string courseName)
        {
            var filtered = await _context.Students
                .Where(s => s.Course.ToLower() == courseName.ToLower())
                .OrderBy(s => s.LastName)
                .ToListAsync();

            return filtered;
        }

        // GET: api/students/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudentById(int id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                return NotFound(new { message = $"Student with ID {id} not found." }); // Returns 404
            }

            return student;
        }

        // POST: api/students
        [HttpPost]
        public async Task<ActionResult<Student>> CreateStudent([FromBody] Student newStudent)
        {
            if (string.IsNullOrWhiteSpace(newStudent.FirstName) || string.IsNullOrWhiteSpace(newStudent.LastName))
            {
                return BadRequest(new { message = "First name and last name are required." }); // Returns 400
            }

            if (newStudent.YearLevel < 1 || newStudent.YearLevel > 4)
            {
                return BadRequest(new { message = "YearLevel must be between 1 and 4." });
            }

            // The database auto-generates the Id
            newStudent.Id = 0;
            _context.Students.Add(newStudent);
            await _context.SaveChangesAsync();

            // Returns 201 Created and includes the URI to the new resource
            return CreatedAtAction(nameof(GetStudentById), new { id = newStudent.Id }, newStudent);
        }

        // PUT: api/students/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] Student updatedStudent)
        {
            if (id != updatedStudent.Id)
            {
                return BadRequest(new { message = "ID mismatch." });
            }

            if (updatedStudent.YearLevel < 1 || updatedStudent.YearLevel > 4)
            {
                return BadRequest(new { message = "YearLevel must be between 1 and 4." });
            }

            var existingStudent = await _context.Students.FindAsync(id);
            if (existingStudent == null)
            {
                return NotFound();
            }

            existingStudent.FirstName = updatedStudent.FirstName;
            existingStudent.LastName = updatedStudent.LastName;
            existingStudent.Course = updatedStudent.Course;
            existingStudent.Email = updatedStudent.Email;
            existingStudent.YearLevel = updatedStudent.YearLevel;

            await _context.SaveChangesAsync();

            return NoContent(); // Returns 204 No Content
        }

        // DELETE: api/students/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

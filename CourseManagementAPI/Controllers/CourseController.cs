using CourseManagementAPI.DataAccessLayer;
using CourseManagementAPI.Entities;
using CourseManagementAPI.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace CourseManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly byte[] salt = Encoding.ASCII.GetBytes("opakjogpkjdopajgkoirkjatki");

        public CourseController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Course
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CoursesListModel>>> GetAllCourses()
        {
            if (_context.Courses == null)
            {
                return NotFound();
            }

            var result = await _context.Courses.Select(x => new CoursesListModel
            {
                Id = x.Id,
                CourseTitle = x.CourseTitle,
                CourseStartDateTime = x.CourseStartDateTime,
                CourseCapacity = $"{x.Attendees.Count()}/{x.MaxNumberOfAtendees}",
                CourseTeacher = x.CourseTeacher,
                IsCapacityFull = x.Attendees.Count() == x.MaxNumberOfAtendees
            }).ToListAsync();

            return result;
        }

        // GET: api/Course/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CourseDetailsModel>> GetCourseById(int id)
        {
            if (_context.Courses == null)
            {
                return NotFound();
            }

            var course = await _context.Courses.Include(x => x.Attendees).FirstOrDefaultAsync(x => x.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            var result = new CourseDetailsModel
            {
                Id = course.Id,
                CourseTitle = course.CourseTitle,
                CourseDescription = course.CourseDescription,
                CourseStartDateTime = course.CourseStartDateTime,
                CourseTeacher = course.CourseTeacher,
                CourseTeacherEmail = course.CourseTeacherEmail,
                MaxNumberOfAtendees = course.MaxNumberOfAtendees,
                EditDeleteCoursePIN = string.Empty,
                CourseAttendees = course.Attendees.Select(x => new AttendeeModel
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    CourseId = x.CourseId,
                }).ToList(),
            };

            return result;
        }

        // PUT: api/Course/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> EditCourse(int id, CourseNewEditModel course)
        {
            if (id != course.Id)
            {
                return BadRequest();
            }

            var courseToEdit = await _context.Courses.FindAsync(id);
            if (courseToEdit == null)
            {
                return NotFound();
            }

            if (courseToEdit.EditDeleteCoursePin != HashPin(course.EditDeleteCoursePIN))
            {
                return StatusCode(401);
            }

            courseToEdit.CourseTitle = course.CourseTitle;
            courseToEdit.CourseDescription = course.CourseDescription;
            courseToEdit.CourseStartDateTime = course.CourseStartDateTime;
            courseToEdit.CourseTeacher = course.CourseTeacher;
            courseToEdit.CourseTeacherEmail = course.CourseTeacherEmail;
            courseToEdit.MaxNumberOfAtendees = course.MaxNumberOfAtendees;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }

            return NoContent();
        }

        // POST: api/Course
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CourseNewEditModel>> CreateNewCourse(CourseNewEditModel course)
        {
            if (_context.Courses == null)
            {
                return NotFound();
            }

            var courseToAdd = new Course
            {
                CourseTitle = course.CourseTitle,
                CourseDescription = course.CourseDescription,
                CourseStartDateTime = course.CourseStartDateTime,
                CourseTeacher = course.CourseTeacher,
                CourseTeacherEmail = course.CourseTeacherEmail,
                MaxNumberOfAtendees = course.MaxNumberOfAtendees,
                EditDeleteCoursePin = HashPin(course.EditDeleteCoursePIN),
            };

            try
            {
                _context.Courses.Add(courseToAdd);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }


            return CreatedAtAction("CreateNewCourse", new { id = courseToAdd.Id }, courseToAdd);
        }

        // DELETE: api/Course/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            if (_context.Courses == null)
            {
                return NotFound();
            }
            var courseToDelete = await _context.Courses.FindAsync(id);
            if (courseToDelete == null)
            {
                return NotFound();
            }

            try
            {
                _context.Courses.Remove(courseToDelete);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }

            return NoContent();
        }

        private string HashPin(string pin)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: pin,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 100000,
                    numBytesRequested: 256 / 8));
        }
    }
}

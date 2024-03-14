using CourseManagementAPI.DataAccessLayer;
using CourseManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

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

        //// GET: api/Course/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<CourseNewEditModel>> GetCourseNewEditModel(int id)
        //{
        //  if (_context.CourseNewEditModel == null)
        //  {
        //      return NotFound();
        //  }
        //    var courseNewEditModel = await _context.CourseNewEditModel.FindAsync(id);

        //    if (courseNewEditModel == null)
        //    {
        //        return NotFound();
        //    }

        //    return courseNewEditModel;
        //}

        //// PUT: api/Course/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutCourseNewEditModel(int id, CourseNewEditModel courseNewEditModel)
        //{
        //    if (id != courseNewEditModel.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(courseNewEditModel).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!CourseNewEditModelExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/Course
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<CourseNewEditModel>> PostCourseNewEditModel(CourseNewEditModel courseNewEditModel)
        //{
        //  if (_context.CourseNewEditModel == null)
        //  {
        //      return Problem("Entity set 'ApplicationDbContext.CourseNewEditModel'  is null.");
        //  }
        //    _context.CourseNewEditModel.Add(courseNewEditModel);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetCourseNewEditModel", new { id = courseNewEditModel.Id }, courseNewEditModel);
        //}

        //// DELETE: api/Course/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteCourseNewEditModel(int id)
        //{
        //    if (_context.CourseNewEditModel == null)
        //    {
        //        return NotFound();
        //    }
        //    var courseNewEditModel = await _context.CourseNewEditModel.FindAsync(id);
        //    if (courseNewEditModel == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.CourseNewEditModel.Remove(courseNewEditModel);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool CourseNewEditModelExists(int id)
        //{
        //    return (_context.CourseNewEditModel?.Any(e => e.Id == id)).GetValueOrDefault();
        //}
    }
}

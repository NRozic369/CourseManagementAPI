using CourseManagementAPI.DataAccessLayer;
using CourseManagementAPI.Entities;
using CourseManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendeeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AttendeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Attendee
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AttendeeModel>>> GetAttendees()
        {
            if (_context.Attendees == null)
            {
                return NotFound();
            }

            var result = await _context.Attendees.Select(x => new AttendeeModel
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                CourseId = x.CourseId,
            }).ToListAsync();

            return result;
        }

        // GET: api/Attendee/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AttendeeModel>> GetAttendeeById(int id)
        {
            if (_context.Attendees == null)
            {
                return NotFound();
            }
            var attendee = await _context.Attendees.FindAsync(id);

            if (attendee == null)
            {
                return NotFound();
            }

            var result = new AttendeeModel
            {
                Id = attendee.Id,
                FirstName = attendee.FirstName,
                LastName = attendee.LastName,
                Email = attendee.Email,
                CourseId = attendee.CourseId,
            };

            return result;
        }

        // PUT: api/Attendee/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> EditAttendee(int id, AttendeeModel attendeeModel)
        {
            if (id != attendeeModel.Id)
            {
                return BadRequest();
            }

            var attendeeToEdit = await _context.Attendees.FindAsync(id);
            if (attendeeToEdit == null)
            {
                return NotFound();
            }

            attendeeToEdit.FirstName = attendeeModel.FirstName;
            attendeeToEdit.LastName = attendeeModel.LastName;
            attendeeToEdit.Email = attendeeModel.Email;

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

        // POST: api/Attendee
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AttendeeModel>> CreateNewAttendee(AttendeeModel attendeeModel)
        {
            if (_context.Attendees == null)
            {
                return NotFound();
            }

            var attendeeToAdd = new Attendee
            {
                FirstName = attendeeModel.FirstName,
                LastName = attendeeModel.LastName,
                Email = attendeeModel.Email,
                CourseId = attendeeModel.CourseId,
            };

            try
            {
                _context.Attendees.Add(attendeeToAdd);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }


            return CreatedAtAction("CreateNewAttendee", new { id = attendeeToAdd.Id }, attendeeToAdd);
        }

        // DELETE: api/Attendee/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAttendee(int id)
        {
            if (_context.Attendees == null)
            {
                return NotFound();
            }
            var attendeeToDelete = await _context.Attendees.FindAsync(id);
            if (attendeeToDelete == null)
            {
                return NotFound();
            }

            try
            {
                _context.Attendees.Remove(attendeeToDelete);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }

            return NoContent();
        }
    }
}

using API.Data;
using API.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
	[ApiController]
	[Route("api/registrations")]
	public class RegistrationsController : ControllerBase
	{
		// Database
        private readonly Context _context;

		// Constructor
        public RegistrationsController(Context context)
		{
			_context = context;
		}

		// Task returning registrations list
		[HttpGet]
		public async Task<IEnumerable<Registration>> GetAll()
		{
			return await _context.Registrations.ToListAsync();
		}

		// Task returning single registration
		[HttpGet("{id}")]
		public async Task<ActionResult<Registration>> Get(int id)
		{
			var registration = await _context.Registrations.FirstOrDefaultAsync(o => o.Id == id);
			if (registration == null) 
				return NotFound();
			return Ok(registration);
		}

		// Task inserting registration
		[HttpPost]
		public async Task<ActionResult<Registration>> Insert(Registration registration)
		{
			_context.Registrations.Add(registration);
			await _context.SaveChangesAsync();
			return Created($"/api/projects/{registration.Id}", registration);
		}

		// Task deleting registration
		[HttpDelete("{id}")]
		public async Task<ActionResult<Registration>> Delete(int id)
		{
			var existingRegistration = await _context.Registrations.FirstOrDefaultAsync(o => o.Id == id);
			if (existingRegistration == null) 
				return NotFound();
			_context.Registrations.Remove(existingRegistration);
			await _context.SaveChangesAsync();

			return NoContent();
		}
	}
}

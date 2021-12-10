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
	[Route("api/projects/{projectId}/students")]
	public class StudentsController : ControllerBase
	{
		// Database
        private readonly Context _context;

		// Constructor
        public StudentsController(Context context)
		{
			_context = context;
		}

		// Task returning students list
		[HttpGet]
		public async Task<IEnumerable<Student>> GetAll(int projectId)
		{
			return await _context.Students.Where(o => o.ProjectId == projectId).ToListAsync();
		}

		// Task returning single student
		[HttpGet("{id}")]
		public async Task<ActionResult<Student>> Get(int projectId, int id)
		{
			var student = await _context.Students.FirstOrDefaultAsync(o => o.Id == id && o.ProjectId == projectId);
			if (student == null) 
				return NotFound();
			return Ok(student);
		}

		// Task inserting student
		[HttpPost]
		public async Task<ActionResult<Student>> Insert(int projectId, StudentDto student)
		{
			if (student.CheckIfValid())
			{
				var project = await _context.Projects.FirstOrDefaultAsync(o => o.Id == projectId);
				if (project == null)
					return NotFound($"Couldn't find a project with id of {projectId}");

				var existingStudent = await _context.Students.FirstOrDefaultAsync(o => o.ProjectId == projectId &&
				o.FirstName == student.FirstName && o.LastName == student.LastName);
				if (existingStudent != null)
					return UnprocessableEntity();

				Student newStudent = student.MapFromDto(projectId);

				_context.Students.Add(newStudent);
				await _context.SaveChangesAsync();
				return Created($"/api/projects/{projectId}/groups/{newStudent.Id}", newStudent);
			}
			else
			{
				return UnprocessableEntity();
			}
		}

		// Task updating student
		[HttpPatch("{id}")]
		public async Task<ActionResult<Student>> Update(int projectId, int id, StudentDto student)
		{
			if (student.CheckIfValid())
			{
				var project = await _context.Projects.FirstOrDefaultAsync(o => o.Id == projectId);
				if (project == null)
					return NotFound($"Couldn't find a project with id of {projectId}");

				var existingStudent = await _context.Students.FirstOrDefaultAsync(o => o.Id == id && o.ProjectId == projectId);
				if (existingStudent == null)
					return NotFound();

				existingStudent.Update(student);
				_context.Students.Update(existingStudent); 
				await _context.SaveChangesAsync();
				return Ok(existingStudent);
			}

			return UnprocessableEntity();
		}

		// Task deleting student
		[HttpDelete("{id}")]
		public async Task<ActionResult<Student>> Delete(int projectId, int id)
		{
			var project = await _context.Projects.FirstOrDefaultAsync(o => o.Id == projectId);
			if (project == null)
				return NotFound($"Couldn't find a project with id of {projectId}");

			var existingStudent = await _context.Students.FirstOrDefaultAsync(o => o.Id == id && o.ProjectId == projectId);
			if (existingStudent == null)
				return NotFound();

			var existingRegistrations = await _context.Registrations.Where(o => o.StudentId == id).ToListAsync();
            foreach (var registration in existingRegistrations)
            {
				_context.Registrations.Remove(registration);
			}

			_context.Students.Remove(existingStudent);
			await _context.SaveChangesAsync();

			return NoContent();
		}
	}
}

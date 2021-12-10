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
	[Route("api/projects/{projectId}/groups")]
	public class GroupsController : ControllerBase
	{
		// Database
        private readonly Context _context;

		// Constructor
        public GroupsController(Context context)
		{
			_context = context;
		}

		// Task returning groups list
		[HttpGet]
		public async Task<IEnumerable<Group>> GetAll(int projectId)
		{
			return await _context.Groups.Where(o => o.ProjectId == projectId).ToListAsync();
		}

		// Task returning single group
		[HttpGet("{id}")]
		public async Task<ActionResult<Group>> Get(int projectId, int id)
		{
			var group = await _context.Groups.FirstOrDefaultAsync(o => o.Id == id && o.ProjectId == projectId);
			if (group == null) 
				return NotFound();
			return Ok(group);
		}

		// Task inserting group
		[HttpPost]
		public async Task<ActionResult<Group>> Insert(int projectId, GroupDto group)
		{
			if (group.CheckIfValid())
			{
				var project = await _context.Projects.FirstOrDefaultAsync(o => o.Id == projectId);
				if (project == null)
					return NotFound($"Couldn't find a project with id of {projectId}");

				Group newGroup = group.MapFromDto(projectId);

				_context.Groups.Add(newGroup);
				await _context.SaveChangesAsync();
				return Created($"/api/projects/{projectId}/groups/{newGroup.Id}", newGroup);
			}
			else
			{
				return UnprocessableEntity();
			}
		}

		// Task updating group
		[HttpPatch("{id}")]
		public async Task<ActionResult<Group>> Update(int projectId, int id, GroupDto group)
		{
			if (group.CheckIfValid())
			{
				var project = await _context.Projects.FirstOrDefaultAsync(o => o.Id == projectId);
				if (project == null)
					return NotFound($"Couldn't find a project with id of {projectId}");

				var existingGroup = await _context.Groups.FirstOrDefaultAsync(o => o.Id == id && o.ProjectId == projectId);
				if (existingGroup == null)
					return NotFound();

				existingGroup.Update(group);
				_context.Groups.Update(existingGroup); 
				await _context.SaveChangesAsync();
				return Ok(existingGroup);
			}

			return UnprocessableEntity();
		}

		// Task deleting group
		[HttpDelete("{id}")]
		public async Task<ActionResult<Group>> Delete(int projectId, int id)
		{
			var project = await _context.Projects.FirstOrDefaultAsync(o => o.Id == projectId);
			if (project == null)
				return NotFound($"Couldn't find a project with id of {projectId}");

			var existingGroup = await _context.Groups.FirstOrDefaultAsync(o => o.Id == id && o.ProjectId == projectId);
			if (existingGroup == null)
				return NotFound();

			var existingRegistrations = await _context.Registrations.Where(o => o.GroupId == id).ToListAsync();
			foreach (var registration in existingRegistrations)
			{
				_context.Registrations.Remove(registration);
			}

			_context.Groups.Remove(existingGroup);
			await _context.SaveChangesAsync();

			return NoContent();
		}
	}
}

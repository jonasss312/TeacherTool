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
	[Route("api/projects")]
	public class ProjectsController : ControllerBase
	{
		// Database
        private readonly Context _context;

		// Constructor
        public ProjectsController(Context context)
		{
			_context = context;
		}

		// Task returning projects list
		[HttpGet]
		public async Task<IEnumerable<Project>> GetAll()
		{
			return await _context.Projects.ToListAsync();
		}

		// Task returning single project
		[HttpGet("{id}")]
		public async Task<ActionResult<Project>> Get(int id)
		{
			var project = await _context.Projects.FirstOrDefaultAsync(o => o.Id == id);
			if (project == null) 
				return NotFound();
			return Ok(project);
		}

		// Task inserting project
		[HttpPost]
		public async Task<ActionResult<Project>> Insert(Project project)
		{
			if (project.CheckIfValid())
			{
				_context.Projects.Add(project);
				await _context.SaveChangesAsync();

				for (int i = 1; i <= project.GroupNumber; i++)
                {
					Group group = new Group();
					group.Insert("Group #" + i.ToString(), project.MaxGroupStudents, project.Id);
					_context.Groups.Add(group);
					await _context.SaveChangesAsync();
				}

				await _context.SaveChangesAsync();
				return Created($"/api/projects/{project.Id}", project);
			}
			else
			{
				return UnprocessableEntity();
			}
		}

		// Task updating project
		[HttpPatch("{id}")]
		public async Task<ActionResult<Project>> Update(int id, Project project)
		{
			var existingProject = await _context.Projects.FirstOrDefaultAsync(o => o.Id == id);
			if (existingProject == null) 
				return NotFound();


			if (existingProject.CheckIfValid())
			{
				existingProject.Update(project);
				_context.Projects.Update(existingProject); 
				await _context.SaveChangesAsync();
				return Ok(project);
			}

			return UnprocessableEntity();
		}

		// Task deleting project
		[HttpDelete("{id}")]
		public async Task<ActionResult<Project>> Delete(int id)
		{
			var existingProject = await _context.Projects.FirstOrDefaultAsync(o => o.Id == id);
			if (existingProject == null) 
				return NotFound();

			var groups = await _context.Groups.Where(o => o.ProjectId == id).ToArrayAsync();

            foreach (var group in groups)
			{
				_context.Groups.Remove(group);
			}
			_context.Projects.Remove(existingProject);
			await _context.SaveChangesAsync();

			return NoContent();
		}
	}
}

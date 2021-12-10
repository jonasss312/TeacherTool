using API.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class Context : DbContext
	{
		public DbSet<Project> Projects { get; set; }
		public DbSet<Group> Groups { get; set; }
		public DbSet<Student> Students { get; set; }
		public DbSet<Registration> Registrations { get; set; }
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog=NFQ_TeacherAppJonas");
			//optionsBuilder.UseSqlServer("Data Source =tcp:coursesrestdbserver.database.windows.net,1433;Initial Catalog = CoursesREST_db; User Id = coursesadmin@coursesrestdbserver; Password = Coursesfornoobs123");
		}
	}
}

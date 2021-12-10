using System.ComponentModel.DataAnnotations;

namespace API.Data.Entities
{
    public class Project
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public int GroupNumber { get; set; }
        [Required]
        public int MaxGroupStudents { get; set; }

        public bool CheckIfValid()
        {
            if (GroupNumber > 0 && MaxGroupStudents > 0 &&
                Title != null && Title != "")
                return true;
            return false;
        }

        public void Update(Project project)
        {
            if (project != null)
            {
                this.Title = project.Title;
                this.GroupNumber = project.GroupNumber;
                this.MaxGroupStudents = project.MaxGroupStudents;
            }
        }
    }
}

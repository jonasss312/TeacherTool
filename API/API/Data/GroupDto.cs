using API.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace API.Data
{
    public class GroupDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int MaxGroupStudents { get; set; }

        public bool CheckIfValid()
        {
            if (MaxGroupStudents > 0 &&
                Name != null && Name != "")
                return true;
            return false;
        }

        public Group MapFromDto(int projectId)
        {
            Group group = new Group();
            group.Name = this.Name;
            group.MaxGroupStudents = this.MaxGroupStudents;
            group.ProjectId = projectId;
            return group;
        }
    }
}

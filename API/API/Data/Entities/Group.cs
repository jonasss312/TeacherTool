using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Data.Entities
{
    public class Group
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int MaxGroupStudents { get; set; }

        [Required]
        public int ProjectId { get; set; }

        public void Update(GroupDto dto)
        {
            this.Name= dto.Name;
            this.MaxGroupStudents = dto.MaxGroupStudents;
        }

        public void Insert(string Name, int MaxGroupStudents, int ProjectId)
        {
            this.Name = Name;
            this.MaxGroupStudents = MaxGroupStudents;
            this.ProjectId = ProjectId;
        }
    }
}

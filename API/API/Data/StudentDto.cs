using API.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace API.Data
{
    public class StudentDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        public bool CheckIfValid()
        {
            if (FirstName != null && FirstName != "" &&
                LastName != null && LastName != "")
                return true;
            return false;
        }

        public Student MapFromDto(int projectId)
        {
            Student student = new Student();
            student.FirstName = this.FirstName;
            student.LastName = this.LastName;
            student.ProjectId = projectId;
            return student;
        }
    }
}

using System.ComponentModel.DataAnnotations;

namespace API.Data.Entities
{
    public class Student
    {
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public int ProjectId { get; set; }

        public void Update(StudentDto dto)
        {
            this.FirstName = dto.FirstName;
            this.LastName = dto.LastName;
        }
    }
}

using System.ComponentModel.DataAnnotations;

namespace API.Data.Entities
{
    public class Registration
    {
        public int Id { get; set; }
        [Required]
        public int GroupId { get; set; }
        [Required]
        public int StudentId { get; set; }

    }
}

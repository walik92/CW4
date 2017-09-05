using System.ComponentModel.DataAnnotations;

namespace CW4.DatabaseObjects
{
    public class Note
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(5000)]
        public string Value { get; set; }

        public int UserId { get; set; }

        [Required]
        public virtual User User { get; set; }
    }
}
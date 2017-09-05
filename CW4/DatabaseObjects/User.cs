using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CW4.DatabaseObjects
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(30)]
        [Required]
        public string Login { get; set; }

        [MaxLength(30)]
        [Required]
        public string Password { get; set; }

        public virtual ICollection<Note> Notes { get; set; }
    }
}
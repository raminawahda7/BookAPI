using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookAPI.Data  
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="It's not allowed to be null")]
        [MaxLength(50)]
        public string Title { get; set; }

        // deleted to be in seaparation Table.

        [MaxLength(150)]
        public string Description { get; set; }

        public bool IsAvailable { get; set; }

        // Navigation properties - > add foreign key
        public int? PublisherId { get; set; }
        public Publisher Publisher { get; set; }

        public ICollection<Author> Authors { get; set; }
    }
}

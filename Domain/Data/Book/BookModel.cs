using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookAPI.Data
{
    public class BookModel
    {
        [Required(ErrorMessage = "The field {0} is required")]
        [StringLength(150, ErrorMessage = "The field {0} must be between {2} and {1} characters", MinimumLength = 2)]
        public string Title { get; set; }

        public string Description { get; set; }
        public bool IsAvailable { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        public int PublisherId { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime PublishedDate { get; set; }
        public List<int> AuthorIds { get; set; }

    }

}

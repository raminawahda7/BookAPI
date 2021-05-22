using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookAPI.Data
{
    public class Book_Author
    {
        public int Id { get; set; }

        // Navigations Properties
        public int BookId { get; set; }
        public Book Book { get; set; }

        public int AutorId { get; set; }
        public Author Author { get; set; }
    }
}

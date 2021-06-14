using BookAPI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookAPI.Data
{
    public class AuthorResource
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int? Age { get; set; }
        public string? Email { get; set; }
        public List<BookAuthorResource> Books { get; set; }
    }
}
// IF YOU WANT TO RETURN BOOKS FOR SPECIFIC AUTHOR ----> YOU HAVE TO CREATE A NEW RESOURCE:
//                                                       CONTAINS FULLNAME AND BOOK TITLES
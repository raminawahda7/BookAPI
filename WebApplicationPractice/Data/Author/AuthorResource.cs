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
        public List<string> BookTitles { get; set; }
    }
}
// IF YOU WANT TO RETURN BOOKS FOR SPECIFIC AUTHOR ----> YOU HAVE TO CREATE A NEW RESOURCE:
//                                                       CONTAINS FULLNAME AND BOOK TITLES
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookAPI.Data
{
    public class PublisherResource
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<BookResource> Books { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookAPI.Data
{
    public class PublisherBookResource
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsAvailable { get; set; }

        public List<AuthorCreateResource> AuthorNames { get; set; }

    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookAPI.Data
{
    public class BookModel
    {
        public string Title { get; set; }

        public string Description { get; set; }
        public bool IsAvailable { get; set; }

        public int PublisherId { get; set; }
        public List<int> AuthorIds { get; set; }

    }

}

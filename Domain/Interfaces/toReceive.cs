using BookAPI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookAPI.Repositories.Interfaces
{
    public class toReceive
    {
        public int Id { get; set; }
        public string entityType { get; set; }
        public string ProcessType { get; set; }
    }
}

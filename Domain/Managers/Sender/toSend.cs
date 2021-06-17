using BookAPI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Managers.Sender
{
    public class toSend
    {
        public int Id { get; set; }
        public string entityType { get; set; }
        public string ProcessType { get; set; }
    }
}

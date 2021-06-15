using BookAPI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Managers.Sender
{
    public interface IAuthorUpdateSender
    {
        void SendAuthor(Author author);
    }
}

using BookAPI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Consumer
{
    public interface ICreateAuthorService
    {
        void CreateAuthor(Author author);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryProject.Data.Models;

namespace LibraryProject.Data.Interfaces
{
    public interface IBookRepository : IRepository<Book>
    {
        Task<IEnumerable<Book>> GetAllWithDetailsAsync();
        Task<Book?> GetByIdWithDetailsAsync(int id);
    }
}

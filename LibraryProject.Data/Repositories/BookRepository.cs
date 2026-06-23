using LibraryProject.Data.Interfaces;
using LibraryProject.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Data.Repositories
{
    public class BookRepository : BaseRepository<Book>, IBookRepository
    {
        public BookRepository(LibraryDbContext context) : base(context) { }

        public async Task<IEnumerable<Book>> GetAllWithDetailsAsync()
            => await _dbSet
                .Include(b => b.Author)
                .Include(b => b.Category)
                .ToListAsync();

        public async Task<Book?> GetByIdWithDetailsAsync(int id)
            => await _dbSet
                .Include(b => b.Author)
                .Include(b => b.Category)
                .FirstOrDefaultAsync(b => b.Id == id);
    }
}

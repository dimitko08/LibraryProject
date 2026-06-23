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
    public class AuthorRepository : BaseRepository<Author>, IAuthorRepository
    {
        public AuthorRepository(LibraryDbContext context) : base(context) { }

        public async Task<IEnumerable<Author>> GetAllWithBooksAsync()
            => await _dbSet
                .Include(a => a.Books)
                .ToListAsync();
    }
}

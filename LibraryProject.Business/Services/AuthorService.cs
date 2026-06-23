using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryProject.Business.Interfaces;
using LibraryProject.Data.Interfaces;
using LibraryProject.Data.Models;

namespace LibraryProject.Business.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;

        public AuthorService(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public async Task<IEnumerable<Author>> GetAllAuthorsAsync()
            => await _authorRepository.GetAllWithBooksAsync();

        public async Task<Author?> GetAuthorByIdAsync(int id)
            => await _authorRepository.GetByIdAsync(id);

        public async Task CreateAuthorAsync(Author author)
        {
            if (string.IsNullOrWhiteSpace(author.FirstName))
                throw new ArgumentException("Името на автора е задължително.");

            if (string.IsNullOrWhiteSpace(author.LastName))
                throw new ArgumentException("Фамилията на автора е задължителна.");

            if (string.IsNullOrWhiteSpace(author.Nationality))
                throw new ArgumentException("Националността е задължителна.");

            if (author.BirthYear < 1 || author.BirthYear > DateTime.Now.Year)
                throw new ArgumentException($"Годината на раждане трябва да е между 1 и {DateTime.Now.Year}.");

            await _authorRepository.AddAsync(author);
        }

        public async Task UpdateAuthorAsync(Author author)
        {
            if (!await _authorRepository.ExistsAsync(author.Id))
                throw new InvalidOperationException($"Автор с ID {author.Id} не съществува.");

            await _authorRepository.UpdateAsync(author);
        }

        public async Task DeleteAuthorAsync(int id)
        {
            if (!await _authorRepository.ExistsAsync(id))
                throw new InvalidOperationException($"Автор с ID {id} не съществува.");

            await _authorRepository.DeleteAsync(id);
        }

        public async Task<bool> AuthorExistsAsync(int id)
            => await _authorRepository.ExistsAsync(id);
    }
}
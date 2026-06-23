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
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
            => await _bookRepository.GetAllWithDetailsAsync();

        public async Task<Book?> GetBookByIdAsync(int id)
            => await _bookRepository.GetByIdWithDetailsAsync(id);

        public async Task CreateBookAsync(Book book)
        {
            if (string.IsNullOrWhiteSpace(book.Title))
                throw new ArgumentException("Заглавието на книгата е задължително.");

            if (string.IsNullOrWhiteSpace(book.ISBN))
                throw new ArgumentException("ISBN е задължително.");

            if (book.PublishYear < 1 || book.PublishYear > DateTime.Now.Year)
                throw new ArgumentException($"Годината на издаване трябва да е между 1 и {DateTime.Now.Year}.");

            if (book.AvailableCopies < 0)
                throw new ArgumentException("Броят на наличните копия не може да е отрицателен.");

            await _bookRepository.AddAsync(book);
        }

        public async Task UpdateBookAsync(Book book)
        {
            if (!await _bookRepository.ExistsAsync(book.Id))
                throw new InvalidOperationException($"Книга с ID {book.Id} не съществува.");

            await _bookRepository.UpdateAsync(book);
        }

        public async Task DeleteBookAsync(int id)
        {
            if (!await _bookRepository.ExistsAsync(id))
                throw new InvalidOperationException($"Книга с ID {id} не съществува.");

            await _bookRepository.DeleteAsync(id);
        }

        public async Task<bool> BookExistsAsync(int id)
            => await _bookRepository.ExistsAsync(id);
    }
}

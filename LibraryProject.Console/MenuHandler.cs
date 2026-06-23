using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryProject.Business.Interfaces;
using LibraryProject.Data.Models;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryProject.Console
{
    public class MenuHandler
    {
        private readonly IServiceProvider _serviceProvider;

        public MenuHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task RunAsync()
        {
            System.Console.OutputEncoding = System.Text.Encoding.UTF8;

            bool running = true;
            while (running)
            {
                PrintMenu();
                var choice = System.Console.ReadLine()?.Trim();

                using var scope = _serviceProvider.CreateScope();
                var bookService = scope.ServiceProvider.GetRequiredService<IBookService>();
                var authorService = scope.ServiceProvider.GetRequiredService<IAuthorService>();

                try
                {
                    switch (choice)
                    {
                        case "1": await ShowAllBooksAsync(bookService); break;
                        case "2": await AddBookAsync(bookService); break;
                        case "3": await EditBookAsync(bookService); break;
                        case "4": await DeleteBookAsync(bookService); break;
                        case "5": await ShowAllAuthorsAsync(authorService); break;
                        case "6": await AddAuthorAsync(authorService); break;
                        case "7": await EditAuthorAsync(authorService); break;
                        case "8": await DeleteAuthorAsync(authorService); break;
                        case "9": running = false; PrintLine("Довиждане!", ConsoleColor.Cyan); break;
                        default: PrintLine("Невалиден избор. Опитайте отново.", ConsoleColor.Yellow); break;
                    }
                }
                catch (Exception ex)
                {
                    PrintLine($"Грешка: {ex.Message}", ConsoleColor.Red);
                }

                if (running) { System.Console.WriteLine("\nНатиснете Enter за продължение..."); System.Console.ReadLine(); }
            }
        }


        private static void PrintMenu()
        {
            System.Console.Clear();
            PrintLine("╔══════════════════════════════════════╗", ConsoleColor.Cyan);
            PrintLine("║        БИБЛИОТЕКА С КНИГИ            ║", ConsoleColor.Cyan);
            PrintLine("╠══════════════════════════════════════╣", ConsoleColor.Cyan);
            PrintLine("║  1. Покажи всички книги              ║", ConsoleColor.White);
            PrintLine("║  2. Добави книга                     ║", ConsoleColor.White);
            PrintLine("║  3. Редактирай книга                 ║", ConsoleColor.White);
            PrintLine("║  4. Изтрий книга                     ║", ConsoleColor.White);
            PrintLine("╠══════════════════════════════════════╣", ConsoleColor.Cyan);
            PrintLine("║  5. Покажи всички автори             ║", ConsoleColor.White);
            PrintLine("║  6. Добави автор                     ║", ConsoleColor.White);
            PrintLine("║  7. Редактирай автор                 ║", ConsoleColor.White);
            PrintLine("║  8. Изтрий автор                     ║", ConsoleColor.White);
            PrintLine("╠══════════════════════════════════════╣", ConsoleColor.Cyan);
            PrintLine("║  9. Изход                            ║", ConsoleColor.White);
            PrintLine("╚══════════════════════════════════════╝", ConsoleColor.Cyan);
            System.Console.Write("Вашият избор: ");
        }


        private static async Task ShowAllBooksAsync(IBookService bookService)
        {
            var books = await bookService.GetAllBooksAsync();
            var bookList = books.ToList();

            PrintLine($"\n═══ ВСИЧКИ КНИГИ ({bookList.Count}) ═══", ConsoleColor.Cyan);

            if (!bookList.Any())
            {
                PrintLine("Няма намерени книги.", ConsoleColor.Yellow);
                return;
            }

            foreach (var book in bookList)
            {
                PrintLine($"\n  ID: {book.Id}", ConsoleColor.Green);
                System.Console.WriteLine($"  Заглавие    : {book.Title}");
                System.Console.WriteLine($"  Автор       : {book.Author.FirstName} {book.Author.LastName}");
                System.Console.WriteLine($"  Категория   : {book.Category.Name}");
                System.Console.WriteLine($"  ISBN        : {book.ISBN}");
                System.Console.WriteLine($"  Година      : {book.PublishYear}");
                System.Console.WriteLine($"  Налични бр. : {book.AvailableCopies}");
                PrintLine("  ─────────────────────────────────", ConsoleColor.DarkGray);
            }
        }

        private static async Task AddBookAsync(IBookService bookService)
        {
            PrintLine("\n═══ ДОБАВИ КНИГА ═══", ConsoleColor.Green);

            var book = new Book
            {
                Title = ReadRequired("Заглавие"),
                ISBN = ReadRequired("ISBN"),
                PublishYear = ReadInt("Година на издаване"),
                AvailableCopies = ReadInt("Брой налични копия"),
                AuthorId = ReadInt("ID на автор"),
                CategoryId = ReadInt("ID на категория")
            };

            await bookService.CreateBookAsync(book);
            PrintLine("Книгата е добавена успешно!", ConsoleColor.Green);
        }

        private static async Task EditBookAsync(IBookService bookService)
        {
            PrintLine("\n═══ РЕДАКТИРАЙ КНИГА ═══", ConsoleColor.Yellow);

            int id = ReadInt("Въведете ID на книгата");
            var book = await bookService.GetBookByIdAsync(id);

            if (book == null) { PrintLine("Книгата не е намерена.", ConsoleColor.Red); return; }

            System.Console.WriteLine($"Текущо заглавие: {book.Title}");
            book.Title = ReadRequired("Ново заглавие");
            book.ISBN = ReadRequired("Нов ISBN");
            book.PublishYear = ReadInt("Нова година на издаване");
            book.AvailableCopies = ReadInt("Нов брой налични копия");
            book.AuthorId = ReadInt("Ново ID на автор");
            book.CategoryId = ReadInt("Ново ID на категория");

            await bookService.UpdateBookAsync(book);
            PrintLine("Книгата е обновена успешно!", ConsoleColor.Green);
        }

        private static async Task DeleteBookAsync(IBookService bookService)
        {
            PrintLine("\n═══ ИЗТРИЙ КНИГА ═══", ConsoleColor.Red);

            int id = ReadInt("Въведете ID на книгата за изтриване");
            System.Console.Write("Сигурни ли сте? (д/н): ");
            if (System.Console.ReadLine()?.Trim().ToLower() != "д") { PrintLine("Отказано.", ConsoleColor.Yellow); return; }

            await bookService.DeleteBookAsync(id);
            PrintLine("Книгата е изтрита успешно!", ConsoleColor.Green);
        }


        private static async Task ShowAllAuthorsAsync(IAuthorService authorService)
        {
            var authors = await authorService.GetAllAuthorsAsync();
            var authorList = authors.ToList();

            PrintLine($"\n═══ ВСИЧКИ АВТОРИ ({authorList.Count}) ═══", ConsoleColor.Cyan);

            if (!authorList.Any())
            {
                PrintLine("Няма намерени автори.", ConsoleColor.Yellow);
                return;
            }

            foreach (var author in authorList)
            {
                PrintLine($"\n  ID: {author.Id}", ConsoleColor.Green);
                System.Console.WriteLine($"  Име           : {author.FirstName} {author.LastName}");
                System.Console.WriteLine($"  Националност  : {author.Nationality}");
                System.Console.WriteLine($"  Роден         : {author.BirthYear} г.");
                System.Console.WriteLine($"  Брой книги    : {author.Books.Count}");
                PrintLine("  ─────────────────────────────────────", ConsoleColor.DarkGray);
            }
        }

        private static async Task AddAuthorAsync(IAuthorService authorService)
        {
            PrintLine("\n═══ ДОБАВИ АВТОР ═══", ConsoleColor.Green);

            var author = new Author
            {
                FirstName = ReadRequired("Собствено име"),
                LastName = ReadRequired("Фамилия"),
                Nationality = ReadRequired("Националност"),
                BirthYear = ReadInt("Година на раждане")
            };

            await authorService.CreateAuthorAsync(author);
            PrintLine("Авторът е добавен успешно!", ConsoleColor.Green);
        }

        private static async Task EditAuthorAsync(IAuthorService authorService)
        {
            PrintLine("\n═══ РЕДАКТИРАЙ АВТОР ═══", ConsoleColor.Yellow);

            int id = ReadInt("Въведете ID на автора");
            var author = await authorService.GetAuthorByIdAsync(id);

            if (author == null) { PrintLine("Авторът не е намерен.", ConsoleColor.Red); return; }

            System.Console.WriteLine($"Текущо ime: {author.FirstName} {author.LastName}");
            author.FirstName = ReadRequired("Ново собствено ime");
            author.LastName = ReadRequired("Нова фамилия");
            author.Nationality = ReadRequired("Нова националност");
            author.BirthYear = ReadInt("Нова година на раждане");

            await authorService.UpdateAuthorAsync(author);
            PrintLine("Авторът е обновен успешно!", ConsoleColor.Green);
        }

        private static async Task DeleteAuthorAsync(IAuthorService authorService)
        {
            PrintLine("\n═══ ИЗТРИЙ АВТОР ═══", ConsoleColor.Red);

            int id = ReadInt("Въведете ID на автора за изтриване");
            System.Console.Write("Сигурни ли сте? (д/н): ");
            if (System.Console.ReadLine()?.Trim().ToLower() != "д") { PrintLine("Отказано.", ConsoleColor.Yellow); return; }

            await authorService.DeleteAuthorAsync(id);
            PrintLine("Авторът е изтрит успешно!", ConsoleColor.Green);
        }


        private static string ReadRequired(string prompt)
        {
            string? value;
            do
            {
                System.Console.Write($"  {prompt}: ");
                value = System.Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(value))
                    PrintLine("  Полето е задължително!", ConsoleColor.Red);
            } while (string.IsNullOrEmpty(value));
            return value;
        }

        private static int ReadInt(string prompt)
        {
            int result;
            while (true)
            {
                System.Console.Write($"  {prompt}: ");
                if (int.TryParse(System.Console.ReadLine(), out result))
                    return result;
                PrintLine("  Моля въведете валидно число!", ConsoleColor.Red);
            }
        }

        private static void PrintLine(string text, ConsoleColor color)
        {
            System.Console.ForegroundColor = color;
            System.Console.WriteLine(text);
            System.Console.ResetColor();
        }
    }
}
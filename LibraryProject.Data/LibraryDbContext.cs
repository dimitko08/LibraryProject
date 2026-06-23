using LibraryProject.Data.Models;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;

namespace LibraryProject.Data
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options) { }

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    Id = 1,
                    Name = "Българска литература",
                    Description = "Класически и съвременни произведения на български автори",
                    FloorNumber = 1,
                    CreatedDate = new DateTime(2020, 1, 1)
                },
                new Category
                {
                    Id = 2,
                    Name = "Фантастика",
                    Description = "Научна фантастика, фентъзи и алтернативни светове",
                    FloorNumber = 2,
                    CreatedDate = new DateTime(2020, 1, 1)
                },
                new Category
                {
                    Id = 3,
                    Name = "Класическа литература",
                    Description = "Световна класика и признати шедьоври",
                    FloorNumber = 3,
                    CreatedDate = new DateTime(2020, 1, 1)
                },
                new Category
                {
                    Id = 4,
                    Name = "Дистопия",
                    Description = "Книги за общества в упадък и тоталитарни режими",
                    FloorNumber = 2,
                    CreatedDate = new DateTime(2021, 3, 15)
                },
                new Category
                {
                    Id = 5,
                    Name = "Приключения",
                    Description = "Вълнуващи приключенски романи и истории",
                    FloorNumber = 1,
                    CreatedDate = new DateTime(2021, 6, 10)
                }
            );

            modelBuilder.Entity<Author>().HasData(
                new Author
                {
                    Id = 1,
                    FirstName = "Иван",
                    LastName = "Вазов",
                    Nationality = "Българска",
                    BirthYear = 1850
                },
                new Author
                {
                    Id = 2,
                    FirstName = "Алеко",
                    LastName = "Константинов",
                    Nationality = "Българска",
                    BirthYear = 1863
                },
                new Author
                {
                    Id = 3,
                    FirstName = "Джоан",
                    LastName = "Роулинг",
                    Nationality = "Британска",
                    BirthYear = 1965
                },
                new Author
                {
                    Id = 4,
                    FirstName = "Джон Роналд Руел",
                    LastName = "Толкин",
                    Nationality = "Британска",
                    BirthYear = 1892
                },
                new Author
                {
                    Id = 5,
                    FirstName = "Джордж",
                    LastName = "Оруел",
                    Nationality = "Британска",
                    BirthYear = 1903
                }
            );

            modelBuilder.Entity<Book>().HasData(
                new Book
                {
                    Id = 1,
                    Title = "Под игото",
                    ISBN = "978-954-01-0001-1",
                    PublishYear = 1894,
                    AvailableCopies = 5,
                    AuthorId = 1,
                    CategoryId = 1
                },
                new Book
                {
                    Id = 2,
                    Title = "Бай Ганьо",
                    ISBN = "978-954-01-0002-2",
                    PublishYear = 1895,
                    AvailableCopies = 3,
                    AuthorId = 2,
                    CategoryId = 1
                },
                new Book
                {
                    Id = 3,
                    Title = "Хари Потър и философският камък",
                    ISBN = "978-954-01-0003-3",
                    PublishYear = 1997,
                    AvailableCopies = 8,
                    AuthorId = 3,
                    CategoryId = 2
                },
                new Book
                {
                    Id = 4,
                    Title = "Властелинът на пръстените",
                    ISBN = "978-954-01-0004-4",
                    PublishYear = 1954,
                    AvailableCopies = 4,
                    AuthorId = 4,
                    CategoryId = 2
                },
                new Book
                {
                    Id = 5,
                    Title = "1984",
                    ISBN = "978-954-01-0005-5",
                    PublishYear = 1949,
                    AvailableCopies = 6,
                    AuthorId = 5,
                    CategoryId = 4
                }
            );
        }
    }
}
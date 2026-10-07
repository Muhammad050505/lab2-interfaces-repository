using System;

namespace Lab2.Part2
{
    public static class Part2Demo
    {
        public static void Run()
        {
            Console.WriteLine("\n=== ЧАСТЬ 2. Репозиторий ===");
            var uow = new UnitOfWork();

            var author = new Author { Name = "Лев Толстой" };
            uow.Authors.Add(author);

            uow.Books.Add(new Book { Title = "Война и мир", Year = 1869, AuthorId = author.Id });
            uow.Books.Add(new Book { Title = "Анна Каренина", Year = 1877, AuthorId = author.Id });

            Console.WriteLine($"Автор #{author.Id}: {author.Name}");
            foreach (var b in uow.Books.GetByAuthor(author.Id))
                Console.WriteLine($"  - {b.Title} ({b.Year})");
        }
    }
}

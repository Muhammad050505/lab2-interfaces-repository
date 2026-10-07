using System.Collections.Generic;

namespace Lab2.Part2
{
    // Сущности
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<Book> Books { get; set; } = new();
    }

    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int Year { get; set; }
        public int AuthorId { get; set; }
        public Author? Author { get; set; }
    }
}

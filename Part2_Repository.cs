using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab2.Part2
{
    // В этой реализации мы НЕ используем EF Core напрямую,
    // чтобы файл компилировался без внешних пакетов.
    // В реальном проекте здесь был бы DbContext.

    public class InMemoryBookRepository : IBookRepository
    {
        private readonly List<Book> _books = new();
        private int _nextId = 1;

        public Book? GetById(int id) => _books.FirstOrDefault(b => b.Id == id);
        public IEnumerable<Book> GetAll() => _books;
        public IEnumerable<Book> GetByAuthor(int authorId) =>
            _books.Where(b => b.AuthorId == authorId);

        public void Add(Book entity)
        {
            entity.Id = _nextId++;
            _books.Add(entity);
        }

        public void Update(Book entity)
        {
            var idx = _books.FindIndex(b => b.Id == entity.Id);
            if (idx >= 0) _books[idx] = entity;
        }

        public void Delete(int id) => _books.RemoveAll(b => b.Id == id);

        public Task<Book?> GetByIdAsync(int id) => Task.FromResult(GetById(id));
        public Task<IEnumerable<Book>> GetAllAsync() => Task.FromResult(GetAll());
        public Task AddAsync(Book entity) { Add(entity); return Task.CompletedTask; }
    }

    public class InMemoryAuthorRepository : IAuthorRepository
    {
        private readonly List<Author> _authors = new();
        private int _nextId = 1;

        public Author? GetById(int id) => _authors.FirstOrDefault(a => a.Id == id);
        public IEnumerable<Author> GetAll() => _authors;
        public Author? GetWithBooks(int authorId)
        {
            var a = GetById(authorId);
            if (a != null) a.Books = _authors.Where(x => x.Id == authorId)
                .SelectMany(x => x.Books).ToList();
            return a;
        }
        public void Add(Author entity) { entity.Id = _nextId++; _authors.Add(entity); }
        public void Update(Author entity)
        {
            var idx = _authors.FindIndex(a => a.Id == entity.Id);
            if (idx >= 0) _authors[idx] = entity;
        }
        public void Delete(int id) => _authors.RemoveAll(a => a.Id == id);

        public Task<Author?> GetByIdAsync(int id) => Task.FromResult(GetById(id));
        public Task<IEnumerable<Author>> GetAllAsync() => Task.FromResult(GetAll());
        public Task AddAsync(Author entity) { Add(entity); return Task.CompletedTask; }
    }

    public class UnitOfWork : IUnitOfWork
    {
        public IBookRepository Books { get; }
        public IAuthorRepository Authors { get; }

        public UnitOfWork()
        {
            Books = new InMemoryBookRepository();
            Authors = new InMemoryAuthorRepository();
        }

        public Task<int> SaveChangesAsync() => Task.FromResult(0);
    }
}

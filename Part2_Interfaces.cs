using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lab2.Part2
{
    // Обобщённый интерфейс репозитория
    public interface IRepository<T>
    {
        T? GetById(int id);
        IEnumerable<T> GetAll();
        void Add(T entity);
        void Update(T entity);
        void Delete(int id);

        // Асинхронные версии (повышенный уровень)
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
    }

    // Специализированные интерфейсы
    public interface IBookRepository : IRepository<Book>
    {
        IEnumerable<Book> GetByAuthor(int authorId);
    }

    public interface IAuthorRepository : IRepository<Author>
    {
        Author? GetWithBooks(int authorId);
    }

    // Unit of Work
    public interface IUnitOfWork
    {
        IBookRepository Books { get; }
        IAuthorRepository Authors { get; }
        Task<int> SaveChangesAsync();
    }
}

using Services.Contracts;
using Entities.Models;
using Repositories.Contracts;
using Microsoft.Extensions.Logging;
using Entities.Exceptions;


namespace Services
{
    public class BookManager : IBookService
    {
        private readonly IRepositoryManager _manager;
        private readonly ILoggerService _logger;
        public BookManager(IRepositoryManager manager , 
            ILoggerService logger)
        {
            _manager = manager;
            _logger = logger;
        }

        public Book CreateOneBook(Book book)
        {
            _manager.Book.CreateOneBook(book);
            _manager.Save();
            return book;
        }

        public void DeleteOneBook(int id, bool trackChanges)
        {

            var entity = _manager.Book.GetOneBookById(id, trackChanges);
            if (entity is null)
            {
                throw new BookNotFoundException(id);
            }
            _manager.Book.DeleteOneBook(entity);
            _manager.Save();
        }

        public IEnumerable<Book> GetAllBooks(bool trackChanges)
        {
            return _manager.Book.GetAllBooks(trackChanges);
        }

        public Book GetOneBookById(int id, bool trackChanges)
        {
            var book = _manager.Book.GetOneBookById(id,trackChanges);
            if(book is null)
                throw new BookNotFoundException(id);
                return book;
        }

        public void UpdateOneBook(int id, Book book , bool trackChanges)
        {
            // Check entity
            var entitiy = _manager.Book.GetOneBookById(id,trackChanges);
            if (entitiy is null)
                throw new BookNotFoundException(id);
            
            entitiy.Title = book.Title;
            entitiy.Price = book.Price;

            _manager.Book.Update(entitiy);
            _manager.Save();
        }
    }
}
using Services.Contracts;
using Entities.Models;
using Repositories.Contracts;
using Microsoft.Extensions.Logging;


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
                var message = $"The book with id:{id} could not found.";
                _logger.LogInfo(message);
                throw new Exception($"Book with id {id} could not found.");
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
            return _manager.Book.GetOneBookById(id,trackChanges);
        }

        public void UpdateOneBook(int id, Book book , bool trackChanges)
        {
            // Check entity
            var entitiy = _manager.Book.GetOneBookById(id,trackChanges);
            if (entitiy is null)
            {
                string msg = $"Book with id:{id} could not found.";
                _logger.LogInfo(msg);
                throw new Exception(msg);
            }

            entitiy.Title = book.Title;
            entitiy.Price = book.Price;

            _manager.Book.Update(entitiy);
            _manager.Save();
        }
    }


}
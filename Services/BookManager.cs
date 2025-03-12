using Services.Contracts;
using Entities.Models;
using Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Services
{
    public class BookManager : IBookService
    {
        private readonly IRepositoryManager _manager;

        public BookManager(IRepositoryManager manager)
        {
            _manager = manager;
        }

        public Book CreateOneBook(Book book)
        {
            if(book is null)
                throw new ArgumentNullException(nameof(book));

            _manager.Book.CreateOneBook(book);
            _manager.Save();
            return book;
        }

        public void DeleteOneBook(int id, bool trackChanges)
        {
            Console.WriteLine($"[SERVICE] DeleteOneBook called for ID: {id}");

            var entity = _manager.Book.GetOneBookById(id, trackChanges);
            if (entity is null)
            {
                Console.WriteLine($"[SERVICE] Book with ID {id} not found in DB.");
                throw new Exception($"Book with id {id} could not be found.");
            }

            Console.WriteLine($"[SERVICE] Book found: {entity.Title}");

            _manager.Book.DeleteOneBook(entity);
            Console.WriteLine($"[SERVICE] Book deleted from repository. ID: {id}");

            _manager.Save();
            Console.WriteLine($"[SERVICE] Save method executed after delete.");
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
                throw new Exception($"Book with id:{id} could not found");
            //check params
            if(book is null)
                throw new ArgumentNullException(nameof(book));

            entitiy.Title = book.Title;
            entitiy.Price = book.Price;

            _manager.Book.Update(entitiy);
            _manager.Save();
        }
    }


}
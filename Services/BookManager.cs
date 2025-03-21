using Services.Contracts;
using Entities.Models;
using Repositories.Contracts;
using Microsoft.Extensions.Logging;
using Entities.Exceptions;
using AutoMapper;
using Entities.DataTransferObjects;
using System.Transactions;
using Microsoft.EntityFrameworkCore;


namespace Services
{
    public class BookManager : IBookService
    {
        private readonly IRepositoryManager _manager;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;
        public BookManager(IRepositoryManager manager , 
            ILoggerService logger,
            IMapper mapper )
        {
            _manager = manager;
            _logger = logger;
            _mapper = mapper;
        }

        public BookDto CreateOneBook(BookDtoForInsertion bookDto)
        {
            var entity = _mapper.Map<Book>(bookDto);
            _manager.Book.CreateOneBook(entity);
            _manager.Save();
            return _mapper.Map<BookDto>(entity);
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

        public IEnumerable<BookDto> GetAllBooks(bool trackChanges)
        {
            var books = _manager.Book.GetAllBooks(trackChanges);
            return _mapper.Map<IEnumerable<BookDto>>(books);
        }

        public BookDto GetOneBookById(int id, bool trackChanges)
        {
            var book = _manager.Book.GetOneBookById(id,trackChanges);
            if(book is null)
                throw new BookNotFoundException(id);
            return _mapper.Map<BookDto>(book);
        }

        public (BookDtoForUpdate bookDtoForUpdate, Book book) GetOneBookForPatch(int id, bool trackChanges)
        {
            var book = _manager.Book.GetOneBookById(id , trackChanges);
            
            if (book is null)
                throw new BookNotFoundException(id);
            
            var bookDtoForUpdate = _mapper.Map<BookDtoForUpdate>(book);
            
            return (bookDtoForUpdate,book);
        }

        public void SaveChangesForPatch(BookDtoForUpdate bookDtoForUpdate, Book book)
        {
            try
            {
                var initialBook = new Book
                {
                    Id = book.Id,
                    Title = book.Title,
                    Price = book.Price
                };
                Console.WriteLine($"{initialBook.Price},{initialBook.Title},{initialBook.Id}");
                _mapper.Map(bookDtoForUpdate, book);
                Console.WriteLine($"Yeni Book: {book.Price},{book.Title},{book.Id}");

                // // Boş if else 
                // if (initialBook.Title is not null)
                // {
                //     Console.WriteLine($"Yeni Title bookmanager: {book.Title}");
                // }
                // else if (initialBook.Price != 0)
                // {
                //     Console.WriteLine($"Yeni Price bookmanager: {book.Price}");
                // }
                // else if (initialBook.Id == 0)
                // {
                //     Console.WriteLine($"book with id not found");
                // }
                // else{
                //     Console.WriteLine("çıkış");
                // }
                // ---------------------------- //
                if (book.Title != initialBook.Title || book.Price != initialBook.Price) // Eğer book nesnesi değişmişse
                    {
                        _manager.Save();
                        Console.WriteLine("✅ Veritabanına kaydedildi!");
                    }
                else
                {
                    Console.WriteLine("❌ Hiçbir değişiklik yok, veritabanına kaydedilmedi.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Hata oluştu: {ex.Message}");
                // Hata mesajını döndürebilirsin
                throw;
            }
        }

        public void UpdateOneBook(int id , BookDtoForUpdate bookDto , bool trackChanges )
        {
            // Check entity
            var entity = _manager.Book.GetOneBookById(id,trackChanges);
            
            if (entity is null)
                throw new BookNotFoundException(id);
            Console.Write($"Güncellenen kitap {entity.Title}");
            // Mapping
            // entity.Title = book.Title;
            // entity.Price = book.Price;

            entity = _mapper.Map<Book>(bookDto);

            _manager.Book.Update(entity);
            _manager.Save();
            Console.Write("KAydedildi looooooo!");
            
        }
    }
}
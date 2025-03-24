using Services.Contracts;
using Entities.Models;
using Entities.RequestFeatures;
using Repositories.Contracts;
using Microsoft.Extensions.Logging;
using Entities.Exceptions;
using AutoMapper;
using Entities.DataTransferObjects;
using System.Transactions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;


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
        public async Task<IEnumerable<BookDto>> GetAllBooksAsync(BookParameters bookParameters , bool trackChanges)
        {
            var books = await _manager.Book.GetAllBooksAsync(bookParameters , trackChanges);
            return _mapper.Map<IEnumerable<BookDto>>(books);
        }

        public async Task<BookDto> GetOneBookByIdAsync(int id, bool trackChanges)
        {
            var book = await GetOneBookAndCheckExits(id , trackChanges);
            return _mapper.Map<BookDto>(book);
        }

        public async Task<BookDto> CreateOneBookAsync(BookDtoForInsertion bookDto)
        {
            var entity = _mapper.Map<Book>(bookDto);
            _manager.Book.CreateOneBook(entity);
            await _manager.SaveAsync();
            return  _mapper.Map<BookDto>(entity);
        }

        public async Task DeleteOneBookAsync(int id, bool trackChanges)
        {

            var entity =await GetOneBookAndCheckExits(id , trackChanges);
            _manager.Book.DeleteOneBook(entity);
            await _manager.SaveAsync();
        }

        public async Task UpdateOneBookAsync(int id , BookDtoForUpdate bookDto , bool trackChanges )
        {
            // Check entity
            var entity = await GetOneBookAndCheckExits(id , trackChanges);

            Console.Write($"Güncellenen kitap {entity.Title}");
            // Mapping
            // entity.Title = book.Title;
            // entity.Price = book.Price;

            entity = _mapper.Map<Book>(bookDto);

            _manager.Book.Update(entity);
            await _manager.SaveAsync();
            
        }
        public async Task<(BookDtoForUpdate bookDtoForUpdate, Book book)> GetOneBookForPatchAsync(int id, bool trackChanges)
        {
            var book = await GetOneBookAndCheckExits(id ,trackChanges);

            var bookDtoForUpdate = _mapper.Map<BookDtoForUpdate>(book);
            
            return (bookDtoForUpdate,book);
        }

        public async Task SaveChangesForPatchAsync(BookDtoForUpdate bookDtoForUpdate, Book book)
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

                if (book.Title != initialBook.Title || book.Price != initialBook.Price) // Eğer book nesnesi değişmişse
                    {
                        await _manager.SaveAsync();
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

        private async Task<Book> GetOneBookAndCheckExits(int id , bool trackChanges)
        {
            var entity = await _manager.Book.GetOneBookByIdAsync(id, trackChanges);
            if (entity is null)
            {
                throw new BookNotFoundException(id);
            }
            return entity;
        }
    }
}
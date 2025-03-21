using System.Threading.Tasks;
using Entities.DataTransferObjects;
using Entities.Exceptions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/books")]
    public class BooksController : ControllerBase
    {
        // Önemli bu pattern öğrenilecek
        private readonly IServiceManager _manager;

        public BooksController(IServiceManager manager)
        {
            _manager = manager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBooksAsync()
        {
            var books = await _manager.BookService.GetAllBooksAsync(false);
            return Ok(books);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOneBookAsync([FromRoute(Name = "id")]int id)
        //  public IActionResult GetOneBook(int id)
        {

            var book = await _manager
            .BookService
            .GetOneBookByIdAsync(id , false);

            if(book is null)
            {
                throw new BookNotFoundException(id);
            }
            return Ok(book);


        }

        [HttpPost]
        public async Task<IActionResult> CreateOneBookAsync([FromBody]BookDtoForInsertion bookDto)
        {
            if (bookDto is null)
            {
                return BadRequest(); //400
            }            
            if(!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }
            var book = await _manager.BookService.CreateOneBookAsync(bookDto);

            return StatusCode(201, book); // CreatedAtRoute response un headerına bilgi koyabiliyor ve bir url alabiliyoruz
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateOneBookAsync([FromRoute(Name = "id")]int id ,
            [FromBody] BookDtoForUpdate bookDto)
        {
            if(bookDto is null)
                return BadRequest();
            if(!ModelState.IsValid)
                return UnprocessableEntity(ModelState);
            await _manager.BookService.UpdateOneBookAsync(id, bookDto, false);

            return NoContent(); //204
        }
                
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteOneBookAsync([FromRoute(Name = "id")] int id)
        {
            await _manager.BookService.DeleteOneBookAsync(id, false);
            return NoContent();  // 204 No Content
        }


        [HttpPatch("{id:int}")]
        public async Task<IActionResult> PartiallyUpdateOneBookAsync([FromRoute(Name = "id")] int id, 
        [FromBody] JsonPatchDocument<BookDtoForUpdate> bookPatch)
        {
            if (bookPatch is null)
            {
                Console.WriteLine("⚠️ bookPatch NULL GELİYOR!"); // Log atalım
                return BadRequest(); // 400
            }

            var result = await _manager.BookService.GetOneBookForPatchAsync(id , true);


            Console.WriteLine("📌 ApplyTo çağrılıyor...");

            bookPatch.ApplyTo(result.bookDtoForUpdate, ModelState);

            Console.WriteLine($"Güncellenen Title: {result.bookDtoForUpdate.Title}");
            Console.WriteLine($"Güncellenen Price: {result.bookDtoForUpdate.Price}");
            Console.WriteLine("✅ ApplyTo başarıyla çalıştı!");

            TryValidateModel(result.bookDtoForUpdate);
            
            if(!ModelState.IsValid)
            {
                Console.WriteLine($"❌ ModelState HATALI: {ModelState}");
                return UnprocessableEntity(ModelState);
            }
            
            await _manager.BookService.SaveChangesForPatchAsync(result.bookDtoForUpdate ,result.book);

            return NoContent(); //204   
        }
    
    }
}

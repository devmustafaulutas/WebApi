using System.Threading.Tasks;
using Entities.DataTransferObjects;
using Entities.Exceptions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Services.Contracts;
using Presentation.ActionFilters;

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
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [HttpPost]
        public async Task<IActionResult> CreateOneBookAsync([FromBody]BookDtoForInsertion bookDto)
        {
            var book = await _manager.BookService.CreateOneBookAsync(bookDto);

            return StatusCode(201, book); // CreatedAtRoute response un headerına bilgi koyabiliyor ve bir url alabiliyoruz
        }
        
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateOneBookAsync([FromRoute(Name = "id")]int id ,
            [FromBody] BookDtoForUpdate bookDto)
        {
            await _manager.BookService.UpdateOneBookAsync(id, bookDto, false);
            return NoContent(); //204
        }
                
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteOneBookAsync([FromRoute(Name = "id")] int id)
        {
            await _manager.BookService.DeleteOneBookAsync(id, false);
            return NoContent();  // 204 No Content
        }

        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [HttpPatch("{id:int}")]
        public async Task<IActionResult> PartiallyUpdateOneBookAsync([FromRoute(Name = "id")] int id, 
        [FromBody] JsonPatchDocument<BookDtoForUpdate> bookPatch)
        {
            var result = await _manager.BookService.GetOneBookForPatchAsync(id , true);
            bookPatch.ApplyTo(result.bookDtoForUpdate, ModelState);

            TryValidateModel(result.bookDtoForUpdate);            
            
            await _manager.BookService.SaveChangesForPatchAsync(result.bookDtoForUpdate ,result.book);
            
            return NoContent(); //204   
        }
    
    }
}

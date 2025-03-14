using Entities.DataTransferObjects;
using Entities.Exceptions;
using Entities.Models;
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
        public IActionResult GetAllBooks()
        {
            var books = _manager.BookService.GetAllBooks(false);
            return Ok(books);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetOneBook([FromRoute(Name = "id")]int id)
        //  public IActionResult GetOneBook(int id)
        {

            var book = _manager
            .BookService
            .GetOneBookById(id , false);
            if(book is null)
            {
                throw new BookNotFoundException(id);
            }
            return Ok(book);


        }

        [HttpPost]
        public IActionResult CreateOneBook([FromBody]Book book)
        {
            if (book is null)
            {
                return BadRequest(); //400
            }            
            _manager.BookService.CreateOneBook(book);

            return StatusCode(201, book);
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateOneBook([FromRoute(Name = "id")]int id ,
            [FromBody] BookDtoForUpdate bookDto)
        {
            if(bookDto is null)
                return BadRequest();

            _manager.BookService.UpdateOneBook(id, bookDto, true);

            return NoContent(); //204
        }
                
        [HttpDelete("{id:int}")]
        public IActionResult DeleteOneBook([FromRoute(Name = "id")] int id)
        {
            _manager.BookService.DeleteOneBook(id, false);
            return NoContent();  // 204 No Content
        }


        [HttpPatch("{id:int}")]
        public IActionResult PartiallyUpdateOneBook([FromRoute(Name = "id")] int id, [FromBody] JsonPatchDocument<Book> bookPatch)
        {
            var entity = _manager
                .BookService
                .GetOneBookById(id, true);
                
            bookPatch.ApplyTo(entity);
                _manager.BookService.UpdateOneBook(id , 
                new BookDtoForUpdate(entity.Id , entity.Title , entity.Price), 
                true);

            return NoContent(); //204   
        }
    }
}
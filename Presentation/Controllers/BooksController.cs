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
        public IActionResult CreateOneBook([FromBody]BookDtoForInsertion bookDto)
        {
            if (bookDto is null)
            {
                return BadRequest(); //400
            }            
            if(!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }
            var book = _manager.BookService.CreateOneBook(bookDto);

            return StatusCode(201, book); // CreatedAtRoute response un headerına bilgi koyabiliyor ve bir url alabiliyoruz
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateOneBook([FromRoute(Name = "id")]int id ,
            [FromBody] BookDtoForUpdate bookDto)
        {
            if(bookDto is null)
                return BadRequest();
            if(!ModelState.IsValid)
                return UnprocessableEntity(ModelState);
            _manager.BookService.UpdateOneBook(id, bookDto, false);

            return NoContent(); //204
        }
                
        [HttpDelete("{id:int}")]
        public IActionResult DeleteOneBook([FromRoute(Name = "id")] int id)
        {
            _manager.BookService.DeleteOneBook(id, false);
            return NoContent();  // 204 No Content
        }


        [HttpPatch("{id:int}")]
        public IActionResult PartiallyUpdateOneBook([FromRoute(Name = "id")] int id, 
        [FromBody] JsonPatchDocument<BookDtoForUpdate> bookPatch)
        {
            if(bookPatch is null)
                return BadRequest(); //400

            var result = _manager.BookService.GetOneBookForPatch(id , false);
                
            bookPatch.ApplyTo(result.bookDtoForUpdate , ModelState);
            TryValidateModel(result.bookDtoForUpdate);
            
            if(!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            return NoContent(); //204   
        }
    }
}
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
            try
            {
                var books = _manager.BookService.GetAllBooks(false);
                return Ok(books);
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }

        }
        [HttpGet("{id:int}")]
        public IActionResult GetOneBook([FromRoute(Name = "id")]int id)
        //  public IActionResult GetOneBook(int id)
        {
            try
            {
                var book = _manager
                .BookService
                .GetOneBookById(id , false);

                if(book is null)
                {
                    return NotFound(); //404
                }
                return Ok(book);

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult CreateOneBook([FromBody]Book book)
        {
            try
            {
                if (book is null)
                    return BadRequest(); //400
                
                _manager.BookService.CreateOneBook(book);

                return StatusCode(201, book);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message); // ex.Message hatanın detayları için bilgi veriyor
            }
        }
        [HttpPut("{id:int}")]
        public IActionResult UpdateOneBook([FromRoute(Name = "id")]int id ,
            [FromBody] Book book)
        {
            try
            {
                if (book is null)
                    return BadRequest(); //400

                var existingBook = _manager.BookService.GetOneBookById(id, true);
                if (existingBook is null)
                    return NotFound(); //404

                _manager.BookService.UpdateOneBook(id, book, true);
                return NoContent(); //204
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}"); // 500: Server error
            }
        }

                
        [HttpDelete("{id:int}")]
        public IActionResult DeleteOneBook([FromRoute(Name = "id")]int id)
        {
            try
            {
                var entitiy = _manager.BookService.GetOneBookById(id, false);
                if (entitiy is null)
                    return NotFound(); //404

                _manager.BookService.DeleteOneBook(id, false);
                return NoContent();  // 204 No Content: Silme işlemi başarılı
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");  // 500: Server error
            }
        }


        [HttpPatch("{id:int}")]
        public IActionResult PartiallyUpdateOneBook([FromRoute(Name = "id")] int id, [FromBody] JsonPatchDocument<Book> bookPatch)
        {
            try
            {
                if (bookPatch is null)
                    return BadRequest(); //400

                var entity = _manager
                    .BookService
                    .GetOneBookById(id, true);
                if(entity is null)
                    return NotFound(); //404

                _manager.BookService.UpdateOneBook(id , entity, true);
                return NoContent(); //204   
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}"); // 500: Server error
            }
        }
    }
}
using System.Security.Cryptography.X509Certificates;
using bookDemo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
namespace bookDemo.Controllers

{
    [Route("api/[Controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
    

     [HttpGet]
        public IActionResult GetAllBooks()
        {
            var books = ApplicationContext.Books.ToList();
            return Ok(books);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetOneBook([FromRoute(Name = "id")]int id)
        //  public IActionResult GetOneBook(int id)
        {
            var book = ApplicationContext
                .Books
                .Where(b => b.Id.Equals(id))
                .SingleOrDefault();

            if(book is null)
            {
                return NotFound(); //404
            }

            return Ok(book);

        }
        [HttpPost]
        public IActionResult CreateOneBook([FromBody]Book book)
        {
            try
            {
                if (book is null)
                {
                    return BadRequest(); //400
                }
                ApplicationContext.Books.Add(book);
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
            // check
            var entity = ApplicationContext
                .Books
                .Find(b => b.Id.Equals(id));
            if(entity is null)
                return NotFound(); //404
            
            if(id!= book.Id)
                return BadRequest(); //400

            ApplicationContext.Books.Remove(entity);
            book.Id = entity.Id;
            ApplicationContext.Books.Add(book);
            return Ok(book);
        }
        [HttpDelete]
        public IActionResult DeleteAllBook()
        {
            ApplicationContext.Books.Clear();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteOneBook([FromRoute(Name = "id")]int id)
        {
            var entity = ApplicationContext
                .Books
                .Find(b => b.Id.Equals(id));
            if(entity is null)
                return NotFound(new
                {
                    StatusCode = 404,
                    message = $"Book with id:{id} could not found !"
                });
            ApplicationContext.Books.Remove(entity);
            return NoContent();
        }
        [HttpPatch("{id:int}")]
        public IActionResult PartiallyUpdateOneBook( [FromRoute(Name = "Id")] int id, [FromBody] JsonPatchDocument<Book> bookPatch)
        {
            // check
            var entity = ApplicationContext.Books 
                .Find(b => b.Id.Equals(id));

            if(entity is null)
                return NotFound(); //404

            bookPatch.ApplyTo(entity);
            return NoContent(); //204
        }
    }
}
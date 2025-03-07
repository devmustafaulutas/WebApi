using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Repostories;
using Microsoft.AspNetCore.JsonPatch;

namespace WebApi.Controlers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        // Önemli bu pattern öğrenilecek
        private readonly RepositoryContext _context;

        public BooksController(RepositoryContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult GetAllBooks()
        {
            try
            {
                var books = _context.Books.ToList();
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
                var book = _context
                .Books
                .Where(b => b.Id.Equals(id))
                .SingleOrDefault();

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
                
                _context.Books.Add(book);
                _context.SaveChanges();
                return StatusCode(201, book);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message); // ex.Message hatanın detayları için bilgi veriyor
            }
        }
        [HttpPut("{id:int}")]
        public IActionResult UpdateOneBook([FromRoute(Name = "id")]int id , [FromBody] Book book)
        {
            try
            {
            // check

            // 1. Adım kitap verisini çekiyoruz
            var entity = _context
                .Books
                .Where(b => b.Id.Equals(id))
                .SingleOrDefault();

            //Varlığını kontrol ediyoruz
            if(entity is null)
                return NotFound(); //404

            //Body ve Routte'a ki idleri checkliyoruz
            if(id!= book.Id)
                return BadRequest(); //400

            //Yeni değerleri geçiriyoruz
            entity.Title = book.Title;
            entity.Price = book.Price;
            
            //Kalıcı güncelleme için save alıyoruz
            _context.SaveChanges();

            return Ok(book);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

                
        [HttpDelete("{id:int}")]
        public IActionResult DeleteOneBook([FromRoute(Name = "id")]int id)
        {
            try
            {
                var entity = _context
                    .Books
                    .Where(b => b.Id.Equals(id))
                    .SingleOrDefault();

                if(entity is null)
                    return NotFound(new
                    {
                        StatusCode = 404,
                        message = $"Book with id:{id} could not found !"
                    });
                _context.Books.Remove(entity);
                _context.SaveChanges();

                return NoContent();                
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
        }

        [HttpPatch("{id:int}")]
        public IActionResult PartiallyUpdateOneBook( [FromRoute(Name = "Id")] int id, [FromBody] JsonPatchDocument<Book> bookPatch)
        {
            // check
            var entity = _context.Books 
                .Where(b => b.Id.Equals(id))
                .SingleOrDefault();

            if(entity is null)
                return NotFound(); //404

            bookPatch.ApplyTo(entity);
            _context.SaveChanges();
            return NoContent(); //204
        }
    }
}
namespace Entities.DataTransferObjects
{

    // [Serializable] 
    // Burada proplar olmadığı için get / set vs Serialize edebilmek için bunu ekliyoruz
    // public record BookDto(int Id, String Title,decimal Price);
    public record BookDto
    {
        public int Id { get; set; }
        public String Title { get; set; }
        public decimal Price { get; set; }
    }

}
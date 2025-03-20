namespace Entities.DataTransferObjects
{

    // [Serializable] 
    // Burada proplar olmadığı için get / set vs Serialize edebilmek için bunu ekliyoruz
    // public record BookDto(int Id, String Title,decimal Price);
    public record BookDto
    {
        public int Id { get; init; }
        public String Title { get; init; }
        public decimal Price { get; init; }
    }
}
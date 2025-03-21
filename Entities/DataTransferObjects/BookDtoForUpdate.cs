using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects
{
    public record BookDtoForUpdate : BookDtoForManipulation
    {   
        [Required]
        public int Id { get; init; }
    }
    // Yukarıdaki gibi tanımlamak ve aşağıdaki gibi tanımlamak aynı 
    // anlama geliyor ve açık açık tanımlamak istediğimizde set bloğu yerine init bloğu kullanmalıyız.
    //
    // Yerine
    // {
    //     public int Id { get; init; }
    //     public String Title { get; init; }
    //     public decimal Price { get; init; }
    // }
}
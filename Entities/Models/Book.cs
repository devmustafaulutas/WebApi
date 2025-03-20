using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Book
    {
        public int Id {get; set;}
        public string Title {get; set;} = default!;
        
        [Column(TypeName = "decimal(10,2)")]

        public decimal Price {get; set;}
    }
}
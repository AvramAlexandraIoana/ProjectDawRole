using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjectDaw.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Denumirea categoriei este obligatorie")]
        public string CategoryName { get; set; }
        public virtual ICollection<Product> Product  { get; set; }
    }
}
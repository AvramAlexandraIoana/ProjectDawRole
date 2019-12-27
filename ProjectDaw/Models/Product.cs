using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectDaw.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Titlul este obligatoriu")]
        [StringLength(20, ErrorMessage = "Titlul nu poate avea mai mult de 20 caractere")]
        public string Title { get; set; }


        [Required(ErrorMessage = "Descrierea produsului este obligatorie")]
        [MinLength(10, ErrorMessage = "Descrierea produsului trebuie sa aiba minimum 10 caractere")]
        [MaxLength(50, ErrorMessage = "Descrierea produsului trebuie sa aiba maximum 50 caractere")]

        public string Description { get; set; }

        [Required(ErrorMessage = "Poza produsului este obligatorie")]
        public string ImagePath { get; set; }


        [Required(ErrorMessage = "Pretul produsului este obligatoriu")]
        [Range(0.000001, 100000000, ErrorMessage = "Pretul produsului trebuie sa fie mai mare decat 0")]
        public float Price { get; set; }


        [Required(ErrorMessage = "Rating-ul produsului este obligatoriu")]
        [Range(1,5, ErrorMessage ="Ratingul trebuie sa fie intre 1 si 5")]
        public int Rating { get; set; }


        [Required(ErrorMessage = "Review-ul produsului este obligatoriu")]
        [MinLength(10, ErrorMessage = "Review-ul produsului trebuie sa aiba minimum 10 caractere")]
        [MaxLength(50, ErrorMessage = "Review-ul produsului trebuie sa aiba maximum 50 caractere")]
        public string Review { get; set; }


        [Required(ErrorMessage = "Categoria este  obligatorie")]
        public int CategoryId { get; set; }

        public string UserId { get; set; }

        public virtual Category Category { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }


        public virtual ApplicationUser User { get; set; }
    }
    public enum ListForSearch
    {
        Title,
        Description,
        Review
    }

    /* public class ProductDBContext : DbContext
     {
         public ProductDBContext() : base("DBConnectionString") { }
         public DbSet <Product> Products { get; set; }
         public DbSet<Category> Categories{ get; set; }

     }*/

}
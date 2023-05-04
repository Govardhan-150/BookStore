using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class ProductType
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public string ISBN { get; set; }
        [Required]
        public string Author { get; set; }
        [DisplayName("List Price")]
        [Range(1,10000)]
        [Required]
        public double ListPrice { get; set; }
        [Range(1, 10000)]
        [Required]
        public double Price { get; set; }
        [DisplayName("Price for 50")]
        [Required]
        [Range(1, 10000)]
        public double ListPrice50 { get; set; }
        [DisplayName("Price for 100")]
        [Range(1, 10000)]
        [Required]
        public double ListPrice100 { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category Category { get; set; }
        [ValidateNever]
        public string ImageUrl { get; set; }
    }
}

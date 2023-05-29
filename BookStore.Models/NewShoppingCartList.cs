using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class NewShoppingCartList
    {
        public int Id { get; set; }
        public int ProductTypeId { get; set; }
        [ForeignKey("ProductTypeId")]
        [ValidateNever]
        public ProductType ProductType { get; set; }
        [Range(1, 1000, ErrorMessage = "range should be greater than zero and less than 1000")]
        public int count { get; set; }
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }
        [NotMapped]
        public double Price { get; set; }
    }
}

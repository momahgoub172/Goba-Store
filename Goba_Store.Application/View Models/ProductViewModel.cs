using Goba_Store.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Goba_Store.Application.View_Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public string ShortDescription { get; set; }
        public string Description { get; set; }
        [Range(int.MinValue, int.MaxValue)]
        public int Price { get; set; }
        public string? Image { get; set; }
        public Category? Category { get; set; }
        public int? CategoryId { get; set; }

        public IEnumerable<SelectListItem>? CategoryDropDown { get; set; }
    }
}

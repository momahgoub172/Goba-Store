using System.ComponentModel.DataAnnotations;

namespace Goba_Store.Application.View_Models
{

    public class CategoryViewModel
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(150)]
        [Display(Name = "Category Name")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Dispaly Number")]
        public int DisplayOrder { get; set; }
    }
}
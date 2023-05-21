using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Goba_Store.Models;

public class Product
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }

    public string ShortDescription { get; set; }
    public string Description { get; set; }
    [Range(int.MinValue,int.MaxValue)]
    public int Price { get; set; }
    public string? Image { get; set; }
    
    
    //RelationShips
    [Display(Name = "Category Type")]
    public int CategoryId { get; set; }
    [ForeignKey("CategoryId")]
    public virtual Category? Category { get; set; }
}
﻿using System.ComponentModel.DataAnnotations;

namespace Goba_Store.View_Models;

public class CategoryViewModel
{
    [Required]
    [MaxLength(150)]
    [Display(Name = "Category Name")]
    public string Name { get; set; }
    [Required]
    [Display(Name= "Dispaly Number")]
    public int DisplayOrder { get; set; }
}
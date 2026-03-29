using System.ComponentModel.DataAnnotations;

namespace ContosoPizza.Models;

public class Pizza
{
    public int Id { get; set; }

    [Required(ErrorMessage = "名称不能为空")]
    [Display(Name = "名称")]
    public string? Name { get; set; }

    [Display(Name = "尺寸")]
    public PizzaSize Size { get; set; }

    [Display(Name = "无麸质")]
    public bool IsGlutenFree { get; set; }

    [Range(0.01, 9999.99, ErrorMessage = "价格必须在 0.01 到 9999.99 之间")]
    [Display(Name = "价格")]
    public decimal Price { get; set; }
}

public enum PizzaSize { Small, Medium, Large }

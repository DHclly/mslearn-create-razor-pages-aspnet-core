using System.ComponentModel.DataAnnotations;

namespace ContosoPizza.Models;

public class OrderItem
{
    public int Id { get; set; }

    [Display(Name = "订单")]
    public int OrderId { get; set; }
    public Order Order { get; set; } = default!;

    [Display(Name = "披萨")]
    public int PizzaId { get; set; }
    public Pizza Pizza { get; set; } = default!;

    [Required(ErrorMessage = "数量不能为空")]
    [Range(1, 99, ErrorMessage = "数量必须在 1 到 99 之间")]
    [Display(Name = "数量")]
    public int Quantity { get; set; } = 1;

    [Display(Name = "单价")]
    public decimal UnitPrice { get; set; }

    [Display(Name = "小计")]
    public decimal Subtotal => Quantity * UnitPrice;
}

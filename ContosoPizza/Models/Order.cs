using System.ComponentModel.DataAnnotations;

namespace ContosoPizza.Models;

// 订单模型，对应数据库 Orders 表
public class Order
{
    public int Id { get; set; }

    // 外键：关联客户，订单必须属于某个客户
    [Required]
    [Display(Name = "客户")]
    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = default!;

    [Display(Name = "优惠券")]
    public string? CouponCode { get; set; }

    [Display(Name = "折扣")]
    public decimal Discount { get; set; }

    [Display(Name = "下单时间")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    // 导航属性：一个订单包含多个明细
    public IList<OrderItem> Items { get; set; } = new List<OrderItem>();

    // 计算属性：订单总金额（折扣后）
    [Display(Name = "总金额")]
    public decimal TotalAmount => Items.Sum(i => i.Subtotal) * (1 - Discount);
}

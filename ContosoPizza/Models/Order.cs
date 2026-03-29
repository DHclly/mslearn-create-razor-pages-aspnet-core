using System.ComponentModel.DataAnnotations;

namespace ContosoPizza.Models;

// 订单模型，对应数据库 Orders 表
public class Order
{
    // 主键，EF Core 自动递增
    public int Id { get; set; }

    [Required]
    [Display(Name = "姓名")]
    public string? CustomerName { get; set; }

    [Required]
    [Display(Name = "电话")]
    [Phone]
    public string? Phone { get; set; }

    [Required]
    [Display(Name = "地址")]
    public string? Address { get; set; }

    [Display(Name = "优惠券")]
    public string? CouponCode { get; set; }

    [Display(Name = "折扣")]
    public decimal Discount { get; set; }

    [Display(Name = "下单时间")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

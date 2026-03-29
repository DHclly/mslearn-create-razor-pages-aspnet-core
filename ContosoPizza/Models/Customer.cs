using System.ComponentModel.DataAnnotations;

namespace ContosoPizza.Models;

public class Customer
{
    public int Id { get; set; }

    [Required(ErrorMessage = "姓名不能为空")]
    [Display(Name = "姓名")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "电话不能为空")]
    [Phone(ErrorMessage = "电话格式不正确")]
    [Display(Name = "电话")]
    public string? Phone { get; set; }

    [Required(ErrorMessage = "地址不能为空")]
    [Display(Name = "地址")]
    public string? Address { get; set; }

    [Display(Name = "注册时间")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public IList<Order> Orders { get; set; } = new List<Order>();
}

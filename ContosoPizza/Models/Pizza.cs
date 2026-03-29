using System.ComponentModel.DataAnnotations;

namespace ContosoPizza.Models;

// 模型类（Model）：用来描述一条数据的结构，对应数据库里的一张表
// 每个属性对应表里的一列
public class Pizza
{
    // 主键，EF Core 约定：名为 Id 的属性会自动作为主键，数据库会自动递增
    public int Id { get; set; }

    // [Required] 验证特性：表单提交时此字段不能为空，否则验证失败
    // [Display(Name = "...")] 指定在页面上显示的标签文字，
    //   asp-for、DisplayNameFor 等 Tag Helper 都会自动读取这个值
    [Required]
    [Display(Name = "名称")]
    public string? Name { get; set; }

    [Display(Name = "尺寸")]
    public PizzaSize Size { get; set; }

    [Display(Name = "无麸质")]
    public bool IsGlutenFree { get; set; }

    // [Range] 验证特性：限制数值范围，超出范围时验证失败
    [Range(0.01, 9999.99)]
    [Display(Name = "价格")]
    public decimal Price { get; set; }
}

// 枚举：定义一组固定的选项值，比直接用字符串更安全
public enum PizzaSize { Small, Medium, Large }

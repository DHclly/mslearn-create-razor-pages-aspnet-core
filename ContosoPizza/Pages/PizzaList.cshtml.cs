using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ContosoPizza.Models;
using ContosoPizza.Services;

namespace ContosoPizza.Pages
{
    // PageModel 是 Razor Pages 的后台逻辑类，对应同名的 .cshtml 文件
    // 相当于 MVC 里的 Controller，但每个页面有自己专属的 PageModel
    public class PizzaListModel : PageModel
    {
        // 通过依赖注入获得 PizzaService，不需要自己 new
        // private readonly 表示只在这个类内部使用，且赋值后不能再改
        private readonly PizzaService _service;

        // 普通公开属性：用于向页面（.cshtml）传递数据
        // 页面里通过 Model.PizzaList 访问
        // = default! 告诉编译器"我保证用之前会赋值"，消除 null 警告
        public IList<Pizza> PizzaList { get; set; } = default!;

        // [BindProperty]：表单提交时，ASP.NET Core 会自动把表单数据
        // 填充到这个属性里，不需要手动从 Request 里读取
        // 新增（Id=0）和编辑（Id>0）共用这一个属性
        [BindProperty]
        public Pizza NewPizza { get; set; } = default!;

        // 构造函数：ASP.NET Core 自动把注册好的 PizzaService 传进来
        public PizzaListModel(PizzaService service)
        {
            _service = service;
        }

        // OnGet()：处理 GET 请求（浏览器直接访问页面时触发）
        // 命名规则：On + HTTP方法，Razor Pages 会自动路由到对应方法
        public void OnGet()
        {
            PizzaList = _service.GetPizzas();
        }

        // OnPost()：处理 POST 请求（表单提交时触发）
        // 返回 IActionResult 可以控制响应：返回页面或跳转
        public IActionResult OnPost()
        {
            // ModelState.IsValid：检查所有 [BindProperty] 属性是否通过验证
            // 比如 [Required] 不能为空、[Range] 不能超出范围等
            if (!ModelState.IsValid)
            {
                // 验证失败时重新加载列表（因为 POST 不会自动执行 OnGet）
                PizzaList = _service.GetPizzas();
                // 返回当前页面，页面上会显示验证错误信息
                return Page();
            }

            // Id == 0 说明是新增（新对象还没有数据库分配的 Id）
            // Id > 0 说明是编辑（Id 是从隐藏字段传过来的已有记录的主键）
            if (NewPizza.Id == 0)
                _service.AddPizza(NewPizza);
            else
                _service.UpdatePizza(NewPizza);

            // RedirectToPage()：重定向回当前页面（触发一次新的 GET 请求）
            // 这样刷新页面不会重复提交表单（PRG 模式：Post-Redirect-Get）
            return RedirectToPage();
        }

        // OnPostDelete()：处理名为 "Delete" 的 POST handler
        // 对应页面上 asp-page-handler="Delete" 的表单
        // int id 来自 asp-route-id="@pizza.Id"，框架自动从路由里提取
        public IActionResult OnPostDelete(int id)
        {
            _service.DeletePizza(id);
            return RedirectToPage();
        }
    }
}

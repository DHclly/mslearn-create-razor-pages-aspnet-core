using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ContosoPizza.Models;
using ContosoPizza.Services;

namespace ContosoPizza.Pages
{
    // 演示：同一个页面合理使用三个 [BindProperty] 的场景
    // 三个属性分别对应不同的触发时机，互不干扰：
    //   1. SearchKeyword  —— GET 请求（SupportsGet = true，搜索披萨）
    //   2. CouponCode     —— OnPostApplyCoupon handler（独立的优惠券表单）
    //   3. NewOrder       —— OnPost handler（提交订单表单）
    public class OrderModel : PageModel
    {
        private readonly PizzaService _pizzaService;
        private readonly OrderService _orderService;

        // 场景一：SupportsGet = true
        // 允许在 GET 请求时绑定，比如 /Order?SearchKeyword=cheese
        // 适合搜索、筛选这类不修改数据的操作
        // 默认 [BindProperty] 只在 POST 时绑定，加了 SupportsGet 才能在 GET 时绑定
        [BindProperty(SupportsGet = true)]
        public string? SearchKeyword { get; set; }

        // 场景二：独立 handler 的 [BindProperty]
        // 只在 OnPostApplyCoupon() 触发时绑定，提交订单时不会干扰
        // 对应页面上 asp-page-handler="ApplyCoupon" 的表单
        [BindProperty]
        public string? CouponCode { get; set; }

        // 场景三：主表单的 [BindProperty]
        // 只在 OnPost() 触发时绑定
        // 使用 Models/Order.cs 里定义的模型，和 Pizza 保持一致的结构
        [BindProperty]
        public Order NewOrder { get; set; } = default!;

        // 只读属性，向页面传递数据，不需要绑定
        public IList<Pizza> PizzaList { get; set; } = default!;
        public IList<Order> OrderList { get; set; } = default!;

        // 优惠券验证结果，用于在页面上显示折扣信息
        public decimal Discount { get; set; }

        public OrderModel(PizzaService pizzaService, OrderService orderService)
        {
            _pizzaService = pizzaService;
            _orderService = orderService;
        }

        // OnGet：处理 GET 请求
        // SearchKeyword 因为 SupportsGet = true，会自动从 URL 参数绑定
        // 访问 /Order 显示全部，访问 /Order?SearchKeyword=cheese 自动过滤
        public void OnGet()
        {
            LoadData();
        }

        // OnPostApplyCoupon：独立 handler，只处理优惠券表单
        // 此时 NewOrder 没有提交，ModelState 里不会有它的验证错误
        // 两个表单的验证完全隔离
        public IActionResult OnPostApplyCoupon()
        {
            if (string.IsNullOrWhiteSpace(CouponCode))
            {
                ModelState.AddModelError(nameof(CouponCode), "请输入优惠券码");
                LoadData();
                return Page();
            }

            Discount = _orderService.ValidateCoupon(CouponCode);

            if (Discount == 0)
            {
                ModelState.AddModelError(nameof(CouponCode), "优惠券码无效，试试 PIZZA10 或 PIZZA20");
                LoadData();
                return Page();
            }

            // TempData：跨请求传递一次性消息，RedirectToPage 后仍能读取，读完自动清除
            TempData["CouponMsg"] = $"优惠券已应用：{CouponCode.ToUpper()}，优惠 {Discount * 100}%";
            TempData["CouponCode"] = CouponCode;
            // 把 SearchKeyword 拼回 URL，保持搜索状态
            return RedirectToPage(new { SearchKeyword });
        }

        // OnPost：处理提交订单表单（默认 handler）
        // 此时 CouponCode 表单没有提交，不会干扰 NewOrder 的验证
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                LoadData();
                return Page();
            }

            // 从 TempData 读取已应用的优惠券（如果有）
            if (TempData.Peek("CouponCode") is string code)
            {
                NewOrder.CouponCode = code;
                NewOrder.Discount = _orderService.ValidateCoupon(code);
            }

            _orderService.AddOrder(NewOrder);

            TempData["OrderMsg"] = $"订单已提交！感谢 {NewOrder.CustomerName} 的惠顾 🍕";
            return RedirectToPage(new { SearchKeyword });
        }

        // 抽取公共的数据加载逻辑，避免在每个 handler 里重复写
        private void LoadData()
        {
            var all = _pizzaService.GetPizzas();
            PizzaList = string.IsNullOrWhiteSpace(SearchKeyword)
                ? all
                : all.Where(p => p.Name != null &&
                      p.Name.Contains(SearchKeyword, StringComparison.OrdinalIgnoreCase))
                     .ToList();

            OrderList = _orderService.GetOrders();
        }
    }
}

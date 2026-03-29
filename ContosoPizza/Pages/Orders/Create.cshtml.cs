using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ContosoPizza.Models;
using ContosoPizza.Services;

namespace ContosoPizza.Pages.Orders
{
    public class CreateModel : PageModel
    {
        private readonly OrderService _orderService;
        private readonly CustomerService _customerService;
        private readonly PizzaService _pizzaService;

        // BindProperty 1：选择客户（SupportsGet=true 保持页面刷新后仍选中）
        [BindProperty(SupportsGet = true)]
        public int SelectedCustomerId { get; set; }

        // BindProperty 2：优惠券，独立 handler 处理
        [BindProperty]
        public string? CouponCode { get; set; }

        // BindProperty 3：提交订单，包含选中的披萨和数量
        [BindProperty]
        public List<OrderItemInput> Items { get; set; } = new();

        // 页面展示数据
        public SelectList CustomerOptions { get; set; } = default!;
        public IList<Pizza> PizzaList { get; set; } = default!;
        public decimal Discount { get; set; }

        public CreateModel(OrderService orderService, CustomerService customerService, PizzaService pizzaService)
        {
            _orderService = orderService;
            _customerService = customerService;
            _pizzaService = pizzaService;
        }

        public void OnGet()
        {
            LoadData();
            // 从 TempData 恢复已应用的优惠券折扣
            if (TempData.Peek("CouponCode") is string code)
                Discount = _orderService.ValidateCoupon(code);
        }

        // 独立 handler：验证优惠券
        public IActionResult OnPostApplyCoupon()
        {
            if (string.IsNullOrWhiteSpace(CouponCode))
            {
                ModelState.AddModelError(nameof(CouponCode), "请输入优惠券码");
                LoadData();
                return Page();
            }

            var discount = _orderService.ValidateCoupon(CouponCode);
            if (discount == 0)
            {
                ModelState.AddModelError(nameof(CouponCode), "优惠券无效，试试 PIZZA10 或 PIZZA20");
                LoadData();
                return Page();
            }

            TempData["CouponCode"] = CouponCode;
            TempData["CouponMsg"] = $"优惠券已应用：{CouponCode.ToUpper()}，优惠 {discount * 100}%";
            return RedirectToPage(new { SelectedCustomerId });
        }

        // 提交订单
        public IActionResult OnPost()
        {
            // 先过滤掉数量为 0 的项，再清除对应的 ModelState 错误
            // 因为 ModelState 在绑定时就已经验证，过滤后需要手动清理
            Items = Items.Where(i => i.Quantity > 0).ToList();
            for (int i = Items.Count; i < 100; i++)
            {
                ModelState.Remove($"Items[{i}].PizzaId");
                ModelState.Remove($"Items[{i}].Quantity");
            }

            // 业务验证
            if (SelectedCustomerId == 0)
                ModelState.AddModelError(nameof(SelectedCustomerId), "请选择客户");

            if (!Items.Any())
                ModelState.AddModelError(string.Empty, "请至少选择一种披萨");

            if (!ModelState.IsValid)
            {
                LoadData();
                // 把所有验证错误加到 TempData 方便调试（上线前可删除）
                TempData["Debug"] = string.Join("; ", ModelState
                    .Where(x => x.Value?.Errors.Any() == true)
                    .SelectMany(x => x.Value!.Errors.Select(e => $"{x.Key}: {e.ErrorMessage}")));
                return Page();
            }

            var pizzas = _pizzaService.GetPizzas();
            var order = new Order
            {
                CustomerId = SelectedCustomerId,
                CouponCode = TempData.Peek("CouponCode") as string,
                Discount = TempData.Peek("CouponCode") is string code
                    ? _orderService.ValidateCoupon(code) : 0,
                Items = Items.Select(i =>
                {
                    var pizza = pizzas.First(p => p.Id == i.PizzaId);
                    return new OrderItem
                    {
                        PizzaId = i.PizzaId,
                        Quantity = i.Quantity,
                        UnitPrice = pizza.Price
                    };
                }).ToList()
            };

            _orderService.AddOrder(order);

            TempData.Remove("CouponCode");
            TempData["Msg"] = "订单提交成功 🍕";
            return RedirectToPage("/Orders/Index");
        }

        private void LoadData()
        {
            var customers = _customerService.GetCustomers();
            CustomerOptions = new SelectList(customers, "Id", "Name", SelectedCustomerId);
            PizzaList = _pizzaService.GetPizzas();
            if (TempData.Peek("CouponCode") is string code)
                Discount = _orderService.ValidateCoupon(code);
        }
    }

    // 用于接收表单中每行披萨的选择（PizzaId + Quantity）
    public class OrderItemInput
    {
        public int PizzaId { get; set; }
        public int Quantity { get; set; }
    }
}

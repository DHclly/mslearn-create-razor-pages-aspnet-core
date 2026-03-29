using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ContosoPizza.Models;
using ContosoPizza.Services;

namespace ContosoPizza.Pages.Orders
{
    public class IndexModel : PageModel
    {
        private readonly OrderService _service;

        public IList<Order> OrderList { get; set; } = default!;

        public IndexModel(OrderService service)
        {
            _service = service;
        }

        public void OnGet()
        {
            OrderList = _service.GetOrders();
        }

        public IActionResult OnPostDelete(int id)
        {
            _service.DeleteOrder(id);
            TempData["Msg"] = "订单已删除";
            return RedirectToPage();
        }
    }
}

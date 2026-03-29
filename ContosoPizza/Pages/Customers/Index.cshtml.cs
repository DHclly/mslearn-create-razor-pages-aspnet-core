using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ContosoPizza.Models;
using ContosoPizza.Services;

namespace ContosoPizza.Pages.Customers
{
    public class IndexModel : PageModel
    {
        private readonly CustomerService _service;

        public IList<Customer> CustomerList { get; set; } = default!;

        [BindProperty]
        public Customer NewCustomer { get; set; } = default!;

        public IndexModel(CustomerService service)
        {
            _service = service;
        }

        public void OnGet()
        {
            CustomerList = _service.GetCustomers();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                CustomerList = _service.GetCustomers();
                return Page();
            }

            if (NewCustomer.Id == 0)
                _service.AddCustomer(NewCustomer);
            else
                _service.UpdateCustomer(NewCustomer);

            return RedirectToPage();
        }

        public IActionResult OnPostDelete(int id)
        {
            _service.DeleteCustomer(id);
            return RedirectToPage();
        }
    }
}

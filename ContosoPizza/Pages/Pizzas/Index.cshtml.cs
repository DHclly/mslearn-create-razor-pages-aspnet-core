using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ContosoPizza.Models;
using ContosoPizza.Services;

namespace ContosoPizza.Pages.Pizzas
{
    public class IndexModel : PageModel
    {
        private readonly PizzaService _service;

        public IList<Pizza> PizzaList { get; set; } = default!;

        [BindProperty]
        public Pizza NewPizza { get; set; } = default!;

        public IndexModel(PizzaService service)
        {
            _service = service;
        }

        public void OnGet()
        {
            PizzaList = _service.GetPizzas();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                PizzaList = _service.GetPizzas();
                return Page();
            }

            if (NewPizza.Id == 0)
                _service.AddPizza(NewPizza);
            else
                _service.UpdatePizza(NewPizza);

            return RedirectToPage();
        }

        public IActionResult OnPostDelete(int id)
        {
            _service.DeletePizza(id);
            return RedirectToPage();
        }
    }
}

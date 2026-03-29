using ContosoPizza.Data;
using ContosoPizza.Models;
using Microsoft.EntityFrameworkCore;

namespace ContosoPizza.Services
{
    public class CustomerService
    {
        private readonly PizzaContext _context;

        public CustomerService(PizzaContext context)
        {
            _context = context;
        }

        public IList<Customer> GetCustomers()
            => _context.Customers.OrderBy(c => c.Name).ToList();

        public Customer? GetCustomer(int id)
            => _context.Customers.Find(id);

        public void AddCustomer(Customer customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();
        }

        public void UpdateCustomer(Customer customer)
        {
            _context.Customers.Update(customer);
            _context.SaveChanges();
        }

        public void DeleteCustomer(int id)
        {
            var customer = _context.Customers.Find(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                _context.SaveChanges();
            }
        }
    }
}

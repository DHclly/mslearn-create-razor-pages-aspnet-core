using ContosoPizza.Data;
using ContosoPizza.Models;
using Microsoft.EntityFrameworkCore;

namespace ContosoPizza.Services
{
    public class OrderService
    {
        private readonly PizzaContext _context;

        public OrderService(PizzaContext context)
        {
            _context = context;
        }

        // 查询所有订单，Include 关联的客户和明细（EF Core 的预加载）
        public IList<Order> GetOrders()
            => _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Items).ThenInclude(i => i.Pizza)
                .OrderByDescending(o => o.CreatedAt)
                .ToList();

        // 查询单个订单
        public Order? GetOrder(int id)
            => _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Items).ThenInclude(i => i.Pizza)
                .FirstOrDefault(o => o.Id == id);

        // 新增订单（含明细）
        public void AddOrder(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
        }

        // 删除订单（EF Core 级联删除会自动删除 OrderItems）
        public void DeleteOrder(int id)
        {
            var order = _context.Orders.Find(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                _context.SaveChanges();
            }
        }

        // 验证优惠券码，返回折扣比例（0 表示无效）
        public decimal ValidateCoupon(string couponCode)
        {
            return couponCode.ToUpper() switch
            {
                "PIZZA10" => 0.1m,
                "PIZZA20" => 0.2m,
                _ => 0m
            };
        }
    }
}

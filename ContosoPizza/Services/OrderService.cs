using ContosoPizza.Data;
using ContosoPizza.Models;

namespace ContosoPizza.Services
{
    // 订单服务层：封装订单相关的业务逻辑和数据库操作
    // 页面（PageModel）通过注入此服务来操作数据，不直接接触 DbContext
    public class OrderService
    {
        private readonly PizzaContext _context;

        // 构造函数注入：Program.cs 注册后，ASP.NET Core 自动传入
        public OrderService(PizzaContext context)
        {
            _context = context;
        }

        // 查询所有订单，按下单时间倒序排列
        public IList<Order> GetOrders()
        {
            if (_context.Orders != null)
                return _context.Orders.OrderByDescending(o => o.CreatedAt).ToList();
            return new List<Order>();
        }

        // 新增订单，保存到数据库
        public void AddOrder(Order order)
        {
            if (_context.Orders != null)
            {
                _context.Orders.Add(order);
                _context.SaveChanges();
            }
        }

        // 验证优惠券码，返回折扣比例（0 表示无效）
        // 实际项目可以从数据库读取优惠券表，这里用固定值演示
        public decimal ValidateCoupon(string couponCode)
        {
            return couponCode.ToUpper() switch
            {
                "PIZZA10" => 0.1m,  // 九折，优惠 10%
                "PIZZA20" => 0.2m,  // 八折，优惠 20%
                _ => 0m             // 无效
            };
        }
    }
}

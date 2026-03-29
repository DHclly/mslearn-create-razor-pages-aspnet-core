using ContosoPizza.Data;
using ContosoPizza.Models;

namespace ContosoPizza.Services
{
    // Service（服务层）：专门负责业务逻辑，把数据库操作封装在这里
    // 好处是页面代码（PageModel）不直接操作数据库，职责分离，更好维护
    public class PizzaService
    {
        // 通过依赖注入获得 DbContext，不需要自己 new
        private readonly PizzaContext _context = default!;

        // 构造函数注入：ASP.NET Core 会自动把 PizzaContext 实例传进来
        // 前提是在 Program.cs 里注册了 AddDbContext<PizzaContext>
        public PizzaService(PizzaContext context) 
        {
            _context = context;
        }
        
        // 查询所有披萨，返回列表
        // ToList() 会立即执行 SQL 查询，把结果加载到内存
        public IList<Pizza> GetPizzas()
        {
            if(_context.Pizzas != null)
            {
                return _context.Pizzas.ToList();
            }
            return new List<Pizza>();
        }

        // 新增一条披萨记录
        // Add() 只是告诉 EF Core "我要加这条数据"
        // SaveChanges() 才是真正执行 SQL INSERT 写入数据库
        public void AddPizza(Pizza pizza)
        {
            if (_context.Pizzas != null)
            {
                _context.Pizzas.Add(pizza);
                _context.SaveChanges();
            }
        }

        // 更新一条已有的披萨记录
        // Update() 会追踪这个对象的所有字段变化
        // SaveChanges() 执行 SQL UPDATE
        public void UpdatePizza(Pizza pizza)
        {
            if (_context.Pizzas != null)
            {
                _context.Pizzas.Update(pizza);
                _context.SaveChanges();
            }
        }

        // 根据 id 删除一条披萨记录
        // Find() 先按主键查出对象，再 Remove()，最后 SaveChanges() 执行 SQL DELETE
        public void DeletePizza(int id)
        {
            if (_context.Pizzas != null)
            {
                var pizza = _context.Pizzas.Find(id);
                if (pizza != null)
                {
                    _context.Pizzas.Remove(pizza);
                    _context.SaveChanges();
                }
            }            
        } 
    }
}

using Microsoft.EntityFrameworkCore;

namespace ContosoPizza.Data
{
    // DbContext 是 Entity Framework Core 的核心类
    // 它负责和数据库通信，相当于数据库的"代理人"
    // 你通过它来查询、新增、修改、删除数据，不需要自己写 SQL
    public class PizzaContext : DbContext
    {
        // 构造函数：接收配置选项（比如数据库连接字符串），传给父类
        // 这些选项在 Program.cs 里通过依赖注入配置好后自动传进来
        public PizzaContext(DbContextOptions<PizzaContext> options)
            : base(options)
        {
        }

        // DbSet<T> 代表数据库里的一张表
        // 通过 _context.Pizzas 就可以对 Pizza 表进行增删改查
        // 加 ? 是因为数据库可能还没初始化，允许为 null
        public DbSet<ContosoPizza.Models.Pizza>? Pizzas { get; set; }
    }
}

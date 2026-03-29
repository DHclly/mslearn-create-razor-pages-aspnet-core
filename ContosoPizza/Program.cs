using ContosoPizza.Data;
using ContosoPizza.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ===== 注册服务（依赖注入容器）=====
// 这里注册的服务，可以在任何 PageModel / Controller 的构造函数里直接使用

// 启用 Razor Pages 支持
builder.Services.AddRazorPages();

// 注册数据库上下文，使用 SQLite，数据库文件是 ContosoPizza.db
// AddDbContext 会自动管理连接的生命周期
builder.Services.AddDbContext<PizzaContext>(options =>
    options.UseSqlite("Data Source=ContosoPizza.db"));

// 注册 PizzaService，Scoped 表示每次 HTTP 请求创建一个新实例，请求结束后销毁
builder.Services.AddScoped<PizzaService>();

var app = builder.Build();

// ===== 配置中间件管道（请求处理流程）=====
// 中间件按顺序执行，每个请求都会经过这里

if (!app.Environment.IsDevelopment())
{
    // 生产环境：统一错误页面
    app.UseExceptionHandler("/Error");
    // HSTS：告诉浏览器以后只用 HTTPS 访问（安全策略）
    app.UseHsts();
}

// 把 HTTP 请求自动跳转到 HTTPS
app.UseHttpsRedirection();

// 允许访问 wwwroot 里的静态文件（CSS、JS、图片等）
app.UseStaticFiles();

// 启用路由，让框架知道该把请求交给哪个页面处理
app.UseRouting();

// 启用授权（这个项目暂时没用到，但是标准管道里需要放在这里）
app.UseAuthorization();

// 把所有 Razor Pages 的路由注册进来（自动根据 Pages 文件夹结构生成路由）
app.MapRazorPages();

app.Run();

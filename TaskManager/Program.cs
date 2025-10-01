using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Controllers;
using TaskManager.Data;
using TaskManager.Models;

var builder = WebApplication.CreateBuilder(args);

// DbContext 
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();


// Controller + Swagger
builder.Services.AddControllers();
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (!dbContext.Users.Any())
    {
        dbContext.Users.AddRange(
            new User { UserName = "Melike Akyol", Email = "MelikeAkyol.com" },
            new User { UserName = "Zeynep Yilmaz", Email = "zeynepYilmaz.com" },
            new User { UserName = "Ahmet Demir", Email = "ahmetDemir.com" },
            new User { UserName = "Fatma Kara", Email = "fatmaKara.com" },
            new User { UserName = "Emre Can", Email = "emreCan.com" }
        );
        dbContext.SaveChanges();
    }//    
    var users = dbContext.Users.ToList();

    if (!dbContext.Tasks.Any())
    {
        dbContext.Tasks.AddRange(
            new TaskItem
            {
                Title = "Planlama",
                Description = "Projenin konusunu ve amacýný netleþtir.Gereksinimleri belirle. Zaman planý (deadline, ara hedefler) çýkar. ",
                IsCompleted = true,
                UserId = users[0].Id
            },
            new TaskItem
            {
                Title = "Araþtýrma ve Analiz",
                Description = "Daha önce yapýlmýþ benzer projeleri incele. Kullanýlacak teknolojileri, araçlarý seç. Gereksinimleri analiz edip dökümana dök.",
                IsCompleted = false,
                UserId = users[1 % users.Count].Id
            },
            new TaskItem
            {
                Title = "Tasarým",
                Description = "Yazýlýmsa: veri tabaný þemasý, sistem mimarisi, ekran tasarýmlarý hazýrla. Donaným veya gömülü sistemse: devre þemasý, blok diyagram oluþtur. Kullanýcý arayüzü için wireframe/mockup hazýrla.",
                IsCompleted = false,
                UserId = users[2 % users.Count].Id
            });
        dbContext.SaveChanges();
    }
}

/*if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}*/
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseRouting();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllers();
app.Run();


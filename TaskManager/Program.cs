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
                Description = "Projenin konusunu ve amac�n� netle�tir.Gereksinimleri belirle. Zaman plan� (deadline, ara hedefler) ��kar. ",
                IsCompleted = true,
                UserId = users[0].Id
            },
            new TaskItem
            {
                Title = "Ara�t�rma ve Analiz",
                Description = "Daha �nce yap�lm�� benzer projeleri incele. Kullan�lacak teknolojileri, ara�lar� se�. Gereksinimleri analiz edip d�k�mana d�k.",
                IsCompleted = false,
                UserId = users[1 % users.Count].Id
            },
            new TaskItem
            {
                Title = "Tasar�m",
                Description = "Yaz�l�msa: veri taban� �emas�, sistem mimarisi, ekran tasar�mlar� haz�rla. Donan�m veya g�m�l� sistemse: devre �emas�, blok diyagram olu�tur. Kullan�c� aray�z� i�in wireframe/mockup haz�rla.",
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


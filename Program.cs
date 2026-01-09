using System.Globalization;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

var builder = WebApplication.CreateBuilder(args);
// 註冊資料庫服務
builder.Services.AddDbContext<AppDbContext>();

var app = builder.Build();

// 1. 初始化資料庫並匯入 CSV (僅在專案啟動時執行一次)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();

    if (!db.CarLocation.Any() && File.Exists("stupid.csv"))
    {
        using var reader = new StreamReader("stupid.csv");
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        var records = csv.GetRecords<CarLocation>().ToList();
        db.CarLocation.AddRange(records);
        db.SaveChanges();
        Console.WriteLine($"✅ 資料匯入完成，共 {records.Count} 筆。");
    }
}

// 2. 設定靜態檔案 (為了讀取 index.html)
app.UseDefaultFiles();
app.UseStaticFiles();

// 3. 建立 API 接口：取得所有車輛資料
app.MapGet("/api/cars", async (AppDbContext db) => 
    await db.CarLocation.ToListAsync());

// 4. 建立 API 接口：根據車牌篩選
app.MapGet("/api/cars/{carId}", async (string carId, AppDbContext db) => 
    await db.CarLocation.Where(c => c.Car == carId).ToListAsync());

app.Run();

// --- DbContext 定義 ---
public class AppDbContext : DbContext {
    public DbSet<CarLocation> CarLocation { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite("Data Source=mydata.db");
}
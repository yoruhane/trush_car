using System.Globalization;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

// 1. 定義資料模型 (對應 CSV 欄位)

public class CarLocation
{
    [Key]
    public int LineId { get; set; }        // 對應 lineid
    public string Car { get; set; } = "";   // 對應 car (車牌)
    public DateTime Time { get; set; }      // 對應 time
    public string Location { get; set; } = ""; // 對應 location
    public double Longitude { get; set; }   // 對應 longitude (經度)
    public double Latitude { get; set; }    // 對應 latitude (緯度)
    public int CityId { get; set; }         // 對應 cityid
    public string CityName { get; set; } = ""; // 對應 cityname
}
// 2. 定義資料庫上下文 (DbContext)
public class AppDbContext : DbContext
{
    public DbSet<CarLocation> CarLocations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite("Data Source=mydata.db"); // 資料庫檔名
}

// 3. 主程式邏輯
class Program
{
    static void Main()
    {
        using var db = new AppDbContext();
        
        // 自動建立資料庫檔案
        db.Database.EnsureCreated();

        Console.WriteLine("正在讀取 CSV 並匯入資料庫...");

        // 讀取 CSV
        using (var reader = new StreamReader("stupid.csv"))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            // 將 CSV 轉為 List<CarLocation>
            var records = csv.GetRecords<CarLocation>().ToList();

            // 檢查是否已存在資料（避免重複匯入）
            if (!db.CarLocations.Any())
            {
                db.CarLocations.AddRange(records);
                db.SaveChanges();
                Console.WriteLine($"成功匯入 {records.Count} 筆資料！");
            }
            else
            {
                Console.WriteLine("資料庫已有資料，跳過匯入。");
            }
        }

        // 4. 示範篩選功能 (針對車牌與行政區)

// 情境 A：篩選特定車牌 "076-UY" 的所有路徑記錄
    Console.WriteLine("\n--- 篩選結果 (車牌: 076-UY) ---");
    var carRecords = db.CarLocations
        .Where(c => c.Car == "076-UY")
        .OrderBy(c => c.Time) // 依時間排序，方便追蹤軌跡
        .ToList();

    foreach (var c in carRecords)
    {
        Console.WriteLine($"時間: {c.Time:yyyy-MM-dd HH:mm:ss} | 位置: {c.Location}");
    }

    // 情境 B：篩選在 "坪林區" 的所有車輛 (統計功能)
    Console.WriteLine("\n--- 篩選結果 (行政區: 坪林區) ---");
    var pinglinData = db.CarLocations
        .Where(c => c.CityName == "坪林區")
        .ToList();

    Console.WriteLine($"坪林區目前共有 {pinglinData.Count} 筆位置記錄");
        }
}
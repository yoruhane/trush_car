using System.ComponentModel.DataAnnotations;

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
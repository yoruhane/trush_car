using System.ComponentModel.DataAnnotations;
using CsvHelper.Configuration.Attributes;

public class CarLocation
{
    [Key]
    [Name("lineid")]
    public int LineId { get; set; }        // 對應 lineid
    [Name("car")]
    public string Car { get; set; } = "";   // 對應 car (車牌)
    [Name("time")]
    public DateTime Time { get; set; }      // 對應 time
    [Name("location")]
    public string Location { get; set; } = ""; // 對應 location
    [Name("longitude")]
    public double Longitude { get; set; }   // 對應 longitude (經度)
    [Name("latitude")]
    public double Latitude { get; set; }    // 對應 latitude (緯度)
    [Name("cityid")]
    public int CityId { get; set; }         // 對應 cityid
    [Name("cityname")]
    public string CityName { get; set; } = ""; // 對應 cityname
}
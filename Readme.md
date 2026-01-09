##  這啥

# 這是新北的垃圾車運輸路線導覽圖
我找不到全台的，要做全台的要把每個縣市的資料庫都收集起來，我看每個縣市的資料都還有點不太一樣，不會整合所以就只做了新北的

# 如何使用 

net9.0

在CsvToSqliteProject的資料夾裡的Program.cs
專案建置
dotnet build
dotnet run
找到localhost:5xxx
進去，有了

##結構
graph LR
A[CSV Files] --> B(C# Importer)
B --> C[(SQLite DB)]
C --> D[ASP.NET Core API]
D --> E[Leaflet.js Map]
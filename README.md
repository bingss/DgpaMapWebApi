# 事求人開放資料地圖 - 公務人員版的104職缺地圖

[事求人開放資料地圖網址](https://agreeable-island-01a306f00.4.azurestaticapps.net)

<img src="https://github.com/bingss/dgpamapf/blob/main/%E5%AF%A6%E9%9A%9B%E9%A0%81%E9%9D%A2.png" height="65%" width="65%" /> <img src="https://github.com/bingss/dgpamapf/blob/main/%E5%AF%A6%E9%9A%9B%E9%A0%81%E9%9D%A2_%E6%89%8B%E6%A9%9F%E6%9D%BF.png" height="20%" width="20%" />


 - 每日定時爬取人事行政總處事求人網站職缺開放資料，並轉為坐標資訊
 - 將職缺位置視覺化，位置分佈情形一目了然
 - 下拉式選單依工作地及職系顯示職缺總數及篩選職缺
 - 點選地圖(列表)上職缺，顯示職缺資料(位置)
 - 桌面及行動裝置響應式設計

## Release Notes

### 2024-12-29
* 第一版上線，採前後端分離，並部署於Azure雲端
* 前端：vue3、vue3-openlayers、bootstrap5
* 後端：C#、ASP.NET Core Web API、MSSQL(AzureSQL)
* AzureFunction排程撈取開放資料
* HereMap Geocoding Api轉為坐標

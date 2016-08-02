# bing-Image-JSON

This C# UWP solution demonstrates the URL retreival after JSON string parsing from the Bing Image of the Day API.  
Each line of code is well detailed in order to demonstrate the thinking behind using the method and better understanding.  

## Description
•	Ability to specify the region and the number of images to retrieve
•	Solution to parseURL from the JSON string retrieved from the Bing API  
•	Explored native and third-party ways to parse JSON in C# UWP  
• Demonstrated parsing utilizing JsonObject using Windows.Data.Json  
•	Implemented different parsing techniques employing both JObject and JsonConvert.DeserializeObject using NewtonSoft Json.Net  

## Parsing Methods  
Windows.Data.Json (JsonObject)  
Newtonsoft.Json (JObject)  
Newtonsoft.Json (DeserializeObject<dynamic>)  

## Screenshots
![Alt text](/BIOD.jpg?raw=true "Bing Image of the Day screenshot")

## Bing JSON String
Given below is a sample JSON string that is retrieved when number of images are set to 2.
```json
{{
  "images": [
    {
      "startdate": "20160728",
      "fullstartdate": "201607280700",
      "enddate": "20160729",
      "url": "/az/hprichbg/rb/Castelluccio_EN-US14033484396_1920x1080.jpg",
      "urlbase": "/az/hprichbg/rb/Castelluccio_EN-US14033484396",
      "copyright": "Castelluccio in Monti Sibillini National Park, Italy (© Brian Jannsen/Alamy)",
      "copyrightlink": "http://www.bing.com/search?q=Castelluccio,+Umbria&form=hpcapt&filters=HpDate:%2220160728_0700%22",
      "wp": true,
      "hsh": "3e471d31e42b8319a63b4f3384ba6207",
      "drk": 1,
      "top": 1,
      "bot": 1,
      "hs": []
    },
    {
      "startdate": "20160727",
      "fullstartdate": "201607270700",
      "enddate": "20160728",
      "url": "/az/hprichbg/rb/Coot_EN-US11668116958_1920x1080.jpg",
      "urlbase": "/az/hprichbg/rb/Coot_EN-US11668116958",
      "copyright": "A Eurasian coot resting on one leg in Derbyshire, England (© Andrew Parkinson/Minden Pictures)",
      "copyrightlink": "http://www.bing.com/search?q=Eurasian+coot&form=hpcapt&filters=HpDate:%2220160727_0700%22",
      "wp": true,
      "hsh": "10fc89ef15cb10884646cdc1d78cd144",
      "drk": 1,
      "top": 1,
      "bot": 1,
      "hs": []
    }
  ],
  "tooltips": {
    "loading": "Loading...",
    "previous": "Previous image",
    "next": "Next image",
    "walle": "This image is not available to download as wallpaper.",
    "walls": "Download this image. Use of this image is restricted to wallpaper only."
  }
}}
```

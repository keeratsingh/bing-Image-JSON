/*
* @author  Keerat Singh
* @Date    24-Jul-2016
*/

// Unused default namespaces
/*using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;*/

// Default Namespaces
using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
// Displaying Images
using Windows.UI.Xaml.Media.Imaging;
// Http Client
using Windows.Web.Http;
// Aync tasks
using System.Threading.Tasks;
// Parse JSON String
using Windows.Data.Json;
using Newtonsoft.Json.Linq;

namespace bingImage
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

        }

        /* 
         * This Method fetches the JSON string from the BingAPI endpoint and then returns the Raw String to the caller.
         * We need to use either void or Task if we use await in the method body.
        */
        public async Task<string> getJSONString(int _numOfImages)
        {

            // We can specify the region we want for the Bing Image of the Day.
            string strRegion = "en-US";
            string strBingImageURL = string.Format("http://www.bing.com/HPImageArchive.aspx?format=js&idx=0&n={0}&mkt={1}", _numOfImages, strRegion);
            string strJSONString = "";

            // Use Windows.Web.Http Namespace as System.Net.Http will be deprecated in future versions.
            HttpClient client = new HttpClient();

            // Using an Async call makes sure the app is responsive during the time the response is fetched.
            // GetAsync sends a Async GET request to the Specified URI.
            HttpResponseMessage response = await client.GetAsync(new Uri(strBingImageURL));

            // Content property get or sets the content of a HTTP response message.
            // ReadAsStringAsync is a method of the HttpContent which asynchronously reads the content of the HTTP Response and returns as a string.
            strJSONString = await response.Content.ReadAsStringAsync();

            return strJSONString;
        }

        /*
         * This Method parses the fetched JSON string and retrieves the Image URLs using Microsoft Windows.Data.Json class
         * Each Url is stored as a separate List item and the list of URLs is returned to the caller.
        */
        public List<string> parseJSONString(int _numOfImages, string _strRawJSONString)
        {
            List<string> _lstBingImageURLs = new List<string>(_numOfImages);
            // JsonObject class implements the IMap interface, which helps in manipulating the name/value pairs like a dictionary.
            JsonObject jsonObject;
            
            // JsonObject.TryParse parses the JSON string into a JSON value, which returns a boolean value, indicating success or failure.
            // TryParse is an added safe measure to avoid an execption while parsing.
            bool boolParsed = JsonObject.TryParse(_strRawJSONString, out jsonObject);
            if (boolParsed)
            {
                for (int i = 0; i < _numOfImages; i++)
                {
                    // The retrieval structure depends upon JSON string, the base key in our case is "images".
                    // If you retrieve more than one image, "images" key will have more than one array values.
                    // Each Array value has a key/value pair "url", which we retrieve and conver it to a string.
                    _lstBingImageURLs.Add(jsonObject["images"].GetArray()[i].GetObject()["url"].GetString());
                }
            }

            // Return the list containing URLs.
            return _lstBingImageURLs;
        }


        /*
        * This Method parses the fetched JSON string and retrieves the Image URLs using NewtonSoft Json.Net
        * Each Url is stored as a separate List item and the list of URLs is returned to the caller.
       */
        public List<string> parseJSONString_Newtonsoft(int _numOfImages, string _strRawJSONString)
        {
            List<string> _lstBingImageURLs = new List<string>(_numOfImages);

            // -------------------- Method 1 -------------------- //
            // Parse using LINQ-to-JSON API's JObject explicitly.
            // We use this method when we don't know the JSON Structure.
            JObject jResults = JObject.Parse(_strRawJSONString);
            foreach (var image in jResults["images"])
            {
                _lstBingImageURLs.Add((string)image["url"]);
            }

            /*
            // -------------------- Method 2 -------------------- //
            // Parse using DeserializeObject method.
            // We use this method when we know the JSON Structure ahead of time.
            for (int i = 0; i < _numOfImages; i++)
            {
                // We use dynamic object, as it saves us the trouble of declaring a specific class to hold the JSON object
                _lstBingImageURLs.Add(Convert.ToString((JsonConvert.DeserializeObject<dynamic>(_strRawJSONString)).images[i].url));
            }
            */
           
            // Return the list containing URLs.
            return _lstBingImageURLs;
        }

        /*
         * This Method takes the list of parsed URLs, converts each URL into a Bitmap Image and then adds the
         * Bitmap Image into the Stack Panel dynamically to display as an Image Object.
        */
        public void displayImages(List<string> _lstBingImageURLs)
        {
            // Clear the Stack Panel in order to avoid duplication and start afresh.
            if (spImages != null)
                spImages.Children.Clear();

            // Just reverse the order of the retrieved Images, just to have a better visibility when the number of images change.
            _lstBingImageURLs.Reverse();

            foreach (string url in _lstBingImageURLs)
            {
                // We use the Image control to display an image on the UI.
                Image imgbingImage = new Image();
                var bingURL = "https://www.bing.com" + url;

                // BitmapImage class inherits from the BitmapSource class and is specialized BitmapSource that is
                // optimized for loading images using XAML.
                // WPF essentially runs a background thread that downloads and save the image in memory before it is displayed.
                BitmapSource imgbingImageSource = new BitmapImage(new Uri(bingURL));
                imgbingImage.Source = imgbingImageSource;
                // Add the Image control to the Stack Panel
                spImages.Children.Add(imgbingImage);
            }
        }


       /*
        * This Method updates the UI based on the value of the slider and the type of parsing method selected.
       */
        private async void sldNoOfImages_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            List<string> lstBingImageURLs;
            try
            {
                // We use Convert.ToInt32 rather than simple cast (int) as it can convert any primitive type to int.
                // Also Convert.ToInt32 handles conversion better as compared to (int)
                int _numOfImages = Convert.ToInt32(e.NewValue);

                string strRawJSONString = await getJSONString(_numOfImages);
                
                if (rbDataJson.IsChecked == true)
                {
                    // Use Windows.Data.Json to parse JSON string.
                    lstBingImageURLs = parseJSONString_Newtonsoft(_numOfImages, strRawJSONString);
                }
                else
                {
                    // Use NewtonSoft Json.Net to parse JSON string.
                    lstBingImageURLs = parseJSONString(_numOfImages, strRawJSONString);
                }

                displayImages(lstBingImageURLs);
                txtStatus.Text = String.Format("{0} Image(s) retrieved successfully", _numOfImages);
            }
            
            // Display a generalized exception message
            catch(Exception ex)
            {
                txtStatus.Text = ex.Message;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Http Client
using Windows.Web.Http;
// Aync tasks
using System.Threading.Tasks;
// Parse JSON Bing String
// using Newtonsoft.Json; // JsonConvert
using Windows.Data.Json;

// Displaying Images
using Windows.UI.Xaml.Media.Imaging;

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

            // Just reverse the order of the retrieved Images, just to have a better visibility when the number of images change.
            _lstBingImageURLs.Reverse();
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
        * This Method updates the UI based on the value of the slider.
       */
        private async void sldNoOfImages_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            try
            {
                int _numOfImages = Convert.ToInt32(e.NewValue);
                string strRawJSONString = await getJSONString(_numOfImages);
                List<string> lstBingImageURLs = parseJSONString(_numOfImages, strRawJSONString);
                displayImages(lstBingImageURLs);
                txtStatus.Text = "";
            }
            
            // Display an exception message
            catch(Exception ex)
            {
                txtStatus.Text = ex.Message;
            }
        }
    }
}

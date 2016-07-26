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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace bingImage
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            // Variables
            int numOfImages = 3;
            string strRegion = "en-US";

            string strRawJSONString = await getJSONString(numOfImages, strRegion);
            List<string> lstBingImageURLs = parseJSONString(numOfImages, strRawJSONString);
        }


        /* 
         * This Method fetches the JSON string from the BingAPI endpoint and then returns the Raw String to the caller
         * We need to use either void or Task if we use await in the method body.
        */
        public async Task<string> getJSONString(int _numOfImages, string _strRegion)
        {
           
            // We can specify the region and number of images we want for the Bing Image of the Day.
            string strBingImageURL = string.Format("http://www.bing.com/HPImageArchive.aspx?format=js&idx=0&n={0}&mkt={1}", _numOfImages, _strRegion);
            string strJSONString = "";

            // Use Windows.Web.Http Namespace as System.Net.Http will be deprecated in future versions.
            HttpClient client = new HttpClient();

            // Using an Async call makes sure the app is responsive during the time the response is fetched.
            // GetAsync sends a Async GET request to the Specified URI
            HttpResponseMessage response = await client.GetAsync(new Uri(strBingImageURL));

            // Content property get or sets the content of a HTTP response message
            // ReadAsStringAsync is a method of the HttpContent which asynchronously reads the content of the HTTP Response and returns as a string
            strJSONString = await response.Content.ReadAsStringAsync();

            return strJSONString;
        }

        /*
         * This Method parses the fetched JSON string and retrieves the Image URLs using Microsoft Windows.Data.Json class
         * Each Url is stored as a separate List item and the list of URLs is returned to the caller  
         */
        public List<string> parseJSONString(int _numOfImages, string _strRawJSONString)
        {
            List<string> _lstBingImageURLs = new List<string>(_numOfImages);
            JsonObject jsonObject = JsonObject.Parse(_strRawJSONString);

            for(int i=0; i < _numOfImages; i++)
            {
                _lstBingImageURLs.Add( jsonObject["images"].GetArray()[i].GetObject()["url"].GetString() );
            }

            return _lstBingImageURLs;
        }
    }
}

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
            string test = await getJSONString();
        }

        // We need to use either void or Task if we use await in the method body.
        public async Task<string> getJSONString()
        {
            // Variables
            string noOfImages = "1";
            string region = "en-US";
            // We can specify the region and number of images we want for the Bing Image of the Day.
            string bingImageURL = string.Format("http://www.bing.com/HPImageArchive.aspx?format=js&idx=0&n={0}&mkt={1}", noOfImages, region);
            string jsonString = "";

            // Use Windows.Web.Http Namespace as System.Net.Http will be deprecated in future versions.
            HttpClient client = new HttpClient();

            // GetAsync sends a Async GET request to the Specified URI
            HttpResponseMessage response = await client.GetAsync(new Uri(bingImageURL));

            // Content property get or sets the content of a HTTP response message
            // ReadAsStringAsync is a method of the HttpContent which asynchronously reads the content of the HTTP Response and returns as a string
            jsonString = await response.Content.ReadAsStringAsync();

            return jsonString;

        }
    }
}

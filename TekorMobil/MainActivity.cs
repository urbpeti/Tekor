using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace TekorMobil
{
    [Activity(Label = "TekorMobil", MainLauncher = true)]
    public class MainActivity : Activity
    {
        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            bool isLogged = await IsLoggedIn();
            if (!isLogged) {
                //at navigalas ha nincs bejelentkezve
                var loginIntent = new Intent(this, typeof(LoginActivity));
                StartActivity(loginIntent);
                Finish();
            }

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Menu);

            Button goalsButotn = FindViewById<Button>(Resource.Id.GoalsMenuButton);
            Button myCuponsButotn = FindViewById<Button>(Resource.Id.MyCuponsButton);


            goalsButotn.Click += async (sender, e) =>
            {
                string url = "http://urbpeti.sch.bme.hu:44310";
                var client = new HttpClient
                {
                    BaseAddress = new Uri(url)
                };
                HttpResponseMessage response;
                try
                {
                    response = await client.GetAsync("/Goals/GetList?token=" + (Application as TekorApplication).Token);
                }
                catch (Exception)
                {
                    Toast.MakeText(this.ApplicationContext, "Error Occured", ToastLength.Short).Show();
                    return;
                }

                // var result = await response.Content.ReadAsStringAsync();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var intent = new Intent(this, typeof(GoalsActivity));
                    intent.PutExtra("Data", await response.Content.ReadAsStringAsync());
                    StartActivity(intent);
                    return;
                }
                Toast.MakeText(this.ApplicationContext, "Error Occured!", ToastLength.Short).Show();
            };
        }

        private async Task<bool> IsLoggedIn()
        {
            var application = (Application as TekorApplication);
            if (application.Email == "" || application.Token == "")
                return false;

            string url = "http://urbpeti.sch.bme.hu:44310";
            var client = new HttpClient
            {
                BaseAddress = new Uri(url)
            };

            string jsonData = $@"{{""email"" : ""{application.Email}"", ""token"" : ""{application.Token}""}}";


            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("/Login/isLoggedIn", content);

            // var result = await response.Content.ReadAsStringAsync();
            if (response.StatusCode == HttpStatusCode.OK) {
                return true;
            }
            return false;
        }


        /*private async Task<JsonValue> FetchWeatherAsync(string url)
        {
            // Create an HTTP web request using the URL:
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "GET";

            // Send the request to the server and wait for the response:
            using (WebResponse response = await request.GetResponseAsync())
            {
                // Get a stream representation of the HTTP web response:
                using (Stream stream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(stream);
                    string text = reader.ReadToEnd();
                    // Use this stream to build a JSON document object:
                    JsonValue jsonDoc = await Task.Run(() => JsonObject.Load(stream));

                    // Return the JSON document:
                    return jsonDoc;
                }
            }
        }*/
    }
}


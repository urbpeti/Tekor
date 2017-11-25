using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Net;
using System.IO;
using System.Json;
using System.Threading.Tasks;
using System.Linq;

namespace TekorMobil
{
    [Activity(Label = "TekorMobil", MainLauncher = true)]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            EditText userName = FindViewById<EditText>(Resource.Id.UserName);
            EditText password = FindViewById<EditText>(Resource.Id.Password);
            Button loginButotn = FindViewById<Button>(Resource.Id.LoginButton);
            
            loginButotn.Click += async (sender, e) =>
            {
                var result = await FetchWeatherAsync("http://urbpeti.sch.bme.hu:44310/Goals/Getlist");
                var list = result["goalItems"];
                var fos = (list as JsonArray).Select(x=> (x as JsonObject)).ToList();
                var intent = new Intent(this, typeof(GoalsActivity));
                intent.PutExtra("Data",list.ToString());
                StartActivity(intent);
            };
        }

        private async Task<JsonValue> FetchWeatherAsync(string url)
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
                    // Use this stream to build a JSON document object:
                    JsonValue jsonDoc = await Task.Run(() => JsonObject.Load(stream));
                    Console.Out.WriteLine("Response: {0}", jsonDoc.ToString());

                    // Return the JSON document:
                    return jsonDoc;
                }
            }
        }
    }
}


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
using System.Threading;

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

            Button goalsButton = FindViewById<Button>(Resource.Id.GoalsMenuButton);
            Button myCuponsButton = FindViewById<Button>(Resource.Id.MyCuponsButton);


            goalsButton.Click += async (sender, e) =>
            {

                string url = Resources.GetString(Resource.String.service_url);
                var client = new HttpClient
                {
                    BaseAddress = new Uri(url)
                };
                HttpResponseMessage response = null;
                try
                {
                    var progressDialog = ProgressDialog.Show(this, "Please wait...", "Checking account info...", true);
                    new Thread(new ThreadStart(async delegate
                    {
                        //LOAD METHOD TO GET ACCOUNT INFO
                        response = await client.GetAsync("/Goals/GetList?token=" + (Application as TekorApplication).Token);

                        if (response?.StatusCode == HttpStatusCode.OK)
                        {
                            //save application email Token
                            var intent = new Intent(this, typeof(GoalsActivity));
                            intent.PutExtra("Data", await response.Content.ReadAsStringAsync());
                            RunOnUiThread(() => progressDialog.Hide());
                            StartActivity(intent);
                            return;
                        }
                        RunOnUiThread(() =>
                        {
                            Toast.MakeText(this.ApplicationContext, "Error Occured", ToastLength.Short).Show();
                        });

                        RunOnUiThread(() => progressDialog.Hide());
                    })).Start();
                }
                catch (Exception)
                {
                    Toast.MakeText(this.ApplicationContext, "Server is unavailable", ToastLength.Short).Show();
                    return;
                }
            };


            myCuponsButton.Click += async (sender, e) =>
            {

                string url = Resources.GetString(Resource.String.service_url);
                var client = new HttpClient
                {
                    BaseAddress = new Uri(url)
                };
                HttpResponseMessage response = null;
                try
                {
                    var progressDialog = ProgressDialog.Show(this, "Please wait...", "Checking account info...", true);
                    new Thread(new ThreadStart(async delegate
                    {
                        //LOAD METHOD TO GET ACCOUNT INFO
                        response = await client.GetAsync("/Goals/GetFinishedList?token=" + (Application as TekorApplication).Token);

                        if (response?.StatusCode == HttpStatusCode.OK)
                        {
                            //save application email Token
                            var intent = new Intent(this, typeof(GoalsActivity));
                            intent.PutExtra("Data", await response.Content.ReadAsStringAsync());
                            RunOnUiThread(() => progressDialog.Hide());
                            StartActivity(intent);
                            return;
                        }
                        RunOnUiThread(() =>
                        {
                            Toast.MakeText(this.ApplicationContext, "Error Occured", ToastLength.Short).Show();
                        });

                        RunOnUiThread(() => progressDialog.Hide());
                    })).Start();
                }
                catch (Exception)
                {
                    Toast.MakeText(this.ApplicationContext, "Server is unavailable", ToastLength.Short).Show();
                    return;
                }
            };
        }

        private async Task<bool> IsLoggedIn()
        {
            var application = (Application as TekorApplication);
            if (application.Email == "" || application.Token == "")
                return false;

            string url = Resources.GetString(Resource.String.service_url);
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

    }
}


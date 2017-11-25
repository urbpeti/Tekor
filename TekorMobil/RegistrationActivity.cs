using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Net.Http;
using System.Net;

namespace TekorMobil
{
    [Activity(Label = "RegistrationActivity")]
    public class RegistrationActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Registration);

            Button registrationButton = FindViewById<Button>(Resource.Id.Registration);

            EditText email = FindViewById<EditText>(Resource.Id.Email);
            EditText password = FindViewById<EditText>(Resource.Id.Password);
            TextView errorText = FindViewById<TextView>(Resource.Id.ErrorMessage);


            registrationButton.Click += async (sender, e) =>
            {
                string url = "http://urbpeti.sch.bme.hu:44310";
                var client = new HttpClient
                {
                    BaseAddress = new Uri(url)
                };

                string jsonData = $@"{{""email"" : ""{email.Text}"", ""token"" : ""{RestService.Base64Encode(email.Text + ":" + password.Text)}""}}";

                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                HttpResponseMessage response;
                try
                {
                    response = await client.PostAsync("/Registration/Registration", content);
                }
                catch (Exception)
                {
                    errorText.Text = "Server is unavailable";
                    errorText.Visibility = ViewStates.Visible;
                    return;
                }

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Finish();
                    return;
                }
                errorText.Text = "Error";
                errorText.Visibility = ViewStates.Visible;

            };

            // Create your application here
        }
    }
}
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
    [Activity(Label = "LoginActivity")]
    public class LoginActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Login);

            Button loginButton = FindViewById<Button>(Resource.Id.LoginButton);
            Button registrationButton = FindViewById<Button>(Resource.Id.RegistrationButton);

            EditText email = FindViewById<EditText>(Resource.Id.Email);
            EditText password = FindViewById<EditText>(Resource.Id.Password);
            TextView errorText = FindViewById<TextView>(Resource.Id.ErrorMessage);


            registrationButton.Click += (sender, e) =>
            {
                var registrationIntent = new Intent(this, typeof(RegistrationActivity));
                StartActivity(registrationIntent);
            };

            loginButton.Click += async (sender, e) =>
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
                    response = await client.PostAsync("/Login/isLoggedIn", content);
                }
                catch (Exception)
                {
                    errorText.Text = "Server is unavailable";
                    errorText.Visibility = ViewStates.Visible;
                    return;
                }

                // var result = await response.Content.ReadAsStringAsync();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    (Application as TekorApplication).Email = email.Text;
                    (Application as TekorApplication).Token = RestService.Base64Encode(email.Text + ":" + password.Text);

                    //save application email Token
                    var intent = new Intent(this, typeof(MainActivity));
                    StartActivity(intent);
                    Finish();
                    return;
                }
                errorText.Text = "Wrong User and Password combination";
                errorText.Visibility = ViewStates.Visible;
            };
        }
    }
}
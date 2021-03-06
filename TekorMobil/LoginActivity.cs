﻿using System;
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
using System.Threading;

namespace TekorMobil
{
    [Activity(Label = "Login")]
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

            loginButton.Click += (sender, e) =>
            {
                string url = Resources.GetString(Resource.String.service_url);
                var client = new HttpClient
                {
                    BaseAddress = new Uri(url)
                };

                string jsonData = $@"{{""email"" : ""{email.Text}"", ""token"" : ""{RestService.Base64Encode(email.Text + ":" + password.Text)}""}}";

                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                HttpResponseMessage response = null;
                try
                {
                    var progressDialog = ProgressDialog.Show(this, "Please wait...", "Checking account info...", true);
                    new Thread(new ThreadStart(async delegate
                    {
                        //LOAD METHOD TO GET ACCOUNT INFO
                        response = await client.PostAsync("/Login/isLoggedIn", content);

                        if (response?.StatusCode == HttpStatusCode.OK)
                        {
                            (Application as TekorApplication).Email = email.Text;
                            (Application as TekorApplication).Token = RestService.Base64Encode(email.Text + ":" + password.Text);

                            //save application email Token
                            var intent = new Intent(this, typeof(MainActivity));
                            StartActivity(intent);
                            Finish();
                            return;
                        }
                        RunOnUiThread(() =>
                        {
                            errorText.Text = "Wrong User and Password combination";
                            errorText.Visibility = ViewStates.Visible;
                        });
                        
                        RunOnUiThread(() => progressDialog.Hide());
                    })).Start();
                }
                catch (Exception)
                {
                    errorText.Text = "Server is unavailable";
                    errorText.Visibility = ViewStates.Visible;
                    return;
                }

                // var result = await response.Content.ReadAsStringAsync();

            };
        }
    }
}
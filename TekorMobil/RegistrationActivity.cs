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
                    var progressDialog = ProgressDialog.Show(this, "Please wait...", "Registration in progress...", true);
                    new Thread(new ThreadStart(async delegate
                    {
                        //LOAD METHOD TO GET ACCOUNT INFO
                        response = await client.PostAsync("/Registration/Registration", content);

                        if (response?.StatusCode == HttpStatusCode.OK)
                        {
                            //save application email Token
                            RunOnUiThread(() =>
                            {
                                Toast.MakeText(this.ApplicationContext, "Success", ToastLength.Long).Show();
                            });
                            RunOnUiThread(() => progressDialog.Hide());
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

            // Create your application here
        }
    }
}
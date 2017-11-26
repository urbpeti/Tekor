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
using System.Threading;
using System.Net;
using Newtonsoft.Json;

namespace TekorMobil
{
    [Activity(Label = "Goal Details" )]
    public class GoalDetailsActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.GoalDetails);
            var goalID = Intent.GetStringExtra("GoalID");
            ProgressBar progressBar = FindViewById<ProgressBar>(Resource.Id.GoalDetails_progressBar);
            TextView progressBarText = FindViewById<TextView>(Resource.Id.GoalDetails_progressText);
            TextView cuponStaticText = FindViewById<TextView>(Resource.Id.GoalDetails_cuponStaticText);
            EditText cuponText = FindViewById<EditText>(Resource.Id.GoalDetails_cuponText);
            Button addButton = FindViewById<Button>(Resource.Id.GoalDetails_addButton);
            bool addButtonVisible = true;
            var str = $@"/ActiveGoals/GetGoal?usertoken={(Application as TekorApplication).Token}&goalID={goalID}";
            var addUrl = $@"/ActiveGoals/AddProgress?usertoken={(Application as TekorApplication).Token}&goalID={goalID}";

            string url = Resources.GetString(Resource.String.service_url);
            
            var client = new HttpClient
            {
                BaseAddress = new Uri(url)
            };
            
            var progressDialog = ProgressDialog.Show(this, "Please wait...", "", true);
            new Thread(new ThreadStart(async delegate
            {
                //LOAD METHOD TO GET ACCOUNT INFO
                try
                {
                    HttpResponseMessage response = null;
                    response = await client.GetAsync(str);

                    if (response?.StatusCode == HttpStatusCode.OK)
                    {
                        TextView nameText = FindViewById<TextView>(Resource.Id.GoalDetails_nameText);
                        TextView descText = FindViewById<TextView>(Resource.Id.GoalDetails_descText);

                        var content = await response.Content.ReadAsStringAsync();
                        var data = JsonConvert.DeserializeObject<Data.GoalDetailsData>(content);

                        var goalvalue = Convert.ToInt32(data.GoalValue);
                        var actualvalue = Convert.ToInt32(data.ActualValue);

                        if (string.IsNullOrEmpty(data.CuponCode) == false)
                        {
                            addButtonVisible = false;
                            RunOnUiThread(() =>
                            {
                                addButton.Visibility = ViewStates.Invisible;
                                cuponStaticText.Visibility = ViewStates.Visible;
                                cuponText.Text = data.CuponCode;
                                cuponText.Visibility = ViewStates.Visible;
                            });
                        }

                        RunOnUiThread(() =>
                        {
                            nameText.Text = data.RewardName;
                            descText.Text = data.Description;
                            progressBar.Max = goalvalue;
                            progressBar.Progress = actualvalue;
                            progressBarText.Text = $"{actualvalue}/{goalvalue}";
                        });

                    }
                    else
                    {
                        RunOnUiThread(() =>
                        {
                            Android.Widget.Toast.MakeText(this, "Error", Android.Widget.ToastLength.Short).Show();
                        });
                    }

                    RunOnUiThread(() => progressDialog.Hide());
                }
                catch (Exception)
                {
                    RunOnUiThread(() => progressDialog.Hide());
                    RunOnUiThread(() =>
                    {
                        Android.Widget.Toast.MakeText(this, "Error", Android.Widget.ToastLength.Short).Show();
                    });
                }
            })).Start();

            addButton.Click += async (sender, e) =>
            {
                if (addButtonVisible == false)
                {
                    return;
                }
                HttpResponseMessage response = null;
                try
                {
                    new Thread(new ThreadStart(async delegate
                    {
                        response = await client.PostAsync(addUrl, null);

                        if (response?.StatusCode == HttpStatusCode.OK)
                        {
                            var cupon = await response.Content.ReadAsStringAsync();
                            RunOnUiThread(() =>
                            {
                                progressBar.SetProgress(progressBar.Progress + 1, true);
                                progressBarText.Text = $"{progressBar.Progress}/{progressBar.Max}";
                                if (string.IsNullOrEmpty(cupon) == false)
                                {
                                    addButtonVisible = false;
                                    addButton.Visibility = ViewStates.Invisible;
                                    cuponStaticText.Visibility = ViewStates.Visible;
                                    cuponText.Text = cupon;
                                    cuponText.Visibility = ViewStates.Visible;
                                }
                            });
                        }
                        else
                        {
                            RunOnUiThread(() =>
                            {
                                Toast.MakeText(this.ApplicationContext, "Error Occured", ToastLength.Short).Show();
                            });
                        }
                    })).Start();
                }
                catch (Exception)
                {
                    Toast.MakeText(this.ApplicationContext, "Server is unavailable", ToastLength.Short).Show();
                    return;
                }
            };
        }
    }
}
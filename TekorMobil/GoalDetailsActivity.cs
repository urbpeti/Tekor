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

namespace TekorMobil
{
    [Activity(Label = "GoalDetailsActivity")]
    public class GoalDetailsActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.GoalDetails);
            var goalID = Intent.GetStringExtra("GoalID");
            var str = $@"/ActiveGoals/GetGoal?usertoken={(Application as TekorApplication).Token}&goalID={goalID}";

            string url = "http://urbpeti.sch.bme.hu:44310";
            var client = new HttpClient
            {
                BaseAddress = new Uri(url)
            };
            HttpResponseMessage response = null;
            try
            {
                var progressDialog = ProgressDialog.Show(this, "Please wait...", "", true);
                new Thread(new ThreadStart(async delegate
                {
                    //LOAD METHOD TO GET ACCOUNT INFO
                    response = await client.GetAsync(str);

                    if (response?.StatusCode == HttpStatusCode.OK)
                    {
                        //save application email Token
                        RunOnUiThread(() => progressDialog.Hide());
                        return;
                    }
                    RunOnUiThread(() =>
                    {
                        Android.Widget.Toast.MakeText(this, "Error", Android.Widget.ToastLength.Short).Show();
                    });

                    RunOnUiThread(() => progressDialog.Hide());
                })).Start();
            }
            catch (Exception)
            {
                Android.Widget.Toast.MakeText(this, "Error", Android.Widget.ToastLength.Short).Show();
                return;
            }

        }
    }
}
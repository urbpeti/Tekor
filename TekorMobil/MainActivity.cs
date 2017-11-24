using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using System.Collections.Generic;

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
            
            loginButotn.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(GoalsActivity));
                List<string> fos = new List<string>() { "asdf", "asdgsdag"};
                intent.PutStringArrayListExtra("phone_numbers", fos);
                StartActivity(intent);
            };
        }
    }
}


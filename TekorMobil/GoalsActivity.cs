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

namespace TekorMobil
{
    [Activity(Label = "GoalsActivity")]
    public class GoalsActivity : ListActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            List<string> test = new List<string> { "a", "b", "c" };

            ListAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1,test);
        }
    }
}
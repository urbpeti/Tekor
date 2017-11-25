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
using Newtonsoft.Json;

namespace TekorMobil
{
    [Activity(Label = "GoalsActivity")]
    public class GoalsActivity : Activity
    {
        public class GoalsListData {
            public List<GoalsAdapter.ListItemData> goalItems;
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Goals);

            var goals = Intent.GetStringExtra("Data") ?? string.Empty;

            var list = JsonConvert.DeserializeObject<GoalsListData>(goals);

            IList<GoalsAdapter.ListItemData> goalList = list.goalItems;
            var goalAdapter = new GoalsAdapter(this, goalList);
            var goalListView = FindViewById<ListView>(Resource.Id.GoalListView);
            goalListView.Adapter = goalAdapter;

        }
    }
}
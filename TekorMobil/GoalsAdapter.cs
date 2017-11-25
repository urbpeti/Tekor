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
using Java.Lang;

namespace TekorMobil
{
    public class GoalsAdapter : BaseAdapter
    {
        private Activity _activity;
        private List<ListItemData> _goals;

        public GoalsAdapter(Activity activity, IList<ListItemData> goals)
        {
            _activity = activity;
            _goals = new List<ListItemData>(goals);
        }
        public override int Count => +_goals.Count;

        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }

        public override long GetItemId(int position)
        {
            return _goals[position].Description.Length;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? _activity.LayoutInflater.Inflate(Resource.Layout.GoalsListItem, parent, false);
            var goalName = view.FindViewById<TextView>(Resource.Id.GoalName);
            var goalDescription = view.FindViewById<TextView>(Resource.Id.GoalDescription);


            goalName.Text = _goals[position].Name;
            goalDescription.Text = _goals[position].Description;
            return view;
        }

        public class ListItemData {
            public string Description;
            public string Name;
            public ListItemData(string description, string name)
            {
                this.Description = description;
                this.Name = name;
            }
        }
    }
}
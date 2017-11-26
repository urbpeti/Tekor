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

namespace TekorMobil.Data
{
    public class GoalDetailsData
    {
        public string ID { get; set; }
        public string RewardName { get; set; }
        public string Description { get; set; }
        public double GoalValue { get; set; }
        public double ActualValue { get; set; }
        public string UserToken { get; set; }
        public string CuponCode { get; set; }
    }
}
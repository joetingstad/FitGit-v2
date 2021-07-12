using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace fitgit.DataModels
{
    public class ActivityCompleted
    {
        public string user { get; set; }
        public int duration { get; set; }
        public string activity { get; set; }
        public DateTime startTime { get; set; }
        public double calories { get; set; }
    }
}
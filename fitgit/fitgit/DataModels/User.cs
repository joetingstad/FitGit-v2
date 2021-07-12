using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace fitgit.DataModels
{
    public class User
    {
        public string username { get; set; }
        public string password { get; set; }
        public double weight { get; set; }
    }
}
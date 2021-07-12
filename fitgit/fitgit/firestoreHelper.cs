using Android.App;
using Android.Content;
using Android.Gms.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Firebase;
using Firebase.Firestore;
using fitgit.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace fitgit
{
    public class firestoreHelper
    {
        // Global variables for the class
        public static FirebaseFirestore database = GetDataBase();
        public static List<User> listOfUsers = new List<User>();
        public static List<DataModels.Activity> listOfActivities = new List<DataModels.Activity>();
        public static List<ActivityCompleted> listOfActivitiesCompleted = new List<ActivityCompleted>();

        // Store data to reference the logged in user
        public static string UserName;
        public static List<ActivityCompleted> userCompletedActivities = new List<ActivityCompleted>();

        public static FirebaseFirestore GetDataBase()
        {
            FirebaseFirestore database;

            var options = new FirebaseOptions.Builder()
                .SetProjectId("fitgit-v2")
                .SetApplicationId("fitgit-v2")
                .SetApiKey("AIzaSyCGBXmLrWAOxsSrNho7dTlKtxgAvhz0ECA")
                .SetDatabaseUrl("https://fitgit-v2.firebaseio.com")
                .SetStorageBucket("fitgit-v2.appspot.com")
                .Build();

            var app = FirebaseApp.InitializeApp(Android.App.Application.Context, options);
            database = FirebaseFirestore.GetInstance(app);

            return database;
        }

        public static void fetchUserData()
        {
            database.Collection("Users").Get().AddOnSuccessListener(new UserSuccessListener());
        }

        public static void fetchActivityData()
        {
            database.Collection("Activities").Get().AddOnSuccessListener(new ActivitySuccessListener());
        }

        public static void FetchandListen()
        {
            database.Collection("Users").AddSnapshotListener(new UserSuccessListener());
        }

        public static void fetchActivityCompletedData()
        {
            database.Collection("CompletedActivities").Get().AddOnSuccessListener(new ActivityCompletedSuccessListener());
        }
    }

}
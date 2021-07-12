using Android.App;
using Android.Content;
using Android.Gms.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Firestore;
using fitgit.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace fitgit
{
    public class ActivityCompletedSuccessListener : Java.Lang.Object, IOnSuccessListener, Firebase.Firestore.IEventListener
    {
        public void OnEvent(Java.Lang.Object result, FirebaseFirestoreException error)
        {
            var snapshot = (QuerySnapshot)result;

            if (!snapshot.IsEmpty)
            {
                var documents = snapshot.Documents;

                firestoreHelper.listOfActivitiesCompleted.Clear();
                foreach (DocumentSnapshot item in documents)
                {
                    ActivityCompleted activityCompleted = new ActivityCompleted();
                    activityCompleted.user = item.Get("username") != null ? item.Get("username").ToString() : "";
                    activityCompleted.duration = item.Get("duration") != null ? Convert.ToInt32(item.Get("duration")) : 0;
                    activityCompleted.activity = item.Get("activity") != null ? item.Get("activity").ToString() : "";
                    activityCompleted.startTime = item.Get("startTime") != null ? DateTime.Parse(item.Get("startTime").ToString()) : DateTime.Parse("");
                    activityCompleted.calories = item.Get("calories") != null ? Convert.ToDouble(item.Get("calories")) : 0;

                    firestoreHelper.listOfActivitiesCompleted.Add(activityCompleted);
                }
            }
        }

        public void OnSuccess(Java.Lang.Object result)
        {
            var snapshot = (QuerySnapshot)result;

            if (!snapshot.IsEmpty)
            {
                var documents = snapshot.Documents;
                firestoreHelper.userCompletedActivities.Clear();
                firestoreHelper.listOfActivitiesCompleted.Clear();
                foreach (DocumentSnapshot item in documents)
                {
                    ActivityCompleted activityCompleted = new ActivityCompleted();

                    activityCompleted.user = item.Get("username") != null ? item.Get("username").ToString() : "";
                    activityCompleted.duration = item.Get("duration") != null ? Convert.ToInt32(item.Get("duration")) : 0;
                    activityCompleted.activity = item.Get("activity") != null ? item.Get("activity").ToString() : "";
                    activityCompleted.startTime = item.Get("startTime") != null ? DateTime.Parse(item.Get("startTime").ToString()) : DateTime.Parse("");
                    activityCompleted.calories = item.Get("calories") != null ? Convert.ToDouble(item.Get("calories")) : 0;

                    if (activityCompleted.user == firestoreHelper.UserName)
                    {
                        firestoreHelper.userCompletedActivities.Add(activityCompleted);
                    }

                    firestoreHelper.listOfActivitiesCompleted.Add(activityCompleted);
                }
            }
        }
    }
}
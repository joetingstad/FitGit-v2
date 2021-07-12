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
    public class ActivitySuccessListener : Java.Lang.Object, IOnSuccessListener, Firebase.Firestore.IEventListener
    {
        public void OnEvent(Java.Lang.Object result, FirebaseFirestoreException error)
        {
            var snapshot = (QuerySnapshot)result;

            if (!snapshot.IsEmpty)
            {
                var documents = snapshot.Documents;

                firestoreHelper.listOfActivities.Clear();
                foreach (DocumentSnapshot item in documents)
                {
                    DataModels.Activity activity = new DataModels.Activity();
                    activity.name = item.Get("Name") != null ? item.Get("Name").ToString() : "";
                    activity.MET = item.Get("MET") != null ? Convert.ToInt32(item.Get("MET")) : 0;
                    activity.type = item.Get("Type") != null ? item.Get("Type").ToString() : "";

                    firestoreHelper.listOfActivities.Add(activity);
                }
            }
        }

        public void OnSuccess(Java.Lang.Object result)
        {
            var snapshot = (QuerySnapshot)result;

            if (!snapshot.IsEmpty)
            {
                var documents = snapshot.Documents;

                firestoreHelper.listOfActivities.Clear();
                foreach (DocumentSnapshot item in documents)
                {
                    DataModels.Activity activity = new DataModels.Activity();
                    activity.name = item.Get("Name") != null ? item.Get("Name").ToString() : "";
                    activity.MET = item.Get("MET") != null ? Convert.ToInt32(item.Get("MET")) : 0;
                    activity.type = item.Get("Type") != null ? item.Get("Type").ToString() : "";

                    firestoreHelper.listOfActivities.Add(activity);
                }
            }
        }
    }
}
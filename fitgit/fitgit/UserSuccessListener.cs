using Android.App;
using Android.Content;
using Android.Gms.Tasks;
using Firebase;
using Firebase.Firestore;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using fitgit.DataModels;

namespace fitgit
{
    public class UserSuccessListener : Java.Lang.Object, IOnSuccessListener, Firebase.Firestore.IEventListener
    {
        public void OnSuccess(Java.Lang.Object result)
        {
            var snapshot = (QuerySnapshot)result;

            if (!snapshot.IsEmpty)
            {
                var documents = snapshot.Documents;

                firestoreHelper.listOfUsers.Clear();
                foreach (DocumentSnapshot item in documents)
                {
                    User user = new User();
                    user.username = item.Get("Username") != null ? item.Get("Username").ToString() : "";
                    user.password = item.Get("Password") != null ? item.Get("Password").ToString() : "";
                    user.weight = item.Get("Weight") != null ? Convert.ToDouble(item.Get("Weight")) : 0;

                    firestoreHelper.listOfUsers.Add(user);
                }
            }
        }

        public void OnEvent(Java.Lang.Object value, FirebaseFirestoreException error)
        {
            var snapshot = (QuerySnapshot)value;

            if (!snapshot.IsEmpty)
            {
                var documents = snapshot.Documents;

                firestoreHelper.listOfUsers.Clear();
                foreach (DocumentSnapshot item in documents)
                {
                    User user = new User();
                    user.username = item.Get("Username") != null ? item.Get("Username").ToString() : "";
                    user.password = item.Get("Password") != null ? item.Get("Password").ToString() : "";

                    firestoreHelper.listOfUsers.Add(user);
                }

                //if (dataAdapter != null)
                //{
                //    dataAdapter.NotifyDataSetChanged();
                //}
            }
        }
    }
}
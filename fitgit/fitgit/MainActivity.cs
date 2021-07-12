using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Firebase.Firestore;
using Firebase;
using Java.Util;
using System;
using Android.Gms.Tasks;
using fitgit.DataModels;
using System.Collections.Generic;
using Android.Support.V7.Widget;
using fitgit.Adapters;
using System.Linq;


namespace fitgit
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        EditText username_text;
        EditText password_text;
        Button login_btn;
        Button sign_up_btn;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            
            // Set view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            firestoreHelper.fetchUserData();
            connected_views();
        }

        void connected_views()
        {
            username_text = (EditText)FindViewById(Resource.Id.username_line);
            password_text = (EditText)FindViewById(Resource.Id.password_line);
            login_btn = (Button)FindViewById(Resource.Id.login_btn);
            sign_up_btn = (Button)FindViewById(Resource.Id.sign_up_btn);

            sign_up_btn.Click += sign_up_clicked;
            login_btn.Click += login_clicked;
        }

        private void sign_up_clicked(object sender, System.EventArgs e)
        {
            // Alert dialog builder to create dialogs to notify a user when they have entered invalid data
            Android.App.AlertDialog.Builder alertDialogBuilder = new Android.App.AlertDialog.Builder(this);

            // Hash map to store the fields and then push to the Firestore database
            HashMap map = new HashMap();
            List<string> existing_usernames = new List<string>();
            firestoreHelper.fetchUserData();

            // Strings to hold the username and password fields
            foreach (User temp in firestoreHelper.listOfUsers)
            {
                existing_usernames.Add(temp.username.ToString());
            }
            string temp_username_holder = username_text.Text;
            string temp_pw_holder = password_text.Text;
            bool uname_valid = false;
            bool pw_valid = false;

            // Username input sanitization
            if (temp_username_holder.Length < 5)
            {
                // Create and display an alert when username does not meet requirements
                Android.App.AlertDialog alert = alertDialogBuilder.Create();
                alert.SetTitle("Invalid Username");
                alert.SetMessage("Username must be longer than four characters.");
                alert.SetButton("OK", (c, ev) =>
                {
                    // OK Button click task to clear data fields after alert is dismissed
                    username_text.Text = "";
                    password_text.Text = "";
                    alert.Dismiss();
                });

                // Show the alert
                alert.Show();
            }
            else if (existing_usernames.Contains(temp_username_holder))
            {
                Android.App.AlertDialog alert = alertDialogBuilder.Create();
                alert.SetTitle("Invalid Username");
                alert.SetMessage("That username is already taken. Please choose another!");
                alert.SetButton("OK", (c, ev) =>
                {
                    // OK Button click task to clear data fields after alert is dismissed
                    username_text.Text = "";
                    password_text.Text = "";
                    alert.Dismiss();
                });

                // Show the alert
                alert.Show();
            }
            else
            {
                // Username and passed the sanitization check, add to the hashmap
                map.Put("Username", temp_username_holder);
                uname_valid = true;
            }

            if (temp_pw_holder.Length < 6)
            {
                Android.App.AlertDialog alert = alertDialogBuilder.Create();
                alert.SetTitle("Invalid Password");
                alert.SetMessage("Password must be at least 6 characters long.");
                alert.SetButton("OK", (c, ev) =>
                {
                    // Clear the password text and dismiss the alert
                    password_text.Text = "";
                    alert.Dismiss();
                });

                // Show the alert
                alert.Show();
            }
            else if (!temp_pw_holder.Contains("!") && !temp_pw_holder.Contains("#") && !temp_pw_holder.Contains("$") && !temp_pw_holder.Contains("&")
                && !temp_pw_holder.Contains("*") && !temp_pw_holder.Contains("-") && !temp_pw_holder.Contains("+") && !temp_pw_holder.Contains(".")
                && !temp_pw_holder.Contains("^"))
            {
                Android.App.AlertDialog alert = alertDialogBuilder.Create();
                alert.SetTitle("Invalid Password");
                alert.SetMessage("Password must contain one or more of the following special characters: !, #, $, &, *, ^, -, +, ., ^ ");
                alert.SetButton("OK", (c, ev) =>
                {
                    password_text.Text = "";
                    alert.Dismiss();
                });

                // Show the alert
                alert.Show();
            }
            else
            {
                // Password passed sanitization check, add to the hashmap
                map.Put("Password", temp_pw_holder);
                map.Put("Weight", 0);
                pw_valid = true;
            }

            if (uname_valid && pw_valid)
            {
                // Push the data to the database. Document name will be the username
                DocumentReference docRef = firestoreHelper.database.Collection("Users").Document(temp_username_holder);
                   
                docRef.Set(map);
                firestoreHelper.fetchActivityData();
                firestoreHelper.UserName = temp_username_holder;
                firestoreHelper.fetchActivityCompletedData();
                // Show the main page of the application
                System.Threading.Thread.Sleep(500);
                StartActivity(typeof(tabbedPage));
                //SetContentView(Resource.Layout.main_tabed_page);
            }
        }

        private void login_clicked(object sender, System.EventArgs e)
        {
            // Alert dialog builder to create dialogs to notify a user when they have entered invalid data
            Android.App.AlertDialog.Builder alertDialogBuilder = new Android.App.AlertDialog.Builder(this);

            // Hash map to store the fields and then push to the Firestore database
            HashMap map = new HashMap();
            List<string> existing_usernames = new List<string>();

            // Strings to hold the username and password fields
            string temp_username_holder = username_text.Text;
            string temp_pw_holder = password_text.Text;

            // Search for users based on the username that was inputted
            firestoreHelper.fetchUserData();
            var match = firestoreHelper.listOfUsers.FirstOrDefault(u => u.username == temp_username_holder);
            if (match == null)
            {
                Android.App.AlertDialog alert = alertDialogBuilder.Create();
                alert.SetTitle("Invalid Credentials");
                alert.SetMessage("Invalid username or password.");
                alert.SetButton("OK", (c, ev) =>
                {
                    username_text.Text = "";
                    password_text.Text = "";
                    alert.Dismiss();
                });
                alert.Show();
            }
            else
            {
                if (temp_pw_holder == match.password.ToString())
                {
                    firestoreHelper.fetchActivityData();
                    firestoreHelper.UserName = temp_username_holder;
                    firestoreHelper.fetchActivityCompletedData();
                    // Login successful. Send user to the main tabbed page.
                    System.Threading.Thread.Sleep(500);
                    StartActivity(typeof(tabbedPage));
                }
                else
                {
                    Android.App.AlertDialog alert = alertDialogBuilder.Create();
                    alert.SetTitle("Invalid Credentials");
                    alert.SetTitle("Invalid username or password.");
                    alert.SetButton("OK", (c, ev) =>
                    {
                        username_text.Text = "";
                        password_text.Text = "";
                        alert.Dismiss();
                    });
                    alert.Show();
                }
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
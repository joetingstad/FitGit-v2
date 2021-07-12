using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Android.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using fitgit.DataModels;
using Java.Util;
using Firebase.Firestore;

namespace fitgit.Fragments
{
    public class userFragment : Android.Support.V4.App.Fragment
    {
        string _title;
        string _icon;
        string _page;

        public static userFragment NewInstance(string title, string icon, string page)
        {
            var fragment = new userFragment();
            fragment.Arguments = new Bundle();
            fragment.Arguments.PutString("title", title);
            fragment.Arguments.PutString("icon", icon);
            fragment.Arguments.PutString("page", page);

            return fragment;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            if (Arguments != null)
            {
                if (Arguments.ContainsKey("title"))
                    _title = (string)Arguments.Get("title");

                if (Arguments.ContainsKey("icon"))
                    _icon = (string)Arguments.Get("icon");

                if (Arguments.ContainsKey("page"))
                    _page = (string)Arguments.Get("page");
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragView, container, false);

            if (_page == "HomePage")
            {
                // Inflate the homepage view
                view = inflater.Inflate(Resource.Layout.HomePage, container, false);
                var homePageLastWeek = view.FindViewById<LinearLayout>(Resource.Id.LastWeekActivites);
                var homePageWeekSummary = view.FindViewById<LinearLayout>(Resource.Id.WeekSummary);

                // Get the values for the dates over the last seven days
                DateTime[] pastWeek = Enumerable.Range(0, 7).Select(i => DateTime.Now.Date.AddDays(-i)).ToArray();

                // Set up some counter variables to display weekly totals
                int numberLastWeekActivites = 0;
                int exerciseMinutes = 0;
                double total_cals = 0;

                // Find activities completed over the last 7 days
                foreach (ActivityCompleted activityCompleted in firestoreHelper.userCompletedActivities)
                {
                    if (pastWeek.Contains(activityCompleted.startTime.Date))
                    {
                        // Increment the counter values
                        numberLastWeekActivites += 1;
                        exerciseMinutes += activityCompleted.duration;
                        total_cals += activityCompleted.calories;

                        // Set up layout to display values
                        LinearLayout myLayout = new LinearLayout(Context);
                        TextView activity_name = new TextView(Context);
                        activity_name.SetPadding(5, 5, 20, 5);
                        TextView date = new TextView(Context);
                        DateTime dateCompleted = activityCompleted.startTime;
                        string temp = activityCompleted.activity;
                        activity_name.Text = temp;
                        activity_name.SetTextColor(Android.Graphics.Color.ParseColor("#FF6000"));
                        date.Text = dateCompleted.ToString("MM/dd/yyyy");
                        date.SetTextColor(Android.Graphics.Color.ParseColor("#FF6000"));
                        myLayout.AddView(activity_name);
                        myLayout.AddView(date);
                        homePageLastWeek.AddView(myLayout);
                    }
                }

                // Setup layout to hold more values
                LinearLayout WeeklySummay = new LinearLayout(Context);
                WeeklySummay.Orientation = Orientation.Vertical;
                TextView numberActivites = new TextView(Context);
                numberActivites.SetPadding(10, 10, 10, 10);
                TextView minutesExercised = new TextView(Context);
                minutesExercised.SetPadding(10, 10, 10, 10);

                TextView total_week_calories = new TextView(Context);
                total_week_calories.SetPadding(10, 10, 10, 10);

                // Set layouts to the layout being shown on the screen
                total_week_calories.Text = "Total Calories Burned: " + total_cals.ToString();
                minutesExercised.Text = "Total Activity Minutes: " + exerciseMinutes.ToString(); 
                numberActivites.Text = "Activities Completed: " + numberLastWeekActivites.ToString();
                numberActivites.SetTextColor(Android.Graphics.Color.ParseColor("#FF6000"));
                minutesExercised.SetTextColor(Android.Graphics.Color.ParseColor("#FF6000"));
                total_week_calories.SetTextColor(Android.Graphics.Color.ParseColor("#FF6000"));
                WeeklySummay.AddView(numberActivites);
                WeeklySummay.AddView(minutesExercised);
                WeeklySummay.AddView(total_week_calories);
                homePageWeekSummary.AddView(WeeklySummay);
            }
            else if (_page == "Activities")
            {
                view = inflater.Inflate(Resource.Layout.Activities, container, false);
                var activity_page = view.FindViewById<LinearLayout>(Resource.Id.Activities);
                List<string> activity_names = new List<string>();
                foreach (DataModels.Activity act in firestoreHelper.listOfActivities)
                {
                    activity_names.Add(act.name);
                }
                ListView activity_view = new ListView(Context);
                ArrayAdapter<string> adapter = new ArrayAdapter<string>(Context, Android.Resource.Layout.SimpleListItem1, activity_names);
                activity_view.Adapter = adapter;
                activity_view.ItemClick += ActivityList_ItemClicked;
                activity_page.AddView(activity_view);
            }
            else if (_page == "UserPage")
            {
                view = inflater.Inflate(Resource.Layout.UserPage, container, false);
                var user_page = view.FindViewById<LinearLayout>(Resource.Id.UserPage);
                var curr_user = firestoreHelper.listOfUsers.FirstOrDefault(u => u.username == firestoreHelper.UserName);

                // Username textview object
                TextView username_textview = new TextView(Context);
                username_textview.Text = curr_user.username;
                username_textview.SetTextSize(Android.Util.ComplexUnitType.Pt, 14);
                //username_textview
                username_textview.SetPadding(10, 10, 10, 10);
                user_page.AddView(username_textview);

                // Edit password button
                Button edit_password_btn = new Button(Context);
                edit_password_btn.Text = "Change password (requires login)";

                // Add behavior to the edit_password button
                edit_password_btn.Click += EditPass_ClickEvent;
                user_page.AddView(edit_password_btn);

                // Edit user weight button
                Button edit_weight_btn = new Button(Context);
                edit_weight_btn.Text = "Enter user weight";
                edit_weight_btn.Click += EditWeight_ClickEvent;
                user_page.AddView(edit_weight_btn);
            }
            else if (_page == "ExplorePage")
            {
                var linLayout = view.FindViewById<LinearLayout>(Resource.Id.Explore);
                //textView.SetText(_title, null);
            }

            return view;
        }

        void EditWeight_ClickEvent(object sender, EventArgs e)
        {
            Android.App.AlertDialog.Builder alertDialogBuilder = new Android.App.AlertDialog.Builder(Context);
            var curr_user = firestoreHelper.listOfUsers.FirstOrDefault(u => u.username == firestoreHelper.UserName);

            // Create popup for user to enter weight value
            Android.App.AlertDialog alert = alertDialogBuilder.Create();
            alert.SetTitle("Enter Weight");
            alert.SetMessage("Enter user weight in kg. This value is used to more accurately calculate the number of calories burned during a given activity.");
            EditText weight_line = new EditText(Context);
            weight_line.InputType = Android.Text.InputTypes.NumberFlagDecimal;
            alert.SetView(weight_line);

            // Set button functionality
            alert.SetButton("Enter", (c, ev) =>
            {
                HashMap map = new HashMap();
                var weight_entered = weight_line.Text;
                try
                {
                    double temp_weight = Convert.ToDouble(weight_entered);

                    // Set map values
                    map.Put("Username", curr_user.username);
                    map.Put("Password", curr_user.password);
                    map.Put("Weight", temp_weight);

                    DocumentReference docRef = firestoreHelper.database.Collection("Users").Document(curr_user.username);
                    docRef.Set(map);

                    firestoreHelper.fetchUserData();
                }
                catch
                {
                    Android.App.AlertDialog newAlert = alertDialogBuilder.Create();
                    alert.Dismiss();
                    newAlert.SetTitle("Error");
                    newAlert.SetMessage("User entered an invalid value in the weight field.");
                    newAlert.SetButton("OK", (c, ev) =>
                    {
                        newAlert.Dismiss();
                    });
                    newAlert.Show();
                }
            });
            alert.SetButton2("Cancel", (c, ev) =>
            {
                alert.Dismiss();
            });
            alert.Show();
        }

        void ActivityList_ItemClicked(object sender, AdapterView.ItemClickEventArgs e)
        {
            // Define a dialog builder
            Android.App.AlertDialog.Builder alertDialogBuilder = new Android.App.AlertDialog.Builder(Context);

            // Store the activity that the user clicked in the list
            DataModels.Activity act_clicked = firestoreHelper.listOfActivities[e.Position];

            // Initialize an alert
            Android.App.AlertDialog alert = alertDialogBuilder.Create();

            // Set alert fields
            alert.SetTitle("Activity Data");
            alert.SetMessage("Enter the duration of your completed activity (in minutes).");
            EditText duration_line = new EditText(Context);
            alert.SetView(duration_line);

            // Define alert behavior when "Enter" button is clicked
            alert.SetButton("Enter", (c, ev) =>
            {
                // Add a completed activity instance to the database
                HashMap map = new HashMap();
                string duration_entered = duration_line.Text;
                int duration_as_num;
                try
                {
                    // Store the current user as an object
                    var curr_user = firestoreHelper.listOfUsers.FirstOrDefault(u => u.username == firestoreHelper.UserName);
                    duration_as_num = Convert.ToInt32(duration_entered);

                    // Add values to the map
                    map.Put("duration", duration_as_num);
                    map.Put("activity", act_clicked.name);
                    map.Put("startTime", DateTime.Now.ToString());
                    map.Put("username", firestoreHelper.UserName);

                    // Calculate the number of calories burned using the MET value and the duration
                    double temp_weight = curr_user.weight;
                    if (temp_weight == 0)
                    {
                        temp_weight = 80.7;
                    }
                    double cals_burned_per_min = act_clicked.MET * (temp_weight / 200);
                    double cals_burned = cals_burned_per_min * duration_as_num;
                    map.Put("calories", cals_burned);

                    // Add new data to the DB
                    DocumentReference docRef = firestoreHelper.database.Collection("CompletedActivities").Document();
                    docRef.Set(map);
                    firestoreHelper.fetchActivityCompletedData();
                }
                catch
                {
                    // User entered invalid value for the duration
                    Android.App.AlertDialog newAlert = alertDialogBuilder.Create();
                    alert.Dismiss();
                    newAlert.SetTitle("Error");
                    newAlert.SetMessage("User entered an invalid value in the duration field.");
                    newAlert.SetButton("OK", (c, ev) =>
                    {
                        newAlert.Dismiss();
                    });
                    newAlert.Show();
                }
            });
            // Define alert behavior when "Cancel" button is clicked
            alert.SetButton2("Cancel", (c, ev) =>
            {
                // Close the popup
                alert.Dismiss();
            });

            // Show the alert
            alert.Show();
        }

        void EditPass_ClickEvent(object sender, EventArgs e)
        {
            // Request password for current logged in user
            var curr_user = firestoreHelper.listOfUsers.FirstOrDefault(u => u.username == firestoreHelper.UserName);
            Android.App.AlertDialog.Builder alertDialogBuilder = new Android.App.AlertDialog.Builder(Context);
            Android.App.AlertDialog alert = alertDialogBuilder.Create();
            alert.SetTitle("Password");
            alert.SetMessage("Enter password.");

            // Build a line for user to enter text
            EditText pass_line = new EditText(Context);
            pass_line.InputType = Android.Text.InputTypes.TextVariationPassword;
            alert.SetView(pass_line);

            alert.SetButton("Login", (c, ev) =>
            {
                if (pass_line.Text == curr_user.password)
                {
                    alert.Dismiss();

                    Android.App.AlertDialog valid_alert = alertDialogBuilder.Create();
                    LinearLayout layout = new LinearLayout(Context);

                    // Add two textview objects to the linear layout
                    EditText pw_line1 = new EditText(Context);
                    EditText pw_line2 = new EditText(Context);
                    pw_line1.InputType = Android.Text.InputTypes.TextVariationPassword;
                    pw_line2.InputType = Android.Text.InputTypes.TextVariationPassword;
                    layout.Orientation = Orientation.Vertical;
                    layout.AddView(pw_line1);
                    layout.AddView(pw_line2);

                    valid_alert.SetTitle("Change Password");
                    valid_alert.SetMessage("Enter matching new passwords below.");

                    valid_alert.SetView(layout);
                    valid_alert.SetButton("Change", (c, ev) =>
                    {
                        // Password reset button clicked
                        if (pw_line1.Text == pw_line2.Text && pw_line1.Text.Length > 5)
                        {
                            string t = pw_line1.Text;
                            if (!t.Contains("!") && !t.Contains("#") && !t.Contains("$") && !t.Contains("&")
                                && !t.Contains("*") && !t.Contains("-") && !t.Contains("+") && !t.Contains(".")
                                && !t.Contains("^"))
                            {
                                Android.App.AlertDialog special_char_alert = alertDialogBuilder.Create();
                                special_char_alert.SetTitle("Invalid Password");
                                special_char_alert.SetMessage("Password must contain one or more of the following special characters: !, #, $, &, *, ^, -, +, ., ^ ");
                                special_char_alert.SetButton("OK", (c, ev) =>
                                {
                                    pw_line1.Text = "";
                                    pw_line2.Text = "";
                                    special_char_alert.Dismiss();
                                });

                                // Show the alert
                                special_char_alert.Show();
                            }
                            else
                            {
                                // Update the user's password and push the updated instance of the user to the database
                                curr_user.password = pw_line1.Text;

                                HashMap map = new HashMap();
                                map.Put("Username", curr_user.username);
                                map.Put("Password", curr_user.password);
                                map.Put("Weight", curr_user.weight);

                                DocumentReference docRef = firestoreHelper.database.Collection("Users").Document(curr_user.username);
                                docRef.Set(map);
                                firestoreHelper.fetchUserData();
                            }
                        }
                        else
                        {
                            Android.App.AlertDialog no_match_dialog = alertDialogBuilder.Create();
                            no_match_dialog.SetTitle("Password Invalid");
                            no_match_dialog.SetMessage("Passwords did not match, or length was less than 6 characters.");
                            no_match_dialog.SetButton("OK", (c, ev) =>
                            {
                                no_match_dialog.Dismiss();
                            });
                            no_match_dialog.Show();
                        }
                    });
                    valid_alert.SetButton2("Cancel", (c, ev) =>
                    {
                        valid_alert.Dismiss();
                    });
                    valid_alert.Show();
                }
                else
                {
                    // User entered invalid password. Dismiss current alert
                    alert.Dismiss();

                    // Notify user that password was invalid
                    Android.App.AlertDialog invalid_alert = alertDialogBuilder.Create();
                    invalid_alert.SetTitle("Invalid Credentials");
                    invalid_alert.SetMessage("The password entered was incorrect.");
                    invalid_alert.SetButton("OK", (c, ev) =>
                    {
                        invalid_alert.Dismiss();
                    });
                    invalid_alert.Show();
                }
            });
            alert.SetButton2("Cancel", (c, ev) =>
            {
                alert.Dismiss();
            });
            alert.Show();
        }
    }
}
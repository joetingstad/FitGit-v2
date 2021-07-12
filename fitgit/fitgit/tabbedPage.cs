using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using fitgit.Adapters;
using fitgit.Fragments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace fitgit
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class tabbedPage : FragmentActivity
    {
        ViewPager myViewPager;
        BottomNavigationView nav_view;
        Android.Support.V4.App.Fragment[] _fragments;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.main_tabed_page);
            initTabs();

            // Find view and set the adapter
            myViewPager = FindViewById<ViewPager>(Resource.Id.viewpager);
            myViewPager.Adapter = new ViewPagerAdapter(SupportFragmentManager, _fragments);

            // Add event handler
            myViewPager.PageSelected += ViewPager_PageSelected;

            // Find navigation view and add event handler
            nav_view = FindViewById<BottomNavigationView>(Resource.Id.navigation_bar);
            nav_view.NavigationItemSelected += NavigationView_NavigationItemSelected;
        }

        void initTabs()
        {
            _fragments = new Android.Support.V4.App.Fragment[4];
            _fragments[0] = userFragment.NewInstance("Home", "home_icon", "HomePage");
            _fragments[1] = userFragment.NewInstance("Activities", "activity_icon", "Activities");
            _fragments[2] = userFragment.NewInstance("Explore", "explore_icon", "ExplorePage");
            _fragments[3] = userFragment.NewInstance("User", "user_icon", "UserPage");

            /*{
                userFragment.NewInstance("Home", "home_icon", "HomePage"),
                userFragment.NewInstance("Activities", "activity_icon", "Activities"),
                userFragment.NewInstance("Explore", "explore_icon", "ExplorePage"),
                userFragment.NewInstance("User", "user_icon", "UserPage")
            };*/
        }

        void NavigationView_NavigationItemSelected(object sender, BottomNavigationView.NavigationItemSelectedEventArgs e)
        {
            myViewPager.SetCurrentItem(e.Item.Order, true);
        }

        private void ViewPager_PageSelected(object sender, ViewPager.PageSelectedEventArgs e)
        {
            var item = nav_view.Menu.GetItem(e.Position);
            nav_view.SelectedItemId = item.ItemId;
        }

        protected override void OnDestroy()
        {
            myViewPager.PageSelected -= ViewPager_PageSelected;
            nav_view.NavigationItemSelected -= NavigationView_NavigationItemSelected;
            base.OnDestroy();
        }
    }
}

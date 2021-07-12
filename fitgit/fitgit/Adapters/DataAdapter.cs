using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using fitgit.DataModels;
using System;
using System.Collections.Generic;

namespace fitgit.Adapters
{
    class DataAdapter : RecyclerView.Adapter
    {
        public event EventHandler<DataAdapterClickEventArgs> ItemClick;
        public event EventHandler<DataAdapterClickEventArgs> ItemLongClick;
        List <User> userList;

        public DataAdapter(List <User> user_data) 
        {
            userList = user_data;
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {

            //Setup your layout here
            View itemView = null;

            itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.main_tabed_page, parent, false);
            var vh = new DataAdapterViewHolder(itemView, OnClick, OnLongClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var user = userList[position];

            // Replace the contents of the view with that element
            var holder = viewHolder as DataAdapterViewHolder;
            holder.usernameText.Text = user.username;
            holder.passwordText.Text = user.password;
        }

        public override int ItemCount => userList.Count;

        void OnClick(DataAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(DataAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);

    }

    public class DataAdapterViewHolder : RecyclerView.ViewHolder
    {
        //public TextView TextView { get; set; }
        public TextView usernameText { get; set; }
        public TextView passwordText { get; set; }

        public DataAdapterViewHolder(View itemView, Action<DataAdapterClickEventArgs> clickListener,
                            Action<DataAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            //TextView = v;
            //usernameText = (TextView)ItemView.FindViewById(Resource.Id.username);
            //passwordText = (TextView)ItemView.FindViewById(Resource.Id.password);

            itemView.Click += (sender, e) => clickListener(new DataAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new DataAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
        }
    }

    public class DataAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}
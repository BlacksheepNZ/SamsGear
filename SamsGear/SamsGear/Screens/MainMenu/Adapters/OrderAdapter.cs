using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using System.IO;
using System.Drawing;
using Android.Graphics;

namespace SamsGear
{
    public class OrderAdapter : BaseAdapter
    {
        Activity context;

        List<OrderEntity> orders;

        public OrderAdapter(Activity context, List<OrderEntity> orders)
        {
            this.context = context;

            this.orders = orders;
        }
        public override int Count
        {
            get { return this.orders.Count(); }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (view == null) // no view to re-use, create new
            {
                view = LayoutInflater.From(context).Inflate(Resource.Layout.SingleOrderView, parent, false);

                TextView textView1 = view.FindViewById<TextView>(Resource.Id.textView1);    //DateTime
                TextView textView2 = view.FindViewById<TextView>(Resource.Id.textView2);    //Name
                TextView textView3 = view.FindViewById<TextView>(Resource.Id.textView3);    //Email

                textView1.Text = orders[position].DateTime;
                textView2.Text = orders[position].Name;
                textView3.Text = orders[position].Email;
            }
            else
            {
                view = convertView;
            }

            return view;
        }
    }
}
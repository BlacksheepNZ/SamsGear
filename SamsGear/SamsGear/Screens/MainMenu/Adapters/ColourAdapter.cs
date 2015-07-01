using System;
using System.Xml;
using System.Collections.Generic;
using System.Collections;
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
    public class ColourAdapter : BaseAdapter
    {
        Activity context;

        private String[] colours;

		public ColourAdapter(Activity context, String[] colours)
        {
            this.context = context;
            this.colours = colours;
        }
        public override int Count
        {
            get { return this.colours.Count(); }
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
                view = LayoutInflater.From(context).Inflate(Resource.Layout.SingleColourView, parent, false);

                ImageView imageView = view.FindViewById<ImageView>(Resource.Id.imageView1);
				TextView textview = view.FindViewById<TextView> (Resource.Id.myImageViewText);

                String c = colours[position];

				textview.Text = c;
			
				if (c.Equals ("White")) {
					textview.SetTextColor(Android.Graphics.Color.Black);
				} else {
					textview.SetTextColor(Android.Graphics.Color.White);
				}

				if (c.Equals ("White")) {
					imageView.SetImageResource (Resource.Drawable.White);
				} else if (c.Equals ("Lime")) {
					imageView.SetImageResource (Resource.Drawable.Lime);
				} else if (c.Equals ("Navy")) {
					imageView.SetImageResource (Resource.Drawable.Navy);
				} else if (c.Equals ("Red")) {
					imageView.SetImageResource (Resource.Drawable.Red);
				} else if (c.Equals ("Black")) {
					imageView.SetImageResource (Resource.Drawable.Black);
				} else if (c.Equals ("HeatherGrey")) {
					imageView.SetImageResource (Resource.Drawable.HeatherGrey);
				} else if (c.Equals ("Maroon")) {
					imageView.SetImageResource (Resource.Drawable.Maroon);
				} else if (c.Equals ("RoyalBlue")) {
					imageView.SetImageResource (Resource.Drawable.RoyalBlue);
				} else if (c.Equals ("CarolinaBlue")) {
					imageView.SetImageResource (Resource.Drawable.CarolinaBlue);
				} else if (c.Equals ("Navy")) {
					imageView.SetImageResource (Resource.Drawable.Navy);
				} else if (c.Equals ("SportGrey")) {
					imageView.SetImageResource (Resource.Drawable.SportGrey);
				} else {
					imageView.SetImageResource (Resource.Drawable.White);
				}
            }
            else
            {
                view = convertView;
            }

            return view;
        }
    }
}
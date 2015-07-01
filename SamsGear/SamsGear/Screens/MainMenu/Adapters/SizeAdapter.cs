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
    public class SizeAdapter : BaseAdapter
    {
        Activity context;

        private String[] size;

        public SizeAdapter(Activity context, String[] size)
        {
            this.context = context;
            this.size = size;
        }
        public override int Count
        {
            get { return this.size.Count(); }
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
                view = LayoutInflater.From(context).Inflate(Resource.Layout.SizeView, parent, false);

				TextView sizeText = view.FindViewById<TextView>(Resource.Id.myImageViewText);
				ImageView imageView = view.FindViewById<ImageView>(Resource.Id.imageView1);

                String c = size[position];

				sizeText.SetTextColor(Android.Graphics.Color.Black);

                if (c.Equals("S"))
                {
                    sizeText.Text = "Small";
                    imageView.SetImageResource(Resource.Drawable.White);
                }
                else if (c.Equals("M"))
                {
                    sizeText.Text = "Medium";
                    imageView.SetImageResource(Resource.Drawable.White);
                }
                else if (c.Equals("L"))
                {
                    sizeText.Text = "Large";
                    imageView.SetImageResource(Resource.Drawable.White);
                }
                else if (c.Equals("XL"))
                {
                    sizeText.Text = "XLarge";
                    imageView.SetImageResource(Resource.Drawable.White);
                }
                else if (c.Equals("2XL"))
                {
                    sizeText.Text = "2XLarge";
                    imageView.SetImageResource(Resource.Drawable.White);
                }
                else if (c.Equals("3XL"))
                {
                    sizeText.Text = "3XLarge";
                    imageView.SetImageResource(Resource.Drawable.White);
                }
                else if (c.Equals("4XL"))
                {
                    sizeText.Text = "4XLarge";
                    imageView.SetImageResource(Resource.Drawable.White);
                }
                else if (c.Equals("5XL"))
                {
                    sizeText.Text = "5XLarge";
                    imageView.SetImageResource(Resource.Drawable.White);
                }
                else
                {
                    sizeText.Text = "Null";
                    imageView.SetImageResource(Resource.Drawable.White);
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
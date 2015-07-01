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
    public class ProductAdapter : BaseAdapter
    {
        Activity context;

        private String ImageName;

        List<DesignEntity> DesignEntity = new List<DesignEntity>();
        List<ColourEntity> ColourEntity = new List<ColourEntity>();
        List<SizeEntity> SizeEntity = new List<SizeEntity>();

        List<int> d = new List<int>();
        List<int> t = new List<int>();
        List<int> c = new List<int>();
        List<int> s = new List<int>();

        public ProductAdapter(Activity context,
            List<DesignEntity> DesignEntity,
            List<ColourEntity> ColourEntity,
            List<SizeEntity> SizeEntity,
            List<int> d,
            List<int> c,
            List<int> s)
        {
            this.context = context;

            this.DesignEntity.AddRange(DesignEntity);
            this.ColourEntity.AddRange(ColourEntity);
            this.SizeEntity.AddRange(SizeEntity);

            this.d.AddRange(d);
            this.c.AddRange(c);
            this.s.AddRange(s);
        }
        public override int Count
        {
            get { return this.d.Count(); }
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
                view = LayoutInflater.From(context).Inflate(Resource.Layout.SingleProductView, parent, false);

                ImageView imageView = view.FindViewById<ImageView>(Resource.Id.imageView1); //image
                TextView textView1 = view.FindViewById<TextView>(Resource.Id.textView1);    //size
                TextView textView2 = view.FindViewById<TextView>(Resource.Id.textView2);    //color
                TextView textView3 = view.FindViewById<TextView>(Resource.Id.textView3);    //price

                try
                {
                    ImageName = DesignEntity[d[position]].Image;
                    textView1.Text = ColourEntity[c[position]].Color;
                    textView2.Text = SizeEntity[s[position]].Size;
                    textView3.Text = DesignEntity[d[position]].Price.ToString();
                }catch(Exception ex)
                {
                    throw ex;
                }

                String img = ImageName;

                if (img.Equals("FiaBotz"))
                    imageView.SetImageResource(Resource.Drawable.FiaBotz);
                else if (img.Equals("TokoUso"))
                    imageView.SetImageResource(Resource.Drawable.TokoUso);
                else if (img.Equals("Beatz"))
                    imageView.SetImageResource(Resource.Drawable.Beatz);
                else if (img.Equals("Blues"))
                    imageView.SetImageResource(Resource.Drawable.Blues);
                else if (img.Equals("Bulu"))
                    imageView.SetImageResource(Resource.Drawable.Bulu);
                else if (img.Equals("Usos"))
                    imageView.SetImageResource(Resource.Drawable.Usos);
                else if (img.Equals("CookIsland"))
                    imageView.SetImageResource(Resource.Drawable.CookIslands);
                else if (img.Equals("Fobzilla"))
                    imageView.SetImageResource(Resource.Drawable.Fobzilla);
                else if (img.Equals("iFob"))
                    imageView.SetImageResource(Resource.Drawable.iFob);
                else if (img.Equals("iFobMini"))
                    imageView.SetImageResource(Resource.Drawable.iFobMini);
                else if (img.Equals("MaoriStrong"))
                    imageView.SetImageResource(Resource.Drawable.MaoriStrong);
                else if (img.Equals("Maroons"))
                    imageView.SetImageResource(Resource.Drawable.Maroons);
                else if (img.Equals("NiueStrong"))
                    imageView.SetImageResource(Resource.Drawable.NiueStrong);
                else if (img.Equals("ObeyDeez"))
                    imageView.SetImageResource(Resource.Drawable.ObeyDeez);
                else if (img.Equals("SonsSth"))
                    imageView.SetImageResource(Resource.Drawable.SonsSth);
                else if (img.Equals("ToaSamoa"))
                    imageView.SetImageResource(Resource.Drawable.ToaSamoa);
                else if (img.Equals("Warriors"))
                    imageView.SetImageResource(Resource.Drawable.Warriors);
                else
                {
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
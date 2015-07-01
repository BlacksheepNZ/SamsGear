using System;
using System.Collections.Generic;
using System.Linq;

using Android.App;
using Android.Views;
using Android.Widget;

namespace SamsGear
{
    public class StockAdapter : BaseAdapter
    {
		private readonly Activity context;

		private readonly String ImageName;
		private readonly TShirtEntity[] tshirtEntity;
		private readonly List<string> Size;
		private readonly List<string> Colour;

        public StockAdapter(Activity context, 
            String ImageName, 
            TShirtEntity[] tshirtEntity,
            List<string> Size,
            List<string> Colour)
        {
            this.context = context;

            this.ImageName = ImageName;
            this.tshirtEntity = tshirtEntity;

            this.Size = Size;
            this.Colour = Colour;
        }
        public override int Count
        {
            get { return this.tshirtEntity.Count(); }
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
                view = LayoutInflater.From(context).Inflate(Resource.Layout.SingleStockView, parent, false);

                ImageView imageView = view.FindViewById<ImageView>(Resource.Id.imageView1); //image
                TextView textView1 = view.FindViewById<TextView>(Resource.Id.textView5);    //color
                TextView textView2 = view.FindViewById<TextView>(Resource.Id.textView4);    //size
                EditText editText1 = view.FindViewById<EditText>(Resource.Id.editText1);    //nzstock
                EditText editText2 = view.FindViewById<EditText>(Resource.Id.editText2);    //austock

                String img = ImageName;

                textView1.Text = Colour[position];
                textView2.Text = Size[position];

                editText1.Text = tshirtEntity[position].StockNZ.ToString();
                editText2.Text = tshirtEntity[position].StockAU.ToString();

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

            
            Button buttonInsert = view.FindViewById<Button>(Resource.Id.button1);
            buttonInsert.Focusable = false;
            buttonInsert.FocusableInTouchMode = false;
            buttonInsert.Clickable = true;

            buttonInsert.Click += delegate
            {
                TShirtEntity t = new TShirtEntity();
                t.ID = tshirtEntity[position].ID;
                t.Name = tshirtEntity[position].Name;
                t.StockNZ = tshirtEntity[position].StockNZ;
                t.StockAU = tshirtEntity[position].StockAU;

                using (Database db = new Database())
                {
                    db.AddORUpdateTShirtEntity(t);
                }
            };

            return view;
        } 
    }
}
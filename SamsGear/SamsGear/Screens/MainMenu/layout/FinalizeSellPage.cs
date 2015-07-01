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

using System.Data;
using SQLite;

namespace SamsGear
{
    [Activity(Label = "Sams Gear", Icon = "@drawable/icon", ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape, Theme = "@android:style/Theme.NoTitleBar")]
    public class FinalizeSellPage : Activity
    {
        private ImageButton buttonPay;
        //---------------------

        private EditText name;
        private EditText email;

        //---------------------

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            try
            {
                SetContentView(Resource.Layout.FinalizeSellPage);

                buttonPay = FindViewById<ImageButton>(Resource.Id.imageButton3);
                buttonPay.Click += buttonPay_Click;

                name = FindViewById<EditText>(Resource.Id.editText1);
                email = FindViewById<EditText>(Resource.Id.editText2);

                ListView listCart = FindViewById<ListView>(Resource.Id.listView1);
                listCart.Adapter = new CartAdapter(this, SellPage.cartItems);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //private void PopulateView()
        //{
        //    cost.Text = MainActivity.totalcost.ToString();
        //}

        private void buttonPay_Click(object sender, EventArgs e)
        {
            try
            {
                buttonPay.Click -= buttonPay_Click;

                OrderEntity order = new OrderEntity();
                order.Name = name.Text;
                order.Email = email.Text;
                order.DateTime = DateTimeSQLite(DateTime.Now);

                int oCount = 0;
                int pCount = 0;
                using (var db = new Database())
                {
                    oCount = db.Order().Count() + 1;

                    db.AddORUpdateOrderEntity(order);
                }

                List<int> pID = new List<int>();
                foreach (var items in SellPage.finalCartItem)
                {
                    ProductEntity p = new ProductEntity();
                    p.DesignID = items.Item1;
                    p.TShirtID = items.Item2;
                    p.TShirtColorID = items.Item3;
                    p.TShirtSizeID = items.Item4;

                    using (var db = new Database())
                    {
                        db.AddORUpdateProductEntity(p);

                        pCount = db.Product().Count() + 1;
                        OrderProductEntity op = new OrderProductEntity();
                        op.OrderID = oCount;
                        op.ProductID = pCount;
                        db.AddORUpdateOrderProductEntity(op);
                    }
                }

                SellPage.totalcost = 0;
                SellPage.cartItems.Clear();
                SellPage.finalCartItem.Clear();

                var MainPage = new Intent(this, typeof(SellPage));
                StartActivity(MainPage);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        private string DateTimeSQLite(DateTime datetime)
        {
            string dateTimeFormat = "{0}-{1}-{2} {3}:{4}:{5}.{6}";
            return string.Format(dateTimeFormat, datetime.Year, datetime.Month, datetime.Day, datetime.Hour, datetime.Minute, datetime.Second, datetime.Millisecond);
        }
    }
}
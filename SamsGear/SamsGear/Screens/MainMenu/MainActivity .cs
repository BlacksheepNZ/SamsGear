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

namespace SamsGear
{
    [Activity(Label = "Sams Gear", MainLauncher = true, Icon = "@drawable/icon", ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape, Theme = "@android:style/Theme.NoTitleBar")]
    public class MainActivity : TabActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            //need to change this later on release
            INIDatabase(); //preload database on device

            base.OnCreate(bundle);
            SetContentView(Resource.Layout.MainMenu);

            CreateTab(typeof(DayPage), "", "", Resource.Drawable.Day);
            CreateTab(typeof(SellPage), "", "", Resource.Drawable.Sell);
            CreateTab(typeof(StockPage), "", "", Resource.Drawable.Stock);
        }

        protected override void OnRestart()
        {
            SetContentView(Resource.Layout.MainMenu);

            CreateTab(typeof(DayPage), "", "", Resource.Drawable.Day);
            CreateTab(typeof(SellPage), "", "", Resource.Drawable.Sell);
            CreateTab(typeof(StockPage), "", "", Resource.Drawable.Stock);

            base.OnRestart();
        }

        private void CreateTab(Type activityType, string tag, string label, int drawableId)
        {
            var intent = new Intent(this, activityType);
            intent.AddFlags(ActivityFlags.NewTask);

            var spec = TabHost.NewTabSpec(tag);
            var drawableIcon = Resources.GetDrawable(drawableId);
            spec.SetIndicator(label, drawableIcon);
            spec.SetContent(intent);

            TabHost.AddTab(spec);
        }

        public void INIDatabase()
        {
            //Reads from local database file
            //Transfers file to device

            var readStream = Resources.OpenRawResource(Resource.Raw.easyDB);
            FileStream writeStream = new FileStream(Database.databasePath, FileMode.OpenOrCreate, FileAccess.Write);
            Database.ReadWriteStream(readStream, writeStream);
            writeStream.Close();
        }
    }
}
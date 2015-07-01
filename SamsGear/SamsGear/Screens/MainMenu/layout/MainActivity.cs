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

namespace SamsGear
{
    [Activity(Label = "Sams Gear", Icon = "@drawable/icon", ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape, Theme = "@android:style/Theme.NoTitleBar")]
    public class MainActivity : TabActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.MainMenu);

            //TabHost.TabSpec spec;
            //Intent intent;

            //intent = new Android.Content.Intent(this, typeof(DayPage));
            //intent.AddFlags(ActivityFlags.NewTask);
            //spec = TabHost.NewTabSpec("Day");
            //spec.SetIndicator("Day");
            //spec.SetContent(intent);
            //TabHost.AddTab(spec);

            //CreateTab(typeof(DayPage), "DayPage", "DayPage", Resource.Drawable.Day);
            //CreateTab(typeof(SellPage), "SellPage", "SellPage", Resource.Drawable.Sell);
            //CreateTab(typeof(StockPage), "StockPage", "StockPage", Resource.Drawable.Stock);
        }

        //private void CreateTab(Type activityType, string tag, string label, int drawableId)
        //{
        //    var intent = new Intent(this, activityType);
        //    intent.AddFlags(ActivityFlags.NewTask);

        //    var spec = TabHost.NewTabSpec(tag);
        //    var drawableIcon = Resources.GetDrawable(drawableId);
        //    spec.SetIndicator(label, drawableIcon);
        //    spec.SetContent(intent);

        //    TabHost.AddTab(spec);
        //}
    }
}
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
    public static class Utility
    {
        public static string GetPictureDirectory(Application application) //Returns picture folder location
        {
            string directory;
            //checks for external hard drive
            if(Android.OS.Environment.ExternalStorageState == Android.OS.Environment.MediaMounted)
            {
                directory = application.GetExternalFilesDir(Android.OS.Environment.DirectoryPictures).ToString();
            }
            else
            {
                //use local hard drive
                directory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyPictures);
            }
            return directory;
        }
    }
}

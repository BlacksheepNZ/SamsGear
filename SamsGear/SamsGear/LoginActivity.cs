//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using System.Text;
//using System.Security.Cryptography;

//using Android.App;
//using Android.Content;
//using Android.Runtime;
//using Android.Views;
//using Android.Widget;
//using Android.OS;

//using SQLite;

//namespace EasyMode
//{
//    [Activity(Label = "LoginActivity",  Icon = "@drawable/icon")]
//    public class LoginActivity : Activity
//    {
//        SQLiteConnection conn;

//        TableQuery<UserEntity> objUserEnitity;

//        protected override void OnCreate(Bundle bundle)
//        {
//            base.OnCreate(bundle);

//            // Set our view from the "main" layout resource
//            //SetContentView(Resource.Layout.Login);

//            string dbPath = @" Data Source=.\SQLEXPRESS;
//                                      AttachDbFilename=C:\USERS\MATTHEW\DESKTOP\EASYMODE\EASYMODE\EASYMODE\EASYMODE.MDF;
//                                      Integrated Security=True;
//                                      Connect Timeout=30;
//                                      User Instance=True;
//                                      MultipleActiveResultSets=true";

//            conn = new SQLiteConnection(dbPath);

//            objUserEnitity = conn.Table<UserEntity>();

//            Button login = FindViewById<Button>(Resource.Id.buttonSignIn);
//            login.Click += login_Click;
//        }

//        void login_Click(object sender, EventArgs e)
//        {
//            EditText username = FindViewById<EditText>(Resource.Id.editTextUserNameToLogin);
//            EditText password = FindViewById<EditText>(Resource.Id.editTextPasswordToLogin);
//            Login(username.Text, password.Text);
//        }

//        public void Login(string username, string password)
//        {
//            foreach (UserEntity objUser in objUserEnitity)
//            {
//                if (objUser.AuthenticateUser(username, password, conn))
//                {
//                    Intent intent = new Android.Content.Intent(new Intent(this, typeof(MainActivity)));
//                    StartActivity(intent);
//                }
//                else
//                {
//                    Error("Could not authenticate " + username);
//                }
//            }
//        }

//        private void Error(string error)
//        {
//            Toast.MakeText(this, error, ToastLength.Short).Show();
//        }
//    }
//}
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Security.Cryptography;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using System.Data;

using SQLite.Net;

using System.Linq;
using Path = System.IO.Path;

using SQLiteNetExtensions.Attributes;
using SQLiteNetExtensions.Extensions;
using SQLiteNetExtensions.Exceptions;

namespace SamsGear
{

    [Activity(Label = "Sams Gear", Icon = "@drawable/icon", ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape, Theme = "@android:style/Theme.NoTitleBar")]
    public class DayPage : Activity
    {
        //---------------------

        //View components

        private GridView gridViewAdapter;

        //---------------------

        //Entity Refs

        private List<DesignEntity> design;
        private List<DesignTShirtEntity> designtshirt = new List<DesignTShirtEntity>();
        private List<TShirtEntity> tshirt = new List<TShirtEntity>();

        private List<TShirtColourEntity> tshirtcolour = new List<TShirtColourEntity>();
        private List<ColourEntity> colour = new List<ColourEntity>();

        private List<TShirtSizeEntity> tshirtsize = new List<TShirtSizeEntity>();
        private List<SizeEntity> size = new List<SizeEntity>();

        //sales

        private List<OrderEntity> order = new List<OrderEntity>();
        private List<OrderProductEntity> orderproduct = new List<OrderProductEntity>();
        private List<ProductEntity> product = new List<ProductEntity>();

        //---------------------

        private List<string> designImages = new List<string>();

        private List<string> finalColour = new List<string>();
        private List<int> tshirtColourList = new List<int>();

        private List<string> finalSize = new List<string>();
        private List<int> tshirtSizeList = new List<int>();

        //---------------------

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.DayPage);

            //---------------------
            //Load resource views

            gridViewAdapter = FindViewById<GridView>(Resource.Id.gridView1);

            PopulateOrder();
        }

        private void buttonStock_Click(object sender, EventArgs e)
        {
            StartActivity(new Intent(this, typeof(StockPage)));
        }

        private void buttonSell_Click(object sender, EventArgs e)
        {
            StartActivity(new Intent(this, typeof(SellPage)));
        }

        /// <summary>
        /// Local all database entrys
        /// </summary>
        private void LoadDatabaseEntitys()
        {
            try
            {
                //load data from database into our entity system
                using (Database db = new Database())
                {
                    design = db.GetDesignEntity();
                    designtshirt = db.GetDesignTShirtEntity();
                    tshirt = db.GetTShirtEntity();

                    tshirtcolour = db.GetTShirtColourEntity();
                    colour = db.GetColourEntity();

                    tshirtsize = db.GetTShirtSizeEntity();
                    size = db.GetSizeEntity();

                    order = db.GetOrderEntity();
                    orderproduct = db.GetOrderProductEntity();
                    product = db.GetProductEntity();
                }
            }
            catch (Exception ex)
            {
                //catch any errors
                throw ex;
            }
        }

        #region DayView

        private void PopulateOrder()
        {
            gridViewAdapter = null;

            try
            {
                LoadDatabaseEntitys();

                if (order.Any())
                {
                    GridView adapter = FindViewById<GridView>(Resource.Id.gridView1);
                    adapter.Adapter = new OrderAdapter(this, order);
                    adapter.SetNumColumns(1);
                    adapter.SetGravity(GravityFlags.CenterHorizontal);
                    adapter.SetColumnWidth(15);

                    gridViewAdapter = adapter;
                    gridViewAdapter.FastScrollEnabled = true;
                    gridViewAdapter.ItemClick += OrderAdapter_ItemClick;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void OrderAdapter_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            try
            {
                gridViewAdapter.ItemClick -= OrderAdapter_ItemClick;
                gridViewAdapter = null;

                List<Tuple<int, int, int, int>> finalProduct = new List<Tuple<int, int, int, int>>();
                List<int> orderProductList = new List<int>();
                List<Tuple<int, int, int, int>> noProductDupes = new List<Tuple<int, int, int, int>>();

                List<OrderProductEntity> indexOrder = order[e.Position].OrderProductEntity;

                //------------------------
                for (int productIndex = 0; productIndex < indexOrder.Count(); productIndex++)
                {
                    if (tshirt.Any())
                    {
                        int tID = indexOrder[productIndex].ProductID;

                        foreach (ProductEntity p in product)
                        {
                            if (p.OrderProductEntity != null)
                            {
                                if (tID == p.ID)
                                {
                                    foreach (OrderProductEntity selectedIndex in p.OrderProductEntity)//gets x
                                    {
                                        orderProductList.Add(selectedIndex.ProductID);//gets y
                                    }
                                }
                            }
                        }
                    }
                }

                //------------------------
                for (int productIndex = 0; productIndex < orderProductList.Count(); productIndex++)
                {
                    if (product.Any())
                    {
                        int cID = orderProductList[productIndex];

                        foreach (ProductEntity p in product)
                        {
                            if (cID == p.ID)
                            {
                                finalProduct.Add(new Tuple<int, int, int, int>(p.DesignID, p.TShirtID, p.TShirtColorID, p.TShirtSizeID));
                            }
                        }
                    }
                }

                noProductDupes = finalProduct.Distinct().ToList();

                List<int> d = new List<int>();
                List<int> c = new List<int>();
                List<int> s = new List<int>();

                if (noProductDupes.Any())
                {
                    for (int i = 0; i < noProductDupes.Count; i++)
                    {
                        d.Add(noProductDupes[i].Item1);
                        c.Add(noProductDupes[i].Item3);
                        s.Add(noProductDupes[i].Item4);
                    }

                    GridView adapter = FindViewById<GridView>(Resource.Id.gridView1);
                    adapter.Adapter = new ProductAdapter(this, design, colour, size, d, c, s);
                    adapter.SetNumColumns(1);
                    adapter.SetGravity(GravityFlags.CenterHorizontal);
                    adapter.SetColumnWidth(15);

                    gridViewAdapter = adapter;
                    gridViewAdapter.FastScrollEnabled = true;
                    //gridViewAdapter.ItemClick += PopulateSizeList_ItemClick;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        protected override void OnResume()
        {
            base.OnResume();
        }
    }
}


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
using System.Globalization;

using SQLiteNetExtensions.Attributes;
using SQLiteNetExtensions.Extensions;
using SQLiteNetExtensions.Exceptions;

namespace SamsGear
{

    [Activity(Label = "Sams Gear", Icon = "@drawable/icon", ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape, Theme = "@android:style/Theme.NoTitleBar")]
    public class StockPage : Activity
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

        private List<TShirtEntity> finalTShirt = new List<TShirtEntity>();
        private List<string> noDupes = new List<string>();

        //---------------------

        private List<string> designImages = new List<string>();

        private List<string> finalColour = new List<string>();
        private List<int> tshirtColourList = new List<int>();

        private List<string> finalSize = new List<string>();
        private List<int> tshirtSizeList = new List<int>();

        //---------------------

        //---------------------

        protected override void OnCreate(Bundle bundle)
        {
            //              Database layout
            //
            //  Design  -> DesignTShirt <- TShirt
            //                                  -> TShirtColour <- Colour
            //                                  -> TShirtSize   <- Size
            //
            //
            //  Order   -> OrderProduct <- Product

            //Initialize entitys
            design = new List<DesignEntity>();
            designtshirt = new List<DesignTShirtEntity>();

            tshirt = new List<TShirtEntity>();

            tshirtcolour = new List<TShirtColourEntity>();
            colour = new List<ColourEntity>();

            tshirtsize = new List<TShirtSizeEntity>();
            size = new List<SizeEntity>();

            order = new List<OrderEntity>();
            orderproduct = new List<OrderProductEntity>();
            product = new List<ProductEntity>();

            LoadDatabaseEntitys();

            base.OnCreate(bundle);
            SetContentView(Resource.Layout.StockPage);

            //---------------------

            LoadDesignImages();
            PopulateStockList();
        }

        protected override void OnRestart()
        {
            PopulateStockList();

            base.OnRestart();
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

        private void LoadDesignImages()
        {
            foreach (var d in design) //Get all design items
            {
                designImages.Add(d.Image);
            }
        }

        #region StockView

        private void PopulateStockList()
        {
            //reset
            gridViewAdapter = null;

            try
            {
                //Populate mainview with design images
                if (designImages.Any())
                {
                    GridView adapter = FindViewById<GridView>(Resource.Id.gridView1);
                    adapter.Adapter = new DesignAdapter(this, designImages.ToArray());
                    adapter.SetNumColumns(6);
                    adapter.SetColumnWidth(15);

                    gridViewAdapter = adapter;
                    gridViewAdapter.FastScrollEnabled = true;
                    gridViewAdapter.ItemClick += PopulateStockView_ItemClick;
                }
            }
            catch
            {
                throw;
            }
        }
        private void PopulateStockView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            //reset
            gridViewAdapter.ItemClick -= PopulateStockView_ItemClick;

            finalTShirt.Clear();

            #region get tshirt
            //get selected DesignTShirt
            List<DesignTShirtEntity> indexDesign = design[e.Position].DesignTShirtEntity;

            //------------------------
            for (int tshirtIndex = 0; tshirtIndex < indexDesign.Count(); tshirtIndex++)
            {
                if (tshirt.Any())
                {
                    //Find matching TShirt
                    int tID = indexDesign[tshirtIndex].IDTShirt;

                    foreach (TShirtEntity t in tshirt)
                    {
                        //using selected DesignTShirt get all matching items
                        if (tID == t.ID)
                        {
                            //Add items
                            finalTShirt.Add(t);
                        }
                    }
                }
            }
            #endregion

            #region get colours

            finalColour.Clear();
            tshirtColourList.Clear();

            for (int tshirtIndex = 0; tshirtIndex < indexDesign.Count(); tshirtIndex++)
            {
                if (tshirt.Any())
                {
                    int tID = indexDesign[tshirtIndex].IDTShirt;

                    foreach (TShirtEntity t in tshirt)
                    {
                        if (t.TShirtColourEntity != null)
                        {
                            if (tID == t.ID)
                            {
                                foreach (TShirtColourEntity selectedTShirt in t.TShirtColourEntity)//gets x
                                {
                                    tshirtColourList.Add(selectedTShirt.IDColour);//gets y
                                }
                            }
                        }
                    }
                }
            }

            for (int colourIndex = 0; colourIndex < tshirtColourList.Count(); colourIndex++)
            {
                if (colour.Any())
                {
                    int cID = tshirtColourList[colourIndex];

                    foreach (ColourEntity c in colour)
                    {
                        if (cID == c.ID)
                        {
                            finalColour.Add(c.Color);
                        }
                    }
                }
            }

            #endregion get colours

            #region get size

            finalSize.Clear();
            tshirtSizeList.Clear();

            for (int tshirtIndex = 0; tshirtIndex < indexDesign.Count(); tshirtIndex++)
            {
                if (tshirt.Any())
                {
                    int tID = indexDesign[tshirtIndex].IDTShirt;

                    foreach (TShirtEntity t in tshirt)
                    {
                        if (t.TShirtSizeEntity != null)
                        {
                            if (tID == t.ID)
                            {
                                foreach (TShirtSizeEntity selectedTShirt in t.TShirtSizeEntity)//gets x
                                {
                                    tshirtSizeList.Add(selectedTShirt.IDSize);//gets y
                                }
                            }
                        }
                    }
                }
            }

            for (int colourIndex = 0; colourIndex < tshirtSizeList.Count(); colourIndex++)
            {
                if (size.Any())
                {
                    int cID = tshirtSizeList[colourIndex];

                    foreach (SizeEntity s in size)
                    {
                        if (cID == s.ID)
                        {
                            finalSize.Add(s.Size);
                        }
                    }
                }
            }

            #endregion get size

            //Populate mainview
            if (finalTShirt.Any())
            {
                GridView adapter = FindViewById<GridView>(Resource.Id.gridView1);
                adapter.Adapter = new StockAdapter(this, design[e.Position].Image, finalTShirt.ToArray(), finalColour, finalSize);
                adapter.SetNumColumns(4);
                adapter.SetColumnWidth(15);
                gridViewAdapter = adapter;
            }
        }

        #endregion

        protected override void OnResume()
        {
            base.OnResume();
        }
    }
}


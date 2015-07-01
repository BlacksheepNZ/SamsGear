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

//using System.Data;

//using SQLite.Net;

//using System.Linq;
//using Path = System.IO.Path;
//using System.Globalization;

//using SQLiteNetExtensions.Attributes;
//using SQLiteNetExtensions.Extensions;
//using SQLiteNetExtensions.Exceptions;

//namespace EasyMode
//{

//    [Activity(Label = "MainActivity",  Icon = "@drawable/icon", Theme = "@android:style/Theme.NoTitleBar")] //ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape
//    public class MainActivity : Activity
//    {
//        //---------------------

//        //View components

//        private GridView gridViewAdapter;
//        private ListView listCart;

//        private TextView cost;

//        //---------------------

//        //Button components

//        private ImageButton buttonSearch;
//        private ImageButton buttonPay;

//        private ImageButton buttonDay;
//        private ImageButton buttonSell;
//        private ImageButton buttonStock;

//        private ImageButton buttonChangeView;

//        //---------------------

//        //Entity Refs

//        private List<DesignEntity> design;
//        private List<DesignTShirtEntity> designtshirt = new List<DesignTShirtEntity>();
//        private List<TShirtEntity> tshirt = new List<TShirtEntity>();

//        private List<TShirtColourEntity> tshirtcolour = new List<TShirtColourEntity>();
//        private List<ColourEntity> colour = new List<ColourEntity>();

//        private List<TShirtSizeEntity> tshirtsize = new List<TShirtSizeEntity>();
//        private List<SizeEntity> size = new List<SizeEntity>();

//        //sales

//        private List<OrderEntity> order = new List<OrderEntity>();
//        private List<OrderProductEntity> orderproduct = new List<OrderProductEntity>();
//        private List<ProductEntity> product = new List<ProductEntity>();

//        //---------------------

//        /// <summary>
//        /// Image, Colour, Size, Price
//        /// </summary>
//        public static List<Tuple<string, string, string, decimal>> cartItems;

//        /// <summary>
//        /// DesignID, TShirtID, TShirtColour, TShirtSize
//        /// </summary>
//        public static List<Tuple<int,  int, int, int>> finalCartItem;

//        //Main item selection
//        private int selectedDesign;
//        private int selectedTShirt;
//        private int selectedTShirtColour;
//        private int selectedTShirtSize;

//        public static decimal totalcost = 0;

//        private List<TShirtEntity> finalTShirt = new List<TShirtEntity>();
//        private List<string> noDupes = new List<string>();

//        //---------------------

//        private List<string> designImages = new List<string>();

//        private List<string> finalColour = new List<string>();
//        private List<int> tshirtColourList = new List<int>();

//        private List<string> finalSize = new List<string>();
//        private List<int> tshirtSizeList = new List<int>();

//        //---------------------

//        private enum SelectedButton
//        {
//            Day,
//            Sell,
//            Stock,
//            PayScreen,
//        }

//        CurrentView currentView = CurrentView.Day;
//        private enum CurrentView
//        {
//            Day,
//            Sell,
//            Stock,
//        }

//        OrderType orderType = OrderType.Alphabetical;
//        private enum OrderType
//        {
//            Ascending,
//            Descending,
//            Alphabetical,
//        }

//        //---------------------

//        protected override void OnCreate(Bundle bundle)
//        {
//            //Initialize database
//            INIDatabase();

//            //              Database layout
//            //
//            //  Design  -> DesignTShirt <- TShirt
//            //                                  -> TShirtColour <- Colour
//            //                                  -> TShirtSize   <- Size
//            //
//            //
//            //  Order   -> OrderProduct <- Product

//            //Initialize entitys
//            design = new List<DesignEntity>();
//            designtshirt = new List<DesignTShirtEntity>();

//            tshirt = new List<TShirtEntity>();

//            tshirtcolour = new List<TShirtColourEntity>();
//            colour = new List<ColourEntity>();

//            tshirtsize = new List<TShirtSizeEntity>();
//            size = new List<SizeEntity>();

//            order = new List<OrderEntity>();
//            orderproduct = new List<OrderProductEntity>();
//            product = new List<ProductEntity>();

//            LoadDatabaseEntitys ();

//            base.OnCreate(bundle);
//            SetContentView(Resource.Layout.MainPage);

//            //---------------------
//            //Load resource views

//            cost = FindViewById<TextView>(Resource.Id.myImageViewText);
//            cost.Text = "Total" + "\n" + "0.00";

//            gridViewAdapter = FindViewById<GridView>(Resource.Id.gridView1);

//            listCart = FindViewById<ListView>(Resource.Id.listView1);
//            listCart.ItemClick += listCart_ItemClick;

//            buttonDay = FindViewById<ImageButton>(Resource.Id.imageButton3);
//            buttonDay.Click += buttonDay_Click;

//            buttonSell = FindViewById<ImageButton>(Resource.Id.imageButton4);
//            buttonSell.Click += buttonSell_Click;

//            buttonStock = FindViewById<ImageButton>(Resource.Id.imageButton5);
//            buttonStock.Click += buttonStock_Click;

//            buttonPay = FindViewById<ImageButton>(Resource.Id.imageButton1);
//            buttonPay.Click += buttonPay_Click;

//            buttonChangeView = FindViewById<ImageButton>(Resource.Id.imageButton6);
//            buttonChangeView.Click += buttonChangeView_Click;

//            buttonChangeView.SetImageResource(Resource.Drawable.UpArrow);

//            //Local Content

//            SelectButton(SelectedButton.Sell);

//            //---------------------

//            finalCartItem = new List<Tuple<int, int, int, int>>();
//            cartItems = new List<Tuple<string, string, string, decimal>>();

//            //---------------------

//            LoadDesignImages();

//            //---------------------

//            PopulateProductList();
//        }

//        private void buttonPay_Click(object sender, EventArgs e)
//        {
//            if (finalCartItem.Count() > 0)
//            {
//                var SellPage = new Intent(this, typeof(FinalizeSellPage));

//                StartActivity(SellPage);
//            }
//            else
//            {
//                Toast.MakeText(ApplicationContext, "No items in cart.", ToastLength.Long);
//            }
//        }

//        /// <summary>
//        /// Local all database entrys
//        /// </summary>
//        private void LoadDatabaseEntitys()
//        {
//            try
//            {
//                //load data from database into our entity system
//                using (Database db = new Database())
//                {
//                    design = db.GetDesignEntity();
//                    designtshirt = db.GetDesignTShirtEntity();
//                    tshirt = db.GetTShirtEntity();

//                    tshirtcolour = db.GetTShirtColourEntity();
//                    colour = db.GetColourEntity();

//                    tshirtsize = db.GetTShirtSizeEntity();
//                    size = db.GetSizeEntity();

//                    order = db.GetOrderEntity();
//                    orderproduct = db.GetOrderProductEntity();
//                    product = db.GetProductEntity();
//                }
//            }
//            catch (Exception ex)
//            {
//                //catch any errors
//                throw ex;
//            }
//        }

//        OrderType currentSelected = OrderType.Ascending;
//        OrderType lastSelected;
//        private void buttonChangeView_Click(object sender, EventArgs e)
//        {
//            //check what view where in
//            switch (currentView)
//            {
//                case CurrentView.Day:
//                    {
//                        break;
//                    }
//                case CurrentView.Sell:
//                    {
//                        if (currentSelected == OrderType.Ascending)
//                        {
//                            buttonChangeView.SetImageResource(Resource.Drawable.UpArrow);
//                            designImages = Order(designImages, currentSelected);

//                            //switch
//                            lastSelected = OrderType.Descending;
//                            currentSelected = lastSelected;
//                            break;
//                        }
//                        else if (currentSelected == OrderType.Descending)
//                        {
//                            buttonChangeView.SetImageResource(Resource.Drawable.DownArrow);
//                            designImages = Order(designImages, currentSelected);

//                            //switch
//                            lastSelected = OrderType.Ascending;
//                            currentSelected = lastSelected;
//                            break;
//                        }

//                        break;
//                    }
//                case CurrentView.Stock:
//                    {

//                        break;
//                    }

//            }

//            PopulateProductList();

//        }

//        public void INIDatabase()
//        {
//            //Reads from local database file
//            //Transfers file to device

//            var readStream = Resources.OpenRawResource(Resource.Raw.easyDB);
//            FileStream writeStream = new FileStream(Database.databasePath, FileMode.OpenOrCreate, FileAccess.Write);
//            Database.ReadWriteStream(readStream, writeStream);
//            writeStream.Close();
//        }

//        private void SelectButton(SelectedButton sb)
//        {
//            switch (sb)
//            {
//                case SelectedButton.Day:
//                    {
//                        currentView = CurrentView.Day;
//                        buttonDay.SetImageResource(Resource.Drawable.DaySelected);
//                        buttonSell.SetImageResource(Resource.Drawable.Sell);
//                        buttonStock.SetImageResource(Resource.Drawable.Stock);

//                        RemoveCartView();
//                        break;
//                    }
//                case SelectedButton.Sell:
//                    {
//                        currentView = CurrentView.Sell;
//                        buttonDay.SetImageResource(Resource.Drawable.Day);
//                        buttonSell.SetImageResource(Resource.Drawable.SellSelected);
//                        buttonStock.SetImageResource(Resource.Drawable.Stock);

//                        AddCartView();
//                        break;
//                    }
//                case SelectedButton.Stock:
//                    {
//                        currentView = CurrentView.Stock;
//                        buttonDay.SetImageResource(Resource.Drawable.Day);
//                        buttonSell.SetImageResource(Resource.Drawable.Sell);
//                        buttonStock.SetImageResource(Resource.Drawable.StockSelected);

//                        RemoveCartView();
//                        break;
//                    }

//            }
//        }

//        #region Add/Remove CartView

//        private void RemoveCartView()
//        {
//            //expand view
//            LinearLayout view = FindViewById<LinearLayout>(Resource.Id.linearLayout4);
//            view.LayoutParameters.Width = 1828;

//            //breaker
//            LinearLayout breaker = FindViewById<LinearLayout>(Resource.Id.linearLayout9);
//            breaker.Visibility = ViewStates.Gone;

//            //Cart area
//            LinearLayout layoutCart = FindViewById<LinearLayout>(Resource.Id.linearLayout5);
//            layoutCart.Visibility = ViewStates.Gone;
//        }

//        private void AddCartView()
//        {
//            //expand view
//            LinearLayout view = FindViewById<LinearLayout>(Resource.Id.linearLayout4);
//            view.LayoutParameters.Width = 1400;

//            //breaker
//            LinearLayout breaker = FindViewById<LinearLayout>(Resource.Id.linearLayout9);
//            breaker.Visibility = ViewStates.Visible;

//            //Cart area
//            LinearLayout layoutCart = FindViewById<LinearLayout>(Resource.Id.linearLayout5);
//            layoutCart.Visibility = ViewStates.Visible;
//        }

//        #endregion

//        private void LoadDesignImages()
//        {
//            foreach (var d in design) //Get all design items
//            {
//                designImages.Add(d.Image);
//            }
//        }

//        private List<string> Order(List<string> items, OrderType ot)
//        {
//            switch (ot)
//            {
//                case OrderType.Alphabetical:
//                    {
//                        //order by aphabetical
//                        items.Sort();
//                        break;
//                    }
//                case OrderType.Ascending:
//                    {
//                        //ascending
//                        items = items.Select(o => o.Substring(0).ToString())
//                            .Where(s => Char.IsLetter(s, 0))
//                            .Distinct().ToList();
//                        break;
//                    }
//                case OrderType.Descending:
//                    {
//                        //descending
//                        items = items.OrderByDescending(x => x.Substring(0)).ToList();
//                        break;
//                    }

//            }

//            return items;
//        }

//        #region DayView

//        /// <summary>
//        /// Populate Dayview
//        /// </summary>
//        /// <param name="sender"></param>
//        /// <param name="e"></param>
//        private void buttonDay_Click(object sender, EventArgs e)
//        {
//            SelectButton(SelectedButton.Day);

//            PopulateOrder();
//        }
//        private void PopulateOrder()
//        {
//            gridViewAdapter.ItemClick -= buttonDay_Click;
//            gridViewAdapter = null;

//            try
//            {
//                LoadDatabaseEntitys();

//                if (order.Any())
//                {
//                    GridView adapter = FindViewById<GridView>(Resource.Id.gridView1);
//                    adapter.Adapter = new OrderAdapter(this, order);
//                    adapter.SetNumColumns(1);
//                    adapter.SetGravity(GravityFlags.CenterHorizontal);
//                    adapter.SetColumnWidth(15);

//                    gridViewAdapter = adapter;
//                    gridViewAdapter.FastScrollEnabled = true;
//                    gridViewAdapter.ItemClick += OrderAdapter_ItemClick;
//                }
//            }
//            catch(Exception ex)
//            {
//                throw ex;
//            }
//        }

//        private void OrderAdapter_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
//        {
//            try
//            {
//                gridViewAdapter.ItemClick -= OrderAdapter_ItemClick;
//                gridViewAdapter = null;

//                List<Tuple<int, int, int, int>> finalProduct = new List<Tuple<int, int, int, int>>();
//                List<int> orderProductList = new List<int>();
//                List<Tuple<int, int, int, int>> noProductDupes = new List<Tuple<int, int, int, int>>();

//                List<OrderProductEntity> indexOrder = order[e.Position].OrderProductEntity;

//                //------------------------
//                for (int productIndex = 0; productIndex < indexOrder.Count(); productIndex++)
//                {
//                    if (tshirt.Any())
//                    {
//                        int tID = indexOrder[productIndex].ProductID;

//                        foreach (ProductEntity p in product)
//                        {
//                            if (p.OrderProductEntity != null)
//                            {
//                                if (tID == p.ID)
//                                {
//                                    foreach (OrderProductEntity selectedIndex in p.OrderProductEntity)//gets x
//                                    {
//                                        orderProductList.Add(selectedIndex.ProductID);//gets y
//                                    }
//                                }
//                            }
//                        }
//                    }
//                }

//                //------------------------
//                for (int productIndex = 0; productIndex < orderProductList.Count(); productIndex++)
//                {
//                    if (product.Any())
//                    {
//                        int cID = orderProductList[productIndex];

//                        foreach (ProductEntity p in product)
//                        {
//                            if (cID == p.ID)
//                            {
//                                finalProduct.Add(new Tuple<int, int, int, int>(p.DesignID, p.TShirtID, p.TShirtColorID, p.TShirtSizeID));
//                            }
//                        }
//                    }
//                }

//                noProductDupes = finalProduct.Distinct().ToList();

//                List<int> d = new List<int>();
//                List<int> c = new List<int>();
//                List<int> s = new List<int>();

//                if (noProductDupes.Any())
//                {
//                    for (int i = 0; i < noProductDupes.Count; i++)
//                    {
//                        d.Add(noProductDupes[i].Item1);
//                        c.Add(noProductDupes[i].Item3);
//                        s.Add(noProductDupes[i].Item4);
//                    }

//                    GridView adapter = FindViewById<GridView>(Resource.Id.gridView1);
//                    adapter.Adapter = new ProductAdapter(this, design, colour, size, d, c, s);
//                    adapter.SetNumColumns(1);
//                    adapter.SetGravity(GravityFlags.CenterHorizontal);
//                    adapter.SetColumnWidth(15);

//                    gridViewAdapter = adapter;
//                    gridViewAdapter.FastScrollEnabled = true;
//                    //gridViewAdapter.ItemClick += PopulateSizeList_ItemClick;
//                }
//            }
//            catch(Exception ex)
//            {
//                throw ex;
//            }
//        }

//        #endregion

//        #region SellView
//        /// <summary>
//        /// Populate Sellview
//        /// </summary>
//        /// <param name="sender"></param>
//        /// <param name="e"></param>
//        private void buttonSell_Click(object sender, EventArgs e)
//        {
//            SelectButton(SelectedButton.Sell);
//            PopulateProductList();
//        }
//        private void PopulateProductList()
//        {
//            LoadDatabaseEntitys();

//            //reset
//            selectedDesign = -1;
//            selectedTShirt = -1;
//            selectedTShirtColour = -1;
//            selectedTShirtSize = -1;

//            gridViewAdapter = null;

//            try
//            {
//                if (designImages.Any())
//                {
//                    GridView adapter = FindViewById<GridView>(Resource.Id.gridView1);
//                    adapter.Adapter = new DesignAdapter(this, designImages.ToArray());
//                    adapter.SetNumColumns(5);
//                    adapter.SetColumnWidth(15);

//                    gridViewAdapter = adapter;
//                    gridViewAdapter.FastScrollEnabled = true;
//                    gridViewAdapter.ItemClick += PopulateColourList_ItemClick;
//                }
//            }
//            catch
//            {
//                throw;
//            }

//            if (cartItems.Count > 0)
//            {
//                UpdateCost();
//            }
//        }
//        private void PopulateColourList_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
//        {
//            gridViewAdapter.ItemClick -= PopulateColourList_ItemClick;
//            selectedDesign = e.Position;

//            finalColour.Clear();
//            tshirtColourList.Clear();
//            noDupes.Clear();

//            List<DesignTShirtEntity> indexDesign = design[e.Position].DesignTShirtEntity;

//            //------------------------
//            for (int tshirtIndex = 0; tshirtIndex < indexDesign.Count(); tshirtIndex++)
//            {
//                if (tshirt.Any())
//                {
//                    int tID = indexDesign[tshirtIndex].IDTShirt;

//                    foreach (TShirtEntity t in tshirt)
//                    {
//                        if (t.TShirtColourEntity != null)
//                        {
//                            if (tID == t.ID)
//                            {
//                                foreach (TShirtColourEntity selectTShirt in t.TShirtColourEntity)//gets x
//                                {
//                                    tshirtColourList.Add(selectTShirt.IDColour);//gets y
//                                }
//                            }
//                        }
//                    }
//                }
//            }

//            //------------------------
//            for (int colourIndex = 0; colourIndex < tshirtColourList.Count(); colourIndex++)
//            {
//                if (colour.Any())
//                {
//                    int cID = tshirtColourList[colourIndex];

//                    foreach (ColourEntity c in colour)
//                    {
//                        if (cID == c.ID)
//                        {
//                            finalColour.Add(c.Color);
//                        }
//                    }
//                }
//            }

//            noDupes = finalColour.Distinct().ToList();

//            if (noDupes.Any())
//            {
//                GridView adapter = FindViewById<GridView>(Resource.Id.gridView1);
//                adapter.Adapter = new ColourAdapter(this, noDupes.ToArray());
//                adapter.SetNumColumns(5);
//                adapter.SetColumnWidth(15);

//                gridViewAdapter = adapter;
//                gridViewAdapter.FastScrollEnabled = true;
//                gridViewAdapter.ItemClick += PopulateSizeList_ItemClick;
//            }
//        }
//        private void PopulateSizeList_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
//        {
//            gridViewAdapter.ItemClick -= PopulateColourList_ItemClick;
//            gridViewAdapter.ItemClick -= PopulateSizeList_ItemClick;

//            using (Database db = new Database())
//            {
//                var color = db.GetColours();

//                foreach (var c in color)
//                {
//                    if (e.Position < noDupes.Count())
//                    {
//                        if (noDupes[e.Position].Contains(c.Item2))
//                        {
//                            selectedTShirtColour = c.Item1 - 1;
//                            break;
//                        }
//                    }
//                    else
//                    {
//                        PopulateProductList();
//                    }
//                }
//            }


//            finalSize.Clear();
//            tshirtSizeList.Clear();

//            List<DesignTShirtEntity> indexDesign = design[e.Position].DesignTShirtEntity;

//            //------------------------
//            for (int tshirtIndex = 0; tshirtIndex < indexDesign.Count(); tshirtIndex++)
//            {
//                if (tshirt.Any())
//                {
//                    int tID = indexDesign[tshirtIndex].IDTShirt;

//                    foreach (TShirtEntity t in tshirt)
//                    {
//                        if (t.TShirtSizeEntity != null)
//                        {
//                            if (tID == t.ID)
//                            {
//                                foreach (TShirtSizeEntity selectTShirt in t.TShirtSizeEntity)//gets x
//                                {
//                                    tshirtSizeList.Add(selectTShirt.IDSize);//gets y
//                                }
//                            }
//                        }
//                    }
//                }
//            }

//            //------------------------
//            for (int colourIndex = 0; colourIndex < tshirtSizeList.Count(); colourIndex++)
//            {
//                if (size.Any())
//                {
//                    int cID = tshirtSizeList[colourIndex];

//                    foreach (SizeEntity s in size)
//                    {
//                        if (cID == s.ID)
//                        {
//                            finalSize.Add(s.Size);
//                        }
//                    }
//                }
//            }


//            if (finalSize.Any())
//            {
//                GridView adapter = FindViewById<GridView>(Resource.Id.gridView1);
//                adapter.Adapter = new SizeAdapter(this, finalSize.ToArray());
//                adapter.SetNumColumns(5);
//                adapter.SetColumnWidth(15);

//                gridViewAdapter = adapter;
//                gridViewAdapter.FastScrollEnabled = true;
//                gridViewAdapter.ItemClick += AddItemToCart;
//            }
//        }
//        private void AddItemToCart(object sender, AdapterView.ItemClickEventArgs e)
//        {
//            gridViewAdapter.ItemClick -= PopulateColourList_ItemClick;
//            gridViewAdapter.ItemClick -= PopulateSizeList_ItemClick;
//            gridViewAdapter.ItemClick -= AddItemToCart;
//            selectedTShirtSize = e.Position;

//            try
//            {
//                //Add item to cart
//                if (selectedDesign >= 0 && selectedTShirtColour >= 0 && selectedTShirtSize >= 0)
//                {
//                    cartItems.Add(new Tuple<string, string, string, decimal>(
//                        design[selectedDesign].Image,
//                        colour[selectedTShirtColour].Color,
//                        size[selectedTShirtSize].Size,
//                        design[selectedDesign].Price));

//                    listCart.Adapter = new CartAdapter(this, cartItems);

//                    //remove item from stock
//                    List<DesignTShirtEntity> indexDesign = design[selectedDesign].DesignTShirtEntity;
//                    selectedTShirt = indexDesign[selectedDesign].IDTShirt;

//                    using(var db = new Database())
//                    {
//                        TShirtEntity t = tshirt[selectedTShirt - 1];
//                        t.StockNZ -= 1;

//                        db.AddORUpdateTShirtEntity(t);
//                    }

//                    finalCartItem.Add(new Tuple<int, int, int, int>(
//                        selectedDesign,
//                        selectedTShirt,
//                        selectedTShirtColour,
//                        selectedTShirtSize));
//                }


//            }
//            catch
//            {
//                throw;
//            }

//            PopulateProductList();
//        }

//        #endregion

//        #region StockView

//        /// <summary>
//        /// Populate Stockview
//        /// </summary>
//        /// <param name="sender"></param>
//        /// <param name="e"></param>
//        private void buttonStock_Click(object sender, EventArgs e)
//        {
//            SelectButton(SelectedButton.Stock);

//            LoadDesignItems();
//        }
//        private void LoadDesignItems()
//        {
//            //reset
//            gridViewAdapter.ItemClick -= buttonStock_Click;
//            gridViewAdapter = null;

//            try
//            {
//                //Populate mainview with design images
//                if (designImages.Any())
//                {
//                    GridView adapter = FindViewById<GridView>(Resource.Id.gridView1);
//                    adapter.Adapter = new DesignAdapter(this, designImages.ToArray());
//                    adapter.SetNumColumns(6);
//                    adapter.SetColumnWidth(15);

//                    gridViewAdapter = adapter;
//                    gridViewAdapter.FastScrollEnabled = true;
//                    gridViewAdapter.ItemClick += PopulateStockView_ItemClick;
//                }
//            }
//            catch
//            {
//                throw;
//            }
//        }
//        private void PopulateStockView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
//        {
//            //reset
//            gridViewAdapter.ItemClick -= PopulateStockView_ItemClick;

//            finalTShirt.Clear();

//            #region get tshirt
//            //get selected DesignTShirt
//            List<DesignTShirtEntity> indexDesign = design[e.Position].DesignTShirtEntity;

//            //------------------------
//            for (int tshirtIndex = 0; tshirtIndex < indexDesign.Count(); tshirtIndex++)
//            {
//                if (tshirt.Any())
//                {
//                    //Find matching TShirt
//                    int tID = indexDesign[tshirtIndex].IDTShirt;

//                    foreach (TShirtEntity t in tshirt)
//                    {
//                        //using selected DesignTShirt get all matching items
//                        if (tID == t.ID)
//                        {
//                            //Add items
//                            finalTShirt.Add(t);
//                        }
//                    }
//                }
//            }
//            #endregion

//            #region get colours

//            finalColour.Clear();
//            tshirtColourList.Clear();

//            for (int tshirtIndex = 0; tshirtIndex < indexDesign.Count(); tshirtIndex++)
//            {
//                if (tshirt.Any())
//                {
//                    int tID = indexDesign[tshirtIndex].IDTShirt;

//                    foreach (TShirtEntity t in tshirt)
//                    {
//                        if (t.TShirtColourEntity != null)
//                        {
//                            if (tID == t.ID)
//                            {
//                                foreach (TShirtColourEntity selectedTShirt in t.TShirtColourEntity)//gets x
//                                {
//                                    tshirtColourList.Add(selectedTShirt.IDColour);//gets y
//                                }
//                            }
//                        }
//                    }
//                }
//            }

//            for (int colourIndex = 0; colourIndex < tshirtColourList.Count(); colourIndex++)
//            {
//                if (colour.Any())
//                {
//                    int cID = tshirtColourList[colourIndex];

//                    foreach (ColourEntity c in colour)
//                    {
//                        if (cID == c.ID)
//                        {
//                            finalColour.Add(c.Color);
//                        }
//                    }
//                }
//            }

//            #endregion get colours

//            #region get size

//            finalSize.Clear();
//            tshirtSizeList.Clear();

//            for (int tshirtIndex = 0; tshirtIndex < indexDesign.Count(); tshirtIndex++)
//            {
//                if (tshirt.Any())
//                {
//                    int tID = indexDesign[tshirtIndex].IDTShirt;

//                    foreach (TShirtEntity t in tshirt)
//                    {
//                        if (t.TShirtSizeEntity != null)
//                        {
//                            if (tID == t.ID)
//                            {
//                                foreach (TShirtSizeEntity selectedTShirt in t.TShirtSizeEntity)//gets x
//                                {
//                                    tshirtSizeList.Add(selectedTShirt.IDSize);//gets y
//                                }
//                            }
//                        }
//                    }
//                }
//            }

//            for (int colourIndex = 0; colourIndex < tshirtSizeList.Count(); colourIndex++)
//            {
//                if (size.Any())
//                {
//                    int cID = tshirtSizeList[colourIndex];

//                    foreach (SizeEntity s in size)
//                    {
//                        if (cID == s.ID)
//                        {
//                            finalSize.Add(s.Size);
//                        }
//                    }
//                }
//            }

//            #endregion get size

//            //Populate mainview
//            if (finalTShirt.Any())
//            {
//                GridView adapter = FindViewById<GridView>(Resource.Id.gridView1);
//                adapter.Adapter = new StockAdapter(this, design[e.Position].Image, finalTShirt.ToArray(), finalColour, finalSize);
//                adapter.SetNumColumns(4);
//                adapter.SetColumnWidth(15);
//                gridViewAdapter = adapter;
//            }
//        }

//        #endregion

//        //Update total cost of items in cart
//        private void UpdateCost()
//        {
//            //clear
//            totalcost = 0;

//            //populate cost
//            foreach(var items in cartItems)
//            {
//                totalcost += items.Item4;
//            }

//            //update total cost
//            cost.Text = "Total" + "\n" + totalcost.ToString();
//        }

//        //Remove item from cart
//        private void listCart_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
//        {
//            //if (cartItems.Count() > 0)
//            //{

//            //}

//            UpdateCost();
//        }

//        protected override void OnResume()
//        {
//            base.OnResume();
//        }
//    }
//}


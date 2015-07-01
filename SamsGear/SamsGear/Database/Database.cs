using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using SQLite.Net;
using SQLite.Net.Attributes;
using SQLite.Net.Interop;
using SQLite.Net.Platform.XamarinAndroid;
using SQLiteNetExtensions.Attributes;
using SQLiteNetExtensions.Extensions;
using SQLiteNetExtensions.Exceptions;
using Path = System.IO.Path;

#if __WIN32__
using SQLitePlatformTest = SQLite.Net.Platform.Win32.SQLitePlatformWin32;
#elif WINDOWS_PHONE
using SQLitePlatformTest = SQLite.Net.Platform.WindowsPhone8.SQLitePlatformWP8;
#elif __WINRT__
using SQLitePlatformTest = SQLite.Net.Platform.WinRT.SQLitePlatformWinRT;
#elif __IOS__
using SQLitePlatformTest = SQLite.Net.Platform.XamarinIOS.SQLitePlatformIOS;
#elif __ANDROID__
using SQLitePlatformTest = SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroid;
#else
using SQLitePlatformTest = SQLite.Net.Platform.Generic.SQLitePlatformGeneric;
#endif

namespace SamsGear
{
    public class Database : SQLiteConnection
    {
        #region Locks
        //Locks

        private object DesignEntityLock = new object();
        private object DesignTShirtEntityLock = new object();

        private object TShirtLock = new object();

        private object TShirtSizeEntityLock = new object();
        private object TShirtColourEntityLock = new object();

        private object ColourEntityLock = new object();
        private object SizeEntityLock = new object();

        private object OrderEntityLock = new object();
        private object OrderProductEntityLock = new object();

        private object ProductEntityLock = new object();

        #endregion

        //---------------------

        public Database()
            : base(new SQLitePlatformAndroid(), databasePath)
        {
            TraceListener = DebugTraceListener.Instance;
        }

        /// <summary>
        /// Database path location
        /// </summary>
        public static string databasePath
        {
            get
            {
                string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // Documents folder
                var path = Path.Combine(documentsPath, "easyDB.sqlite");

                return path;
            }
        }

        /// <summary>
        /// Read database from stream
        /// </summary>
        /// <param name="readStream"></param>
        /// <param name="writeStream"></param>
        public static void ReadWriteStream(Stream readStream, Stream writeStream)
        {
            int Length = 256;
            Byte[] buffer = new Byte[Length];
            int bytesRead = readStream.Read(buffer, 0, Length);
            // write the required bytes
            while (bytesRead > 0)
            {
                writeStream.Write(buffer, 0, bytesRead);
                bytesRead = readStream.Read(buffer, 0, Length);
            }
            readStream.Close();
            writeStream.Close();
        }

        //---------------------

        #region Design

        public List<string> GetDesignImages()
        {
            lock (DesignEntityLock)
            {
                List<string> image = new List<string>();
                var objDesignEntity = GetDesignEntity();
                foreach (DesignEntity d in objDesignEntity)
                {
                    image.Add(d.Image);
                }

                return image;
            }
        }

        public List<DesignEntity> GetDesignEntity()
        {
            lock (DesignEntityLock)
            {
                var list = (from x in Table<DesignEntity>() select x).ToList();

                foreach(var x in list)
                {
                    SQLiteNetExtensions.Extensions.ReadOperations.GetChildren(this, x, true);
                }

                return list;
            }
        }

        /// <summary>
        /// ID, Name, Image, Price
        /// </summary>
        /// <returns></returns>
        public List<Tuple<int, string, string, decimal>> Design()
        {
            List<Tuple<int, string, string, decimal>> items = new List<Tuple<int, string, string, decimal>>();

            List<DesignEntity> objDesignEntity = GetDesignEntity();
            foreach (DesignEntity d in objDesignEntity)
            {
                items.Add(new Tuple<int, string, string, decimal>(d.ID, d.Name, d.Image, d.Price));
            }

            return items;
        }

        public DesignEntity GetDesignEntity(int id)
        {
            lock (DesignEntityLock)
            {
                var x = from y in Table<DesignEntity>()
                        where y.ID == id
                        select y;

                return x.FirstOrDefault();
            }
        }

        public void AddORUpdateDesignEntity(DesignEntity x)
        {
            lock (DesignEntityLock)
            {
                if (x.ID != 0)
                {
                    Update(x);
                }
                else
                {
                    Insert(x);
                }
            }
        }

        public void DeleteDesignEntity(int id)
        {
            lock (DesignEntityLock)
            {
                Delete<TShirtEntity>(id);
            }
        }

        #endregion

        #region DesignTShirtEntity

        public List<DesignTShirtEntity> GetDesignTShirtEntity()
        {
            lock (DesignTShirtEntityLock)
            {
                return (from x in Table<DesignTShirtEntity>() select x).ToList();
            }
        }

        /// <summary>
        /// ID, Design, TShirt
        /// </summary>
        /// <returns></returns>
        public List<Tuple<int, int, int>> DesignTShirt()
        {
            List<Tuple<int, int, int>> x = new List<Tuple<int, int, int>>();

            var objDesignTShirtEntity = GetDesignTShirtEntity();
            foreach (DesignTShirtEntity y in objDesignTShirtEntity)
            {
                x.Add(new Tuple<int, int, int>(y.ID, y.IDDesign, y.IDTShirt));
            }

            return x;
        }

        public DesignTShirtEntity GetDesignTShirtEntity(int id)
        {
            lock (DesignTShirtEntityLock)
            {
                var stock = from x in Table<DesignTShirtEntity>()
                            where x.ID == id
                            select x;

                return stock.FirstOrDefault();
            }
        }

        public void AddORUpdateDesignTShirtEntity(DesignTShirtEntity item)
        {
            lock (DesignTShirtEntityLock)
            {
                if (item.ID != 0)
                {
                    Update(item);
                }
                else
                {
                    Insert(item);
                }
            }
        }

        public void DeleteDesignTShirtEntity(int id)
        {
            lock (DesignTShirtEntityLock)
            {
                Delete<DesignTShirtEntity>(id);
            }
        }

        #endregion

        //---------------------

        #region TShirt

        public List<TShirtEntity> GetTShirtEntity()
        {
            lock (TShirtLock)
            {
                var list = (from x in Table<TShirtEntity>() select x).ToList();

                foreach (var x in list)
                {
                    SQLiteNetExtensions.Extensions.ReadOperations.GetChildren(this, x, true);
                }

                return list;
            }
        }

        /// <summary>
        /// ID, Name, StockNZ, StockAU
        /// </summary>
        /// <returns></returns>
        public List<Tuple<int, string, int, int>> TShirt()
        {
            List<Tuple<int, string, int, int>> t = new List<Tuple<int, string, int, int>>();

            var objTShirtEntity = GetTShirtEntity();
            foreach (TShirtEntity se in objTShirtEntity)
            {
                t.Add(new Tuple<int, string, int, int>(se.ID, se.Name, se.StockNZ, se.StockAU));
            }

            return t;
        }

        public int? FindTShirtEntity(string name)
        {
            var stock = from t in Table<TShirtEntity>()
                        where t.Name == name
                        select t;


            return stock.FirstOrDefault().ID;
        }

        public TShirtEntity GetTShirtEntity(int id)
        {
            lock (TShirtLock)
            {
                var stock = from t in Table<TShirtEntity>()
                            where t.ID == id
                            select t;

                return stock.FirstOrDefault();
            }
        }

        public void AddORUpdateTShirtEntity(TShirtEntity item)
        {
            lock (TShirtLock)
            {
                if (item.ID != 0)
                {
                    Update(item);
                }
                else
                {
                    Insert(item);
                }
            }
        }

        public void DeleteTShirtEntity(int id)
        {
            lock (TShirtLock)
            {
                Delete<TShirtEntity>(id);
            }
        }

        #endregion

        #region TShirtColourEntity

        public List<TShirtColourEntity> GetTShirtColourEntity()
        {
            lock (TShirtColourEntityLock)
            {
                return (from x in Table<TShirtColourEntity>() select x).ToList();
            }
        }

        /// <summary>
        /// ID, Colour, TShirt
        /// </summary>
        /// <returns></returns>
        public List<Tuple<int, int, int>> TShirtColour()
        {
            List<Tuple<int, int, int>> x = new List<Tuple<int, int, int>>();

            var objTShirtColourEntity = GetTShirtColourEntity();
            foreach (TShirtColourEntity y in objTShirtColourEntity)
            {
                x.Add(new Tuple<int, int, int>(y.ID, y.IDColour, y.IDTShirt));
            }

            return x;
        }

        public TShirtColourEntity GetTShirtColourEntity(int id)
        {
            lock (TShirtColourEntityLock)
            {
                var x = from y in Table<TShirtColourEntity>()
                            where y.ID == id
                            select y;

                return x.FirstOrDefault();
            }
        }

        public void AddORUpdateTShirtColourEntity(TShirtColourEntity item)
        {
            lock (TShirtColourEntityLock)
            {
                if (item.ID != 0)
                {
                    Update(item);
                }
                else
                {
                    Insert(item);
                }
            }
        }

        public void DeleteTShirtColourEntity(int id)
        {
            lock (TShirtColourEntityLock)
            {
                Delete<TShirtColourEntity>(id);
            }
        }

        #endregion

        #region TShirtSizeEntity

        public List<TShirtSizeEntity> GetTShirtSizeEntity()
        {
            lock (TShirtSizeEntityLock)
            {
                return (from x in Table<TShirtSizeEntity>() select x).ToList();
            }
        }

        /// <summary>
        /// ID, Size, TShirt
        /// </summary>
        /// <returns></returns>
        public List<Tuple<int, int, int>> TShirtSize()
        {
            List<Tuple<int, int, int>> x = new List<Tuple<int, int, int>>();

            var objTShirtSizeEntity = GetTShirtSizeEntity();
            foreach (TShirtSizeEntity y in objTShirtSizeEntity)
            {
                x.Add(new Tuple<int, int, int>(y.ID, y.IDSize, y.IDTShirt));
            }

            return x;
        }

        public TShirtSizeEntity GetTShirtSizeEntity(int id)
        {
            lock (TShirtSizeEntityLock)
            {
                var x = from y in Table<TShirtSizeEntity>()
                            where y.ID == id
                            select y;

                return x.FirstOrDefault();
            }
        }

        public void AddORUpdateTShirtSizeEntity(TShirtSizeEntity item)
        {
            lock (TShirtSizeEntityLock)
            {
                if (item.ID != 0)
                {
                    Update(item);
                }
                else
                {
                    Insert(item);
                }
            }
        }

        public void DeleteTShirtSizeEntity(int id)
        {
            lock (TShirtSizeEntityLock)
            {
                Delete<TShirtSizeEntity>(id);
            }
        }

        #endregion

        //---------------------

        #region Colour

        public List<ColourEntity> GetColourEntity()
        {
            lock (ColourEntityLock)
            {
                return (from i in Table<ColourEntity>() select i).ToList();
            }
        }

        /// <summary>
        /// ID, Colour
        /// </summary>
        /// <returns></returns>
        public List<Tuple<int, string>> GetColours()
        {
            List<Tuple<int, string>> c = new List<Tuple<int, string>>();

            var objColorEntity = GetColourEntity();
            foreach (ColourEntity ce in objColorEntity)
            {
                c.Add(new Tuple<int, string>(ce.ID, ce.Color));
            }

            return c;
        }

        public List<Tuple<int, string>> Colour()
        {
            List<Tuple<int, string>> c = new List<Tuple<int, string>>();

            var objColorEntity = GetColourEntity();
            foreach (ColourEntity ce in objColorEntity)
            {
                c.Add(new Tuple<int, string>(ce.ID, ce.Color));
            }

            return c;
        }

        public ColourEntity GetColourEntity(int id)
        {
            lock (ColourEntityLock)
            {
                var stock = from s in Table<ColourEntity>()
                            where s.ID == id
                            select s;

                return stock.FirstOrDefault();
            }
        }

        public void AddORUpdateColourEntity(ColourEntity item)
        {
            lock (ColourEntityLock)
            {
                if (item.ID != 0)
                {
                    Update(item);
                }
                else
                {
                    Insert(item);
                }
            }
        }

        public void DeleteColourEntity(int id)
        {
            lock (ColourEntityLock)
            {
                Delete<ColourEntity>(id);
            }
        }

        #endregion

        //---------------------

        #region Size

        public List<SizeEntity> GetSizeEntity()
        {
            lock (SizeEntityLock)
            {
                return (from i in Table<SizeEntity>() select i).ToList();
            }
        }

        /// <summary>
        /// ID, Size
        /// </summary>
        /// <returns></returns>
        public List<Tuple<int, string>> Size()
        {
            List<Tuple<int, string>> s = new List<Tuple<int, string>>();

            var objSizeEntity = GetSizeEntity();
            foreach (SizeEntity se in objSizeEntity)
            {
                s.Add(new Tuple<int, string>(se.ID, se.Size));
            }

            return s;
        }

        public SizeEntity GetSizeEntity(int id)
        {
            lock (SizeEntityLock)
            {
                var stock = from s in Table<SizeEntity>()
                            where s.ID == id
                            select s;

                return stock.FirstOrDefault();
            }
        }

        public void AddORUpdateSizeEntity(SizeEntity item)
        {
            lock (SizeEntityLock)
            {
                if (item.ID != 0)
                {
                    Update(item);
                }
                else
                {
                    Insert(item);
                }
            }
        }

        public void DeleteSizeEntity(int id)
        {
            lock (SizeEntityLock)
            {
                Delete<SizeEntity>(id);
            }
        }

        #endregion

        //---------------------

        #region Order

        public List<OrderEntity> GetOrderEntity()
        {
            lock (OrderEntityLock)
            {
                var list = (from x in Table<OrderEntity>() select x).ToList();

                foreach (var x in list)
                {
                    SQLiteNetExtensions.Extensions.ReadOperations.GetChildren(this, x, true);
                }

                return list;
            }
        }

        /// <summary>
        /// ID, Name, Email, DateTime
        /// </summary>
        /// <returns></returns>
        public List<Tuple<int, string, string, string>> Order()
        {
            List<Tuple<int, string, string, string>> items = new List<Tuple<int, string, string, string>>();

            var objOrderEntity = GetOrderEntity();
            foreach (OrderEntity o in objOrderEntity)
            {
                items.Add(new Tuple<int, string, string, string>(o.ID, o.Name, o.Email, o.DateTime));
            }

            return items;
        }

        public OrderEntity GetOrderEntityID(int id)
        {
            lock (OrderEntityLock)
            {
                var x = from y in Table<OrderEntity>()
                        where y.ID == id
                        select y;

                return x.FirstOrDefault();
            }
        }

        public void AddORUpdateOrderEntity(OrderEntity x)
        {
            lock (OrderEntityLock)
            {
                if (x.ID != 0)
                {
                    Update(x);
                }
                else
                {
                    Insert(x);
                }
            }
        }

        public void DeleteOrderEntity(int id)
        {
            lock (OrderEntityLock)
            {
                Delete<OrderEntity>(id);
            }
        }

        #endregion

        #region OrderProductEntity

        public List<OrderProductEntity> GetOrderProductEntity()
        {
            lock (OrderProductEntityLock)
            {
                return (from x in Table<OrderProductEntity>() select x).ToList();
            }
        }

        /// <summary>
        /// ID, Order, Product
        /// </summary>
        /// <returns></returns>
        public List<Tuple<int, int, int>> OrderProductEntity()
        {
            List<Tuple<int, int, int>> x = new List<Tuple<int, int, int>>();

            var objOrderProductEntity = GetOrderProductEntity();
            foreach (OrderProductEntity y in objOrderProductEntity)
            {
                x.Add(new Tuple<int, int, int>(y.ID, y.OrderID, y.ProductID));
            }

            return x;
        }

        public OrderProductEntity GetOrderProductEntityID(int id)
        {
            lock (OrderProductEntityLock)
            {
                var stock = from x in Table<OrderProductEntity>()
                            where x.ID == id
                            select x;

                return stock.FirstOrDefault();
            }
        }

        public void AddORUpdateOrderProductEntity(OrderProductEntity item)
        {
            lock (OrderProductEntityLock)
            {
                if (item.ID != 0)
                {
                    Update(item);
                }
                else
                {
                    Insert(item);
                }
            }
        }

        public void DeleteOrderProductEntity(int id)
        {
            lock (OrderProductEntityLock)
            {
                Delete<OrderProductEntity>(id);
            }
        }

        #endregion

        //---------------------

        #region Product

        public List<ProductEntity> GetProductEntity()
        {
            lock (ProductEntityLock)
            {
                var list = (from x in Table<ProductEntity>() select x).ToList();

                foreach (var x in list)
                {
                    SQLiteNetExtensions.Extensions.ReadOperations.GetChildren(this, x, true);
                }

                return list;
            }
        }

        /// <summary>
        /// ID, DesignID, TShirtID, TShirtColorID, TShirtSizeID
        /// </summary>
        /// <returns></returns>
        public List<Tuple<int, int, int, int, int>> Product()
        {
            List<Tuple<int, int, int, int, int>> items = new List<Tuple<int, int, int, int, int>>();

            var objProductEntity = GetProductEntity();
            foreach (ProductEntity p in objProductEntity)
            {
                items.Add(new Tuple<int, int, int, int, int>(p.ID, p.DesignID, p.TShirtID, p.TShirtColorID, p.TShirtSizeID));
            }

            return items;
        }

        public ProductEntity GetProductEntityID(int id)
        {
            lock (ProductEntityLock)
            {
                var x = from y in Table<ProductEntity>()
                        where y.ID == id
                        select y;

                return x.FirstOrDefault();
            }
        }

        public void AddORUpdateProductEntity(ProductEntity x)
        {
            lock (ProductEntityLock)
            {
                if (x.ID != 0)
                {
                    Update(x);
                }
                else
                {
                    Insert(x);
                }
            }
        }

        public void DeleteProductEntity(int id)
        {
            lock (ProductEntityLock)
            {
                Delete<ProductEntity>(id);
            }
        }

        #endregion

        //---------------------
    }
}
using SQLiteNetExtensions.Attributes;
using SQLite.Net.Attributes;

namespace SamsGear
{
    [Table("OrderProduct")]
    public class OrderProductEntity
    {
        [Column("ID"), PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [Column("IDOrder"), ForeignKey(typeof(OrderEntity))]
        public int OrderID { get; set; }

        [Column("IDProduct"), ForeignKey(typeof(ProductEntity))]
        public int ProductID { get; set; }
    }
}

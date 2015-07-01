using SQLiteNetExtensions.Attributes;
using SQLite.Net.Attributes;

namespace SamsGear
{
    [Table("TShirtSize")]
    public class TShirtSizeEntity
    {
        [Column("ID"), PrimaryKey, AutoIncrement, Unique]
        public int ID { get; set; }

        [Column("IDTShirt"), ForeignKey(typeof(TShirtEntity))]
        public int IDTShirt { get; set; }

        [Column("IDSize"), ForeignKey(typeof(SizeEntity))]
        public int IDSize { get; set; }
    }
}
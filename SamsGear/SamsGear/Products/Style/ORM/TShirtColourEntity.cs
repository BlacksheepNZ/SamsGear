using SQLiteNetExtensions.Attributes;
using SQLite.Net.Attributes;

namespace SamsGear
{
    [Table("TShirtColour")]
    public class TShirtColourEntity
    {
        [Column("ID"), PrimaryKey, AutoIncrement, Unique]
        public int ID { get; set; }

        [Column("IDTShirt"), ForeignKey(typeof(TShirtEntity))]
        public int IDTShirt { get; set; }

        [Column("IDColour"), ForeignKey(typeof(ColourEntity))]
        public int IDColour { get; set; }
    }
}
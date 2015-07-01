using SQLiteNetExtensions.Attributes;
using SQLite.Net.Attributes;

namespace SamsGear
{
    [Table("DesignTShirt")]
    public class DesignTShirtEntity
    {
        [Column("ID"), PrimaryKey, AutoIncrement, Unique]
        public int ID { get; set; }

        [Column("IDDesign"), ForeignKey(typeof(DesignEntity))]
        public int IDDesign { get; set; }

        [Column("IDTShirt"), ForeignKey(typeof(TShirtEntity))]
        public int IDTShirt { get; set; }
    }
}
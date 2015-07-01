using System.Collections.Generic;
using SQLiteNetExtensions.Attributes;
using SQLite.Net.Attributes;

namespace SamsGear
{
    [Table("Colour")]
    public class ColourEntity
    {
        [Column("ID"), PrimaryKey, AutoIncrement, Unique]
        public int ID { get; set; }

        [Column("Color")]
        public string Color { get; set; }


    }
}
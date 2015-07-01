using System.Collections.Generic;
using SQLiteNetExtensions.Attributes;
using SQLite.Net.Attributes;

namespace SamsGear
{
    [Table("TShirt")]
    public class TShirtEntity
    {
        [Column("ID"), PrimaryKey, AutoIncrement, Unique]
        public int ID { get; set; }

        [Column("Name")]
        public string Name { get; set; }

        [Column("StockNZ")]
        public int StockNZ { get; set; }

        [Column("StockAU")]
        public int StockAU { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.CascadeRead)]
        public List<TShirtColourEntity> TShirtColourEntity { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.CascadeRead)]
        public List<TShirtSizeEntity> TShirtSizeEntity { get; set; } 
    }
}

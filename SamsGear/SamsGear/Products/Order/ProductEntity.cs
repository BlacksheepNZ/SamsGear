using System.Collections.Generic;
using SQLiteNetExtensions.Attributes;
using SQLite.Net.Attributes;
using System;

namespace SamsGear
{
    [Table("Product")]
    public class ProductEntity
    {
        [Column("ID"), PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [Column("DesignID")]
        public int DesignID { get; set; }

        [Column("TShirtID")]
        public int TShirtID { get; set; }

        [Column("TShirtColorID")]
        public int TShirtColorID { get; set; }

        [Column("TShirtSizeID")]
        public int TShirtSizeID { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.CascadeRead)]
        public List<OrderProductEntity> OrderProductEntity { get; set; } 
    }
}

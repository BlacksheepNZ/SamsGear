using System.Collections.Generic;
using SQLiteNetExtensions.Attributes;
using SQLite.Net.Attributes;

namespace SamsGear
{
    [Table("Size")]
    public class SizeEntity
    {
        [Column("ID"), PrimaryKey, AutoIncrement, Unique]
        public int ID { get; set; }

        [Column("Size")]
        public string Size { get; set; }

        [ManyToMany(typeof(TShirtSizeEntity))]
        public List<SizeEntity> TShirtSizeEntity { get; set; }
    }
}


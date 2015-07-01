using System.Collections.Generic;
using SQLiteNetExtensions.Attributes;
using SQLite.Net.Attributes;

namespace SamsGear
{
    [Table("Design")]
    public class DesignEntity
    {
        [Column("ID"), PrimaryKey, AutoIncrement, Unique]
        public int ID { get; set; }

        [Column("Name")]
        public string Name { get; set; }

        [Column("Image")]
        public string Image { get; set; }

        [Column("Price")]
        public int Price { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.CascadeRead)]
        public List<DesignTShirtEntity> DesignTShirtEntity { get; set; } 
    }
}

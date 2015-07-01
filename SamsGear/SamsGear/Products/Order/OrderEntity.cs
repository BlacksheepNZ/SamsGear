using System.Collections.Generic;
using SQLiteNetExtensions.Attributes;
using SQLite.Net.Attributes;
using System;

namespace SamsGear
{
    [Table("Order")]
    public class OrderEntity
    {
        [Column("ID"), PrimaryKey, AutoIncrement, Unique]
        public int ID { get; set; }

        [Column("Name")]
        public string Name { get; set; }

        [Column("Email")]
        public string Email { get; set; }

        [Column("DateTime")]
        public string DateTime { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.CascadeRead)]
        public List<OrderProductEntity> OrderProductEntity { get; set; } 
    }
}

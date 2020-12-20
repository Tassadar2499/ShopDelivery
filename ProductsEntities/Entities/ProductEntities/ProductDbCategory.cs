using System;
using System.Collections.Generic;
using System.Text;

namespace ShopsDbEntities
{
    public enum ProductCategory : byte
    {
        None = 0,
        Meat = 1
    }
    public class ProductDbCategory
    {
        public byte Id { get; set; }
        public string Name { get; set; }
        public string ImgUrl { get; set; }
    }
}

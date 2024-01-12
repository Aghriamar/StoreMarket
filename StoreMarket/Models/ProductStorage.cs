namespace StoreMarket.Models
{
    public class ProductStorage
    {
        public virtual Product? Product { get; set; }
        public virtual Storage? Storage { get; set; }
        public int? ProductID { get; set; }
        public int? StorageID { get; set; }
    }
}

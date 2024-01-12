namespace StoreMarket.Models
{
    public class Product : BaseModel
    {
        public virtual List<Storage> Storages { get; set; } = new List<Storage>();
        public virtual Category? Category { get; set; }
        public int CategoryId { get; set; }
        public string? Description { get; set; }
        public int Price { get; set; }

    }
}

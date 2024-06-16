namespace MvcAppUsingADONet.Models
{
    public class Inventory
    {
        public int InventoryId { get; set; }
        public string ProductName { get; set; }
        public int QtyInStock { get; set; }
        public int ReorderLevel { get; set; }
        public DateTime AddedOn { get; set; }
    }
}

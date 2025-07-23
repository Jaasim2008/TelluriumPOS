namespace TelluriumPOS.Catalysts
{
    public class Product
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Barcode { get; set; }
        public int CategoryId { get; set; }
        public double Price { get; set; }
        public double? CostPrice { get; set; }
        public double VAT { get; set; }
        public int Stock { get; set; }
        public bool IsActive { get; set; } = true;
    }
    public class Category
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
    }
}

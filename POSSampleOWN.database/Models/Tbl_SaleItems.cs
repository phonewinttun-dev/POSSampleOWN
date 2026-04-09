

using POSSampleOWN.Models;

namespace POSSampleOWN.database.Models
{
    public class Tbl_SaleItem
    {
        public long Id { get; set; }

        public long ProductId { get; set; }

        public long SaleId { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public Tbl_Product Product { get; set; } = null!;

        public Tbl_Sale Sale { get; set; } = null!;
    }
}

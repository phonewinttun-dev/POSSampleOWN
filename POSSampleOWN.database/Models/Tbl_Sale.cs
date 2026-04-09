
namespace POSSampleOWN.database.Models
{
    public class Tbl_Sale
    {
        public long Id { get; set; }

        public decimal TotalPrice { get; set; }

        public string VoucherCode { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public ICollection<Tbl_SaleItem> SaleItems { get; set; } = new List<Tbl_SaleItem>();

    }
}

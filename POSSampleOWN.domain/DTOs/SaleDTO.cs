using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POSSampleOWN.database.Models;

namespace POSSampleOWN.domain.DTOs;

public class SaleDTO
{
    public long Id { get; set; }

    public decimal TotalPrice { get; set; }

    public string VoucherCode { get; set; } = null!;

    public ICollection<Tbl_SaleItem> SaleItems { get; set; } = new List<Tbl_SaleItem>();
}

public class CreateSaleDTO
{
    List<CreateSaleItemDTO> Items { get; set; } = null!;
}

public class CreateSaleItemDTO
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}


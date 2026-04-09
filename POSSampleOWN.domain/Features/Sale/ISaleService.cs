using POSSampleOWN.domain.DTOs;
using POSSampleOWN.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSSampleOWN.domain.Features.Sale
{
    public interface ISaleService
    {
        Task<ApiResponse<SaleDTO>> CreateSaleAsync(CreateSaleDTO reqSale);
        ApiResponse<List<SaleDTO>> GetAllSales();
        ApiResponse<SaleDTO> GetSaleById(long id);
        bool ValidateSale(CreateSaleDTO sale);
        decimal TotalPrice(CreateSaleDTO reqSale);
        Task<decimal> SubPrice(CreateSaleItemDTO reqSaleItem);
    }
}
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.EntityFrameworkCore;
using POSSampleOWN.Data;
using POSSampleOWN.database.Models;
using POSSampleOWN.domain.DTOs;
using POSSampleOWN.Models;
using POSSampleOWN.Responses;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace POSSampleOWN.domain.Features.Sale;

public class SaleService : ISaleService
{
    private readonly POSDbContext _db;

    public SaleService(POSDbContext db)
    {
        _db = db;
    }

    private IQueryable<Tbl_Product> ActiveProduct => _db.Products.Where(p => !p.DeleteFlag);

    #region Create Sale
    public async Task<ApiResponse<SaleDTO>> CreateSaleAsync(CreateSaleDTO reqSale)
    {
        using var transaction = await _db.Database.BeginTransactionAsync();
        try
        {
            if (!ValidateSale(reqSale))
                return ApiResponse<SaleDTO>.Fail("Invalid sale data.");

            var productIds = reqSale.Items.Select(x => x.ProductId).Distinct().ToList();
            var products = await ActiveProduct
                                   .Where(p => productIds.Contains(p.Id))
                                   .ToDictionaryAsync(p => (long)p.Id);

            // check product exists & sufficient quantity
            foreach (var item in reqSale.Items)
            {
                if (!products.TryGetValue(item.ProductId, out var product))
                    return ApiResponse<SaleDTO>.Fail($"Product {item.ProductId} not found.");

                if (product.StockQuantity < item.Quantity)
                    return ApiResponse<SaleDTO>.Fail($"Insufficient stock for {product.Name}.");

                // reduce stock quantity
                product.StockQuantity -= item.Quantity;
            }

            decimal totalPrice = TotalPrice(reqSale);
            var saveModel = new Tbl_Sale
            {
                TotalPrice = totalPrice,
                VoucherCode = "YM-" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                CreatedAt = DateTime.Now
            };

            saveModel.SaleItems = reqSale.Items.Select(item => new Tbl_SaleItem
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                Price = products[item.ProductId].Price
            }).ToList();

            _db.Sales.Add(saveModel);
            await _db.SaveChangesAsync();
            await transaction.CommitAsync();

            var resModel = new SaleDTO
            {
                Id = saveModel.Id,
                TotalPrice = saveModel.TotalPrice,
                VoucherCode = saveModel.VoucherCode,
                SaleItems = saveModel.SaleItems.Select(x => new SaleItemDTO
                {
                    // Safely get the name from the dictionary we built earlier
                    ProductName = products.TryGetValue(x.ProductId, out var p) ? p.Name : "Unknown Product",
                    Quantity = x.Quantity,
                    Price = x.Price
                }).ToList()
            };

            return ApiResponse<SaleDTO>.Success(resModel);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return ApiResponse<SaleDTO>.Fail($"Error: {ex.Message}");
        }
    }
    #endregion

    #region Get All Sales
    public ApiResponse<List<SaleDTO>> GetAllSales()
    {
        try
        {
            var sales = _db.Sales
                .Include(s => s.SaleItems)
                .OrderByDescending(s => s.Id)
                .ToList();

            var resModel = sales.Select(sale => new SaleDTO
            {
                Id = sale.Id,
                TotalPrice = sale.TotalPrice,
                VoucherCode = sale.VoucherCode,
                SaleItems = sale.SaleItems.Select(x => new SaleItemDTO
                {
                    ProductName = _db.Products
                        .Where(p => p.Id == x.ProductId)
                        .Select(p => p.Name)
                        .FirstOrDefault() ?? "Unknown",
                    Quantity = x.Quantity,
                    Price = x.Price
                }).ToList()
            }).ToList();

            return ApiResponse<List<SaleDTO>>.Success(resModel);
        }
        catch (Exception ex)
        {
            return ApiResponse<List<SaleDTO>>.Fail($"Error: {ex.Message}");
        }
    }
    #endregion

    #region Get Sale By Id
    public ApiResponse<SaleDTO> GetSaleById(long id)
    {
        var sale = _db.Sales.Include(s => s.SaleItems).FirstOrDefault(s => s.Id == id);

        if (sale == null)
            return ApiResponse<SaleDTO>.Fail("Sale not found.");

        var resModel = new SaleDTO
        {
            Id = sale.Id,
            TotalPrice = sale.TotalPrice,
            VoucherCode = sale.VoucherCode,
            SaleItems = sale.SaleItems.Select(x => new SaleItemDTO
            {
                ProductName = _db.Products.Where(p => p.Id == x.ProductId)
                .Select(p => p.Name).
                FirstOrDefault() ?? "",
                Quantity = x.Quantity,
                Price = x.Price
            }).ToList()
        };
        return ApiResponse<SaleDTO>.Success(resModel);
    }
    #endregion

    #region Sale Validation
    public bool ValidateSale(CreateSaleDTO sale)
    {
        if (sale == null)
            return false;
        foreach (var item in sale.Items)
        {
            if (item.Quantity <= 0)
                return false;
        }
        return true;
    }
    #endregion

    #region total price
    public decimal TotalPrice(CreateSaleDTO reqSale)
    {
        decimal totalPrice = 0;
        foreach (var item in reqSale.Items)
        {
            var price = SubPrice(item).Result;
            totalPrice += price;
        }
        return totalPrice;
    }
    #endregion

    #region sub price
    public async Task<decimal> SubPrice(CreateSaleItemDTO reqSaleItem)
    {
        var item = await _db.Products.FirstOrDefaultAsync( 
            x => x.Id == reqSaleItem.ProductId );
        decimal price = item?.Price ?? 0;
        return price * reqSaleItem.Quantity;

    }
    #endregion

}


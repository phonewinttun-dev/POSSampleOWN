using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using POSSampleOWN.Data;

namespace POSSampleOWN.domain.Features.Sale;

public class SaleService
{
    private readonly POSDbContext _db;

    public SaleService(POSDbContext db)
    {
        _db = db;
    }
}

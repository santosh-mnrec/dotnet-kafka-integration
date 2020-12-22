using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Dotnet.Kafka.Integration;
using Dotnet.Kafka.Integration.Model;

namespace Dotnet.Kafka.Integration.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly Dotnet.Kafka.Integration.OrderDbContext _context;

        public IndexModel(Dotnet.Kafka.Integration.OrderDbContext context)
        {
            _context = context;
        }

        public IList<Product> Product { get;set; }

        public async Task OnGetAsync()
        {
            Product = await _context.Product.ToListAsync();
        }
    }
}

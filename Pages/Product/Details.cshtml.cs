using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Dotnet.Kafka.Integration;
using Dotnet.Kafka.Integration.Model;

namespace Dotnet.Kafka.Integration.Pages.Product
{
    public class DetailsModel : PageModel
    {
        private readonly Dotnet.Kafka.Integration.OrderDbContext _context;

        public DetailsModel(Dotnet.Kafka.Integration.OrderDbContext context)
        {
            _context = context;
        }

        public Product Product { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product = await _context.Product.FirstOrDefaultAsync(m => m.Id == id);

            if (Product == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}

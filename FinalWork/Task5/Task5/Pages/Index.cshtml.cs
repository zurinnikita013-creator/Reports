using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Task5.Models;
//Журин Никита

namespace Task5.Pages
{
    public class IndexModel(AppDbContext context) : PageModel
    {
        private readonly AppDbContext _context = context;

        public List<Product> Products { get; set; } = new();

        public async Task OnGetAsync()
        {
            Products = await _context.Products.ToListAsync();
        }
    }
}

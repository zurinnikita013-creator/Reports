using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Task5.Pages
{
    public class OrderSuccessModel : PageModel
    {
        [BindProperty(SupportsGet = true, Name = "name")]
        public string Name { get; set; } = string.Empty;

        [BindProperty(SupportsGet = true, Name = "price")]
        public decimal Price { get; set; }

        public void OnGet() 
        { 
        }
    }
}

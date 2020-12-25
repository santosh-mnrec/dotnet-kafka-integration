
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Dotnet.Kafka.Integration
{

    public class IndexModel : PageModel
    {


        public async Task<IActionResult> GetAsync()
        {
            await Task.Delay(100);
            return Page();
        }


    }

}
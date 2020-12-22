
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace dotnet_kafka_integration
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
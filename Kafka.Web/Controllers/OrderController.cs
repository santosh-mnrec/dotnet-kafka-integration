using System;
using System.Threading.Tasks;
using Confluent.Kafka;
using Domain.Kafka;
using Dotnet.Kafka.Integration.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Dotnet.Kafka.Integration.Controllers
{
     [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly KafkaService _kafkaService;
        public OrderController(IConfiguration configuration)
        {
            _kafkaService = new KafkaService(configuration);

        }
        // POST api/values
        [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody] OrderRequest value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Serialize 
            string serializedOrder = JsonConvert.SerializeObject(value);

            Console.WriteLine("========");
            Console.WriteLine("Info: OrderController => Post => Recieved a new purchase order:");
            Console.WriteLine(serializedOrder);
            Console.WriteLine("=========");

            await _kafkaService.Publish("ValidatedOrders", serializedOrder);

            return Created("TransactionId", "Your order is in progress");
        }

    }
}
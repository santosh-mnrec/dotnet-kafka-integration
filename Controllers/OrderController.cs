using System;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace dotnet_kafka_integration.Controllers
{
    public class OrderController : ControllerBase
    {
        private readonly ProducerConfig config;
        public OrderController(ProducerConfig config)
        {
            this.config = config;

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

            var producer = new ProducerService(this.config, "orderrequests");
            await producer.writeMessage(serializedOrder);

            return Created("TransactionId", "Your order is in progress");
        }

    }
}
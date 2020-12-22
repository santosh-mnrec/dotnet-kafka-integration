

using System;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace dotnet_kafka_integration
{

    public class ProcessOrderService : IHostedService
    {

        private readonly ILogger<ProcessOrderService> _logger;
        private readonly ConsumerConfig consumerConfig;
        private readonly ProducerConfig producerConfig;
        public ProcessOrderService(ConsumerConfig consumerConfig, ProducerConfig producerConfig)
        {
            this.producerConfig = producerConfig;
            this.consumerConfig = consumerConfig;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("OrderProcessing Service Started");

            while (!cancellationToken.IsCancellationRequested)
            {
                var consumerHelper = new ConsumerService(consumerConfig, "demo");
                string orderRequest = consumerHelper.readMessage();

                //Deserilaize 
                OrderRequest order = JsonConvert.DeserializeObject<OrderRequest>(orderRequest);

                //TODO:: Process Order
                Console.WriteLine($"Info: OrderHandler => Processing the order for {order.productname}");
                order.status = OrderStatus.COMPLETED;

                //Write to ReadyToShip Queue

                var producerWrapper = new ProducerService(producerConfig, "readytoship");
                await producerWrapper.writeMessage(JsonConvert.SerializeObject(order));
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
           
            return Task.CompletedTask;
        }

    }
}
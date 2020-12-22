

using System;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Dotnet.Kafka.Integration.Data;
using Dotnet.Kafka.Integration.Model;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Dotnet.Kafka.Integration
{

    public class ProcessOrderService : IHostedService
    {

        private readonly ILogger<ProcessOrderService> _logger;
        private readonly IRepository<Product> _productRepository;
        private readonly ConsumerConfig consumerConfig;
        private readonly ProducerConfig producerConfig;
        public ProcessOrderService(ConsumerConfig consumerConfig, ProducerConfig producerConfig,IRepository<Product> productRepository)
        {
            this.producerConfig = producerConfig;
            this.consumerConfig = consumerConfig;
            _productRepository = productRepository;
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

                if(_productRepository.IsExists(order.id))
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
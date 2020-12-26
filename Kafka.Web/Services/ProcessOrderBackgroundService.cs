using Domain.Kafka;
using Dotnet.Kafka.Integration.Data;
using Dotnet.Kafka.Integration.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Dotnet.Kafka.Integration
{

    public class ProcessOrderBackgroundService : BackgroundService
    {

        private readonly ILogger<ProcessOrderBackgroundService> _logger;
        private IRepository<Product> _productRepository;
        public IServiceScopeFactory _serviceScopeFactory;
        private readonly KafkaService _kafkaService;
      
        public ProcessOrderBackgroundService(IConfiguration configuration, IServiceScopeFactory serviceScopeFactory,ILogger<ProcessOrderBackgroundService> logger)
        {
            _kafkaService = new KafkaService(configuration);
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }
        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("start");

            await ExecuteAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stop!");

            return Task.CompletedTask;
        }
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("OrderProcessing Service Started");

            var consumerHelper = _kafkaService.Subscribe();

          
            while (!cancellationToken.IsCancellationRequested)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var orderRequest = consumerHelper.Item1.Message.Value;
                    if (orderRequest != null)
                    {
                        _productRepository = scope.ServiceProvider.GetRequiredService<IRepository<Product>>();

                        //Deserilaize 
                        OrderRequest order = JsonConvert.DeserializeObject<OrderRequest>(orderRequest);

                        if (_productRepository.GetAll().Where(x => x.Id == order.id).Count() > 0)
                        {
                            //TODO:: Process Order
                            _logger.LogInformation($"Info: OrderHandler => Processing the order for {order.productname}");
                            order.status = OrderStatus.COMPLETED;
                          
                            await _kafkaService.Publish("readytoship",JsonConvert.SerializeObject(order));
                        }
                        else
                        {
                            order.status = OrderStatus.REJECTED;

                            _logger.LogInformation($"Info: OrderHandler => Rejected the order for {order.productname}");

                            await _kafkaService.Publish("readytoship", JsonConvert.SerializeObject(order));
                        }

                    }
                    await Task.Delay(10);
                 
                }
            }
        }

        

    }
}


using System;
using Confluent.Kafka;

namespace Dotnet.Kafka.Integration
{

    public class ConsumerService
    {

        private string _topicName;
        private ConsumerConfig _consumerConfig;
        private ConsumerBuilder<string, string> _consumerBuilder;
        private readonly IConsumer<string, string> _consumer;
        public ConsumerService(ConsumerConfig config, string topicName)
        {
            _topicName = topicName;
            _consumerConfig = config;
            _consumerBuilder = new ConsumerBuilder<string, string>(this._consumerConfig);
            _consumer = _consumerBuilder.Build();
            _consumer.Subscribe(_topicName);
        }
        public string readMessage()
        {
           
            var consumeResult = this._consumer.Consume(TimeSpan.FromSeconds(10));
            return consumeResult?.Message?.Value;
        }
    }
}
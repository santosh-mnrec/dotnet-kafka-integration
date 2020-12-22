

using System;
using Confluent.Kafka;

namespace dotnet_kafka_integration
{

    public class ConsumerService
    {

        private string _topicName;
        private ConsumerConfig _consumerConfig;
        private ConsumerBuilder<string, string> _consumer;
        private static readonly Random rand = new Random();
        public ConsumerService(ConsumerConfig config, string topicName)
        {
            this._topicName = topicName;
            this._consumerConfig = config;
            this._consumer = new ConsumerBuilder<string, string>(this._consumerConfig);
            this._consumer.Build().Subscribe(topicName);
        }
        public string readMessage()
        {
            var consumeResult = this._consumer.Build().Consume();
            return consumeResult.Message.Value;
        }
    }
}
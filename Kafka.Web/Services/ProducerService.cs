

using System;
using System.Threading.Tasks;
using Confluent.Kafka;

namespace Dotnet.Kafka.Integration
{

    public class ProducerService
    {

        private string _topicName;
        private ProducerConfig _consumerConfig;
        private ProducerBuilder<string, string> _producer;
        private static readonly Random rand = new Random();
        public ProducerService(ProducerConfig config, string topicName)
        {
            this._topicName = topicName;
            this._consumerConfig = config;
            this._producer = new ProducerBuilder<string, string>(this._consumerConfig);
            //  this._producer.Build().OnError += (_,e)=>{
            //     Console.WriteLine("Exception:"+e);
            // };
        }
         public async Task writeMessage(string message){
            var dr = await this._producer.Build().ProduceAsync(this._topicName, new Message<string, string>()
                        {
                            Key = rand.Next(5).ToString(),
                            Value = message
                        });
            Console.WriteLine($"KAFKA => Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'");
            return;
        }
    }
}
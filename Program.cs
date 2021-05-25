using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Extensions.Configuration;

namespace EventGridTest
{
   class Program
   {
      static IConfiguration m_Config;
      static async Task Main(string[] args)
      {
         m_Config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

         Console.WriteLine("Event Grid publish to Topic Test...");

         var egClient = new EventGridClient(new TopicCredentials(m_Config.GetSection("EventGridAccessKey").Value));

         var hostName = new Uri(m_Config.GetSection("EventGridTopicEndpoint").Value).Host;
         await egClient.PublishEventsAsync(hostName, new List<EventGridEvent>()
                                               {
                                                  new EventGridEvent()
                                                  {
                                                     Id = Guid.NewGuid().ToString(),
                                                     Data = new ChyaEvent()
                                                            {
                                                               Message = "Chya Event grid test! Also for logic app!"
                                                            },
                                                     Subject = "Test subject",
                                                     EventType = "ChyaEvent",
                                                     DataVersion = "1.0"
                                                  }
                                               });

         Console.WriteLine("Event published.");
         Console.ReadKey();

      }
   }

   class ChyaEvent
   {
      public string Message { get; set; }

      public override string ToString()
      {
         return Message;
      }
   }
}

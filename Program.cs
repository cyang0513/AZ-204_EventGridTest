using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure;
using Azure.Messaging.EventGrid;
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

         var endpoint = new Uri(m_Config.GetSection("EventGridTopicEndpoint").Value);
         var gridClient = new EventGridPublisherClient(endpoint, 
                                                       new AzureKeyCredential(m_Config.GetSection("EventGridAccessKey").Value));


         await gridClient.SendEventAsync(new EventGridEvent("Test subject", "ChyaEvent", "1.0", 
                                                            new ChyaEvent()
                                                            {
                                                           
                                                               Message = "Chya Event grid test! Also for logic app to send teams message!"
                                                            }, typeof(ChyaEvent)));

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

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure;
using Azure.Identity;
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
         var sasBuilder = new EventGridSasBuilder(endpoint, DateTimeOffset.Now.AddHours(1));
         var signature = sasBuilder.GenerateSas(new AzureKeyCredential(m_Config.GetSection("EventGridAccessKey").Value));

         var gridClient = new EventGridPublisherClient(endpoint, new AzureSasCredential(signature));

         var eventList = new List<EventGridEvent>();
         Console.WriteLine("Type in event data, each event data per line (Q to finish input)...");
         while (true)
         {
            var evData = Console.ReadLine();

            if (evData == "Q")
            {
               break;
            }

            eventList.Add(new EventGridEvent("Test subject", "ChyaEvent", "1.0", 
                                             new ChyaEvent()
                                                   {
                                                      Message = evData
                                                   }, typeof(ChyaEvent)));

         }

         Console.WriteLine($"Publish {eventList.Count} events...");
         await gridClient.SendEventsAsync(eventList);

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

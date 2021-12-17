using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Events;
using Events.Models;
using lab1PSSCAmbrusArmando.Events;

namespace EventProcessor
{
    internal class ItemsPublishedEventHandler : AbstractEventHandler<ItemsPaidEvent>
    {
        public override string[] EventTypes => new string[] { typeof(ItemsPaidEvent).Name };

        protected override Task<EventProcessingResult> OnHandleAsync(ItemsPaidEvent eventData)
        {
            Console.WriteLine(eventData.ToString());
            return Task.FromResult(EventProcessingResult.Completed);
        }
    }
}

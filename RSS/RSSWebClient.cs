using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RSS
{
    class RssWebClient : WebClient
    {
        public Subscription Subscription { get; set; }

        public int MaxItems { get; set; }

        public RssWebClient(Subscription subscription, int maxItems)
        {
            Subscription = subscription;
            MaxItems = maxItems;
        }
    }
}

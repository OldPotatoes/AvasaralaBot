using System;
using System.Collections.Generic;
using System.Text;

namespace BotDynamoDB
{
    public class QuoteCollection
    {
        public IEnumerable<Quote> Quotes { get; set; }
        public string PaginationToken { get; set; }
    }
}

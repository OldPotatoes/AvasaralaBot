namespace BotSqlite
{
    public class Quote
    {
        public string book { get; set; }
        public int chapter { get; set; }
        public int pageNum { get; set; }
        public string quote { get; set; }

        public override string ToString()
        {
            return $"Quote: {quote}";
        }
    }
}
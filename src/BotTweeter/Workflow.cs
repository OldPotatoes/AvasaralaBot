namespace BotTweeter
{
    public class Workflow
    {
        public void RunThrough()
        {
            // Get the last tweet replied to (date or id?)
            // Search for mentions - filter out those older than last tweet
            // Put IDs into to-be-replied-to list
            // Search for #WWAS - filter out those older than last tweet
            // Put IDs into to-be-replied-to list
            // If to-be-replied-to list is too short, search for other tweets
            // Put IDs into unsolicited-replies list
            // loop through lists
            //     Get a random quote
            //     Reply to tweet with quote
            // Note latest tweet replied to
        }
    }
}
using Amazon.Lambda.Core;
using LinqToTwitter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BotTweeter
{
    public class Tweeter
    {
        private readonly TwitterContext _context;

        public Tweeter(String apiKey, String apiSecretKey, String accessToken, String accessTokenSecret)
        {
            var auth = new SingleUserAuthorizer
            {
                CredentialStore = new SingleUserInMemoryCredentialStore
                {
                    ConsumerKey = apiKey,
                    ConsumerSecret = apiSecretKey,
                    AccessToken = accessToken,
                    AccessTokenSecret = accessTokenSecret
                }
            };

            _context = new TwitterContext(auth);
        }

        //public List<Status> GetTweets(String searchText, DateTime lastReplyTime, Int32 numberOfTweets =100)
        //{
        //    LambdaLogger.Log($"GetTweets()\n");
        //    LambdaLogger.Log($"    Get {numberOfTweets} tweets with search text {searchText}, from date {lastReplyTime}.\n");

        //    var statuses = new List<Status>();
        //    var searchResponse =
        //        (from search in _context.Search
        //         where search.Type == SearchType.Search &&
        //               search.Query == searchText &&
        //               search.SearchLanguage == "en" &&
        //               //search.TweetMode == TweetMode.Extended &&
        //               search.Until == lastReplyTime &&
        //               search.Count == numberOfTweets
        //         select search).SingleOrDefault();
        //    if (searchResponse != null)
        //        statuses = searchResponse.Statuses;

        //    LambdaLogger.Log($"    Got {statuses.Count} tweets.\n");

        //    return statuses;
        //}

        public async Task<List<Status>> GetMentions(String screenName, DateTime lastReplyTime, Int32 numberOfTweets = 100)
        {
            // This does not pick up retweets
            LambdaLogger.Log($"GetMentions()\n");
            LambdaLogger.Log($"    Get {numberOfTweets} tweets with screen name {screenName}, from date {lastReplyTime}.\n");

            var mentions = await
                (from tweet in _context.Status
                 where tweet.Type == StatusType.Mentions &&
                       tweet.ScreenName == screenName &&
                       tweet.CreatedAt > lastReplyTime &&
                       tweet.Count == numberOfTweets
                 select tweet).ToListAsync();

            LambdaLogger.Log($"    Got {mentions.Count} mentions.\n");

            return mentions;
        }

        //public async Task<TweetQuery> GetMentions_New(Int64 avasaralaBotUserId, DateTime lastReplyTime, Int32 numberOfTweets = 100)
        //{
        //    // This does not pick up retweets
        //    LambdaLogger.Log($"GetMentions()\n");
        //    LambdaLogger.Log($"    Get {numberOfTweets} tweets with for user ID {avasaralaBotUserId}, from date {lastReplyTime}.\n");

        //    TweetQuery tweetResponse = await (  from tweetQuery in _context.Tweets
        //                                        where tweetQuery.Type == TweetType.MentionsTimeline &&
        //                                              tweetQuery.ID == avasaralaBotUserId.ToString() &&
        //                                              tweetQuery.StartTime == lastReplyTime
        //                                        select tweetQuery).SingleOrDefaultAsync();

        //    if (tweetResponse?.Tweets != null)
        //    {
        //        LambdaLogger.Log("Mentions:");
        //        tweetResponse.Tweets.ForEach(tweet => LambdaLogger.Log($"User ID: {tweet.ID} tweeted: {tweet.Text}"));
        //    }
        //    else
        //        LambdaLogger.Log("No mentions found.");

        //    return tweetResponse;
        //}

        public List<Status> GetTweetsFrom(String tweeter, Int32 numberOfTweets = 100)
        {
            LambdaLogger.Log($"GetTweetsFrom()\n");
            LambdaLogger.Log($"    Get {numberOfTweets} tweets from {tweeter}.\n");
            var tweets =
                (from search in _context.Status
                 where search.Type == StatusType.User &&
                       search.ScreenName == tweeter &&
                       //search.TweetMode == TweetMode.Extended &&
                       search.Count == numberOfTweets
                 select search).ToList();

            return tweets;
        }

        public void PrintTweet(UInt64 tweetId)
        {
            LambdaLogger.Log($"PrintTweet()\n");
            var tweet = GetTweetWithId(tweetId);
            if (tweet == null)
            {
                LambdaLogger.Log($"No tweet with ID: {tweetId}\n");
                return;
            }

            PrintTweet(tweet);
        }

        //public async void PrintTweet_New(UInt64 tweetId)
        //{
        //    LambdaLogger.Log($"PrintTweet()\n");
        //    //var tweet = GetTweetWithId(tweetId);
        //    TweetQuery tweetQuery = await GetTweetWithId_New(tweetId);
        //    if (tweetQuery?.Tweets.Count == 0)
        //    {
        //        LambdaLogger.Log($"No tweet with ID: {tweetId}\n");
        //        return;
        //    }

        //    PrintTweet_New(tweetQuery.Tweets.Single());
        //}

        public Status GetTweetWithId(UInt64 tweetId)
        {
            LambdaLogger.Log($"GetTweetWithId()\n");
            LambdaLogger.Log($"    Get tweet with ID {tweetId}.\n");
            var tweet =
                (from search in _context.Status
                 where search.Type == StatusType.User &&
                       //search.TweetMode == TweetMode.Extended &&
                       search.StatusID == tweetId
                 select search).FirstOrDefault();

            return tweet;
        }

        //public async Task<TweetQuery> GetTweetWithId_New(UInt64 tweetId)
        //{
        //    LambdaLogger.Log($"GetTweetWithId()\n");
        //    LambdaLogger.Log($"    Get tweet with ID {tweetId}.\n");


        //    TweetQuery tweetResponse = await (from tweet in _context.Tweets
        //                                      where tweet.Type == TweetType.Lookup &&
        //                                            tweet.Ids == tweetId.ToString()
        //                                      select tweet).SingleOrDefaultAsync();

        //    if (tweetResponse?.Tweets != null)
        //        LambdaLogger.Log($"Found tweet: User ID: {tweetResponse.Tweets.Single().ID} tweeted: {tweetResponse.Tweets.Single().Text}");
        //    else
        //        LambdaLogger.Log("No tweet found.");

        //    return tweetResponse;
        //}

        public void PrintTweet(Status tweet)
        {
            LambdaLogger.Log($"    TweetID: {tweet.StatusID}\n");

            LambdaLogger.Log($"        Created at: {tweet.CreatedAt} UTC\n");
            LambdaLogger.Log($"        User screen name: {tweet.User.ScreenNameResponse}\n");
            LambdaLogger.Log($"        User name: {tweet.User.Name}\n");
            LambdaLogger.Log($"        Text: {tweet.Text}\n");
            LambdaLogger.Log($"        In Reply to tweet: {tweet.InReplyToStatusID}\n");
            LambdaLogger.Log($"        In Reply to user: {tweet.InReplyToUserID}\n");
            LambdaLogger.Log($"        Is Quoted Status: {tweet.IsQuotedStatus}\n");
            LambdaLogger.Log($"        User Mentions count: {String.Join(", ", tweet.Entities.UserMentionEntities.Select(t => t.ScreenName))}\n");
            //LambdaLogger.Log($"        Contributors count: {String.Join(", ", tweet.Contributors.Select(t => t.ScreenName))}\n");
            //LambdaLogger.Log($"        User count: {String.Join(", ", tweet.Users)}\n");
        }

        //public void PrintTweet_New(Tweet tweet)
        //{
        //    LambdaLogger.Log($"    TweetID: {tweet.ID}\n");

        //    LambdaLogger.Log($"        User ID: {tweet.AuthorID}\n");
        //    LambdaLogger.Log($"        Conversation Tweet ID: {tweet.ConversationID}\n");
        //    LambdaLogger.Log($"        Created at: {tweet.CreatedAt} UTC\n");
        //    LambdaLogger.Log($"        Mentions: {String.Join(", ", tweet.Entities.Mentions.Select(t=> t.Username))}\n");
        //    LambdaLogger.Log($"        In Reply to user: {tweet.InReplyToUserID}\n");
        //    LambdaLogger.Log($"        References: {String.Join(", ", tweet.ReferencedTweets.Select(t => $"{t.ID} is a {t.Type}"))}\n");
        //    LambdaLogger.Log($"        Text: {tweet.Text}\n");
        //}

        public UInt64 MaybeTweet(String tweetText, Boolean actuallyTweet)
        {
            LambdaLogger.Log($"MaybeTweet()\n");
            LambdaLogger.Log($"    Maybe ({actuallyTweet}) tweet: {tweetText}.\n");
            UInt64 id = 0;

            if (actuallyTweet)
            {
                id = Tweet(tweetText, null);
                LambdaLogger.Log($"    Actually tweeted: {tweetText}\n");
            }
            else
            {
                LambdaLogger.Log($"    Not actually tweeted: {tweetText}\n");
            }

            return id;
        }

        public UInt64 Tweet(String tweetText, List<UInt64> mediaIds)
        {
            LambdaLogger.Log($"Tweet()\n");
            LambdaLogger.Log($"    Tweet {tweetText} with: {mediaIds?.Count} media IDs.\n");
            UInt64 id = 0;

            try
            {
                Status status;
                if (mediaIds == null || mediaIds.Count == 0)
                    status = _context.TweetAsync(tweetText).Result;
                else
                    status = _context.TweetAsync(tweetText, mediaIds).Result;

                if (status == null)
                {
                    LambdaLogger.Log("    Tweet failed to process, but API did not report an error\n");
                }
                else
                {
                    id = status.StatusID;
                }
            }
            catch (Exception ex)
            {
                LambdaLogger.Log($"Exception: {ex.Message}\n");
            }

            return id;
        }

        public UInt64 MaybeReply(String tweetText, ulong tweetId, Boolean actuallyTweet)
        {
            LambdaLogger.Log($"MaybeReply()\n");
            LambdaLogger.Log($"    Maybe reply ({actuallyTweet}) to tweet ID {tweetId}, tweet: {tweetText}.\n");
            UInt64 id = 0;

            if (actuallyTweet)
            {
                id = Reply(tweetText, tweetId, null);
                LambdaLogger.Log($"    Actually replied: {tweetText}\n");
            }
            else
            {
                LambdaLogger.Log($"    Not actually replied: {tweetText}\n");
            }

            return id;
        }


        public UInt64 Reply(String tweetText, ulong tweetId, List<UInt64> mediaIds)
        {
            LambdaLogger.Log($"Reply()\n");
            LambdaLogger.Log($"    Reply to tweet ID {tweetId}, tweet: {tweetText} with: {mediaIds?.Count} media IDs.\n");
            UInt64 id = 0;

            try
            {
                Status status;
                if (mediaIds == null || mediaIds.Count == 0)
                    status = _context.ReplyAsync(tweetId, tweetText).Result;
                else
                    status = _context.ReplyAsync(tweetId, tweetText, mediaIds).Result;

                if (status == null)
                {
                    LambdaLogger.Log("    Reply failed to process, but API did not report an error\n");
                }
                else
                {
                    id = status.StatusID;
                }
            }
            catch (Exception ex)
            {
                LambdaLogger.Log($"Exception: {ex.Message}\n");
            }

            return id;
        }
    }
}

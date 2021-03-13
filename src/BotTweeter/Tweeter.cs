using Amazon.Lambda.Core;
using LinqToTwitter;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public List<Status> GetTweets(String searchText, DateTime lastReplyTime, Int32 numberOfTweets =100)
        {
            LambdaLogger.Log($"Get {numberOfTweets} tweets with search text {searchText}, from date {lastReplyTime}.\n");

            var statuses = new List<Status>();
            var searchResponse =
                (from search in _context.Search
                 where search.Type == SearchType.Search &&
                       search.Query == searchText &&
                       search.SearchLanguage == "en" &&
                       search.TweetMode == TweetMode.Extended &&
                       search.Until == lastReplyTime &&
                       search.Count == numberOfTweets
                 select search).SingleOrDefault();
            if (searchResponse != null)
                statuses = searchResponse.Statuses;

            LambdaLogger.Log($"Got {statuses.Count} tweets.\n");

            return statuses;
        }

        public List<Status> GetMentions(String screenName, DateTime lastReplyTime, Int32 numberOfTweets = 100)
        {
            // This does not pick up retweets
            LambdaLogger.Log($"Get {numberOfTweets} tweets with screen name {screenName}, from date {lastReplyTime}.\n");

            var mentions =
                (from tweet in _context.Status
                 where tweet.Type == StatusType.Mentions &&
                       tweet.ScreenName == screenName &&
                       tweet.CreatedAt > lastReplyTime &&
                       tweet.Count == numberOfTweets
                 select tweet).ToListAsync().Result;


            LambdaLogger.Log($"Got {mentions.Count} tweets.\n");

            return mentions;
        }

        public List<Status> GetTweetsFrom(String tweeter, Int32 numberOfTweets = 100)
        {
            var tweets =
                (from search in _context.Status
                 where search.Type == StatusType.User &&
                       search.ScreenName == tweeter &&
                       search.TweetMode == TweetMode.Extended &&
                       search.Count == numberOfTweets
                 select search).ToList();

            return tweets;
        }

        public List<Status> GetTweetWithId(UInt64 tweetId)
        {
            var tweetList =
                (from search in _context.Status
                 where search.Type == StatusType.User &&
                       search.TweetMode == TweetMode.Extended &&
                       search.StatusID == tweetId
                 select search).ToList();

            return tweetList;
        }

        public UInt64 MaybeTweet(String tweetText, Boolean actuallyTweet)
        {
            UInt64 id = 0;

            if (actuallyTweet)
            {
                id = Tweet(tweetText, null);
                LambdaLogger.Log($"Actually tweeted: {tweetText}\n");
            }
            else
            {
                LambdaLogger.Log($"Not actually tweeted: {tweetText}\n");
            }

            return id;
        }

        public UInt64 Tweet(String tweetText, List<UInt64> mediaIds)
        {
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
                    LambdaLogger.Log("Tweet failed to process, but API did not report an error\n");
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
            UInt64 id = 0;

            if (actuallyTweet)
            {
                id = Reply(tweetText, tweetId, null);
                LambdaLogger.Log($"Actually replied: {tweetText}\n");
            }
            else
            {
                LambdaLogger.Log($"Not actually replied: {tweetText}\n");
            }

            return id;
        }


        public UInt64 Reply(String tweetText, ulong tweetId, List<UInt64> mediaIds)
        {
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
                    LambdaLogger.Log("Reply failed to process, but API did not report an error\n");
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

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
            LambdaLogger.Log($"GetTweets()\n");
            LambdaLogger.Log($"    Get {numberOfTweets} tweets with search text {searchText}, from date {lastReplyTime}.\n");

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

            LambdaLogger.Log($"    Got {statuses.Count} tweets.\n");

            return statuses;
        }

        public List<Status> GetMentions(String screenName, DateTime lastReplyTime, Int32 numberOfTweets = 100)
        {
            // This does not pick up retweets
            LambdaLogger.Log($"GetMentions()\n");
            LambdaLogger.Log($"    Get {numberOfTweets} tweets with screen name {screenName}, from date {lastReplyTime}.\n");

            var mentions =
                (from tweet in _context.Status
                 where tweet.Type == StatusType.Mentions &&
                       tweet.ScreenName == screenName &&
                       tweet.CreatedAt > lastReplyTime &&
                       tweet.Count == numberOfTweets
                 select tweet).ToListAsync().Result;


            LambdaLogger.Log($"    Got {mentions.Count} mentions.\n");

            return mentions;
        }

        public List<Status> GetTweetsFrom(String tweeter, Int32 numberOfTweets = 100)
        {
            LambdaLogger.Log($"GetTweetsFrom()\n");
            LambdaLogger.Log($"    Get {numberOfTweets} tweets from {tweeter}.\n");
            var tweets =
                (from search in _context.Status
                 where search.Type == StatusType.User &&
                       search.ScreenName == tweeter &&
                       search.TweetMode == TweetMode.Extended &&
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

        public Status GetTweetWithId(UInt64 tweetId)
        {
            LambdaLogger.Log($"GetTweetWithId()\n");
            LambdaLogger.Log($"    Get tweet with ID {tweetId}.\n");
            var tweet =
                (from search in _context.Status
                 where search.Type == StatusType.User &&
                       search.TweetMode == TweetMode.Extended &&
                       search.StatusID == tweetId
                 select search).FirstOrDefault();

            return tweet;
        }

        public void PrintTweet(Status tweet)
        {
            LambdaLogger.Log($"    TweetID: {tweet.StatusID}\n");

            LambdaLogger.Log($"        Created at: {tweet.CreatedAt} UTC\n");
            LambdaLogger.Log($"        User screen name: {tweet.User.ScreenNameResponse}\n");
            LambdaLogger.Log($"        User name: {tweet.User.Name}\n");
            LambdaLogger.Log($"        Text: {tweet.Text}\n");
            LambdaLogger.Log($"        In Reply to tweet: {tweet.InReplyToStatusID}\n");
            LambdaLogger.Log($"        In Reply to user: {tweet.InReplyToUserID}\n");
            LambdaLogger.Log($"        User Mentions count: {tweet.Entities.UserMentionEntities.Count}\n");

            foreach (UserMentionEntity ume in tweet.Entities.UserMentionEntities)
            {
                LambdaLogger.Log($"        User Mention: {ume.Id}, {ume.Name}, {ume.ScreenName}\n");
            }
            LambdaLogger.Log($"        Contributors count: {tweet.Contributors.Count}\n");
            foreach (Contributor contrib in tweet.Contributors)
            {
                LambdaLogger.Log($"        Contributor: {contrib.ID}, {contrib.ScreenName}\n");
            }
            LambdaLogger.Log($"        User count: {tweet.Users.Count}\n");
            foreach (ulong user in tweet.Users)
            {
                LambdaLogger.Log($"        User: {user}\n");
            }
        }

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

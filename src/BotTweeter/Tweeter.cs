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

        public List<Status> GetTweets(String searchText, Int32 numberOfTweets=100)
        {
            var searchResponse =
                (from search in _context.Search
                 where search.Type == SearchType.Search &&
                       search.Query == searchText &&
                       search.SearchLanguage == "en" &&
                       search.TweetMode == TweetMode.Extended &&
                    //    search.Until == new DateTime(2018, 12, 31) &&
                       search.Count == numberOfTweets
                 select search).SingleOrDefault();

            return searchResponse.Statuses;
        }

        public void GetTweetsFrom(String tweeter, Int32 numberOfTweets=100)
        {
            var tweetList =
                (from search in _context.Status
                 where search.Type == StatusType.User &&
                       search.ScreenName == tweeter &&
                       search.TweetMode == TweetMode.Extended &&
                       search.Count == numberOfTweets
                 select search).ToList();
        }

        public void MaybeTweet(String tweetText, Boolean actuallyTweet)
        {
            if (actuallyTweet)
            {
                Tweet(tweetText, null);
                LambdaLogger.Log($"Actually tweeted: {tweetText}\n");
            }
            else
            {
                LambdaLogger.Log($"Not actually tweeted: {tweetText}\n");
            }
        }


        public void Tweet(String tweetText, List<UInt64> mediaIds)
        {
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
            }
            catch (Exception ex)
            {
                LambdaLogger.Log($"Exception: {ex.Message}\n");
            }
        }
    }
}

# AvasaralaBot

AvarsaralaBot is a Twitter bot that quotes lines from the Expanse character Chrisjen Avasarala. It has a database of quotes from both the books and the tv shows. The database records are flagged as statements or replies. Statements are tweeted daily. Replies are used to reply to anyone who interacts with AvasaralaBot.

It is run on AWS Lambda, with the data stored in AWS DynamoDB. It's written in C# and uses LinqToTwitter to interact with Twitter.


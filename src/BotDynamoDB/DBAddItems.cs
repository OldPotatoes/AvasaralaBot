using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;

namespace BotDynamoDB
{
    public class DBAddItems
    {
        public static BatchWriteItemRequest MakeRequest(String tableName)
        {
            var request = new BatchWriteItemRequest
            {
                ReturnConsumedCapacity = "TOTAL",
                RequestItems = MakeRequestItems(tableName)
            };

            return request;
        }

        private static Dictionary<String, List<WriteRequest>> MakeRequestItems(String tableName)
        {
            Dictionary<String, List<WriteRequest>> items = new Dictionary<String, List<WriteRequest>>
            {
                {
                    tableName,
                    MakeWriteRequestsC()
                }
            };

            return items;
        }

        private static List<WriteRequest> MakeWriteRequestsA()
        {
            var requests = new List<WriteRequest>
            {
                new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = new Dictionary<String, AttributeValue>
                        {
                            { "ID", new AttributeValue { N = "1"} },
                            { "Book", new AttributeValue { S = "Caliban's War" } },
                            { "Chapter", new AttributeValue { S = "Chapter Five: Avasarala"} },
                            { "Page", new AttributeValue { N = "48" } },
                            { "QuoteType", new AttributeValue { S = "Statement"} },
                            { "Quote", new AttributeValue { S = "Fuck him." } },
                        }
                    }
                },
                new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = new Dictionary<String, AttributeValue>
                        {
                            { "ID", new AttributeValue { N = "2"} },
                            { "Book", new AttributeValue { S = "Caliban's War" } },
                            { "Chapter", new AttributeValue { S = "Chapter Five: Avasarala"} },
                            { "Page", new AttributeValue { N = "48" } },
                            { "QuoteType", new AttributeValue { S = "Response"} },
                            { "Quote", new AttributeValue { S = "The Afghanis have been resisting external rule since before my ancestors were kicking out the British. As soon as I figure out how to change that, I’ll let him know." } },
                        }
                    }
                },
                new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = new Dictionary<String, AttributeValue>
                        {
                            { "ID", new AttributeValue { N = "3"} },
                            { "Book", new AttributeValue { S = "Caliban's War" } },
                            { "Chapter", new AttributeValue { S = "Chapter Five: Avasarala"} },
                            { "Page", new AttributeValue { N = "48" } },
                            { "QuoteType", new AttributeValue { S = "Response"} },
                            { "Quote", new AttributeValue { S = "I don’t have time to get another PhD to read it, so if it’s not in clear, concise language, fire the sonofabitch and get someone who knows how to write." } },
                        }
                    }
                },
                new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = new Dictionary<String, AttributeValue>
                        {
                            { "ID", new AttributeValue { N = "4"} },
                            { "Book", new AttributeValue { S = "Caliban's War" } },
                            { "Chapter", new AttributeValue { S = "Chapter Five: Avasarala"} },
                            { "Page", new AttributeValue { N = "49" } },
                            { "QuoteType", new AttributeValue { S = "Statement"} },
                            { "Quote", new AttributeValue { S = "They’re all fucking men." } },
                        }
                    }
                },
                new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = new Dictionary<String, AttributeValue>
                        {
                            { "ID", new AttributeValue { N = "5"} },
                            { "Book", new AttributeValue { S = "Caliban's War" } },
                            { "Chapter", new AttributeValue { S = "Chapter Five: Avasarala"} },
                            { "Page", new AttributeValue { N = "51" } },
                            { "QuoteType", new AttributeValue { S = "Statement"} },
                            { "Quote", new AttributeValue { S = "Just don’t let the bobble-head talk." } },
                        }
                    }
                },
                new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = new Dictionary<String, AttributeValue>
                        {
                            { "ID", new AttributeValue { N = "6"} },
                            { "Book", new AttributeValue { S = "Caliban's War" } },
                            { "Chapter", new AttributeValue { S = "Chapter Five: Avasarala"} },
                            { "Page", new AttributeValue { N = "51" } },
                            { "QuoteType", new AttributeValue { S = "Response"} },
                            { "Quote", new AttributeValue { S = "That’s because he’s a fucking bobble-head." } },
                        }
                    }
                },
                new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = new Dictionary<String, AttributeValue>
                        {
                            { "ID", new AttributeValue { N = "7"} },
                            { "Book", new AttributeValue { S = "Caliban's War" } },
                            { "Chapter", new AttributeValue { S = "Chapter Five: Avasarala"} },
                            { "Page", new AttributeValue { N = "53" } },
                            { "QuoteType", new AttributeValue { S = "Response"} },
                            { "Quote", new AttributeValue { S = "But you can give him the news. Hearing it from an old grandma like me makes his dick shrink." } },
                        }
                    }
                },
                new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = new Dictionary<String, AttributeValue>
                        {
                            { "ID", new AttributeValue { N = "8"} },
                            { "Book", new AttributeValue { S = "Caliban's War" } },
                            { "Chapter", new AttributeValue { S = "Chapter Five: Avasarala"} },
                            { "Page", new AttributeValue { N = "54" } },
                            { "QuoteType", new AttributeValue { S = "Response"} },
                            { "Quote", new AttributeValue { S = "So get the fuck out." } },
                        }
                    }
                },
                new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = new Dictionary<String, AttributeValue>
                        {
                            { "ID", new AttributeValue { N = "9"} },
                            { "Book", new AttributeValue { S = "Caliban's War" } },
                            { "Chapter", new AttributeValue { S = "Chapter Nine: Avasarala"} },
                            { "Page", new AttributeValue { N = "90" } },
                            { "QuoteType", new AttributeValue { S = "Response"} },
                            { "Quote", new AttributeValue { S = "Well, that’s worth shit." } },
                        }
                    }
                },
                new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = new Dictionary<String, AttributeValue>
                        {
                            { "ID", new AttributeValue { N = "10"} },
                            { "Book", new AttributeValue { S = "Caliban's War" } },
                            { "Chapter", new AttributeValue { S = "Chapter Nine: Avasarala"} },
                            { "Page", new AttributeValue { N = "91" } },
                            { "QuoteType", new AttributeValue { S = "Statement"} },
                            { "Quote", new AttributeValue { S = "I know what’s in the briefing. I know everything. This is a fucking test. I’m asking what you think is important about him." } },
                        }
                    }
                },
                new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = new Dictionary<String, AttributeValue>
                        {
                            { "ID", new AttributeValue { N = "11"} },
                            { "Book", new AttributeValue { S = "Caliban's War" } },
                            { "Chapter", new AttributeValue { S = "Chapter Nine: Avasarala"} },
                            { "Page", new AttributeValue { N = "92" } },
                            { "QuoteType", new AttributeValue { S = "Response"} },
                            { "Quote", new AttributeValue { S = "Tell her I said to get the hell out of bed before her husband starts getting dirty ideas." } },
                        }
                    }
                },
                new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = new Dictionary<String, AttributeValue>
                        {
                            { "ID", new AttributeValue { N = "12"} },
                            { "Book", new AttributeValue { S = "Caliban's War" } },
                            { "Chapter", new AttributeValue { S = "Chapter Nine: Avasarala"} },
                            { "Page", new AttributeValue { N = "92" } },
                            { "QuoteType", new AttributeValue { S = "Statement"} },
                            { "Quote", new AttributeValue { S = "No one likes a smart-ass." } },
                        }
                    }
                },
                new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = new Dictionary<String, AttributeValue>
                        {
                            { "ID", new AttributeValue { N = "13"} },
                            { "Book", new AttributeValue { S = "Caliban's War" } },
                            { "Chapter", new AttributeValue { S = "Chapter Nine: Avasarala"} },
                            { "Page", new AttributeValue { N = "93" } },
                            { "QuoteType", new AttributeValue { S = "Question"} },
                            { "Quote", new AttributeValue { S = "You want some tea?" } },
                        }
                    }
                },
                new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = new Dictionary<String, AttributeValue>
                        {
                            { "ID", new AttributeValue { N = "14"} },
                            { "Book", new AttributeValue { S = "Caliban's War" } },
                            { "Chapter", new AttributeValue { S = "Chapter Nine: Avasarala"} },
                            { "Page", new AttributeValue { N = "93" } },
                            { "QuoteType", new AttributeValue { S = "Statement"} },
                            { "Quote", new AttributeValue { S = "Go get me some tea." } },
                        }
                    }
                },
                new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = new Dictionary<String, AttributeValue>
                        {
                            { "ID", new AttributeValue { N = "15"} },
                            { "Book", new AttributeValue { S = "Caliban's War" } },
                            { "Chapter", new AttributeValue { S = "Chapter Nine: Avasarala"} },
                            { "Page", new AttributeValue { N = "93" } },
                            { "QuoteType", new AttributeValue { S = "Question"} },
                            { "Quote", new AttributeValue { S = "Those rat fuckers?" } },
                        }
                    }
                },
                new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = new Dictionary<String, AttributeValue>
                        {
                            { "ID", new AttributeValue { N = "16"} },
                            { "Book", new AttributeValue { S = "Caliban's War" } },
                            { "Chapter", new AttributeValue { S = "Chapter Nine: Avasarala"} },
                            { "Page", new AttributeValue { N = "96" } },
                            { "QuoteType", new AttributeValue { S = "Statement"} },
                            { "Quote", new AttributeValue { S = "If I find out that you knew something and you didn’t tell me, I won’t take it well." } },
                        }
                    }
                },
                new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = new Dictionary<String, AttributeValue>
                        {
                            { "ID", new AttributeValue { N = "17"} },
                            { "Book", new AttributeValue { S = "Caliban's War" } },
                            { "Chapter", new AttributeValue { S = "Chapter Nine: Avasarala"} },
                            { "Page", new AttributeValue { N = "96" } },
                            { "QuoteType", new AttributeValue { S = "Statement"} },
                            { "Quote", new AttributeValue { S = "I’m not someone you want to fuck with." } },
                        }
                    }
                },
                new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = new Dictionary<String, AttributeValue>
                        {
                            { "ID", new AttributeValue { N = "18"} },
                            { "Book", new AttributeValue { S = "Caliban's War" } },
                            { "Chapter", new AttributeValue { S = "Chapter Nine: Avasarala"} },
                            { "Page", new AttributeValue { N = "96" } },
                            { "QuoteType", new AttributeValue { S = "Response"} },
                            { "Quote", new AttributeValue { S = "Fat lot of help you were, you smug bastard." } },
                        }
                    }
                },
                new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = new Dictionary<String, AttributeValue>
                        {
                            { "ID", new AttributeValue { N = "19"} },
                            { "Book", new AttributeValue { S = "Caliban's War" } },
                            { "Chapter", new AttributeValue { S = "Chapter Nine: Avasarala"} },
                            { "Page", new AttributeValue { N = "97" } },
                            { "QuoteType", new AttributeValue { S = "Statement"} },
                            { "Quote", new AttributeValue { S = "When I said don’t hurry, I didn’t mean you should take the whole fucking day off." } },
                        }
                    }
                },
                new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = new Dictionary<String, AttributeValue>
                        {
                            { "ID", new AttributeValue { N = "20"} },
                            { "Book", new AttributeValue { S = "Caliban's War" } },
                            { "Chapter", new AttributeValue { S = "Chapter Nine: Avasarala"} },
                            { "Page", new AttributeValue { N = "97" } },
                            { "QuoteType", new AttributeValue { S = "Statement"} },
                            { "Quote", new AttributeValue { S = "You could just send me a fucking copy." } },
                        }
                    }
                },
                new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = new Dictionary<String, AttributeValue>
                        {
                            { "ID", new AttributeValue { N = "21"} },
                            { "Book", new AttributeValue { S = "Caliban's War" } },
                            { "Chapter", new AttributeValue { S = "Chapter Nine : Avasarala"} },
                            { "Page", new AttributeValue { N = "99" } },
                            { "QuoteType", new AttributeValue { S = "Statement"} },
                            { "Quote", new AttributeValue { S = "Do not underestimate his capacity to fuck things up." } },
                        }
                    }
                },
                new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = new Dictionary<String, AttributeValue>
                        {
                            { "ID", new AttributeValue { N = "22"} },
                            { "Book", new AttributeValue { S = "Caliban's War" } },
                            { "Chapter", new AttributeValue { S = "Chapter Twelve: Avasarala"} },
                            { "Page", new AttributeValue { N = "123" } },
                            { "QuoteType", new AttributeValue { S = "Statement"} },
                            { "Quote", new AttributeValue { S = "That’s crap." } },
                        }
                    }
                },
                new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = new Dictionary<String, AttributeValue>
                        {
                            { "ID", new AttributeValue { N = "23"} },
                            { "Book", new AttributeValue { S = "Caliban's War" } },
                            { "Chapter", new AttributeValue { S = "Chapter Twelve: Avasarala"} },
                            { "Page", new AttributeValue { N = "123" } },
                            { "QuoteType", new AttributeValue { S = "Question"} },
                            { "Quote", new AttributeValue { S = "If someone doesn’t get those half-feral bastards out of the slums, who are you university types going to teach?" } },
                        }
                    }
                },
                new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = new Dictionary<String, AttributeValue>
                        {
                            { "ID", new AttributeValue { N = "24"} },
                            { "Book", new AttributeValue { S = "Caliban's War" } },
                            { "Chapter", new AttributeValue { S = "Chapter Twelve: Avasarala"} },
                            { "Page", new AttributeValue { N = "123" } },
                            { "QuoteType", new AttributeValue { S = "Statement"} },
                            { "Quote", new AttributeValue { S = "What makes them less human is they don’t fucking meditate." } },
                        }
                    }
                },
                new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = new Dictionary<String, AttributeValue>
                        {
                            { "ID", new AttributeValue { N = "25"} },
                            { "Book", new AttributeValue { S = "Caliban's War" } },
                            { "Chapter", new AttributeValue { S = "Chapter Twelve: Avasarala"} },
                            { "Page", new AttributeValue { N = "124" } },
                            { "QuoteType", new AttributeValue { S = "Response"} },
                            { "Quote", new AttributeValue { S = "There’s a reason for that." } },
                        }
                    }
                },
            };

            return requests;
        }

        private static List<WriteRequest> MakeWriteRequestsB()
        {
            var requests = new List<WriteRequest>
            {
                new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = new Dictionary<String, AttributeValue>
                        {
                            { "ID", new AttributeValue { N = "26"} },
                            { "Book", new AttributeValue { S = "Caliban's War" } },
                            { "Chapter", new AttributeValue { S = "Chapter Twelve: Avasarala"} },
                            { "Page", new AttributeValue { N = "125" } },
                            { "QuoteType", new AttributeValue { S = "Statement"} },
                            { "Quote", new AttributeValue { S = "These cunts are digging into my grandma time." } },
                        }
                    }
                },
                new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = new Dictionary<String, AttributeValue>
                        {
                            { "ID", new AttributeValue { N = "27"} },
                            { "Book", new AttributeValue { S = "Caliban's War" } },
                            { "Chapter", new AttributeValue { S = "Chapter Twelve: Avasarala"} },
                            { "Page", new AttributeValue { N = "126" } },
                            { "QuoteType", new AttributeValue { S = "Response"} },
                            { "Quote", new AttributeValue { S = "Now why the fuck are you calling me?" } },
                        }
                    }
                },
                new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = new Dictionary<String, AttributeValue>
                        {
                            { "ID", new AttributeValue { N = "28"} },
                            { "Book", new AttributeValue { S = "Caliban's War" } },
                            { "Chapter", new AttributeValue { S = "Chapter Twelve: Avasarala"} },
                            { "Page", new AttributeValue { N = "126" } },
                            { "QuoteType", new AttributeValue { S = "Response"} },
                            { "Quote", new AttributeValue { S = "Your friend has loose lips." } },
                        }
                    }
                },
                new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = new Dictionary<String, AttributeValue>
                        {
                            { "ID", new AttributeValue { N = "29"} },
                            { "Book", new AttributeValue { S = "Caliban's War" } },
                            { "Chapter", new AttributeValue { S = "Chapter Twelve: Avasarala"} },
                            { "Page", new AttributeValue { N = "127" } },
                            { "QuoteType", new AttributeValue { S = "Response"} },
                            { "Quote", new AttributeValue { S = "You live in your own world, dear one." } },
                        }
                    }
                },
                new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = new Dictionary<String, AttributeValue>
                        {
                            { "ID", new AttributeValue { N = "30"} },
                            { "Book", new AttributeValue { S = "Caliban's War" } },
                            { "Chapter", new AttributeValue { S = "Chapter Twelve: Avasarala"} },
                            { "Page", new AttributeValue { N = "127" } },
                            { "QuoteType", new AttributeValue { S = "Response"} },
                            { "Quote", new AttributeValue { S = "Stay here. Read about post-lyricism." } },
                        }
                    }
                },
                new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = new Dictionary<String, AttributeValue>
                        {
                            { "ID", new AttributeValue { N = "31"} },
                            { "Book", new AttributeValue { S = "Caliban's War" } },
                            { "Chapter", new AttributeValue { S = "Chapter Twelve: Avasarala"} },
                            { "Page", new AttributeValue { N = "127" } },
                            { "QuoteType", new AttributeValue { S = "Statement"} },
                            { "Quote", new AttributeValue { S = "Try to keep civilization from blowing up while the children are in it." } },
                        }
                    }
                },
                new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = new Dictionary<String, AttributeValue>
                        {
                            { "ID", new AttributeValue { N = "32"} },
                            { "Book", new AttributeValue { S = "Caliban's War" } },
                            { "Chapter", new AttributeValue { S = "Chapter Twelve: Avasarala"} },
                            { "Page", new AttributeValue { N = "129" } },
                            { "QuoteType", new AttributeValue { S = "Question"} },
                            { "Quote", new AttributeValue { S = "What’s he going to do? Go crying to his mama that I took his toys away?" } },
                        }
                    }
                },
                new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = new Dictionary<String, AttributeValue>
                        {
                            { "ID", new AttributeValue { N = "33"} },
                            { "Book", new AttributeValue { S = "Caliban's War" } },
                            { "Chapter", new AttributeValue { S = "Chapter Twelve: Avasarala"} },
                            { "Page", new AttributeValue { N = "129" } },
                            { "QuoteType", new AttributeValue { S = "Statement"} },
                            { "Quote", new AttributeValue { S = "If he can’t play with the big kids, he shouldn’t be a fucking admiral." } },
                        }
                    }
                },
                new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = new Dictionary<String, AttributeValue>
                        {
                            { "ID", new AttributeValue { N = "34"} },
                            { "Book", new AttributeValue { S = "Caliban's War" } },
                            { "Chapter", new AttributeValue { S = "Chapter Twelve: Avasarala"} },
                            { "Page", new AttributeValue { N = "129" } },
                            { "QuoteType", new AttributeValue { S = "Response"} },
                            { "Quote", new AttributeValue { S = "So fuck them." } },
                        }
                    }
                },
                new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = new Dictionary<String, AttributeValue>
                        {
                            { "ID", new AttributeValue { N = "35"} },
                            { "Book", new AttributeValue { S = "Caliban's War" } },
                            { "Chapter", new AttributeValue { S = "Chapter Twelve: Avasarala"} },
                            { "Page", new AttributeValue { N = "129" } },
                            { "QuoteType", new AttributeValue { S = "Statement"} },
                            { "Quote", new AttributeValue { S = "It’s the damn bogeyman." } },
                        }
                    }
                },
                new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = new Dictionary<String, AttributeValue>
                        {
                            { "ID", new AttributeValue { N = "36"} },
                            { "Book", new AttributeValue { S = "Caliban's War" } },
                            { "Chapter", new AttributeValue { S = "Chapter Twelve: Avasarala"} },
                            { "Page", new AttributeValue { N = "130" } },
                            { "QuoteType", new AttributeValue { S = "Response"} },
                            { "Quote", new AttributeValue { S = "No fucking surprise there." } },
                        }
                    }
                },
                new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = new Dictionary<String, AttributeValue>
                        {
                            { "ID", new AttributeValue { N = "37"} },
                            { "Book", new AttributeValue { S = "Caliban's War" } },
                            { "Chapter", new AttributeValue { S = "Chapter Twelve: Avasarala"} },
                            { "Page", new AttributeValue { N = "130" } },
                            { "QuoteType", new AttributeValue { S = "Statement"} },
                            { "Quote", new AttributeValue { S = "It’s not healthy having God sleeping right there where we can all watch him dream." } },
                        }
                    }
                },
                new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = new Dictionary<String, AttributeValue>
                        {
                            { "ID", new AttributeValue { N = "38"} },
                            { "Book", new AttributeValue { S = "Caliban's War" } },
                            { "Chapter", new AttributeValue { S = "Chapter Twelve: Avasarala"} },
                            { "Page", new AttributeValue { N = "130" } },
                            { "QuoteType", new AttributeValue { S = "Statement"} },
                            { "Quote", new AttributeValue { S = "So we all look away and go about things as if the universe were the same as when we were young, but we know better." } },
                        }
                    }
                },
                new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = new Dictionary<String, AttributeValue>
                        {
                            { "ID", new AttributeValue { N = "39"} },
                            { "Book", new AttributeValue { S = "Caliban's War" } },
                            { "Chapter", new AttributeValue { S = "Chapter Twelve: Avasarala"} },
                            { "Page", new AttributeValue { N = "130" } },
                            { "QuoteType", new AttributeValue { S = "Statement"} },
                            { "Quote", new AttributeValue { S = "The inexplicable didn’t used to eat planets," } },
                        }
                    }
                },
                new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = new Dictionary<String, AttributeValue>
                        {
                            { "ID", new AttributeValue { N = "40"} },
                            { "Book", new AttributeValue { S = "Caliban's War" } },
                            { "Chapter", new AttributeValue { S = "Chapter Eighteen: Avasarala"} },
                            { "Page", new AttributeValue { N = "190" } },
                            { "QuoteType", new AttributeValue { S = "Response"} },
                            { "Quote", new AttributeValue { S = "I can’t tell you what that means coming from you." } },
                        }
                    }
                },
                new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = new Dictionary<String, AttributeValue>
                        {
                            { "ID", new AttributeValue { N = "41"} },
                            { "Book", new AttributeValue { S = "Caliban's War" } },
                            { "Chapter", new AttributeValue { S = "Chapter Eighteen: Avasarala"} },
                            { "Page", new AttributeValue { N = "197" } },
                            { "QuoteType", new AttributeValue { S = "Response"} },
                            { "Quote", new AttributeValue { S = "What the fuck is that supposed to mean?" } },
                        }
                    }
                },
                new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = new Dictionary<String, AttributeValue>
                        {
                            { "ID", new AttributeValue { N = "42"} },
                            { "Book", new AttributeValue { S = "Caliban's War" } },
                            { "Chapter", new AttributeValue { S = "Chapter Eighteen: Avasarala"} },
                            { "Page", new AttributeValue { N = "197" } },
                            { "QuoteType", new AttributeValue { S = "Statement"} },
                            { "Quote", new AttributeValue { S = "God damn it!" } },
                        }
                    }
                },
                new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = new Dictionary<String, AttributeValue>
                        {
                            { "ID", new AttributeValue { N = "43"} },
                            { "Book", new AttributeValue { S = "Caliban's War" } },
                            { "Chapter", new AttributeValue { S = "Chapter Eighteen: Avasarala"} },
                            { "Page", new AttributeValue { N = "198" } },
                            { "QuoteType", new AttributeValue { S = "Response"} },
                            { "Quote", new AttributeValue { S = "Send in the dogs, break up the fight, and take them in." } },
                        }
                    }
                },
                new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = new Dictionary<String, AttributeValue>
                        {
                            { "ID", new AttributeValue { N = "44"} },
                            { "Book", new AttributeValue { S = "Caliban's War" } },
                            { "Chapter", new AttributeValue { S = "Chapter Twenty: Bobbie"} },
                            { "Page", new AttributeValue { N = "215" } },
                            { "QuoteType", new AttributeValue { S = "Statement"} },
                            { "Quote", new AttributeValue { S = "I can’t find a goddamned thing anymore." } },
                        }
                    }
                },
                new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = new Dictionary<String, AttributeValue>
                        {
                            { "ID", new AttributeValue { N = "45"} },
                            { "Book", new AttributeValue { S = "Caliban's War" } },
                            { "Chapter", new AttributeValue { S = "Chapter Twenty: Bobbie"} },
                            { "Page", new AttributeValue { N = "215" } },
                            { "QuoteType", new AttributeValue { S = "Statement"} },
                            { "Quote", new AttributeValue { S = "Lots of bullshit by overpaid consultants who think they can hide the fact that they don’t actually know anything by talking twice as long." } },
                        }
                    }
                },
                new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = new Dictionary<String, AttributeValue>
                        {
                            { "ID", new AttributeValue { N = "46"} },
                            { "Book", new AttributeValue { S = "Caliban's War" } },
                            { "Chapter", new AttributeValue { S = "Chapter Twenty: Bobbie"} },
                            { "Page", new AttributeValue { N = "216" } },
                            { "QuoteType", new AttributeValue { S = "Response"} },
                            { "Quote", new AttributeValue { S = "Stop busting my balls, Soren. I’m out of tea." } },
                        }
                    }
                },
                new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = new Dictionary<String, AttributeValue>
                        {
                            { "ID", new AttributeValue { N = "47"} },
                            { "Book", new AttributeValue { S = "Caliban's War" } },
                            { "Chapter", new AttributeValue { S = "Chapter Twenty: Bobbie"} },
                            { "Page", new AttributeValue { N = "216" } },
                            { "QuoteType", new AttributeValue { S = "Response"} },
                            { "Quote", new AttributeValue { S = "Meow meow cry meow meow, that’s all I heard you say." } },
                        }
                    }
                },
                new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = new Dictionary<String, AttributeValue>
                        {
                            { "ID", new AttributeValue { N = "48"} },
                            { "Book", new AttributeValue { S = "Caliban's War" } },
                            { "Chapter", new AttributeValue { S = "Chapter Twenty: Bobbie"} },
                            { "Page", new AttributeValue { N = "216" } },
                            { "QuoteType", new AttributeValue { S = "Response"} },
                            { "Quote", new AttributeValue { S = "Was I not clear? Have I lost the ability to speak?" } },
                        }
                    }
                },
                new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = new Dictionary<String, AttributeValue>
                        {
                            { "ID", new AttributeValue { N = "49"} },
                            { "Book", new AttributeValue { S = "Caliban's War" } },
                            { "Chapter", new AttributeValue { S = "Chapter Twenty: Bobbie"} },
                            { "Page", new AttributeValue { N = "222" } },
                            { "QuoteType", new AttributeValue { S = "Response"} },
                            { "Quote", new AttributeValue { S = "You’re my liaison, so fucking liaise. Call your people." } },
                        }
                    }
                },
                new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = new Dictionary<String, AttributeValue>
                        {
                            { "ID", new AttributeValue { N = "50"} },
                            { "Book", new AttributeValue { S = "Caliban's War" } },
                            { "Chapter", new AttributeValue { S = "Chapter Twenty-Three: Avasarala"} },
                            { "Page", new AttributeValue { N = "245" } },
                            { "QuoteType", new AttributeValue { S = "Response"} },
                            { "Quote", new AttributeValue { S = "La la fucking la." } },
                        }
                    }
                }
            };

            return requests;
        }

        private static List<WriteRequest> MakeWriteRequestsC()
        {
            var requests = new List<WriteRequest>
            {
                new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = new Dictionary<String, AttributeValue>
                        {
                            { "ID", new AttributeValue { N = "51"} },
                            { "Book", new AttributeValue { S = "Caliban's War" } },
                            { "Chapter", new AttributeValue { S = "Chapter Twenty-Three: Avasarala"} },
                            { "Page", new AttributeValue { N = "246" } },
                            { "QuoteType", new AttributeValue { S = "Statement"} },
                            { "Quote", new AttributeValue { S = "Everything he’d done was as subtle as a lead pipe to the kneecap." } },
                        }
                    }
                },
                new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = new Dictionary<String, AttributeValue>
                        {
                            { "ID", new AttributeValue { N = "52"} },
                            { "Book", new AttributeValue { S = "Caliban's War" } },
                            { "Chapter", new AttributeValue { S = "Chapter Twenty-Three: Avasarala"} },
                            { "Page", new AttributeValue { N = "248" } },
                            { "QuoteType", new AttributeValue { S = "Question"} },
                            { "Quote", new AttributeValue { S = "Will you please sit down? I feel like I’m at the bottom of a fucking well, talking to you." } },
                        }
                    }
                },
                new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = new Dictionary<String, AttributeValue>
                        {
                            { "ID", new AttributeValue { N = "53"} },
                            { "Book", new AttributeValue { S = "Caliban's War" } },
                            { "Chapter", new AttributeValue { S = "Chapter Twenty-Three: Avasarala"} },
                            { "Page", new AttributeValue { N = "248" } },
                            { "QuoteType", new AttributeValue { S = "Question"} },
                            { "Quote", new AttributeValue { S = "Holy shit! What do you know? I want that too." } },
                        }
                    }
                },
                new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = new Dictionary<String, AttributeValue>
                        {
                            { "ID", new AttributeValue { N = "54"} },
                            { "Book", new AttributeValue { S = "Caliban's War" } },
                            { "Chapter", new AttributeValue { S = "Chapter Twenty-Three: Avasarala"} },
                            { "Page", new AttributeValue { N = "248" } },
                            { "QuoteType", new AttributeValue { S = "Statement"} },
                            { "Quote", new AttributeValue { S = "I’ll try not to oversell the point in the future." } },
                        }
                    }
                },
                new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = new Dictionary<String, AttributeValue>
                        {
                            { "ID", new AttributeValue { N = "55"} },
                            { "Book", new AttributeValue { S = "Caliban's War" } },
                            { "Chapter", new AttributeValue { S = "Chapter Twenty-Three: Avasarala"} },
                            { "Page", new AttributeValue { N = "253" } },
                            { "QuoteType", new AttributeValue { S = "Response"} },
                            { "Quote", new AttributeValue { S = "Knowing you has let me bear the unbearable." } },
                        }
                    }
                },
                new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = new Dictionary<String, AttributeValue>
                        {
                            { "ID", new AttributeValue { N = "56"} },
                            { "Book", new AttributeValue { S = "Caliban's War" } },
                            { "Chapter", new AttributeValue { S = "Chapter Twenty-Five: Bobbie"} },
                            { "Page", new AttributeValue { N = "269" } },
                            { "QuoteType", new AttributeValue { S = "Statement"} },
                            { "Quote", new AttributeValue { S = "No one likes a smart-ass." } },
                        }
                    }
                },
                new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = new Dictionary<String, AttributeValue>
                        {
                            { "ID", new AttributeValue { N = "57"} },
                            { "Book", new AttributeValue { S = "Caliban's War" } },
                            { "Chapter", new AttributeValue { S = "Chapter Twenty-Five: Bobbie"} },
                            { "Page", new AttributeValue { N = "270" } },
                            { "QuoteType", new AttributeValue { S = "Response"} },
                            { "Quote", new AttributeValue { S = "Seriously, sit the fuck down. Five minutes talking to you and I can’t tilt my head forward again for an hour." } },
                        }
                    }
                },
                new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = new Dictionary<String, AttributeValue>
                        {
                            { "ID", new AttributeValue { N = "58"} },
                            { "Book", new AttributeValue { S = "Caliban's War" } },
                            { "Chapter", new AttributeValue { S = "Chapter Twenty-Five: Bobbie"} },
                            { "Page", new AttributeValue { N = "271" } },
                            { "QuoteType", new AttributeValue { S = "Response"} },
                            { "Quote", new AttributeValue { S = "Shut the fuck up now, dear, the grown-up is talking." } },
                        }
                    }
                },
                new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = new Dictionary<String, AttributeValue>
                        {
                            { "ID", new AttributeValue { N = "59"} },
                            { "Book", new AttributeValue { S = "Caliban's War" } },
                            { "Chapter", new AttributeValue { S = "Chapter Twenty-Eight: Avasarala"} },
                            { "Page", new AttributeValue { N = "306" } },
                            { "QuoteType", new AttributeValue { S = "Response"} },
                            { "Quote", new AttributeValue { S = "I fucking bet you do." } },
                        }
                    }
                },
                new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = new Dictionary<String, AttributeValue>
                        {
                            { "ID", new AttributeValue { N = "60"} },
                            { "Book", new AttributeValue { S = "Caliban's War" } },
                            { "Chapter", new AttributeValue { S = "Chapter Thirty: Bobbie"} },
                            { "Page", new AttributeValue { N = "331" } },
                            { "QuoteType", new AttributeValue { S = "Statement"} },
                            { "Quote", new AttributeValue { S = "The level we’re playing at has different rules. It’s like playing go. It’s all about exerting influence. Controlling the board without occupying it." } },
                        }
                    }
                },
                new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = new Dictionary<String, AttributeValue>
                        {
                            { "ID", new AttributeValue { N = "61"} },
                            { "Book", new AttributeValue { S = "Caliban's War" } },
                            { "Chapter", new AttributeValue { S = "Chapter Thirty-Five: Avasarala"} },
                            { "Page", new AttributeValue { N = "386" } },
                            { "QuoteType", new AttributeValue { S = "Response"} },
                            { "Quote", new AttributeValue { S = "So we’re all fucking morons." } },
                        }
                    }
                },
                new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = new Dictionary<String, AttributeValue>
                        {
                            { "ID", new AttributeValue { N = "62"} },
                            { "Book", new AttributeValue { S = "Caliban's War" } },
                            { "Chapter", new AttributeValue { S = "Chapter Thirty-Five: Avasarala"} },
                            { "Page", new AttributeValue { N = "387" } },
                            { "QuoteType", new AttributeValue { S = "Statement"} },
                            { "Quote", new AttributeValue { S = "I fucking say so." } },
                        }
                    }
                },
                new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = new Dictionary<String, AttributeValue>
                        {
                            { "ID", new AttributeValue { N = "63"} },
                            { "Book", new AttributeValue { S = "Caliban's War" } },
                            { "Chapter", new AttributeValue { S = "Chapter Thirty-Five: Avasarala"} },
                            { "Page", new AttributeValue { N = "389" } },
                            { "QuoteType", new AttributeValue { S = "Response"} },
                            { "Quote", new AttributeValue { S = "Welcome to the monkey house." } },
                        }
                    }
                },
                new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = new Dictionary<String, AttributeValue>
                        {
                            { "ID", new AttributeValue { N = "64"} },
                            { "Book", new AttributeValue { S = "Caliban's War" } },
                            { "Chapter", new AttributeValue { S = "Chapter Thirty-Seven: Avasarala"} },
                            { "Page", new AttributeValue { N = "404" } },
                            { "QuoteType", new AttributeValue { S = "Statement"} },
                            { "Quote", new AttributeValue { S = "I’ve got to get to work. Goddamn, but I wish I was back at the office." } },
                        }
                    }
                },
                new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = new Dictionary<String, AttributeValue>
                        {
                            { "ID", new AttributeValue { N = "65"} },
                            { "Book", new AttributeValue { S = "Caliban's War" } },
                            { "Chapter", new AttributeValue { S = "Chapter Thirty-Seven: Avasarala"} },
                            { "Page", new AttributeValue { N = "405" } },
                            { "QuoteType", new AttributeValue { S = "Response"} },
                            { "Quote", new AttributeValue { S = "Get back to me before I fire your incompetent ass out of spite." } },
                        }
                    }
                },
                new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = new Dictionary<String, AttributeValue>
                        {
                            { "ID", new AttributeValue { N = "66"} },
                            { "Book", new AttributeValue { S = "Caliban's War" } },
                            { "Chapter", new AttributeValue { S = "Chapter Thirty-Seven: Avasarala"} },
                            { "Page", new AttributeValue { N = "411" } },
                            { "QuoteType", new AttributeValue { S = "Response"} },
                            { "Quote", new AttributeValue { S = "You’re making a mistake, shithead." } },
                        }
                    }
                },
                new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = new Dictionary<String, AttributeValue>
                        {
                            { "ID", new AttributeValue { N = "67"} },
                            { "Book", new AttributeValue { S = "Caliban's War" } },
                            { "Chapter", new AttributeValue { S = "Chapter Thirty-Eight: Bobbie"} },
                            { "Page", new AttributeValue { N = "424" } },
                            { "QuoteType", new AttributeValue { S = "Response"} },
                            { "Quote", new AttributeValue { S = "We don’t have time to cruise around like we’re trying to pick up fucking rent boys!" } },
                        }
                    }
                },
                new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = new Dictionary<String, AttributeValue>
                        {
                            { "ID", new AttributeValue { N = "68"} },
                            { "Book", new AttributeValue { S = "Caliban's War" } },
                            { "Chapter", new AttributeValue { S = "Chapter Forty: Prax"} },
                            { "Page", new AttributeValue { N = "446" } },
                            { "QuoteType", new AttributeValue { S = "Response"} },
                            { "Quote", new AttributeValue { S = "You need a fucking shave." } },
                        }
                    }
                },
                new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = new Dictionary<String, AttributeValue>
                        {
                            { "ID", new AttributeValue { N = "69"} },
                            { "Book", new AttributeValue { S = "Caliban's War" } },
                            { "Chapter", new AttributeValue { S = "Chapter Forty-One: Avasarala"} },
                            { "Page", new AttributeValue { N = "455" } },
                            { "QuoteType", new AttributeValue { S = "Response"} },
                            { "Quote", new AttributeValue { S = "You will be personally responsible for the single deadliest screwup in the history of humankind, and I’m on a ship with Jim fucking Holden, so the bar’s not low." } },
                        }
                    }
                },
                new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = new Dictionary<String, AttributeValue>
                        {
                            { "ID", new AttributeValue { N = "70"} },
                            { "Book", new AttributeValue { S = "Caliban's War" } },
                            { "Chapter", new AttributeValue { S = "Chapter Forty-Two: Holden"} },
                            { "Page", new AttributeValue { N = "461" } },
                            { "QuoteType", new AttributeValue { S = "Response"} },
                            { "Quote", new AttributeValue { S = "Sounds like piss. I’ll take it." } },
                        }
                    }
                },

                new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = new Dictionary<String, AttributeValue>
                        {
                            { "ID", new AttributeValue { N = "71"} },
                            { "Book", new AttributeValue { S = "Caliban's War" } },
                            { "Chapter", new AttributeValue { S = "Chapter Forty-Five: Avasarala"} },
                            { "Page", new AttributeValue { N = "489" } },
                            { "QuoteType", new AttributeValue { S = "Statement"} },
                            { "Quote", new AttributeValue { S = "The die is already cast." } },
                        }
                    }
                },
                new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = new Dictionary<String, AttributeValue>
                        {
                            { "ID", new AttributeValue { N = "72"} },
                            { "Book", new AttributeValue { S = "Caliban's War" } },
                            { "Chapter", new AttributeValue { S = "Chapter Forty-Five: Avasarala"} },
                            { "Page", new AttributeValue { N = "490" } },
                            { "QuoteType", new AttributeValue { S = "Statement"} },
                            { "Quote", new AttributeValue { S = "I could go squeeze a few testicles until they saw it my way." } },
                        }
                    }
                },
                new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = new Dictionary<String, AttributeValue>
                        {
                            { "ID", new AttributeValue { N = "73"} },
                            { "Book", new AttributeValue { S = "Caliban's War" } },
                            { "Chapter", new AttributeValue { S = "Chapter Forty-Five: Avasarala"} },
                            { "Page", new AttributeValue { N = "491" } },
                            { "QuoteType", new AttributeValue { S = "Statement"} },
                            { "Quote", new AttributeValue { S = "Reputation never has very much to do with reality." } },
                        }
                    }
                },
                new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = new Dictionary<String, AttributeValue>
                        {
                            { "ID", new AttributeValue { N = "74"} },
                            { "Book", new AttributeValue { S = "Caliban's War" } },
                            { "Chapter", new AttributeValue { S = "Chapter Forty-Six: Bobbie"} },
                            { "Page", new AttributeValue { N = "493" } },
                            { "QuoteType", new AttributeValue { S = "Statement"} },
                            { "Quote", new AttributeValue { S = "I don’t like my chances, but you never know. I can be damned persuasive." } },
                        }
                    }
                },
                new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = new Dictionary<String, AttributeValue>
                        {
                            { "ID", new AttributeValue { N = "75"} },
                            { "Book", new AttributeValue { S = "Caliban's War" } },
                            { "Chapter", new AttributeValue { S = "Chapter Forty-Six: Bobbie"} },
                            { "Page", new AttributeValue { N = "495" } },
                            { "QuoteType", new AttributeValue { S = "Statement"} },
                            { "Quote", new AttributeValue { S = "We break out the fucking champagne." } },
                        }
                    }
                }

            //new WriteRequest
            //{
            //    PutRequest = new PutRequest
            //    {
            //        Item = new Dictionary<String, AttributeValue>
            //        {
            //            { "ID", new AttributeValue { N = "7"} },
            //            { "Book", new AttributeValue { S = "Caliban's War" } },
            //            { "Chapter", new AttributeValue { S = "Chapter Twelve: Avasarala"} },
            //            { "Page", new AttributeValue { N = "" } },
            //            { "QuoteType", new AttributeValue { S = "Statement/Question/Response"} },
            //            { "Quote", new AttributeValue { S = "" } },
            //        }
            //    }
            //},
            };

            return requests;
        }
    }
}

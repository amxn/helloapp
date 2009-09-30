﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Extensions;
using Hello.Utils;
using Hello.Bot;
using Moq;
using Hello.Bot.TweetTypes;
using Hello.Repo;

namespace Hello.Tests
{
    public class TweetParserTests
    {
        public TweetParserTests()
        {
            var mockSettings = new Mock<ISettingsImpl>();
            mockSettings.Setup(s => s.GetString("TwitterBotUsername")).Returns("apphandle");
            mockSettings.Setup(s => s.GetString("TwitterHashTag")).Returns("thetag");
            Settings.SettingsImplementation = mockSettings.Object;
        }

        [Fact]
        public void BlankTest()
        {
            var t = TweetParser.Parse(new QueuedTweet { Message = "" });

            Assert.Null(t);
        }

        [Theory]
        [InlineData("hello !dev #csharp #dotnet #jquery")]
        [InlineData("hello !dev #c# #dotnet #jquery")]
        [InlineData("@apphandle hello !dev #csharp #dotnet #jquery")]
        [InlineData("#thetag hello !dev #csharp #dotnet #jquery")]
        public void HelloTest(string tweet)
        {
            var t = TweetParser.Parse(new QueuedTweet { Message = tweet }) as HelloTweet;

            Assert.NotNull(t);
            Assert.Equal(t.UserType, "dev");
            Assert.Equal(3, t.Tags.Count);
            Assert.True(t.Tags.Contains("csharp"));
            Assert.True(t.Tags.Contains("dotnet"));
            Assert.True(t.Tags.Contains("jquery"));
        }

        [Fact]
        public void MetTest()
        {
            var t = TweetParser.Parse(new QueuedTweet { Message = "met ryan matt kier" }) as MetTweet;

            Assert.NotNull(t);
            Assert.Equal(3, t.Friends.Count);
            Assert.True(t.Friends.Contains("ryan"));
            Assert.True(t.Friends.Contains("matt"));
            Assert.True(t.Friends.Contains("kier"));
        }

        [Fact]
        public void MetWithAtsTest()
        {
            var t = TweetParser.Parse(new QueuedTweet { Message = "met @ryan @matt @kier" }) as MetTweet;

            Assert.NotNull(t);
            Assert.Equal(3, t.Friends.Count);
            Assert.True(t.Friends.Contains("ryan"));
            Assert.True(t.Friends.Contains("matt"));
            Assert.True(t.Friends.Contains("kier"));
        }

        [Fact]
        public void ClaimTest()
        {
            var t = TweetParser.Parse(new QueuedTweet { Message = "claim ASDF1234" }) as ClaimTweet;

            Assert.NotNull(t);
            Assert.Equal("ASDF1234".ToLower(), t.Token);
        }

        [Fact]
        public void SatTest()
        {
            var t = TweetParser.Parse(new QueuedTweet { Message = "sat ASD12" }) as SatTweet;

            Assert.NotNull(t);
            Assert.Equal("ASD12".ToLower(), t.SeatCode);
        }

        [Fact]
        public void MessageTest()
        {
            var t = TweetParser.Parse(new QueuedTweet { Message = "message This is my shout out to everyone at Carsonified!" }) as MessageTweet;

            Assert.NotNull(t);
            Assert.Equal(t.Message, "This is my shout out to everyone at Carsonified!");
        }

        [Theory]
        [InlineData("hello !dev #csharp #dotnet #jquery (via @helloapptest1)")]
        [InlineData("(via @helloapptest2) hello !des #html #css #js")]
        public void IgnoreViasTest(string tweet)
        {
            var t = TweetParser.Parse(new QueuedTweet { Message = tweet }) as HelloTweet;
            Assert.Null(t);
        }

        [Theory]
        [InlineData("@apphandle hi5 matt")]
        [InlineData("@apphandle hi5 @matt")]
        public void HiFiveTest(string tweet)
        {
            var t = TweetParser.Parse(new QueuedTweet { Message = tweet }) as HiFiveTweet;

            Assert.NotNull(t);
            Assert.Equal(t.Friend, "matt");
        }
    }
}

// Copyright (c) 2020 - 2021 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using AccelByte.Api;
using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Server;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.TestTools;
using Utf8Json;

namespace AccelByte.Models 
{
    [DataContract]
    public class StatCreateRequest
    {
        [DataMember] public float defaultValue { get; set; }
        [DataMember] public string description { get; set; }
        [DataMember] public bool incrementOnly { get; set; }
        [DataMember] public float maximum { get; set; }
        [DataMember] public float minimum { get; set; }
        [DataMember] public string name { get; set; }
        [DataMember] public bool setAsGlobal { get; set; }
        [DataMember] public StatisticSetBy setBy { get; set; }
        [DataMember] public string statCode { get; set; }
        [DataMember] public string[] tags { get; set; }
    }

    [DataContract]
    public class StatCode
    {
        [DataMember] public string statCode { get; set; }
    }
    
    [DataContract]
    public class LeaderboardDailyConfig
    {
        [DataMember] public string resetTime { get; set; } = "00:00";
    }

    [DataContract]
    public class LeaderboardMonthlyConfig
    {
        [DataMember] public int resetDate { get; set; } = 1;
        [DataMember] public string resetTime { get; set; } = "00:00";
    }

    [DataContract]
    public class LeaderboardWeeklyConfig
    {
        [DataMember] public int resetDay { get; set; } = 0;
        [DataMember] public string resetTime { get; set; } = "00:00";
    }

    [DataContract]
    public class LeaderboardConfig
    {
        [DataMember] public LeaderboardDailyConfig daily { get; set; } = new LeaderboardDailyConfig();
        [DataMember] public LeaderboardMonthlyConfig monthly { get; set; } = new LeaderboardMonthlyConfig();
        [DataMember] public LeaderboardWeeklyConfig weekly { get; set; } = new LeaderboardWeeklyConfig();
        [DataMember] public bool descending { get; set; } = true;
        [DataMember] public string iconURL { get; set; } = "";
        [DataMember] public string leaderboardCode { get; set; }
        [DataMember] public string name { get; set; }
        [DataMember] public int seasonPeriod { get; set; } = 32;
        [DataMember] public string startTime { get; set; }
        [DataMember] public string statCode { get; set; }
    }

    [DataContract]
    public class SetUserVisibilityRequest
    {
        [DataMember] public bool visibility { get; set; }
    }
}

namespace Tests.IntegrationTests
{

    internal class MatchAdmin
    {
        private const double StartTimeDelay = 10.0;
        private const double SafeguardDelay = 10.0;
        private const int SeasonPeriodDays = 31 + 5;

        private readonly string baseUrl;
        private readonly string clientId;
        private readonly string clientSecret;
        private readonly IHttpClient httpClient;
        private readonly ServerStatistic statServer = AccelByteServerPlugin.GetStatistic();
        private readonly List<StatCode> statCodes = new List<StatCode>();
        private readonly Dictionary<string, string> leaderboardCodeMap = new Dictionary<string, string>();

        private string accessToken;
        private DateTime latestLeaderboardActiveTime;

        public IEnumerable<string> StatCodes => this.statCodes.Select(c => c.statCode);
        public MatchAdmin(string baseUrl, string clientId, string clientSecret, IHttpClient httpClient)
        {
            this.baseUrl = baseUrl;
            this.clientId = clientId;
            this.clientSecret = clientSecret;
            this.httpClient = httpClient;
        }
        
        public string GetLeaderboardCodeByStatCode(string statCode) => this.leaderboardCodeMap[statCode];

        public IEnumerator Login(ResultCallback callback)
        {
            Result serverLoginResult = null;
            AccelByteServerPlugin.GetDedicatedServer().LoginWithClientCredentials(result => serverLoginResult = result);
            yield return TestHelper.WaitUntil(() => serverLoginResult != null);
            
            IHttpRequest request = HttpRequestBuilder.CreatePost(this.baseUrl + "/iam/v3/oauth/token")
                .WithBasicAuth(this.clientId, this.clientSecret)
                .WithContentType(MediaType.ApplicationForm)
                .Accepts(MediaType.ApplicationJson)
                .WithFormParam("grant_type", "client_credentials")
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            Result<TokenData> leaderboardLoginResult = response.TryParseJson<TokenData>();
            this.accessToken = leaderboardLoginResult.Value.access_token;
            
            callback.TryOk();
        }

        public IEnumerator CreateStatConfig(string statCode, ResultCallback<StatConfig> callback)
        {
            StatCreateRequest requestBody = new StatCreateRequest
            {
                defaultValue = 0,
                description = "StatCode for Leaderboard Unity SDK Test purpose",
                incrementOnly = true,
                maximum = 999999,
                minimum = 0,
                name = statCode,
                setAsGlobal = true,
                setBy = StatisticSetBy.SERVER,
                statCode = statCode,
                tags = new[] {statCode}
            };

            UnityWebRequest request = HttpRequestBuilder
                .CreatePost(this.baseUrl + "/social/v1/admin/namespaces/{namespace}/stats")
                .WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(JsonSerializer.Serialize(requestBody))
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult()
                .GetUnityWebRequest();

            yield return request.SendWebRequest();

            Result<StatConfig> result = request.GetHttpResponse().TryParseJson<StatConfig>();

            if (!result.IsError || result.Error.Code != ErrorCode.Conflict)
            {
                this.statCodes.Add(new StatCode {statCode = statCode});
            }
            
            callback.Try(result);
        }

        public void DeleteStatConfigs()
        {
            this.statCodes.RemoveAll(c => !this.leaderboardCodeMap.ContainsKey(c.statCode));
        }
        
        public IEnumerator CreateStatItems(MatchPlayer matchPlayer, ResultCallback<StatItemOperationResult[]> callback)
        {
            UnityWebRequest request = HttpRequestBuilder
                .CreatePost(this.baseUrl + "/social/v1/admin/namespaces/{namespace}/users/{userId}/statitems/bulk")
                .WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
                .WithPathParam("userId", matchPlayer.UserId)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(JsonSerializer.ToJsonString(this.statCodes))
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult()
                .GetUnityWebRequest();

            yield return request.SendWebRequest();

            Result<StatItemOperationResult[]> result =
                request.GetHttpResponse().TryParseJson<StatItemOperationResult[]>();
            callback.Try(result);
        }

        public IEnumerator ResetStatItems(MatchPlayer matchPlayer, ResultCallback<StatItemOperationResult[]> callback)
        {
            UserStatItemReset[] resets = this.statCodes.Select(
                    statCode => new UserStatItemReset {statCode = statCode.statCode, userId = matchPlayer.UserId})
                .ToArray();

            Result<StatItemOperationResult[]> result = null; 
            this.statServer.ResetManyUsersStatItems(resets, r => result = r);
            yield return TestHelper.WaitUntil(() => result != null);
            
            callback.Try(result);
        }
        
        private IEnumerator DeleteStatItem(MatchPlayer matchPlayer, string statCode, ResultCallback callback)
        {
            UnityWebRequest request = HttpRequestBuilder
                .CreateDelete(
                    this.baseUrl + "/social/v1/admin/namespaces/{namespace}/users/{userId}/stats/{statCode}/statitems")
                .WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
                .WithPathParam("userId", matchPlayer.UserId)
                .WithPathParam("statCode", statCode)
                .WithContentType(MediaType.ApplicationJson)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult()
                .GetUnityWebRequest();

            yield return request.SendWebRequest();

            var result = request.GetHttpResponse().TryParse();
            callback.Try(result);
        }
        
        public IEnumerator DeleteStatItems(MatchPlayer matchPlayer, ResultCallback callback)
        {
            List<ServiceError> errors = new List<ServiceError>();
            foreach (string code in this.StatCodes)
            {
                Result result = null;
                yield return this.DeleteStatItem(matchPlayer, code, r => result = r);

                if (result.IsError)
                {
                    errors.Add(new ServiceError
                    {
                        errorCode = (long) result.Error.Code, errorMessage =  result.Error.Message
                    });
                }
            }

            if (errors.Count == 0)
            {
                callback.TryOk();
            }
            else
            {
                callback.TryError(ErrorCode.UnknownError, JsonSerializer.ToJsonString(errors));
            }
        }

        public IEnumerator CreateLeaderboard(string statCode, ResultCallback<LeaderboardConfig> callback)
        {
            DateTime startTime = DateTime.UtcNow + TimeSpan.FromSeconds(MatchAdmin.StartTimeDelay);
            string leaderboardCode = statCode + "-" + Guid.NewGuid().ToString("N"); 
            LeaderboardConfig requestBody = new LeaderboardConfig
            {
                leaderboardCode = leaderboardCode,
                name = statCode,
                statCode = statCode,
                seasonPeriod = MatchAdmin.SeasonPeriodDays,
                startTime = startTime.ToString("yyyy-MM-ddTHH:mm:ssZ")
            };

            UnityWebRequest request = HttpRequestBuilder
                .CreatePost(this.baseUrl + "/leaderboard/v1/admin/namespaces/{namespace}/leaderboards")
                .WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(JsonSerializer.Serialize(requestBody))
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult()
                .GetUnityWebRequest();

            yield return request.SendWebRequest();

            Result<LeaderboardConfig> result = request.GetHttpResponse().TryParseJson<LeaderboardConfig>();

            if (!result.IsError || result.Error.Code != ErrorCode.Conflict)
            {
                this.latestLeaderboardActiveTime = startTime + TimeSpan.FromSeconds(MatchAdmin.SafeguardDelay);
                this.leaderboardCodeMap[statCode] = leaderboardCode;
            }
            
            callback.Try(result);
        }

        public IEnumerator CreateLeaderboards(ResultCallback callback)
        {
            List<ServiceError> errors = new List<ServiceError>();
            foreach (string code in StatCodes)
            {
                Result<LeaderboardConfig> result = null;
                yield return this.CreateLeaderboard(code, r => result = r);

                if (result.IsError)
                {
                    errors.Add(new ServiceError
                    {
                        errorCode = (long) result.Error.Code, errorMessage =  result.Error.Message
                    });
                }
            }
            
            if (errors.Count == 0)
            {
                callback.TryOk();
            }
            else
            {
                callback.TryError(ErrorCode.UnknownError, JsonSerializer.ToJsonString(errors));
            }
        }

        public IEnumerator WaitUntilLeaderboardsActive()
        {
            if (this.latestLeaderboardActiveTime > DateTime.UtcNow)
            {
                yield return new WaitForSeconds(this.latestLeaderboardActiveTime.Subtract(DateTime.UtcNow).Seconds);
            }
        }

        public IEnumerator GetLeaderboard(string statCode, ResultCallback<LeaderboardConfig> callback)
        {
            if (!this.leaderboardCodeMap.ContainsKey(statCode))
            {
                callback.TryError(new Error(ErrorCode.NotFound, "No leaderboardCode for statCode " + statCode));
                yield break;
            }
            
            UnityWebRequest request = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/leaderboard/v1/admin/namespaces/{namespace}/leaderboards/{leaderboardCode}")
                .WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
                .WithPathParam("leaderboardCode", this.leaderboardCodeMap[statCode])
                .WithContentType(MediaType.ApplicationJson)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult()
                .GetUnityWebRequest();

            yield return request.SendWebRequest();

            Result<LeaderboardConfig> result = request.GetHttpResponse().TryParseJson<LeaderboardConfig>();
            callback.Try(result);
        }

        public IEnumerator DeleteLeaderboard(string statCode, ResultCallback callback)
        {
            if (!this.leaderboardCodeMap.ContainsKey(statCode))
            {
                callback.TryError(new Error(ErrorCode.NotFound, "No leaderboardCode for statCode " + statCode));
                yield break;
            }
            
            UnityWebRequest request = HttpRequestBuilder
                .CreateDelete(this.baseUrl + "/leaderboard/v1/admin/namespaces/{namespace}/leaderboards/{leaderboardCode}")
                .WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
                .WithPathParam("leaderboardCode", this.leaderboardCodeMap[statCode])
                .WithContentType(MediaType.ApplicationJson)
                .WithBearerAuth(this.accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult()
                .GetUnityWebRequest();

            yield return request.SendWebRequest();

            var result = request.GetHttpResponse().TryParse();
            if (!result.IsError)
            {
                this.leaderboardCodeMap.Remove(statCode);
            }
            
            callback.Try(result);
        }
        
        public IEnumerator DeleteLeaderboards(ResultCallback callback)
        {
            List<ServiceError> errors = new List<ServiceError>();
            foreach (string code in this.StatCodes)
            {
                Result result = null;
                yield return this.DeleteLeaderboard(code, r => result = r);

                if (result.IsError)
                {
                    errors.Add(new ServiceError
                    {
                        errorCode = (long) result.Error.Code, errorMessage =  result.Error.Message
                    });
                }
            }
            
            if (errors.Count == 0)
            {
                callback.TryOk();
            }
            else
            {
                callback.TryError(ErrorCode.UnknownError, JsonSerializer.ToJsonString(errors));
            }
        }

        public IEnumerator IncrementManyUsersStatItems(UserStatItemIncrement[] increments,
            ResultCallback<StatItemOperationResult[]> callback)
        {
            Result<StatItemOperationResult[]> result = null;
            this.statServer.IncrementManyUsersStatItems(increments, r => result = r);
            yield return new WaitUntil(() => result != null);
            
            callback.Try(result);
        }
        
        public IEnumerator UpdateManyUsersStatItems(UserStatItemUpdate[] updates,
            ResultCallback<StatItemOperationResult[]> callback)
        {
            Result<StatItemOperationResult[]> result = null;
            this.statServer.UpdateManyUsersStatItems(updates, r => result = r);
            yield return new WaitUntil(() => result != null);
            
            callback.Try(result);
        }

        public IEnumerator SetUserVisibilityStatus(string userId, string statCode, SetUserVisibilityRequest visibilityRequest, ResultCallback callback)
        {
            if (!this.leaderboardCodeMap.ContainsKey(statCode))
            {
                callback.TryError(new Error(ErrorCode.NotFound, "No leaderboardCode for statCode " + statCode));
                yield break;
            }

            UnityWebRequest request = HttpRequestBuilder
                .CreatePut(this.baseUrl + "/leaderboard/v2/admin/namespaces/{namespace}/leaderboards/{leaderboardCode}/users/{userId}/visibility")
                .WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
                .WithPathParam("leaderboardCode", this.leaderboardCodeMap[statCode])
                .WithPathParam("userId", userId)
                .WithBody(visibilityRequest.ToUtf8Json())
                .WithContentType(MediaType.ApplicationJson)
                .WithBearerAuth(this.accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult()
                .GetUnityWebRequest();

            yield return request.SendWebRequest();

            Result result = request.GetHttpResponse().TryParse();
            callback.Try(result);
        }
    }

    internal class MatchPlayer
    {
        private const string Password = "P4ssW0Rd123";
        private readonly string username;
        private string EmailAddress => this.username + "@accelbyte.example.com";
        private UserData userData;

        public string UserId => this.userData?.userId;
        
        public MatchPlayer(string username)
        {
            this.username = username;
        }

        public IEnumerator Initialize(ResultCallback callback)
        {
            var user = AccelBytePlugin.GetUser();
            
            Result logoutCurrentUserResult = null;
            user.Logout(r => logoutCurrentUserResult = r);                    
            yield return TestHelper.WaitForValue(() => logoutCurrentUserResult);

            DateTime dateOfBirth = DateTime.Now.AddYears(-22);
            Result<RegisterUserResponse> registerResult = null;
            user.Register(
                this.EmailAddress,
                MatchPlayer.Password,
                this.username,
                "US",
                dateOfBirth,
                result => registerResult = result);
            yield return TestHelper.WaitForValue(() => registerResult);

            Result loginResult = null;
            user.LoginWithUsername(this.EmailAddress, MatchPlayer.Password, result => loginResult = result);
            yield return TestHelper.WaitForValue(() => loginResult);

            this.userData = null;
            user.GetData(result => this.userData = result.Value);
            yield return TestHelper.WaitForValue(() => this.userData);

            Result logoutResult = null;
            user.Logout(result => logoutResult = result);
            yield return TestHelper.WaitForValue(() => logoutResult);
            
            callback.TryOk();
        }
    }

    internal class MatchObserver
    {
        private readonly User user = AccelBytePlugin.GetUser();
        private readonly Leaderboard leaderboard = AccelBytePlugin.GetLeaderboard();

        public IEnumerator Login(ResultCallback callback)
        {
            Result result = null;
            this.user.LoginWithDeviceId(r => result = r);
            yield return TestHelper.WaitUntil(() => result != null);
            
            callback.Try(result);
        }
        
        public IEnumerator Logout(ResultCallback callback)
        {
            Result result = null;
            this.user.Logout(r => result = r);
            yield return TestHelper.WaitUntil(() => result != null);
            
            callback.Try(result);
        }

        public IEnumerator GetRankings(
            string leaderboardCode,
            LeaderboardTimeFrame timeFrame,
            int offset,
            int limit,
            ResultCallback<LeaderboardRankingResult> callback)
        {
            Result<LeaderboardRankingResult> result = null;
            this.leaderboard.GetRankings(leaderboardCode, timeFrame, offset, limit, r => result = r);
            yield return TestHelper.WaitUntil(() => result != null);
            
            callback.Try(result);
        }
        
        public IEnumerator GetUserRanking(string userId, string leaderboardCode, ResultCallback<UserRankingData> callback)
        {
            Result<UserRankingData> result = null;
            this.leaderboard.GetUserRanking(userId, leaderboardCode, r => result = r);
            yield return TestHelper.WaitUntil(() => result != null);
            
            callback.Try(result);
        }

        public IEnumerator GetUserRanking(
            string userId,
            string additionalKey,
            string leaderboardCode,
            ResultCallback<UserRankingData> callback)
        {
            Result<UserRankingData> result = null;
            this.leaderboard.GetUserRanking(userId, additionalKey, leaderboardCode, r => result = r);
            yield return TestHelper.WaitUntil(() => result != null);
            
            callback.Try(result);
        }

        public IEnumerator GetLeaderboardList(ResultCallback<List<LeaderboardData>> callback)
        {
            const int limit = 25;
            int offset = 0;
            List<LeaderboardData> leaderboards = new List<LeaderboardData>();
            
            Result<LeaderboardPagedList> result;
            do
            {
                result = null;
                this.leaderboard.GetLeaderboardList(r => result = r, offset, limit);
                yield return TestHelper.WaitUntil(() => result != null);

                if (result.Value?.data != null)
                {
                    leaderboards.AddRange(result.Value.data);
                }

                offset += limit;
            }
            while (!string.IsNullOrEmpty(result.Value?.paging?.Next));
            
            callback.Try(Result<List<LeaderboardData>>.CreateOk(leaderboards));
        }
    }
    
    [TestFixture, Timeout(300000)]
    internal class LeaderboardTest
    {
        private readonly MatchAdmin admin = new MatchAdmin(
            AccelBytePlugin.Config.BaseUrl,
            Environment.GetEnvironmentVariable("ADMIN_CLIENT_ID"),
            Environment.GetEnvironmentVariable("ADMIN_CLIENT_SECRET"),
            new AccelByteHttpClient());
        private readonly MatchObserver observer = new MatchObserver();

        private readonly MatchPlayer[] players = 
        {
            new MatchPlayer("unitySdkContestant0"),
            new MatchPlayer("unitySdkContestant1"),
            new MatchPlayer("unitySdkContestant2"),
            new MatchPlayer("unitySdkContestant3")
        };

        private readonly string[] testStatCodes = 
        {
            "unityteststat0",
            "unityteststat1"
        };
        
        private bool shouldSetupFixture = true;
        private bool shouldTearDownFixture;
        
        [UnitySetUp]
        public IEnumerator Setup()
        {
            if (this.shouldSetupFixture)
            {
                Result adminLoginResult = null;
                yield return this.admin.Login(result => adminLoginResult = result);

                foreach (string statCode in this.testStatCodes)
                {
                    Result<StatConfig> createStatConfig = null;
                    yield return this.admin.CreateStatConfig(statCode, result => createStatConfig = result);
                }

                Result createLeaderboardsResult = null;
                yield return this.admin.CreateLeaderboards(result => createLeaderboardsResult = result);

                foreach (MatchPlayer player in this.players)
                {
                    Result initializeResult = null;
                    yield return player.Initialize(result => initializeResult = result);
                }

                this.shouldSetupFixture = false;
            }

            foreach (MatchPlayer player in this.players)
            {
                Result<StatItemOperationResult[]> createStatItemsResult = null;
                yield return this.admin.CreateStatItems(player, result => createStatItemsResult = result);
            }
            
            Result watcherLoginResult = null;
            yield return this.observer.Login(result => watcherLoginResult = result);
        }

        [UnityTest, TestLog, Order(int.MaxValue)]
        public IEnumerator EndTests()
        {
            this.shouldTearDownFixture = true;
            yield break;
        }

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            foreach (MatchPlayer player in this.players)
            {
                Result createStatItemsResult = null;
                yield return this.admin.DeleteStatItems(player, result => createStatItemsResult = result);
            }
            
            this.admin.DeleteStatConfigs();

            Result watcherLogoutResult = null;
            yield return this.observer.Logout(result => watcherLogoutResult = result);

            if (this.shouldTearDownFixture)
            {
                Result deleteLeaderboardResult = null;
                yield return this.admin.DeleteLeaderboards(result => deleteLeaderboardResult = result);
                this.shouldTearDownFixture = false;
            }
        }

        [UnityTest, TestLog, Order(1)]
        public IEnumerator LeaderboardCreated_CreateLeaderboard_LeaderboardCreated()
        {
            const string statCode = "unityteststat";

            Result<StatConfig> createStatConfigResult = null;
            yield return this.admin.CreateStatConfig(statCode, result => createStatConfigResult = result);

            Result<LeaderboardConfig> createLeaderboardResult = null;
            yield return this.admin.CreateLeaderboard(statCode, result => createLeaderboardResult = result);

            Result<LeaderboardConfig> getLeaderboardResult = null;
            yield return this.admin.GetLeaderboard(statCode, result => getLeaderboardResult = result);

            List<LeaderboardData> leaderboards = null;
            yield return this.observer.GetLeaderboardList(result => leaderboards = result.Value);

            Result deleteLeaderboardResult = null;
            yield return this.admin.DeleteLeaderboard(statCode, result => deleteLeaderboardResult = result);

            this.admin.DeleteStatConfigs();

            Assert.False(createLeaderboardResult.IsError);
            Assert.False(getLeaderboardResult.IsError);
            Assert.True(leaderboards.Any(l => l.leaderboardCode == getLeaderboardResult.Value.leaderboardCode));
        }

        [UnityTest, TestLog, Order(1)]
        public IEnumerator PlayerStatItemsCreated_IncrementStatItems_RankingsCorrespondToStatItems()
        {
            yield return this.admin.WaitUntilLeaderboardsActive();

            string statCode = this.testStatCodes[0];
            List<UserStatItemIncrement> increments = new List<UserStatItemIncrement>
            {
                new UserStatItemIncrement
                {
                    inc = 11.0f, userId = this.players[0].UserId, statCode = statCode
                },
                new UserStatItemIncrement
                {
                    inc = 7.0f, userId = this.players[1].UserId, statCode = statCode
                },
                new UserStatItemIncrement
                {
                    inc = 23.0f, userId = this.players[2].UserId, statCode = statCode
                },
                new UserStatItemIncrement
                {
                    inc = 13.0f, userId = this.players[3].UserId, statCode = statCode
                }
            };

            yield return this.admin.IncrementManyUsersStatItems(increments.ToArray(), null);

            yield return new WaitForSeconds(20);

            List<UserPoint> userPoints = new List<UserPoint>();
            string leaderboardCode = this.admin.GetLeaderboardCodeByStatCode(statCode);
            yield return this.observer.GetRankings(
                leaderboardCode,
                LeaderboardTimeFrame.ALL_TIME,
                0,
                int.MaxValue,
                result => userPoints.AddRange(result.Value?.data ?? Array.Empty<UserPoint>())); 
            
            List<UserRankingData> userRanks = new List<UserRankingData>();

            foreach (MatchPlayer player in this.players)
            {
                yield return this.observer.GetUserRanking(
                    player.UserId,
                    leaderboardCode,
                    result => userRanks.Add(result.Value));
            }
            
            IEnumerable<UserStatItemIncrement> incrementsQuery = 
                from increment in increments
                from point in userPoints
                where increment.statCode == statCode &&
                    increment.userId == point.userId &&
                    Math.Abs(increment.inc - point.point) < 0.01
                select increment;
            Assert.That(incrementsQuery.Count(), Is.EqualTo(userPoints.Count));
            
            Assert.AreEqual(userRanks.Max(x => x.current.point), increments.Max(x => x.inc));
            Assert.AreEqual(userRanks.Min(x => x.current.point), increments.Min(x => x.inc));
            Assert.AreEqual(userRanks.Max(x => x.daily.point), increments.Max(x => x.inc));
            Assert.AreEqual(userRanks.Min(x => x.daily.point), increments.Min(x => x.inc));
            Assert.AreEqual(userRanks.Max(x => x.weekly.point), increments.Max(x => x.inc));
            Assert.AreEqual(userRanks.Min(x => x.weekly.point), increments.Min(x => x.inc));
            Assert.AreEqual(userRanks.Max(x => x.monthly.point), increments.Max(x => x.inc));
            Assert.AreEqual(userRanks.Min(x => x.monthly.point), increments.Min(x => x.inc));
            Assert.AreEqual(userRanks.Max(x => x.allTime.point), increments.Max(x => x.inc));
            Assert.AreEqual(userRanks.Min(x => x.allTime.point), increments.Min(x => x.inc));

            increments.Sort((x, y) => x.inc - y.inc < 0 ? -1 : 1);
            
            userRanks.Sort((x, y) => x.current.rank - y.current.rank < 0 ? -1 : 1);
            Assert.AreEqual(userRanks.First().userId, increments.Last().userId);
            Assert.AreEqual(userRanks.Last().userId, increments.First().userId);
            
            userRanks.Sort((x, y) => x.daily.rank - y.current.rank < 0 ? -1 : 1);
            Assert.AreEqual(userRanks.First().userId, increments.Last().userId);
            Assert.AreEqual(userRanks.Last().userId, increments.First().userId);
            
            userRanks.Sort((x, y) => x.weekly.rank - y.current.rank < 0 ? -1 : 1);
            Assert.AreEqual(userRanks.First().userId, increments.Last().userId);
            Assert.AreEqual(userRanks.Last().userId, increments.First().userId);
            
            userRanks.Sort((x, y) => x.monthly.rank - y.current.rank < 0 ? -1 : 1);
            Assert.AreEqual(userRanks.First().userId, increments.Last().userId);
            Assert.AreEqual(userRanks.Last().userId, increments.First().userId);
            
            userRanks.Sort((x, y) => x.allTime.rank - y.current.rank < 0 ? -1 : 1);
            Assert.AreEqual(userRanks.First().userId, increments.Last().userId);
            Assert.AreEqual(userRanks.Last().userId, increments.First().userId);
        }
        
        [UnityTest, TestLog, Order(1)]
        public IEnumerator PlayerStatItemsCreated_UpdateStatItemsWithAdditionalData_RankingsCorrespondToStatItems()
        {
            yield return this.admin.WaitUntilLeaderboardsActive();

            string statCode = this.testStatCodes[1];
            List<UserStatItemUpdate> updates = new List<UserStatItemUpdate>
            {
                new UserStatItemUpdate
                {
                    value = 11,
                    userId = this.players[0].UserId,
                    updateStrategy = StatisticUpdateStrategy.OVERRIDE,
                    statCode = statCode,
                    additionalKey = "kampret",
                    additionalData = new Dictionary<string, object>
                    {
                        {"characterName", "kampret"},
                        {"class", "lawa"},
                        {"skills", new [] {"mabur", "mangan serangga", "mangan woh", "mangan kembang", "sonar"} }
                    }
                },
                new UserStatItemUpdate
                {
                    value = 7,
                    userId = this.players[1].UserId,
                    updateStrategy = StatisticUpdateStrategy.OVERRIDE,
                    statCode = statCode,
                    additionalKey = "kalong",
                    additionalData = new Dictionary<string, object>
                    {
                        {"characterName", "kalong"},
                        {"class", "lawa"},
                        {"skills", new [] {"mabur", "mangan woh", "mangan kembang"} }
                    }
                },
                new UserStatItemUpdate
                {
                    value = 23,
                    userId = this.players[2].UserId,
                    updateStrategy = StatisticUpdateStrategy.OVERRIDE,
                    statCode = statCode,
                    additionalKey = "codhot",
                    additionalData = new Dictionary<string, object>
                    {
                        {"characterName", "codhot"},
                        {"class", "lawa"},
                        {"skills", new [] {"mabur", "mangan woh", "mangan kembang"} }
                    }
                },
                new UserStatItemUpdate
                {
                    value = 13,
                    userId = this.players[3].UserId,
                    updateStrategy = StatisticUpdateStrategy.OVERRIDE,
                    statCode = statCode,
                    additionalKey = "vampir",
                    additionalData = new Dictionary<string, object>
                    {
                        {"characterName", "vampir"},
                        {"class", "lawa"},
                        {"skills", new [] { "mabur", "ngombe getih"}}
                    }
                }
            };

            yield return this.admin.UpdateManyUsersStatItems(updates.ToArray(), null);

            yield return new WaitForSeconds(20);

            List<UserPoint> userPoints = new List<UserPoint>();
            string leaderboardCode = this.admin.GetLeaderboardCodeByStatCode(statCode);
            yield return this.observer.GetRankings(
                leaderboardCode,
                LeaderboardTimeFrame.ALL_TIME,
                0,
                int.MaxValue,
                result => userPoints.AddRange(result.Value?.data ?? Array.Empty<UserPoint>())); 
            
            List<UserRankingData> userRanks = new List<UserRankingData>();

            foreach (var update in updates)
            {
                yield return this.observer.GetUserRanking(
                    update.userId,
                    update.additionalKey,
                    leaderboardCode,
                    result => userRanks.Add(result.Value));
            }
            
            IEnumerable<UserStatItemUpdate> updatesQuery = 
                from update in updates
                from point in userPoints
                where update.statCode == statCode && 
                    update.userId + "_" + update.additionalKey == point.userId &&
                    Math.Abs(update.value - point.point) < 0.01
                select update;
            Assert.That(updatesQuery.Count(), Is.EqualTo(userPoints.Count));
            
            Assert.AreEqual(userRanks.Max(x => x.current.point), updates.Max(x => x.value));
            Assert.AreEqual(userRanks.Min(x => x.current.point), updates.Min(x => x.value));
            Assert.AreEqual(userRanks.Max(x => x.daily.point), updates.Max(x => x.value));
            Assert.AreEqual(userRanks.Min(x => x.daily.point), updates.Min(x => x.value));
            Assert.AreEqual(userRanks.Max(x => x.weekly.point), updates.Max(x => x.value));
            Assert.AreEqual(userRanks.Min(x => x.weekly.point), updates.Min(x => x.value));
            Assert.AreEqual(userRanks.Max(x => x.monthly.point), updates.Max(x => x.value));
            Assert.AreEqual(userRanks.Min(x => x.monthly.point), updates.Min(x => x.value));
            Assert.AreEqual(userRanks.Max(x => x.allTime.point), updates.Max(x => x.value));
            Assert.AreEqual(userRanks.Min(x => x.allTime.point), updates.Min(x => x.value));

            updates.Sort((x, y) => x.value - y.value < 0 ? -1 : 1);
            
            userRanks.Sort((x, y) => x.current.rank - y.current.rank < 0 ? -1 : 1);
            Assert.AreEqual(userRanks.First().userId, updates.Last().userId + "_" + updates.Last().additionalKey);
            Assert.AreEqual(userRanks.Last().userId, updates.First().userId + "_" + updates.First().additionalKey);
            
            userRanks.Sort((x, y) => x.daily.rank - y.current.rank < 0 ? -1 : 1);
            Assert.AreEqual(userRanks.First().userId, updates.Last().userId + "_" + updates.Last().additionalKey);
            Assert.AreEqual(userRanks.Last().userId, updates.First().userId + "_" + updates.First().additionalKey);
            
            userRanks.Sort((x, y) => x.weekly.rank - y.current.rank < 0 ? -1 : 1);
            Assert.AreEqual(userRanks.First().userId, updates.Last().userId + "_" + updates.Last().additionalKey);
            Assert.AreEqual(userRanks.Last().userId, updates.First().userId + "_" + updates.First().additionalKey);
            
            userRanks.Sort((x, y) => x.monthly.rank - y.current.rank < 0 ? -1 : 1);
            Assert.AreEqual(userRanks.First().userId, updates.Last().userId + "_" + updates.Last().additionalKey);
            Assert.AreEqual(userRanks.Last().userId, updates.First().userId + "_" + updates.First().additionalKey);
            
            userRanks.Sort((x, y) => x.allTime.rank - y.current.rank < 0 ? -1 : 1);
            Assert.AreEqual(userRanks.First().userId, updates.Last().userId + "_" + updates.Last().additionalKey);
            Assert.AreEqual(userRanks.Last().userId, updates.First().userId + "_" + updates.First().additionalKey);
        }

        [UnityTest, TestLog, Order(1)]
        public IEnumerator PlayerStatItemsCreated_UpdateVisibilityForSpecificUser_VisibilityStatusOnSpecificUserChange()
        {
            // Arrange (Setup Data)
            yield return this.admin.WaitUntilLeaderboardsActive();

            string statCode = this.testStatCodes[0];
            List<UserStatItemIncrement> increments = new List<UserStatItemIncrement>
            {
                new UserStatItemIncrement
                {
                    inc = 11.0f, userId = this.players[0].UserId, statCode = statCode
                },
                new UserStatItemIncrement
                {
                    inc = 7.0f, userId = this.players[1].UserId, statCode = statCode
                },
                new UserStatItemIncrement
                {
                    inc = 23.0f, userId = this.players[2].UserId, statCode = statCode
                },
                new UserStatItemIncrement
                {
                    inc = 13.0f, userId = this.players[3].UserId, statCode = statCode
                }
            };

            yield return this.admin.IncrementManyUsersStatItems(increments.ToArray(), null);

            yield return new WaitForSeconds(20);

            // Arrange (Set Expected Visibility)
            int expectedHiddenCount = 0;
            SetUserVisibilityRequest SetVisibilityRequest(bool isActive)
            {
                if (!isActive)
                {
                    expectedHiddenCount += 1;
                }

                return new SetUserVisibilityRequest
                {
                    visibility = isActive
                };
            }

            // Act (Set Visibility)
            var random = new System.Random();
            for (int i = 0; i < this.players.Count(); i++)
            {
                bool isActive = random.Next(2) == 1;
                yield return this.admin.SetUserVisibilityStatus(this.players[i].UserId, statCode, SetVisibilityRequest(isActive), null);
            }

            int expectedVisibleCount = this.players.Count() - expectedHiddenCount;

            // Act (Get Leaderboard Data)
            string leaderboardCode = this.admin.GetLeaderboardCodeByStatCode(statCode);

            List<UserPoint> userPoints = new List<UserPoint>();
            yield return this.observer.GetRankings(
                leaderboardCode,
                LeaderboardTimeFrame.ALL_TIME,
                0,
                int.MaxValue,
                result => userPoints.AddRange(result.Value?.data ?? Array.Empty<UserPoint>()));

            int visibleCount = 0;
            int hiddenCount = 0;
            List<bool> userVisibilities = new List<bool>();
            foreach (var userPoint in userPoints)
            {
                if (userPoint.hidden)
                { hiddenCount += 1; }
                else
                { visibleCount += 1; }
            }

            // Assert
            Debug.Log("Expected hidden data count: " + expectedHiddenCount);
            Debug.Log("Expected visible data count: " + expectedVisibleCount);
            Debug.Log("Hidden data count: " + hiddenCount);
            Debug.Log("Visible data count: " + visibleCount);

            Assert.That(hiddenCount, Is.EqualTo(expectedHiddenCount));
            Assert.That(visibleCount, Is.EqualTo(expectedVisibleCount));
        }
    }
}

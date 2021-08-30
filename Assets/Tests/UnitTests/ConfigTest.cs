// Copyright (c) 2021 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Api;
using AccelByte.Models;
using AccelByte.Server;
using NUnit.Framework;

namespace Tests.UnitTests
{
    [TestFixture]
    public class ConfigTest
    {
        [Test]
        public void AssertClientUrl_NotEmpty()
        {
            Config config = AccelBytePlugin.Config;
            Assert.IsNotNull(config.IamServerUrl);
            Assert.IsNotNull(config.PlatformServerUrl);
            Assert.IsNotNull(config.BasicServerUrl);
            Assert.IsNotNull(config.LobbyServerUrl);
            Assert.IsNotNull(config.CloudStorageServerUrl);
            Assert.IsNotNull(config.GameProfileServerUrl);
            Assert.IsNotNull(config.StatisticServerUrl);
            Assert.IsNotNull(config.QosManagerServerUrl);
            Assert.IsNotNull(config.AgreementServerUrl);
            Assert.IsNotNull(config.LeaderboardServerUrl);
            Assert.IsNotNull(config.CloudSaveServerUrl);
            Assert.IsNotNull(config.GameTelemetryServerUrl);
            Assert.IsNotNull(config.AchievementServerUrl);
            Assert.IsNotNull(config.GroupServerUrl);
            Assert.IsNotNull(config.UGCServerUrl);
            Assert.IsNotNull(config.SeasonPassServerUrl);
        } 

        [Test]
        public void AssertServerUrl_NotEmpty()
        {
            ServerConfig config = AccelByteServerPlugin.Config;
            Assert.IsNotNull(config.IamServerUrl);
            Assert.IsNotNull(config.DSMControllerServerUrl);
            Assert.IsNotNull(config.PlatformServerUrl);
            Assert.IsNotNull(config.StatisticServerUrl);
            Assert.IsNotNull(config.QosManagerServerUrl);
            Assert.IsNotNull(config.GameTelemetryServerUrl);
            Assert.IsNotNull(config.AchievementServerUrl);
            Assert.IsNotNull(config.LobbyServerUrl);
            Assert.IsNotNull(config.CloudSaveServerUrl);
            Assert.IsNotNull(config.MatchmakingServerUrl);
            Assert.IsNotNull(config.SeasonPassServerUrl);
        }
    }
}
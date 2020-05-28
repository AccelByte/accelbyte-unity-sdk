// Copyright (c) 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using AccelByte.Core;
using AccelByte.Api;
using AccelByte.Models;
using HybridWebSocket;
using UnityEngine;

namespace AccelByte.Server
{
    public static class AccelByteServerPlugin
    {
        private static readonly ServerOauthLoginSession session;
        private static readonly ServerConfig config;
        private static readonly CoroutineRunner coroutineRunner;
        private static readonly UnityHttpWorker httpWorker;
        private static TokenData accessToken;
        private static DedicatedServer server;
        private static DedicatedServerManager dedicatedServerManager;
        private static ServerEcommerce ecommerce;
        private static ServerStatistic statistic;
        private static ServerQos qos;

        public static ServerConfig Config { get { return AccelByteServerPlugin.config; } }

        static AccelByteServerPlugin()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            Utf8Json.Resolvers.CompositeResolver.RegisterAndSetAsDefault(
                new [] {
                    Utf8Json.Formatters.PrimitiveObjectFormatter.Default
                },
                new[] {
                    Utf8Json.Resolvers.GeneratedResolver.Instance,
                    Utf8Json.Resolvers.BuiltinResolver.Instance,
                    Utf8Json.Resolvers.EnumResolver.Default,
                    // for unity
                    Utf8Json.Unity.UnityResolver.Instance
                }
            );
#endif

            var configFile = Resources.Load("AccelByteServerSDKConfig");

            if (configFile == null)
            {
                throw new Exception(
                    "'AccelByteServerSDKConfig.json' isn't found in the Project/Assets/Resources directory");
            }

            string wholeJsonText = ((TextAsset) configFile).text;

            AccelByteServerPlugin.config = wholeJsonText.ToObject<ServerConfig>();
            AccelByteServerPlugin.config.Expand();
            AccelByteServerPlugin.coroutineRunner = new CoroutineRunner();
            AccelByteServerPlugin.httpWorker = new UnityHttpWorker();

            AccelByteServerPlugin.session = new ServerOauthLoginSession(
                AccelByteServerPlugin.config.IamServerUrl,
                AccelByteServerPlugin.config.ClientId,
                AccelByteServerPlugin.config.ClientSecret,
                AccelByteServerPlugin.httpWorker,
                AccelByteServerPlugin.coroutineRunner);

            AccelByteServerPlugin.server = new DedicatedServer(AccelByteServerPlugin.session, 
            AccelByteServerPlugin.coroutineRunner);
        }

        public static DedicatedServer GetDedicatedServer()
        {
            return AccelByteServerPlugin.server;
        }

        public static DedicatedServerManager GetDedicatedServerManager()
        {
            if (AccelByteServerPlugin.dedicatedServerManager == null)
            {
                AccelByteServerPlugin.dedicatedServerManager = new DedicatedServerManager(
                    new DedicatedServerManagerApi(
                        AccelByteServerPlugin.config.DSMControllerServerUrl,
                        AccelByteServerPlugin.config.Namespace,
                        AccelByteServerPlugin.httpWorker),
                    AccelByteServerPlugin.session,
                    AccelByteServerPlugin.coroutineRunner);
            }

            return AccelByteServerPlugin.dedicatedServerManager;
        }
        
        public static ServerEcommerce GetEcommerce()
        {
            if (AccelByteServerPlugin.ecommerce == null)
            {
                AccelByteServerPlugin.ecommerce = new ServerEcommerce(
                    new ServerEcommerceApi(
                        AccelByteServerPlugin.config.PlatformServerUrl,
                        AccelByteServerPlugin.httpWorker),
                    AccelByteServerPlugin.session,
                    AccelByteServerPlugin.config.Namespace,
                    AccelByteServerPlugin.coroutineRunner);
            }

            return AccelByteServerPlugin.ecommerce;
        }
        
        public static ServerStatistic GetStatistic()
        {
            if (AccelByteServerPlugin.statistic == null)
            {
                AccelByteServerPlugin.statistic = new ServerStatistic(
                    new ServerStatisticApi(
                        AccelByteServerPlugin.config.StatisticServerUrl,
                        AccelByteServerPlugin.httpWorker),
                    AccelByteServerPlugin.session,
                    AccelByteServerPlugin.config.Namespace,
                    AccelByteServerPlugin.coroutineRunner);
            }

            return AccelByteServerPlugin.statistic;
        }

        public static ServerQos GetQos()
        {
            if (AccelByteServerPlugin.qos == null)
            {
                AccelByteServerPlugin.qos = new ServerQos(
                    new ServerQosManagerApi(
                        AccelByteServerPlugin.config.QosManagerServerUrl,
                        AccelByteServerPlugin.httpWorker),
                    AccelByteServerPlugin.coroutineRunner);
            }

            return AccelByteServerPlugin.qos;
        }
    }
}

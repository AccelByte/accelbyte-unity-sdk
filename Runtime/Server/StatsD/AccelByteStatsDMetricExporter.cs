// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace AccelByte.Server
{
    public class AccelByteStatsDMetricExporterApi : ServerApiBase
    {
        internal readonly ServerConfig Config;
        private IAccelByteStatsDMetricCollector statsDMetricCollector;
        private UdpClient socket;
        private AccelByteHeartBeat maintainer;
        private int intervalInSeconds;

        private IPEndPoint endpoint;
        private IPAddress address;
        private int sendBufferSize = 1 << 16;
        private bool isOptionalMetricsEnabled = true;
        private bool isExporting = false;

        private string multiMetricPacket = "";
        private Dictionary<string, List<string>> metricLabel = new Dictionary<string, List<string>>();
        private ConcurrentQueue<string> metricQueue = new ConcurrentQueue<string>();

        public AccelByteStatsDMetricExporterApi(IHttpClient inHttpClient, ServerConfig inServerConfig, ISession inSession) : base(inHttpClient, inServerConfig, inServerConfig.StatsDServerUrl, inSession)
        {
            Config = inServerConfig;
            statsDMetricCollector = new AccelByteStatsDMetricCollector();
        }

        /// <summary>
        /// Initialize Metric Exporter with default / config values.
        /// </summary>
        public void Initialize()
        {
            Initialize(Config.StatsDServerUrl, Config.StatsDServerPort, Config.StatsDMetricInterval);
        }

        /// <summary>
        /// Initialize Metric Exporter with user-provided values.
        /// </summary>
        /// <param name="inAddress">IP Address of UDP server app</param>
        /// <param name="inPort">Port of UDP server app</param>
        /// <param name="intervalInSeconds">Interval in seconds of sending UDP data packets</param>
        public void Initialize(string inAddress, int inPort, int intervalInSeconds)
        {
            if (inAddress == "localhost" || string.IsNullOrEmpty(inAddress))
            {
                address = IPAddress.Loopback;
            }

            else if (!IPAddress.TryParse(inAddress, out address))
            {
                AccelByteDebug.LogWarning($"Invalid IPv4 Address Input - {inAddress}");
                return;
            }

            Initialize(address, inPort, intervalInSeconds);
        }

        /// <summary>
        /// Initialize Metric Exporter with user-provided values.
        /// </summary>
        /// <param name="inAddress">IP Address of UDP server app</param>
        /// <param name="inPort">Port of UDP server app</param>
        /// <param name="intervalInSeconds">Interval in seconds of sending UDP data packets</param>
        public void Initialize(IPAddress inAddress, int inPort, int intervalInSeconds)
        {
            endpoint = new IPEndPoint(inAddress, inPort);

            socket = new UdpClient();
            socket.Client.Blocking = false;
            socket.Client.SendBufferSize = sendBufferSize;

            StartExporting(intervalInSeconds);
        }

        /// <summary>
        /// Set Metric Exporter interval of sending UDP packets in seconds.
        /// </summary>
        /// <param name="inIntervalInSeconds">Interval in seconds of sending UDP data packets</param>
        public void SetIntervalSeconds(int inIntervalInSeconds)
        {
            StartExporting(inIntervalInSeconds);
        }

        /// <summary>
        /// Set Metric Exporter UDP maximum packet size.
        /// </summary>
        /// <param name="bufferSize">Buffer size in decimal bytes (default: 65536 | 1 << 16)</param>
        public void SetSendBufferSize(int bufferSize)
        {
            sendBufferSize = bufferSize;
            if (socket != null)
            {
                socket.Client.SendBufferSize = sendBufferSize;
            }
        }

        /// <summary>
        /// Get Metric Exporter UDP maximum packet size.
        /// </summary>
        /// <returns>Buffer size in decimal bytes</returns>
        public int GetSendBufferSize()
        {
            return sendBufferSize;
        }

        /// <summary>
        /// Set Label to a specific Key of metric
        /// </summary>
        /// <param name="key">Key to add label</param>
        /// <param name="label">Label name for the key</param>
        public void SetLabel(string key, string label)
        {
            if (!metricLabel.ContainsKey(key))
            {
                metricLabel.Add(key, new List<string>());
            }

            if (!metricLabel[key].Contains(label))
            {
                metricLabel[key].Add(label);
            }
        }

        /// <summary>
        /// Get tagged labels of specific key.
        /// </summary>
        /// <param name="key">Key of the metric</param>
        /// <param name="outLabel">The tagged labels of specific key</param>
        public void GetLabels(string key, out List<string> outLabel)
        {
            metricLabel.TryGetValue(key, out outLabel);
        }

        /// <summary>
        /// Queue up a key-pair value as a metric to send to UDP server.
        /// </summary>
        /// <param name="key">Key of the metric</param>
        /// <param name="value">Integer value of metric</param>
        public void EnqueueMetric(string key, int value)
        {
            EnqueueMetric(key, value.ToString());
        }

        /// <summary>
        /// Queue up a key-pair value as a metric to send to UDP server.
        /// </summary>
        /// <param name="key">Key of the metric</param>
        /// <param name="value">Floating point value of metric</param>
        public void EnqueueMetric(string key, double value)
        {
            EnqueueMetric(key, value.ToString());
        }

        /// <summary>
        /// Queue up a key-pair value as a metric to send to UDP server.
        /// </summary>
        /// <param name="key">Key of the metric</param>
        /// <param name="value">String value of metric</param>
        public void EnqueueMetric(string key, string value)
        {
            AccelByteStatsDMetricBuilder metricBuilder = new AccelByteStatsDMetricBuilder(key, value);
            List<string> labels;
            metricLabel.TryGetValue(key, out labels);

            if (labels != null)
            {
                foreach (var label in labels)
                {
                    metricBuilder.AddTag(label);
                }
            }
            
            string metric = metricBuilder.Build();

            if (multiMetricPacket.Length + metric.Length < sendBufferSize)
            {
                if (multiMetricPacket.Length > 0)
                {
                    multiMetricPacket += "\n";
                }
                multiMetricPacket += metric;
            }
            else
            {
                multiMetricPacket = metric;
                metricQueue.Enqueue(multiMetricPacket);
            }
        }

        /// <summary>
        /// Set automatic sending of optional metrics.
        /// </summary>
        /// <param name="isEnabled"></param>
        public void SetOptionalMetricsEnabled(bool isEnabled)
        {
            isOptionalMetricsEnabled = isEnabled;
        }

        /// <summary>
        /// Sets Metric Collector to be used by the exporter
        /// </summary>
        /// <typeparam name="T">Type of: IAccelByteStatsDMetricCollector</typeparam>
        /// <param name="collector">Type of: IAccelByteStatsDMetricCollector</param>
        public void SetStatsDMetricCollector<T>(ref T collector) where T : IAccelByteStatsDMetricCollector
        {
            statsDMetricCollector = collector;
        }

        internal IAccelByteStatsDMetricCollector GetStatsDMetricCollector()
        {
            return statsDMetricCollector;
        }

        /// <summary>
        /// Collect basic and performance metrics and enqueue them.
        /// </summary>
        public void CollectMetrics()
        {
            if (statsDMetricCollector == null)
            {
                return;
            }

            // Basic Metrics
            EnqueueMetric("PlayerCapacity", statsDMetricCollector.GetPlayerCapacity());
            EnqueueMetric("PlayerCount", statsDMetricCollector.GetPlayerCount());
            EnqueueMetric("ClientCount", statsDMetricCollector.GetClientCount());
            EnqueueMetric("AiCount", statsDMetricCollector.GetAiCount());

            // Performance metrics
            EnqueueMetric("FrameTimeAverage", statsDMetricCollector.GetFrameTimeAverage());
            EnqueueMetric("FrameTimeMax", statsDMetricCollector.GetFrameTimeMax());
            EnqueueMetric("FrameStartDelayAverage", statsDMetricCollector.GetFrameStartDelayAverage());
            EnqueueMetric("FrameStartDelayMax", statsDMetricCollector.GetFrameStartDelayMax());
        }

        protected void ExportMetrics()
        {
            if (!string.IsNullOrEmpty(multiMetricPacket))
            {
                metricQueue.Enqueue(multiMetricPacket);
                multiMetricPacket = "";
            }

            if (isOptionalMetricsEnabled)
            {
                CollectMetrics();
            }

            string dequeueResult;
            lock(metricQueue)
            {
                while (!metricQueue.IsEmpty)
                {
                    if (metricQueue.TryDequeue(out dequeueResult))
                    {
                        socket.Send(dequeueResult.ToUtf8Json(), dequeueResult.Length, endpoint);
                        OnMetricSent?.Invoke(dequeueResult);
                    }
                }

                OnExportDone?.Invoke();
            }
        }

        private void StartExporting(int inIntervalInSeconds)
        {
            StopExporting();

            intervalInSeconds = inIntervalInSeconds;
            maintainer = new AccelByteHeartBeat(intervalInSeconds * 1000);
            maintainer.OnHeartbeatTrigger += ExportMetrics;

            maintainer.Start();
            isExporting = true;
        }

        private void StopExporting()
        {
            if (maintainer != null)
            {
                maintainer.Stop();
                maintainer = null;
            }
            
            isExporting = false;
        }

        #region Test Access

        internal int GetInterval()
        {
            return intervalInSeconds;
        }

        internal IPEndPoint GetEndpoint()
        {
            return endpoint;
        }

        internal IPAddress GetAddress()
        {
            return address;
        }

        internal bool IsExporting()
        {
            return isExporting;
        }

        internal string GetMetricPacket()
        {
            return multiMetricPacket;
        }

        internal string[] GetMetricQueue()
        {
            return metricQueue.ToArray();
        }

        internal Action<string> OnMetricSent;
        internal Action OnExportDone;

        #endregion
    }
}

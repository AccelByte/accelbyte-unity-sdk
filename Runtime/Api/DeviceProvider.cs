// Copyright (c) 2018 - 2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    public class DeviceProvider
    {
        public static string EncodeHMAC(string macAddress, string key)
        {
            try
            {
                byte[] byteArray = Encoding.ASCII.GetBytes(macAddress);
                using (var myhmacsha1 = new HMACSHA1(Encoding.ASCII.GetBytes(key)))
                {
                    var hashArray = myhmacsha1.ComputeHash(byteArray);
                    return hashArray.Aggregate("", (s, e) => s + System.String.Format("{0:x2}", e), s => s);
                }
            }
            catch (System.InvalidOperationException e)
            {
                throw e;
            }
        }

        public static DeviceProvider GetFromSystemInfo()
        {
            string identifier = "unity_" + SystemInfo.deviceType + "_" + GetPlatforName();
            string macAddress = GetDeviceMacAddress();
            string publisherNamespace = AccelBytePlugin.Config.PublisherNamespace;

#if UNITY_WEBGL && !UNITY_EDITOR
            if (!PlayerPrefs.HasKey("AccelByteDeviceUniqueId")){
                PlayerPrefs.SetString("AccelByteDeviceUniqueId", System.Guid.NewGuid().ToString());
            }
            return new DeviceProvider(
                "device",
                identifier + "_" + PlayerPrefs.GetString("AccelByteDeviceUniqueId")); 
#else
            return new DeviceProvider(
                "device",
                identifier + "_" + EncodeHMAC(macAddress, publisherNamespace));
#endif
        }

        public readonly string DeviceId;
        public readonly string DeviceType;

        private DeviceProvider( string deviceType
            , string deviceId )
        {
            Assert.IsNotNull(deviceType, "DeviceType is null!");
            Assert.IsNotNull(deviceId, "DeviceId is null!");
            this.DeviceType = deviceType;
            this.DeviceId = deviceId;
        }   

        public static string GetDeviceMacAddress()
        {
            return NetworkInterface.GetAllNetworkInterfaces()
                .Where(nic => nic.OperationalStatus == OperationalStatus.Up)
                .Select(nic => nic.GetPhysicalAddress().ToString())
                .FirstOrDefault();
        }

        public static string[] GetMacAddress()
        {
            NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

            string[] macAddressArray = new string[] { };
            foreach (NetworkInterface adapter in networkInterfaces)
            {
                string physicalAddress = adapter.GetPhysicalAddress().ToString();
                if (!string.IsNullOrEmpty(physicalAddress))
                {
                    macAddressArray = macAddressArray.Concat(new string[] { physicalAddress }).ToArray();
                }

            }
            return macAddressArray;
        }

        public static string GetPlatforName()
        {
            return Application.platform.ToString();
        }
    }
}
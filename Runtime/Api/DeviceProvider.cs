// Copyright (c) 2018 - 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
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
            return GetFromSystemInfo(AccelBytePlugin.Config.PublisherNamespace);
        }

        public static DeviceProvider GetFromSystemInfo(string encodeHMACKey)
        {
            string identifier = "unity_" + SystemInfo.deviceType + "_" + GetPlatforName();
            string platformUniqueIdentifier;

            Utils.Infoware.InfowareUtils iware;

            try
            {
                switch (Application.platform)
                {
                    case RuntimePlatform.OSXEditor:
                    case RuntimePlatform.OSXPlayer:
                        {
                            iware = new Utils.Infoware.MacOS();
                            string macAddress = iware.GetMacAddress();
                            platformUniqueIdentifier = EncodeHMAC(macAddress, encodeHMACKey);
                            break;
                        }
                    case RuntimePlatform.WindowsEditor:
                    case RuntimePlatform.WindowsPlayer:
                        {
                            iware = new Utils.Infoware.Windows();
                            string macAddress = iware.GetMacAddress();
                            platformUniqueIdentifier = EncodeHMAC(macAddress, encodeHMACKey);
                            break;
                        }
                    case RuntimePlatform.LinuxEditor:
                    case RuntimePlatform.LinuxPlayer:
                        {
                            iware = new Utils.Infoware.LinuxOS();
                            string macAddress = iware.GetMacAddress();
                            platformUniqueIdentifier = EncodeHMAC(macAddress, encodeHMACKey);
                            break;
                        }
                    case RuntimePlatform.IPhonePlayer:
                        {
                            iware = new Utils.Infoware.IOS();
                            string deviceId = iware.GetDeviceID();
                            platformUniqueIdentifier = EncodeHMAC(deviceId, encodeHMACKey);
                            break;
                        }
                    case RuntimePlatform.Android:
                        {
                            iware = new Utils.Infoware.Android();
                            string deviceId = iware.GetDeviceID();
                            platformUniqueIdentifier = EncodeHMAC(deviceId, encodeHMACKey);
                            break;
                        }
                    case RuntimePlatform.WebGLPlayer:
                        {
                            string newGuid;
                            if (!PlayerPrefs.HasKey("AccelByteDeviceUniqueId"))
                            {
                                newGuid = System.Guid.NewGuid().ToString();
                                PlayerPrefs.SetString("AccelByteDeviceUniqueId", newGuid);
                            }
                            else
                            {
                                newGuid = PlayerPrefs.GetString("AccelByteDeviceUniqueId");
                            }
                            platformUniqueIdentifier = newGuid;
                            break;
                        }
                    case RuntimePlatform.GameCoreXboxSeries:
                        {
                            iware = new Utils.Infoware.XSX();
                            platformUniqueIdentifier = iware.GetDeviceID();
                            break;
                        }
                    case RuntimePlatform.GameCoreXboxOne:
                        {
                            iware = new Utils.Infoware.XB1();
                            platformUniqueIdentifier = iware.GetDeviceID();
                            break;
                        }
                    case RuntimePlatform.PS4:
                        {
                            iware = new Utils.Infoware.PlayStation4();
                            string deviceId = iware.GetDeviceID();
                            platformUniqueIdentifier = EncodeHMAC(deviceId, encodeHMACKey);
                            break;
                        }
                    case RuntimePlatform.PS5:
                        {
                            iware = new Utils.Infoware.PlayStation4();
                            string deviceId = iware.GetDeviceID();
                            platformUniqueIdentifier = EncodeHMAC(deviceId, encodeHMACKey);
                            break;
                        }
                    default:
                        {
                            iware = new Utils.Infoware.OtherOs();
                            string uniqueIdentifier = iware.GetMacAddress();
                            if (string.IsNullOrEmpty(uniqueIdentifier))
                            {
                                uniqueIdentifier = iware.GetDeviceID();
                            }
                            platformUniqueIdentifier = EncodeHMAC(uniqueIdentifier, encodeHMACKey);
                            break;
                        }
                }
            }
            catch (System.Exception)
            {
                platformUniqueIdentifier = null;
            }

            if (string.IsNullOrEmpty(platformUniqueIdentifier))
            {
                platformUniqueIdentifier = System.Guid.NewGuid().ToString();
            }

            return new DeviceProvider(
                "device",
                identifier,
                platformUniqueIdentifier);
        }

        public readonly string DeviceId;
        public readonly string DeviceType;
        public readonly string UniqueId;

        private DeviceProvider( string deviceType
            , string identifier
            , string uniqueId)
        {
            Assert.IsNotNull(deviceType, "Device Type is null!");
            Assert.IsNotNull(identifier, "Device Id is null!");
            Assert.IsNotNull(uniqueId, "Unique Id is null!");
            this.DeviceType = deviceType;
            this.DeviceId = identifier + "_" + uniqueId;
            this.UniqueId = uniqueId;
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
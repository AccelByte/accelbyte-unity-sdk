// Copyright (c) 2018 - 2024 AccelByte Inc. All Rights Reserved.
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
        public readonly string DeviceId;
        public readonly string DeviceType;
        public readonly string UniqueId;

#if UNITY_SWITCH && !UNITY_EDITOR
        public static readonly string DefaultGeneratedIdCacheFileDir = "AccelByte/";
#else
        public static string DefaultGeneratedIdCacheFileDir = $"{Application.persistentDataPath}/AccelByte/{Application.productName}/";
#endif
        public static string CacheFileName = "DeviceId";

        public static string EncodeHMAC(string macAddress, string key)
        {
            if(string.IsNullOrEmpty(macAddress))
            {
                return macAddress;
            }

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

        public DeviceProvider(string deviceType
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

        public static DeviceProvider GetFromSystemInfo(string encodeKey, string generatedIdCacheFileDir = null)
        {
            string platformUniqueIdentifier = GetDeviceId(encodeKey);

            if (string.IsNullOrEmpty(platformUniqueIdentifier))
            {
#if !UNITY_WEBGL
                try
                {
                    if (string.IsNullOrEmpty(generatedIdCacheFileDir))
                    {
                        generatedIdCacheFileDir = DefaultGeneratedIdCacheFileDir;
                    }
                    AccelByteFileCacheImplementation fileCache = new AccelByteFileCacheImplementation(generatedIdCacheFileDir);
                    if (fileCache.Contains(CacheFileName))
                    {
                        platformUniqueIdentifier = fileCache.Retrieve(CacheFileName);
                        AccelByteDebug.LogVerbose($"Retrieve cached device id: {platformUniqueIdentifier}");
                    }
                    else
                    {
                        platformUniqueIdentifier = GenerateDeviceId();
                        fileCache.Emplace(CacheFileName, platformUniqueIdentifier);
                        AccelByteDebug.LogVerbose($"Generate new device id: {platformUniqueIdentifier}");
                    }
                } 
                catch(System.Exception exception)
                {
                    AccelByteDebug.LogWarning($"Unable to access device id cache, {exception.Message}");
                    if(string.IsNullOrEmpty(platformUniqueIdentifier))
                    {
                        platformUniqueIdentifier = GenerateDeviceId();
                    }
                    AccelByteDebug.LogVerbose($"Generate new device id: {platformUniqueIdentifier}");
                }
#else
                platformUniqueIdentifier = System.Guid.NewGuid().ToString();
#endif
            }

            string identifier = "unity_" + SystemInfo.deviceType + "_" + GetPlatformName();

            return new DeviceProvider(
                "device",
                identifier,
                platformUniqueIdentifier);
        }

#if !UNITY_WEBGL
        public static void CacheDeviceId(string cachedDeviceId, string generatedIdCacheFileDir = null)
        {
            try
            {
                if (string.IsNullOrEmpty(generatedIdCacheFileDir))
                {
                    generatedIdCacheFileDir = DefaultGeneratedIdCacheFileDir;
                }
                AccelByteFileCacheImplementation fileCache = new AccelByteFileCacheImplementation(generatedIdCacheFileDir);
                fileCache.Emplace(CacheFileName, cachedDeviceId);
            }
            catch (System.Exception exception)
            {
                AccelByteDebug.LogWarning($"Unable to cache device id, {exception.Message}");
            }
        }
#endif

        private static string GetDeviceId(string encodeKey)
        {
            Utils.Infoware.InfowareUtils iware;
            string platformUniqueIdentifier;

            try
            {
                switch (Application.platform)
                {
                    case RuntimePlatform.OSXEditor:
                    case RuntimePlatform.OSXPlayer:
                    {
                        iware = new Utils.Infoware.MacOS();
                        string macAddress = iware.GetMacAddress();
                        platformUniqueIdentifier = EncodeHMAC(macAddress, encodeKey);
                        break;
                    }
                    case RuntimePlatform.WindowsEditor:
                    case RuntimePlatform.WindowsPlayer:
                    {
                        iware = new Utils.Infoware.Windows();
                        string macAddress = iware.GetMacAddress();
                        platformUniqueIdentifier = EncodeHMAC(macAddress, encodeKey);
                        break;
                    }
                    case RuntimePlatform.LinuxEditor:
                    case RuntimePlatform.LinuxPlayer:
                    {
                        iware = new Utils.Infoware.LinuxOS();
                        string macAddress = iware.GetMacAddress();
                        platformUniqueIdentifier = EncodeHMAC(macAddress, encodeKey);
                        break;
                    }
                    case RuntimePlatform.IPhonePlayer:
                    {
                        iware = new Utils.Infoware.IOS();
                        string deviceId = iware.GetDeviceID();
                        platformUniqueIdentifier = EncodeHMAC(deviceId, encodeKey);
                        break;
                    }
                    case RuntimePlatform.Android:
                    {
                        iware = new Utils.Infoware.Android();
                        string deviceId = iware.GetDeviceID();
                        platformUniqueIdentifier = EncodeHMAC(deviceId, encodeKey);
                        break;
                    }
                    case RuntimePlatform.WebGLPlayer:
                    {
                        string newGuid;
                        if (!PlayerPrefs.HasKey("AccelByteDeviceUniqueId"))
                        {
                            newGuid = GenerateDeviceId();
                            PlayerPrefs.SetString("AccelByteDeviceUniqueId", newGuid);
                        }
                        else
                        {
                            newGuid = PlayerPrefs.GetString("AccelByteDeviceUniqueId");
                        }
                        platformUniqueIdentifier = newGuid;
                        break;
                    }
                    case RuntimePlatform.PS4:
                    {
                        iware = new Utils.Infoware.PlayStation4Infoware();
                        string deviceId = iware.GetDeviceID();
                        platformUniqueIdentifier = EncodeHMAC(deviceId, encodeKey);
                        break;
                    }
                    case RuntimePlatform.PS5:
                    {
                        iware = new Utils.Infoware.PlayStation5Infoware();
                        string deviceId = iware.GetDeviceID();
                        platformUniqueIdentifier = EncodeHMAC(deviceId, encodeKey);
                        break;
                    }
                    case RuntimePlatform.Switch:
                    {
                        iware = new Utils.Infoware.SwitchInfoware();
                        string deviceId = iware.GetDeviceID();
                        platformUniqueIdentifier = EncodeHMAC(deviceId, encodeKey);
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
                        platformUniqueIdentifier = EncodeHMAC(uniqueIdentifier, encodeKey);
                        break;
                    }
                }
            }
            catch (System.Exception ex)
            {
                AccelByteDebug.LogVerbose(ex.Message);
                platformUniqueIdentifier = null;
            }

            return platformUniqueIdentifier;
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
            string[] macAddressArray = null;
#if !UNITY_SWITCH || UNITY_EDITOR
            NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

            macAddressArray = new string[] { };
            foreach (NetworkInterface adapter in networkInterfaces)
            {
                string physicalAddress = adapter.GetPhysicalAddress().ToString();
                if (!string.IsNullOrEmpty(physicalAddress))
                {
                    macAddressArray = macAddressArray.Concat(new string[] { physicalAddress }).ToArray();
                }

            }
#endif
            return macAddressArray;
        }

        internal static string GetPlatformName()
        {
            return Application.platform.ToString();
        }

        private static string GenerateDeviceId()
        {
            string newGuid = System.Guid.NewGuid().ToString();
            return newGuid;
        }
    }
}
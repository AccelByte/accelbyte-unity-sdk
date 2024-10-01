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
        internal readonly bool IsGenerated;

#if UNITY_SWITCH && !UNITY_EDITOR
        public static readonly string DefaultGeneratedIdCacheFileDir = "AccelByte/";
#else
        public static string DefaultGeneratedIdCacheFileDir = $"{GameCommonInfo.PersistenPath}/AccelByte/";
#endif
        public static string CacheFileName = "DeviceId";

        private readonly static char[] eligibleCharacters =
        {
            '0', '1', '2', '3', '4', '5', '6','7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g',
            'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'
        };
        private static System.Random randomizer;
        private static int randomDeviceIdSeed;

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
            , string uniqueId) : this(deviceType, identifier, uniqueId, isGenerated: false)
        {
        }
        
        internal DeviceProvider(string deviceType
            , string identifier
            , string uniqueId
            , bool isGenerated)
        {
            Assert.IsNotNull(deviceType, "Device Type is null!");
            Assert.IsNotNull(identifier, "Device Id is null!");
            Assert.IsNotNull(uniqueId, "Unique Id is null!");
            this.DeviceType = deviceType;
            this.DeviceId = identifier + "_" + uniqueId;
            this.UniqueId = uniqueId;
            this.IsGenerated = isGenerated;
        }

        public static DeviceProvider GetFromSystemInfo(string encodeKey, string generatedIdCacheFileDir = null, IDebugger logger = null, IFileStream fs = null, Models.DeviceIdGeneratorConfig deviceIdGeneratorConfig = null)
        {
            bool randomizeUniqueIdentifier = deviceIdGeneratorConfig != null ? deviceIdGeneratorConfig.RandomizeDeviceId : false;
            string platformUniqueIdentifier = !randomizeUniqueIdentifier ? GetDeviceId(encodeKey, logger: logger) : null;
            bool isIdentifierGenerated = string.IsNullOrEmpty(platformUniqueIdentifier);

            if (isIdentifierGenerated)
            {
#if !UNITY_WEBGL
                try
                {
                    if (string.IsNullOrEmpty(generatedIdCacheFileDir))
                    {
                        generatedIdCacheFileDir = DefaultGeneratedIdCacheFileDir;
                    }
                    AccelByteFileCacheImplementation fileCache = new AccelByteFileCacheImplementation(generatedIdCacheFileDir, fs, logger);
                    if (fileCache.Contains(CacheFileName))
                    {
                        platformUniqueIdentifier = fileCache.Retrieve(CacheFileName);
                        logger?.LogVerbose($"Retrieve cached device id: {platformUniqueIdentifier}");
                    }
                    else
                    {
                        platformUniqueIdentifier = GenerateDeviceId();
                        fileCache.Emplace(CacheFileName, platformUniqueIdentifier);
                        logger?.LogVerbose($"Generate new device id: {platformUniqueIdentifier}");
                    }
                } 
                catch(System.Exception exception)
                {
                    logger?.LogWarning($"Unable to access device id cache, {exception.Message}");
                    if(string.IsNullOrEmpty(platformUniqueIdentifier))
                    {
                        platformUniqueIdentifier = GenerateDeviceId();
                    }
                    logger?.LogVerbose($"Generate new device id: {platformUniqueIdentifier}");
                }
#else
                platformUniqueIdentifier = System.Guid.NewGuid().ToString();
#endif
            }

            string identifier = "unity_" + GameCommonInfo.DeviceType + "_" + GetPlatformName();

            var retval = new DeviceProvider(
                deviceType:"device",
                identifier,
                platformUniqueIdentifier,
                isIdentifierGenerated);
            return retval;
        }

#if !UNITY_WEBGL
        public static void CacheDeviceId(string cachedDeviceId, string generatedIdCacheFileDir = null, IDebugger logger = null, IFileStream fs = null)
        {
            try
            {
                if (string.IsNullOrEmpty(generatedIdCacheFileDir))
                {
                    generatedIdCacheFileDir = DefaultGeneratedIdCacheFileDir;
                }
                AccelByteFileCacheImplementation fileCache = new AccelByteFileCacheImplementation(generatedIdCacheFileDir, fs, logger);
                fileCache.Emplace(CacheFileName, cachedDeviceId);
            }
            catch (System.Exception exception)
            {
                logger?.LogWarning($"Unable to cache device id, {exception.Message}");
            }
        }
#endif

        private static string GetDeviceId(string encodeKey, IDebugger logger)
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
                logger?.LogVerbose(ex.Message);
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
            return GameCommonInfo.PlatformName;
        }

        private static string GenerateDeviceId()
        {
            string newGuid = System.Guid.NewGuid().ToString();
            return newGuid;
        }
    }
}
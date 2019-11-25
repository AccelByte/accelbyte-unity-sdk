#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168

namespace Utf8Json.Resolvers
{
    using System;

    public class GeneratedTestResolver : global::Utf8Json.IJsonFormatterResolver
    {
        public static readonly global::Utf8Json.IJsonFormatterResolver Instance = new GeneratedTestResolver();

        GeneratedTestResolver()
        {

        }

        public global::Utf8Json.IJsonFormatter<T> GetFormatter<T>()
        {
            return FormatterCache<T>.formatter;
        }

        static class FormatterCache<T>
        {
            public static readonly global::Utf8Json.IJsonFormatter<T> formatter;

            static FormatterCache()
            {
                var f = GeneratedTestResolverGetFormatterHelper.GetFormatter(typeof(T));
                if (f != null)
                {
                    formatter = (global::Utf8Json.IJsonFormatter<T>)f;
                }
            }
        }
    }

    internal static class GeneratedTestResolverGetFormatterHelper
    {
        static readonly global::System.Collections.Generic.Dictionary<Type, int> lookup;

        static GeneratedTestResolverGetFormatterHelper()
        {
            lookup = new global::System.Collections.Generic.Dictionary<Type, int>(20)
            {
                {typeof(global::System.Collections.Generic.Dictionary<string, string>), 0 },
                {typeof(global::AccelByte.Models.Image[]), 1 },
                {typeof(global::AccelByte.Models.RegionDataItem[]), 2 },
                {typeof(global::Tests.TestHelper.StoreInfoModel[]), 3 },
                {typeof(global::Tests.IntegrationTests.EcommerceTest.TestVariables.EcommerceArgumentsModel), 4 },
                {typeof(global::Tests.TestHelper.CurrencyCreateModel), 5 },
                {typeof(global::Tests.TestHelper.CurrencyInfoModel), 6 },
                {typeof(global::Tests.TestHelper.CurrencySummaryModel), 7 },
                {typeof(global::Tests.TestHelper.StoreCreateModel), 8 },
                {typeof(global::Tests.TestHelper.CategoryCreateModel), 9 },
                {typeof(global::Tests.TestHelper.ItemCreateModel.Localization), 10 },
                {typeof(global::Tests.TestHelper.ItemCreateModel.Localizations), 11 },
                {typeof(global::Tests.TestHelper.ItemCreateModel.RegionDatas), 12 },
                {typeof(global::Tests.TestHelper.ItemCreateModel), 13 },
                {typeof(global::Tests.TestHelper.StoreInfoModel), 14 },
                {typeof(global::Tests.TestHelper.StoreListModel), 15 },
                {typeof(global::Tests.TestHelper.FullCategoryInfo), 16 },
                {typeof(global::Tests.TestHelper.FullItemInfo), 17 },
                {typeof(global::Tests.TestHelper.CreditRequestModel), 18 },
                {typeof(global::Tests.TestHelper.UserMapResponse), 19 },
            };
        }

        internal static object GetFormatter(Type t)
        {
            int key;
            if (!lookup.TryGetValue(t, out key)) return null;

            switch (key)
            {
                case 0: return new global::Utf8Json.Formatters.DictionaryFormatter<string, string>();
                case 1: return new global::Utf8Json.Formatters.ArrayFormatter<global::AccelByte.Models.Image>();
                case 2: return new global::Utf8Json.Formatters.ArrayFormatter<global::AccelByte.Models.RegionDataItem>();
                case 3: return new global::Utf8Json.Formatters.ArrayFormatter<global::Tests.TestHelper.StoreInfoModel>();
                case 4: return new Utf8Json.Formatters.Tests.IntegrationTests.EcommerceTest.TestVariables_EcommerceArgumentsModelFormatter();
                case 5: return new Utf8Json.Formatters.Tests.TestHelper_CurrencyCreateModelFormatter();
                case 6: return new Utf8Json.Formatters.Tests.TestHelper_CurrencyInfoModelFormatter();
                case 7: return new Utf8Json.Formatters.Tests.TestHelper_CurrencySummaryModelFormatter();
                case 8: return new Utf8Json.Formatters.Tests.TestHelper_StoreCreateModelFormatter();
                case 9: return new Utf8Json.Formatters.Tests.TestHelper_CategoryCreateModelFormatter();
                case 10: return new Utf8Json.Formatters.Tests.TestHelper_ItemCreateModel_LocalizationFormatter();
                case 11: return new Utf8Json.Formatters.Tests.TestHelper_ItemCreateModel_LocalizationsFormatter();
                case 12: return new Utf8Json.Formatters.Tests.TestHelper_ItemCreateModel_RegionDatasFormatter();
                case 13: return new Utf8Json.Formatters.Tests.TestHelper_ItemCreateModelFormatter();
                case 14: return new Utf8Json.Formatters.Tests.TestHelper_StoreInfoModelFormatter();
                case 15: return new Utf8Json.Formatters.Tests.TestHelper_StoreListModelFormatter();
                case 16: return new Utf8Json.Formatters.Tests.TestHelper_FullCategoryInfoFormatter();
                case 17: return new Utf8Json.Formatters.Tests.TestHelper_FullItemInfoFormatter();
                case 18: return new Utf8Json.Formatters.Tests.TestHelper_CreditRequestModelFormatter();
                case 19: return new Utf8Json.Formatters.Tests.TestHelper_UserMapResponseFormatter();
                default: return null;
            }
        }
    }
}

#pragma warning disable 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612

#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 219
#pragma warning disable 168

namespace Utf8Json.Formatters.Tests.IntegrationTests.EcommerceTest
{
    using Utf8Json;


    public sealed class TestVariables_EcommerceArgumentsModelFormatter : global::Utf8Json.IJsonFormatter<global::Tests.IntegrationTests.EcommerceTest.TestVariables.EcommerceArgumentsModel>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestVariables_EcommerceArgumentsModelFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("rootCategoryPath"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("childCategoryPath"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("grandChildCategoryPath"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("currencyCode"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("inGameItemTitle"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("currencyItemTitle"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("SdkCloneStoreId"), 6},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("ArchiveOriStoreId"), 7},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("rootCategoryPath"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("childCategoryPath"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("grandChildCategoryPath"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("currencyCode"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("inGameItemTitle"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("currencyItemTitle"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("SdkCloneStoreId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("ArchiveOriStoreId"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.IntegrationTests.EcommerceTest.TestVariables.EcommerceArgumentsModel value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.rootCategoryPath);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.childCategoryPath);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.grandChildCategoryPath);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteString(value.currencyCode);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteString(value.inGameItemTitle);
            writer.WriteRaw(this.____stringByteKeys[5]);
            writer.WriteString(value.currencyItemTitle);
            writer.WriteRaw(this.____stringByteKeys[6]);
            writer.WriteString(value.SdkCloneStoreId);
            writer.WriteRaw(this.____stringByteKeys[7]);
            writer.WriteString(value.ArchiveOriStoreId);
            
            writer.WriteEndObject();
        }

        public global::Tests.IntegrationTests.EcommerceTest.TestVariables.EcommerceArgumentsModel Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __rootCategoryPath__ = default(string);
            var __rootCategoryPath__b__ = false;
            var __childCategoryPath__ = default(string);
            var __childCategoryPath__b__ = false;
            var __grandChildCategoryPath__ = default(string);
            var __grandChildCategoryPath__b__ = false;
            var __currencyCode__ = default(string);
            var __currencyCode__b__ = false;
            var __inGameItemTitle__ = default(string);
            var __inGameItemTitle__b__ = false;
            var __currencyItemTitle__ = default(string);
            var __currencyItemTitle__b__ = false;
            var __SdkCloneStoreId__ = default(string);
            var __SdkCloneStoreId__b__ = false;
            var __ArchiveOriStoreId__ = default(string);
            var __ArchiveOriStoreId__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __rootCategoryPath__ = reader.ReadString();
                        __rootCategoryPath__b__ = true;
                        break;
                    case 1:
                        __childCategoryPath__ = reader.ReadString();
                        __childCategoryPath__b__ = true;
                        break;
                    case 2:
                        __grandChildCategoryPath__ = reader.ReadString();
                        __grandChildCategoryPath__b__ = true;
                        break;
                    case 3:
                        __currencyCode__ = reader.ReadString();
                        __currencyCode__b__ = true;
                        break;
                    case 4:
                        __inGameItemTitle__ = reader.ReadString();
                        __inGameItemTitle__b__ = true;
                        break;
                    case 5:
                        __currencyItemTitle__ = reader.ReadString();
                        __currencyItemTitle__b__ = true;
                        break;
                    case 6:
                        __SdkCloneStoreId__ = reader.ReadString();
                        __SdkCloneStoreId__b__ = true;
                        break;
                    case 7:
                        __ArchiveOriStoreId__ = reader.ReadString();
                        __ArchiveOriStoreId__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.IntegrationTests.EcommerceTest.TestVariables.EcommerceArgumentsModel();
            if(__rootCategoryPath__b__) ____result.rootCategoryPath = __rootCategoryPath__;
            if(__childCategoryPath__b__) ____result.childCategoryPath = __childCategoryPath__;
            if(__grandChildCategoryPath__b__) ____result.grandChildCategoryPath = __grandChildCategoryPath__;
            if(__currencyCode__b__) ____result.currencyCode = __currencyCode__;
            if(__inGameItemTitle__b__) ____result.inGameItemTitle = __inGameItemTitle__;
            if(__currencyItemTitle__b__) ____result.currencyItemTitle = __currencyItemTitle__;
            if(__SdkCloneStoreId__b__) ____result.SdkCloneStoreId = __SdkCloneStoreId__;
            if(__ArchiveOriStoreId__b__) ____result.ArchiveOriStoreId = __ArchiveOriStoreId__;

            return ____result;
        }
    }

}

#pragma warning disable 168
#pragma warning restore 219
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612
#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 219
#pragma warning disable 168

namespace Utf8Json.Formatters.Tests
{
    using Utf8Json;


    public sealed class TestHelper_CurrencyCreateModelFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.CurrencyCreateModel>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_CurrencyCreateModelFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("currencyCode"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("currencySymbol"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("currencyType"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("decimals"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("maxAmountPerTransaction"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("maxTransactionAmountPerDay"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("maxBalanceAmount"), 6},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("currencyCode"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("currencySymbol"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("currencyType"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("decimals"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("maxAmountPerTransaction"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("maxTransactionAmountPerDay"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("maxBalanceAmount"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.CurrencyCreateModel value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.currencyCode);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.currencySymbol);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.currencyType);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteInt32(value.decimals);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteInt32(value.maxAmountPerTransaction);
            writer.WriteRaw(this.____stringByteKeys[5]);
            writer.WriteInt32(value.maxTransactionAmountPerDay);
            writer.WriteRaw(this.____stringByteKeys[6]);
            writer.WriteInt32(value.maxBalanceAmount);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.CurrencyCreateModel Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __currencyCode__ = default(string);
            var __currencyCode__b__ = false;
            var __currencySymbol__ = default(string);
            var __currencySymbol__b__ = false;
            var __currencyType__ = default(string);
            var __currencyType__b__ = false;
            var __decimals__ = default(int);
            var __decimals__b__ = false;
            var __maxAmountPerTransaction__ = default(int);
            var __maxAmountPerTransaction__b__ = false;
            var __maxTransactionAmountPerDay__ = default(int);
            var __maxTransactionAmountPerDay__b__ = false;
            var __maxBalanceAmount__ = default(int);
            var __maxBalanceAmount__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __currencyCode__ = reader.ReadString();
                        __currencyCode__b__ = true;
                        break;
                    case 1:
                        __currencySymbol__ = reader.ReadString();
                        __currencySymbol__b__ = true;
                        break;
                    case 2:
                        __currencyType__ = reader.ReadString();
                        __currencyType__b__ = true;
                        break;
                    case 3:
                        __decimals__ = reader.ReadInt32();
                        __decimals__b__ = true;
                        break;
                    case 4:
                        __maxAmountPerTransaction__ = reader.ReadInt32();
                        __maxAmountPerTransaction__b__ = true;
                        break;
                    case 5:
                        __maxTransactionAmountPerDay__ = reader.ReadInt32();
                        __maxTransactionAmountPerDay__b__ = true;
                        break;
                    case 6:
                        __maxBalanceAmount__ = reader.ReadInt32();
                        __maxBalanceAmount__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.CurrencyCreateModel();
            if(__currencyCode__b__) ____result.currencyCode = __currencyCode__;
            if(__currencySymbol__b__) ____result.currencySymbol = __currencySymbol__;
            if(__currencyType__b__) ____result.currencyType = __currencyType__;
            if(__decimals__b__) ____result.decimals = __decimals__;
            if(__maxAmountPerTransaction__b__) ____result.maxAmountPerTransaction = __maxAmountPerTransaction__;
            if(__maxTransactionAmountPerDay__b__) ____result.maxTransactionAmountPerDay = __maxTransactionAmountPerDay__;
            if(__maxBalanceAmount__b__) ____result.maxBalanceAmount = __maxBalanceAmount__;

            return ____result;
        }
    }


    public sealed class TestHelper_CurrencyInfoModelFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.CurrencyInfoModel>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_CurrencyInfoModelFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("currencyCode"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("localizationDescriptions"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("currencySymbol"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("namespace"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("currencyType"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("decimals"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("maxAmountPerTransaction"), 6},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("maxTransactionAmountPerDay"), 7},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("maxBalanceAmount"), 8},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("createdAt"), 9},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("updatedAt"), 10},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("currencyCode"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("localizationDescriptions"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("currencySymbol"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("namespace"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("currencyType"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("decimals"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("maxAmountPerTransaction"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("maxTransactionAmountPerDay"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("maxBalanceAmount"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("createdAt"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("updatedAt"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.CurrencyInfoModel value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.currencyCode);
            writer.WriteRaw(this.____stringByteKeys[1]);
            formatterResolver.GetFormatterWithVerify<object>().Serialize(ref writer, value.localizationDescriptions, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.currencySymbol);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteString(value.Namespace);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteString(value.currencyType);
            writer.WriteRaw(this.____stringByteKeys[5]);
            writer.WriteInt32(value.decimals);
            writer.WriteRaw(this.____stringByteKeys[6]);
            writer.WriteInt32(value.maxAmountPerTransaction);
            writer.WriteRaw(this.____stringByteKeys[7]);
            writer.WriteInt32(value.maxTransactionAmountPerDay);
            writer.WriteRaw(this.____stringByteKeys[8]);
            writer.WriteInt32(value.maxBalanceAmount);
            writer.WriteRaw(this.____stringByteKeys[9]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.createdAt, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[10]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.updatedAt, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.CurrencyInfoModel Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __currencyCode__ = default(string);
            var __currencyCode__b__ = false;
            var __localizationDescriptions__ = default(object);
            var __localizationDescriptions__b__ = false;
            var __currencySymbol__ = default(string);
            var __currencySymbol__b__ = false;
            var __namespace__ = default(string);
            var __namespace__b__ = false;
            var __currencyType__ = default(string);
            var __currencyType__b__ = false;
            var __decimals__ = default(int);
            var __decimals__b__ = false;
            var __maxAmountPerTransaction__ = default(int);
            var __maxAmountPerTransaction__b__ = false;
            var __maxTransactionAmountPerDay__ = default(int);
            var __maxTransactionAmountPerDay__b__ = false;
            var __maxBalanceAmount__ = default(int);
            var __maxBalanceAmount__b__ = false;
            var __createdAt__ = default(global::System.DateTime);
            var __createdAt__b__ = false;
            var __updatedAt__ = default(global::System.DateTime);
            var __updatedAt__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __currencyCode__ = reader.ReadString();
                        __currencyCode__b__ = true;
                        break;
                    case 1:
                        __localizationDescriptions__ = formatterResolver.GetFormatterWithVerify<object>().Deserialize(ref reader, formatterResolver);
                        __localizationDescriptions__b__ = true;
                        break;
                    case 2:
                        __currencySymbol__ = reader.ReadString();
                        __currencySymbol__b__ = true;
                        break;
                    case 3:
                        __namespace__ = reader.ReadString();
                        __namespace__b__ = true;
                        break;
                    case 4:
                        __currencyType__ = reader.ReadString();
                        __currencyType__b__ = true;
                        break;
                    case 5:
                        __decimals__ = reader.ReadInt32();
                        __decimals__b__ = true;
                        break;
                    case 6:
                        __maxAmountPerTransaction__ = reader.ReadInt32();
                        __maxAmountPerTransaction__b__ = true;
                        break;
                    case 7:
                        __maxTransactionAmountPerDay__ = reader.ReadInt32();
                        __maxTransactionAmountPerDay__b__ = true;
                        break;
                    case 8:
                        __maxBalanceAmount__ = reader.ReadInt32();
                        __maxBalanceAmount__b__ = true;
                        break;
                    case 9:
                        __createdAt__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __createdAt__b__ = true;
                        break;
                    case 10:
                        __updatedAt__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __updatedAt__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.CurrencyInfoModel();
            if(__currencyCode__b__) ____result.currencyCode = __currencyCode__;
            if(__localizationDescriptions__b__) ____result.localizationDescriptions = __localizationDescriptions__;
            if(__currencySymbol__b__) ____result.currencySymbol = __currencySymbol__;
            if(__namespace__b__) ____result.Namespace = __namespace__;
            if(__currencyType__b__) ____result.currencyType = __currencyType__;
            if(__decimals__b__) ____result.decimals = __decimals__;
            if(__maxAmountPerTransaction__b__) ____result.maxAmountPerTransaction = __maxAmountPerTransaction__;
            if(__maxTransactionAmountPerDay__b__) ____result.maxTransactionAmountPerDay = __maxTransactionAmountPerDay__;
            if(__maxBalanceAmount__b__) ____result.maxBalanceAmount = __maxBalanceAmount__;
            if(__createdAt__b__) ____result.createdAt = __createdAt__;
            if(__updatedAt__b__) ____result.updatedAt = __updatedAt__;

            return ____result;
        }
    }


    public sealed class TestHelper_CurrencySummaryModelFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.CurrencySummaryModel>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_CurrencySummaryModelFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("currencyCode"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("currencySymbol"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("currencyType"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("namespace"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("decimals"), 4},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("currencyCode"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("currencySymbol"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("currencyType"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("namespace"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("decimals"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.CurrencySummaryModel value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.currencyCode);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.currencySymbol);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.currencyType);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteString(value.Namespace);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteInt32(value.decimals);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.CurrencySummaryModel Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __currencyCode__ = default(string);
            var __currencyCode__b__ = false;
            var __currencySymbol__ = default(string);
            var __currencySymbol__b__ = false;
            var __currencyType__ = default(string);
            var __currencyType__b__ = false;
            var __namespace__ = default(string);
            var __namespace__b__ = false;
            var __decimals__ = default(int);
            var __decimals__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __currencyCode__ = reader.ReadString();
                        __currencyCode__b__ = true;
                        break;
                    case 1:
                        __currencySymbol__ = reader.ReadString();
                        __currencySymbol__b__ = true;
                        break;
                    case 2:
                        __currencyType__ = reader.ReadString();
                        __currencyType__b__ = true;
                        break;
                    case 3:
                        __namespace__ = reader.ReadString();
                        __namespace__b__ = true;
                        break;
                    case 4:
                        __decimals__ = reader.ReadInt32();
                        __decimals__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.CurrencySummaryModel();
            if(__currencyCode__b__) ____result.currencyCode = __currencyCode__;
            if(__currencySymbol__b__) ____result.currencySymbol = __currencySymbol__;
            if(__currencyType__b__) ____result.currencyType = __currencyType__;
            if(__namespace__b__) ____result.Namespace = __namespace__;
            if(__decimals__b__) ____result.decimals = __decimals__;

            return ____result;
        }
    }


    public sealed class TestHelper_StoreCreateModelFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.StoreCreateModel>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_StoreCreateModelFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("title"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("description"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("supportedLanguages"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("supportedRegions"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("defaultRegion"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("defaultLanguage"), 5},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("title"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("description"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("supportedLanguages"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("supportedRegions"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("defaultRegion"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("defaultLanguage"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.StoreCreateModel value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.title);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.description);
            writer.WriteRaw(this.____stringByteKeys[2]);
            formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.supportedLanguages, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[3]);
            formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.supportedRegions, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteString(value.defaultRegion);
            writer.WriteRaw(this.____stringByteKeys[5]);
            writer.WriteString(value.defaultLanguage);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.StoreCreateModel Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __title__ = default(string);
            var __title__b__ = false;
            var __description__ = default(string);
            var __description__b__ = false;
            var __supportedLanguages__ = default(string[]);
            var __supportedLanguages__b__ = false;
            var __supportedRegions__ = default(string[]);
            var __supportedRegions__b__ = false;
            var __defaultRegion__ = default(string);
            var __defaultRegion__b__ = false;
            var __defaultLanguage__ = default(string);
            var __defaultLanguage__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __title__ = reader.ReadString();
                        __title__b__ = true;
                        break;
                    case 1:
                        __description__ = reader.ReadString();
                        __description__b__ = true;
                        break;
                    case 2:
                        __supportedLanguages__ = formatterResolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, formatterResolver);
                        __supportedLanguages__b__ = true;
                        break;
                    case 3:
                        __supportedRegions__ = formatterResolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, formatterResolver);
                        __supportedRegions__b__ = true;
                        break;
                    case 4:
                        __defaultRegion__ = reader.ReadString();
                        __defaultRegion__b__ = true;
                        break;
                    case 5:
                        __defaultLanguage__ = reader.ReadString();
                        __defaultLanguage__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.StoreCreateModel();
            if(__title__b__) ____result.title = __title__;
            if(__description__b__) ____result.description = __description__;
            if(__supportedLanguages__b__) ____result.supportedLanguages = __supportedLanguages__;
            if(__supportedRegions__b__) ____result.supportedRegions = __supportedRegions__;
            if(__defaultRegion__b__) ____result.defaultRegion = __defaultRegion__;
            if(__defaultLanguage__b__) ____result.defaultLanguage = __defaultLanguage__;

            return ____result;
        }
    }


    public sealed class TestHelper_CategoryCreateModelFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.CategoryCreateModel>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_CategoryCreateModelFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("categoryPath"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("localizationDisplayNames"), 1},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("categoryPath"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("localizationDisplayNames"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.CategoryCreateModel value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.categoryPath);
            writer.WriteRaw(this.____stringByteKeys[1]);
            formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<string, string>>().Serialize(ref writer, value.localizationDisplayNames, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.CategoryCreateModel Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __categoryPath__ = default(string);
            var __categoryPath__b__ = false;
            var __localizationDisplayNames__ = default(global::System.Collections.Generic.Dictionary<string, string>);
            var __localizationDisplayNames__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __categoryPath__ = reader.ReadString();
                        __categoryPath__b__ = true;
                        break;
                    case 1:
                        __localizationDisplayNames__ = formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<string, string>>().Deserialize(ref reader, formatterResolver);
                        __localizationDisplayNames__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.CategoryCreateModel();
            if(__categoryPath__b__) ____result.categoryPath = __categoryPath__;
            if(__localizationDisplayNames__b__) ____result.localizationDisplayNames = __localizationDisplayNames__;

            return ____result;
        }
    }


    public sealed class TestHelper_ItemCreateModel_LocalizationFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.ItemCreateModel.Localization>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_ItemCreateModel_LocalizationFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("title"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("description"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("images"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("thumbnailImage"), 3},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("title"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("description"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("images"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("thumbnailImage"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.ItemCreateModel.Localization value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.title);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.description);
            writer.WriteRaw(this.____stringByteKeys[2]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.Image[]>().Serialize(ref writer, value.images, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[3]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.Image>().Serialize(ref writer, value.thumbnailImage, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.ItemCreateModel.Localization Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __title__ = default(string);
            var __title__b__ = false;
            var __description__ = default(string);
            var __description__b__ = false;
            var __images__ = default(global::AccelByte.Models.Image[]);
            var __images__b__ = false;
            var __thumbnailImage__ = default(global::AccelByte.Models.Image);
            var __thumbnailImage__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __title__ = reader.ReadString();
                        __title__b__ = true;
                        break;
                    case 1:
                        __description__ = reader.ReadString();
                        __description__b__ = true;
                        break;
                    case 2:
                        __images__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.Image[]>().Deserialize(ref reader, formatterResolver);
                        __images__b__ = true;
                        break;
                    case 3:
                        __thumbnailImage__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.Image>().Deserialize(ref reader, formatterResolver);
                        __thumbnailImage__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.ItemCreateModel.Localization();
            if(__title__b__) ____result.title = __title__;
            if(__description__b__) ____result.description = __description__;
            if(__images__b__) ____result.images = __images__;
            if(__thumbnailImage__b__) ____result.thumbnailImage = __thumbnailImage__;

            return ____result;
        }
    }


    public sealed class TestHelper_ItemCreateModel_LocalizationsFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.ItemCreateModel.Localizations>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_ItemCreateModel_LocalizationsFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("en"), 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("en"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.ItemCreateModel.Localizations value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            formatterResolver.GetFormatterWithVerify<global::Tests.TestHelper.ItemCreateModel.Localization>().Serialize(ref writer, value.en, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.ItemCreateModel.Localizations Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __en__ = default(global::Tests.TestHelper.ItemCreateModel.Localization);
            var __en__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __en__ = formatterResolver.GetFormatterWithVerify<global::Tests.TestHelper.ItemCreateModel.Localization>().Deserialize(ref reader, formatterResolver);
                        __en__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.ItemCreateModel.Localizations();
            if(__en__b__) ____result.en = __en__;

            return ____result;
        }
    }


    public sealed class TestHelper_ItemCreateModel_RegionDatasFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.ItemCreateModel.RegionDatas>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_ItemCreateModel_RegionDatasFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("US"), 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("US"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.ItemCreateModel.RegionDatas value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.RegionDataItem[]>().Serialize(ref writer, value.US, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.ItemCreateModel.RegionDatas Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __US__ = default(global::AccelByte.Models.RegionDataItem[]);
            var __US__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __US__ = formatterResolver.GetFormatterWithVerify<global::AccelByte.Models.RegionDataItem[]>().Deserialize(ref reader, formatterResolver);
                        __US__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.ItemCreateModel.RegionDatas();
            if(__US__b__) ____result.US = __US__;

            return ____result;
        }
    }


    public sealed class TestHelper_ItemCreateModelFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.ItemCreateModel>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_ItemCreateModelFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("itemType"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("name"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("entitlementType"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("useCount"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("appId"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("appType"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("targetCurrencyCode"), 6},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("targetNamespace"), 7},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("categoryPath"), 8},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("localizations"), 9},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("status"), 10},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("sku"), 11},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("regionData"), 12},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("itemIds"), 13},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("tags"), 14},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("maxCountPerUser"), 15},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("maxCount"), 16},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("itemType"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("name"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("entitlementType"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("useCount"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("appId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("appType"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("targetCurrencyCode"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("targetNamespace"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("categoryPath"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("localizations"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("status"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("sku"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("regionData"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("itemIds"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("tags"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("maxCountPerUser"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("maxCount"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.ItemCreateModel value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.itemType);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.name);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.entitlementType);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteInt32(value.useCount);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteString(value.appId);
            writer.WriteRaw(this.____stringByteKeys[5]);
            writer.WriteString(value.appType);
            writer.WriteRaw(this.____stringByteKeys[6]);
            writer.WriteString(value.targetCurrencyCode);
            writer.WriteRaw(this.____stringByteKeys[7]);
            writer.WriteString(value.targetNamespace);
            writer.WriteRaw(this.____stringByteKeys[8]);
            writer.WriteString(value.categoryPath);
            writer.WriteRaw(this.____stringByteKeys[9]);
            formatterResolver.GetFormatterWithVerify<global::Tests.TestHelper.ItemCreateModel.Localizations>().Serialize(ref writer, value.localizations, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[10]);
            writer.WriteString(value.status);
            writer.WriteRaw(this.____stringByteKeys[11]);
            writer.WriteString(value.sku);
            writer.WriteRaw(this.____stringByteKeys[12]);
            formatterResolver.GetFormatterWithVerify<global::Tests.TestHelper.ItemCreateModel.RegionDatas>().Serialize(ref writer, value.regionData, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[13]);
            formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.itemIds, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[14]);
            formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.tags, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[15]);
            writer.WriteInt32(value.maxCountPerUser);
            writer.WriteRaw(this.____stringByteKeys[16]);
            writer.WriteInt32(value.maxCount);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.ItemCreateModel Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __itemType__ = default(string);
            var __itemType__b__ = false;
            var __name__ = default(string);
            var __name__b__ = false;
            var __entitlementType__ = default(string);
            var __entitlementType__b__ = false;
            var __useCount__ = default(int);
            var __useCount__b__ = false;
            var __appId__ = default(string);
            var __appId__b__ = false;
            var __appType__ = default(string);
            var __appType__b__ = false;
            var __targetCurrencyCode__ = default(string);
            var __targetCurrencyCode__b__ = false;
            var __targetNamespace__ = default(string);
            var __targetNamespace__b__ = false;
            var __categoryPath__ = default(string);
            var __categoryPath__b__ = false;
            var __localizations__ = default(global::Tests.TestHelper.ItemCreateModel.Localizations);
            var __localizations__b__ = false;
            var __status__ = default(string);
            var __status__b__ = false;
            var __sku__ = default(string);
            var __sku__b__ = false;
            var __regionData__ = default(global::Tests.TestHelper.ItemCreateModel.RegionDatas);
            var __regionData__b__ = false;
            var __itemIds__ = default(string[]);
            var __itemIds__b__ = false;
            var __tags__ = default(string[]);
            var __tags__b__ = false;
            var __maxCountPerUser__ = default(int);
            var __maxCountPerUser__b__ = false;
            var __maxCount__ = default(int);
            var __maxCount__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __itemType__ = reader.ReadString();
                        __itemType__b__ = true;
                        break;
                    case 1:
                        __name__ = reader.ReadString();
                        __name__b__ = true;
                        break;
                    case 2:
                        __entitlementType__ = reader.ReadString();
                        __entitlementType__b__ = true;
                        break;
                    case 3:
                        __useCount__ = reader.ReadInt32();
                        __useCount__b__ = true;
                        break;
                    case 4:
                        __appId__ = reader.ReadString();
                        __appId__b__ = true;
                        break;
                    case 5:
                        __appType__ = reader.ReadString();
                        __appType__b__ = true;
                        break;
                    case 6:
                        __targetCurrencyCode__ = reader.ReadString();
                        __targetCurrencyCode__b__ = true;
                        break;
                    case 7:
                        __targetNamespace__ = reader.ReadString();
                        __targetNamespace__b__ = true;
                        break;
                    case 8:
                        __categoryPath__ = reader.ReadString();
                        __categoryPath__b__ = true;
                        break;
                    case 9:
                        __localizations__ = formatterResolver.GetFormatterWithVerify<global::Tests.TestHelper.ItemCreateModel.Localizations>().Deserialize(ref reader, formatterResolver);
                        __localizations__b__ = true;
                        break;
                    case 10:
                        __status__ = reader.ReadString();
                        __status__b__ = true;
                        break;
                    case 11:
                        __sku__ = reader.ReadString();
                        __sku__b__ = true;
                        break;
                    case 12:
                        __regionData__ = formatterResolver.GetFormatterWithVerify<global::Tests.TestHelper.ItemCreateModel.RegionDatas>().Deserialize(ref reader, formatterResolver);
                        __regionData__b__ = true;
                        break;
                    case 13:
                        __itemIds__ = formatterResolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, formatterResolver);
                        __itemIds__b__ = true;
                        break;
                    case 14:
                        __tags__ = formatterResolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, formatterResolver);
                        __tags__b__ = true;
                        break;
                    case 15:
                        __maxCountPerUser__ = reader.ReadInt32();
                        __maxCountPerUser__b__ = true;
                        break;
                    case 16:
                        __maxCount__ = reader.ReadInt32();
                        __maxCount__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.ItemCreateModel();
            if(__itemType__b__) ____result.itemType = __itemType__;
            if(__name__b__) ____result.name = __name__;
            if(__entitlementType__b__) ____result.entitlementType = __entitlementType__;
            if(__useCount__b__) ____result.useCount = __useCount__;
            if(__appId__b__) ____result.appId = __appId__;
            if(__appType__b__) ____result.appType = __appType__;
            if(__targetCurrencyCode__b__) ____result.targetCurrencyCode = __targetCurrencyCode__;
            if(__targetNamespace__b__) ____result.targetNamespace = __targetNamespace__;
            if(__categoryPath__b__) ____result.categoryPath = __categoryPath__;
            if(__localizations__b__) ____result.localizations = __localizations__;
            if(__status__b__) ____result.status = __status__;
            if(__sku__b__) ____result.sku = __sku__;
            if(__regionData__b__) ____result.regionData = __regionData__;
            if(__itemIds__b__) ____result.itemIds = __itemIds__;
            if(__tags__b__) ____result.tags = __tags__;
            if(__maxCountPerUser__b__) ____result.maxCountPerUser = __maxCountPerUser__;
            if(__maxCount__b__) ____result.maxCount = __maxCount__;

            return ____result;
        }
    }


    public sealed class TestHelper_StoreInfoModelFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.StoreInfoModel>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_StoreInfoModelFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("storeId"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("namespace"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("title"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("description"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("published"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("supportedLanguages"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("supportedRegions"), 6},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("defaultRegion"), 7},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("defaultLanguage"), 8},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("createdAt"), 9},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("updatedAt"), 10},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("storeId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("namespace"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("title"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("description"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("published"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("supportedLanguages"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("supportedRegions"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("defaultRegion"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("defaultLanguage"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("createdAt"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("updatedAt"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.StoreInfoModel value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.storeId);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.Namespace);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.title);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteString(value.description);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteBoolean(value.published);
            writer.WriteRaw(this.____stringByteKeys[5]);
            formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.supportedLanguages, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[6]);
            formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.supportedRegions, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[7]);
            writer.WriteString(value.defaultRegion);
            writer.WriteRaw(this.____stringByteKeys[8]);
            writer.WriteString(value.defaultLanguage);
            writer.WriteRaw(this.____stringByteKeys[9]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.createdAt, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[10]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.updatedAt, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.StoreInfoModel Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __storeId__ = default(string);
            var __storeId__b__ = false;
            var __namespace__ = default(string);
            var __namespace__b__ = false;
            var __title__ = default(string);
            var __title__b__ = false;
            var __description__ = default(string);
            var __description__b__ = false;
            var __published__ = default(bool);
            var __published__b__ = false;
            var __supportedLanguages__ = default(string[]);
            var __supportedLanguages__b__ = false;
            var __supportedRegions__ = default(string[]);
            var __supportedRegions__b__ = false;
            var __defaultRegion__ = default(string);
            var __defaultRegion__b__ = false;
            var __defaultLanguage__ = default(string);
            var __defaultLanguage__b__ = false;
            var __createdAt__ = default(global::System.DateTime);
            var __createdAt__b__ = false;
            var __updatedAt__ = default(global::System.DateTime);
            var __updatedAt__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __storeId__ = reader.ReadString();
                        __storeId__b__ = true;
                        break;
                    case 1:
                        __namespace__ = reader.ReadString();
                        __namespace__b__ = true;
                        break;
                    case 2:
                        __title__ = reader.ReadString();
                        __title__b__ = true;
                        break;
                    case 3:
                        __description__ = reader.ReadString();
                        __description__b__ = true;
                        break;
                    case 4:
                        __published__ = reader.ReadBoolean();
                        __published__b__ = true;
                        break;
                    case 5:
                        __supportedLanguages__ = formatterResolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, formatterResolver);
                        __supportedLanguages__b__ = true;
                        break;
                    case 6:
                        __supportedRegions__ = formatterResolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, formatterResolver);
                        __supportedRegions__b__ = true;
                        break;
                    case 7:
                        __defaultRegion__ = reader.ReadString();
                        __defaultRegion__b__ = true;
                        break;
                    case 8:
                        __defaultLanguage__ = reader.ReadString();
                        __defaultLanguage__b__ = true;
                        break;
                    case 9:
                        __createdAt__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __createdAt__b__ = true;
                        break;
                    case 10:
                        __updatedAt__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __updatedAt__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.StoreInfoModel();
            if(__storeId__b__) ____result.storeId = __storeId__;
            if(__namespace__b__) ____result.Namespace = __namespace__;
            if(__title__b__) ____result.title = __title__;
            if(__description__b__) ____result.description = __description__;
            if(__published__b__) ____result.published = __published__;
            if(__supportedLanguages__b__) ____result.supportedLanguages = __supportedLanguages__;
            if(__supportedRegions__b__) ____result.supportedRegions = __supportedRegions__;
            if(__defaultRegion__b__) ____result.defaultRegion = __defaultRegion__;
            if(__defaultLanguage__b__) ____result.defaultLanguage = __defaultLanguage__;
            if(__createdAt__b__) ____result.createdAt = __createdAt__;
            if(__updatedAt__b__) ____result.updatedAt = __updatedAt__;

            return ____result;
        }
    }


    public sealed class TestHelper_StoreListModelFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.StoreListModel>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_StoreListModelFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("storeInfo"), 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("storeInfo"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.StoreListModel value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            formatterResolver.GetFormatterWithVerify<global::Tests.TestHelper.StoreInfoModel[]>().Serialize(ref writer, value.storeInfo, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.StoreListModel Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __storeInfo__ = default(global::Tests.TestHelper.StoreInfoModel[]);
            var __storeInfo__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __storeInfo__ = formatterResolver.GetFormatterWithVerify<global::Tests.TestHelper.StoreInfoModel[]>().Deserialize(ref reader, formatterResolver);
                        __storeInfo__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.StoreListModel();
            if(__storeInfo__b__) ____result.storeInfo = __storeInfo__;

            return ____result;
        }
    }


    public sealed class TestHelper_FullCategoryInfoFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.FullCategoryInfo>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_FullCategoryInfoFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("namespace"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("parentCategoryPath"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("categoryPath"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("createdAt"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("updatedAt"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("localizationDisplayNames"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("root"), 6},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("namespace"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("parentCategoryPath"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("categoryPath"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("createdAt"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("updatedAt"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("localizationDisplayNames"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("root"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.FullCategoryInfo value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.Namespace);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.parentCategoryPath);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.categoryPath);
            writer.WriteRaw(this.____stringByteKeys[3]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.createdAt, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[4]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.updatedAt, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[5]);
            formatterResolver.GetFormatterWithVerify<object>().Serialize(ref writer, value.localizationDisplayNames, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[6]);
            writer.WriteBoolean(value.root);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.FullCategoryInfo Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __namespace__ = default(string);
            var __namespace__b__ = false;
            var __parentCategoryPath__ = default(string);
            var __parentCategoryPath__b__ = false;
            var __categoryPath__ = default(string);
            var __categoryPath__b__ = false;
            var __createdAt__ = default(global::System.DateTime);
            var __createdAt__b__ = false;
            var __updatedAt__ = default(global::System.DateTime);
            var __updatedAt__b__ = false;
            var __localizationDisplayNames__ = default(object);
            var __localizationDisplayNames__b__ = false;
            var __root__ = default(bool);
            var __root__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __namespace__ = reader.ReadString();
                        __namespace__b__ = true;
                        break;
                    case 1:
                        __parentCategoryPath__ = reader.ReadString();
                        __parentCategoryPath__b__ = true;
                        break;
                    case 2:
                        __categoryPath__ = reader.ReadString();
                        __categoryPath__b__ = true;
                        break;
                    case 3:
                        __createdAt__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __createdAt__b__ = true;
                        break;
                    case 4:
                        __updatedAt__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __updatedAt__b__ = true;
                        break;
                    case 5:
                        __localizationDisplayNames__ = formatterResolver.GetFormatterWithVerify<object>().Deserialize(ref reader, formatterResolver);
                        __localizationDisplayNames__b__ = true;
                        break;
                    case 6:
                        __root__ = reader.ReadBoolean();
                        __root__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.FullCategoryInfo();
            if(__namespace__b__) ____result.Namespace = __namespace__;
            if(__parentCategoryPath__b__) ____result.parentCategoryPath = __parentCategoryPath__;
            if(__categoryPath__b__) ____result.categoryPath = __categoryPath__;
            if(__createdAt__b__) ____result.createdAt = __createdAt__;
            if(__updatedAt__b__) ____result.updatedAt = __updatedAt__;
            if(__localizationDisplayNames__b__) ____result.localizationDisplayNames = __localizationDisplayNames__;
            if(__root__b__) ____result.root = __root__;

            return ____result;
        }
    }


    public sealed class TestHelper_FullItemInfoFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.FullItemInfo>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_FullItemInfoFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("itemId"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("appId"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("appType"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("namespace"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("sku"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("name"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("entitlementType"), 6},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("useCount"), 7},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("categoryPath"), 8},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("localizations"), 9},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("status"), 10},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("itemType"), 11},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("targetCurrencyCode"), 12},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("targetNamespace"), 13},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("regionData"), 14},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("itemIds"), 15},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("tags"), 16},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("maxCountPerUser"), 17},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("maxCount"), 18},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("codeFiles"), 19},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("createdAt"), 20},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("updatedAt"), 21},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("itemId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("appId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("appType"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("namespace"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("sku"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("name"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("entitlementType"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("useCount"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("categoryPath"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("localizations"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("status"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("itemType"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("targetCurrencyCode"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("targetNamespace"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("regionData"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("itemIds"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("tags"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("maxCountPerUser"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("maxCount"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("codeFiles"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("createdAt"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("updatedAt"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.FullItemInfo value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.itemId);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.appId);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.appType);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteString(value.Namespace);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteString(value.sku);
            writer.WriteRaw(this.____stringByteKeys[5]);
            writer.WriteString(value.name);
            writer.WriteRaw(this.____stringByteKeys[6]);
            writer.WriteString(value.entitlementType);
            writer.WriteRaw(this.____stringByteKeys[7]);
            writer.WriteInt32(value.useCount);
            writer.WriteRaw(this.____stringByteKeys[8]);
            writer.WriteString(value.categoryPath);
            writer.WriteRaw(this.____stringByteKeys[9]);
            formatterResolver.GetFormatterWithVerify<object>().Serialize(ref writer, value.localizations, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[10]);
            writer.WriteString(value.status);
            writer.WriteRaw(this.____stringByteKeys[11]);
            writer.WriteString(value.itemType);
            writer.WriteRaw(this.____stringByteKeys[12]);
            writer.WriteString(value.targetCurrencyCode);
            writer.WriteRaw(this.____stringByteKeys[13]);
            writer.WriteString(value.targetNamespace);
            writer.WriteRaw(this.____stringByteKeys[14]);
            formatterResolver.GetFormatterWithVerify<object>().Serialize(ref writer, value.regionData, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[15]);
            formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.itemIds, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[16]);
            formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.tags, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[17]);
            writer.WriteInt32(value.maxCountPerUser);
            writer.WriteRaw(this.____stringByteKeys[18]);
            writer.WriteInt32(value.maxCount);
            writer.WriteRaw(this.____stringByteKeys[19]);
            formatterResolver.GetFormatterWithVerify<string[]>().Serialize(ref writer, value.codeFiles, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[20]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.createdAt, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[21]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.updatedAt, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.FullItemInfo Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __itemId__ = default(string);
            var __itemId__b__ = false;
            var __appId__ = default(string);
            var __appId__b__ = false;
            var __appType__ = default(string);
            var __appType__b__ = false;
            var __namespace__ = default(string);
            var __namespace__b__ = false;
            var __sku__ = default(string);
            var __sku__b__ = false;
            var __name__ = default(string);
            var __name__b__ = false;
            var __entitlementType__ = default(string);
            var __entitlementType__b__ = false;
            var __useCount__ = default(int);
            var __useCount__b__ = false;
            var __categoryPath__ = default(string);
            var __categoryPath__b__ = false;
            var __localizations__ = default(object);
            var __localizations__b__ = false;
            var __status__ = default(string);
            var __status__b__ = false;
            var __itemType__ = default(string);
            var __itemType__b__ = false;
            var __targetCurrencyCode__ = default(string);
            var __targetCurrencyCode__b__ = false;
            var __targetNamespace__ = default(string);
            var __targetNamespace__b__ = false;
            var __regionData__ = default(object);
            var __regionData__b__ = false;
            var __itemIds__ = default(string[]);
            var __itemIds__b__ = false;
            var __tags__ = default(string[]);
            var __tags__b__ = false;
            var __maxCountPerUser__ = default(int);
            var __maxCountPerUser__b__ = false;
            var __maxCount__ = default(int);
            var __maxCount__b__ = false;
            var __codeFiles__ = default(string[]);
            var __codeFiles__b__ = false;
            var __createdAt__ = default(global::System.DateTime);
            var __createdAt__b__ = false;
            var __updatedAt__ = default(global::System.DateTime);
            var __updatedAt__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __itemId__ = reader.ReadString();
                        __itemId__b__ = true;
                        break;
                    case 1:
                        __appId__ = reader.ReadString();
                        __appId__b__ = true;
                        break;
                    case 2:
                        __appType__ = reader.ReadString();
                        __appType__b__ = true;
                        break;
                    case 3:
                        __namespace__ = reader.ReadString();
                        __namespace__b__ = true;
                        break;
                    case 4:
                        __sku__ = reader.ReadString();
                        __sku__b__ = true;
                        break;
                    case 5:
                        __name__ = reader.ReadString();
                        __name__b__ = true;
                        break;
                    case 6:
                        __entitlementType__ = reader.ReadString();
                        __entitlementType__b__ = true;
                        break;
                    case 7:
                        __useCount__ = reader.ReadInt32();
                        __useCount__b__ = true;
                        break;
                    case 8:
                        __categoryPath__ = reader.ReadString();
                        __categoryPath__b__ = true;
                        break;
                    case 9:
                        __localizations__ = formatterResolver.GetFormatterWithVerify<object>().Deserialize(ref reader, formatterResolver);
                        __localizations__b__ = true;
                        break;
                    case 10:
                        __status__ = reader.ReadString();
                        __status__b__ = true;
                        break;
                    case 11:
                        __itemType__ = reader.ReadString();
                        __itemType__b__ = true;
                        break;
                    case 12:
                        __targetCurrencyCode__ = reader.ReadString();
                        __targetCurrencyCode__b__ = true;
                        break;
                    case 13:
                        __targetNamespace__ = reader.ReadString();
                        __targetNamespace__b__ = true;
                        break;
                    case 14:
                        __regionData__ = formatterResolver.GetFormatterWithVerify<object>().Deserialize(ref reader, formatterResolver);
                        __regionData__b__ = true;
                        break;
                    case 15:
                        __itemIds__ = formatterResolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, formatterResolver);
                        __itemIds__b__ = true;
                        break;
                    case 16:
                        __tags__ = formatterResolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, formatterResolver);
                        __tags__b__ = true;
                        break;
                    case 17:
                        __maxCountPerUser__ = reader.ReadInt32();
                        __maxCountPerUser__b__ = true;
                        break;
                    case 18:
                        __maxCount__ = reader.ReadInt32();
                        __maxCount__b__ = true;
                        break;
                    case 19:
                        __codeFiles__ = formatterResolver.GetFormatterWithVerify<string[]>().Deserialize(ref reader, formatterResolver);
                        __codeFiles__b__ = true;
                        break;
                    case 20:
                        __createdAt__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __createdAt__b__ = true;
                        break;
                    case 21:
                        __updatedAt__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __updatedAt__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.FullItemInfo();
            if(__itemId__b__) ____result.itemId = __itemId__;
            if(__appId__b__) ____result.appId = __appId__;
            if(__appType__b__) ____result.appType = __appType__;
            if(__namespace__b__) ____result.Namespace = __namespace__;
            if(__sku__b__) ____result.sku = __sku__;
            if(__name__b__) ____result.name = __name__;
            if(__entitlementType__b__) ____result.entitlementType = __entitlementType__;
            if(__useCount__b__) ____result.useCount = __useCount__;
            if(__categoryPath__b__) ____result.categoryPath = __categoryPath__;
            if(__localizations__b__) ____result.localizations = __localizations__;
            if(__status__b__) ____result.status = __status__;
            if(__itemType__b__) ____result.itemType = __itemType__;
            if(__targetCurrencyCode__b__) ____result.targetCurrencyCode = __targetCurrencyCode__;
            if(__targetNamespace__b__) ____result.targetNamespace = __targetNamespace__;
            if(__regionData__b__) ____result.regionData = __regionData__;
            if(__itemIds__b__) ____result.itemIds = __itemIds__;
            if(__tags__b__) ____result.tags = __tags__;
            if(__maxCountPerUser__b__) ____result.maxCountPerUser = __maxCountPerUser__;
            if(__maxCount__b__) ____result.maxCount = __maxCount__;
            if(__codeFiles__b__) ____result.codeFiles = __codeFiles__;
            if(__createdAt__b__) ____result.createdAt = __createdAt__;
            if(__updatedAt__b__) ____result.updatedAt = __updatedAt__;

            return ____result;
        }
    }


    public sealed class TestHelper_CreditRequestModelFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.CreditRequestModel>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_CreditRequestModelFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("amount"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("source"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("reason"), 2},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("amount"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("source"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("reason"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.CreditRequestModel value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteInt32(value.amount);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.source);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteString(value.reason);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.CreditRequestModel Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __amount__ = default(int);
            var __amount__b__ = false;
            var __source__ = default(string);
            var __source__b__ = false;
            var __reason__ = default(string);
            var __reason__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __amount__ = reader.ReadInt32();
                        __amount__b__ = true;
                        break;
                    case 1:
                        __source__ = reader.ReadString();
                        __source__b__ = true;
                        break;
                    case 2:
                        __reason__ = reader.ReadString();
                        __reason__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.CreditRequestModel();
            if(__amount__b__) ____result.amount = __amount__;
            if(__source__b__) ____result.source = __source__;
            if(__reason__b__) ____result.reason = __reason__;

            return ____result;
        }
    }


    public sealed class TestHelper_UserMapResponseFormatter : global::Utf8Json.IJsonFormatter<global::Tests.TestHelper.UserMapResponse>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TestHelper_UserMapResponseFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("Namespace"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("UserId"), 1},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("Namespace"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("UserId"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Tests.TestHelper.UserMapResponse value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.Namespace);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.UserId);
            
            writer.WriteEndObject();
        }

        public global::Tests.TestHelper.UserMapResponse Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __Namespace__ = default(string);
            var __Namespace__b__ = false;
            var __UserId__ = default(string);
            var __UserId__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __Namespace__ = reader.ReadString();
                        __Namespace__b__ = true;
                        break;
                    case 1:
                        __UserId__ = reader.ReadString();
                        __UserId__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Tests.TestHelper.UserMapResponse();
            if(__Namespace__b__) ____result.Namespace = __Namespace__;
            if(__UserId__b__) ____result.UserId = __UserId__;

            return ____result;
        }
    }

}

#pragma warning disable 168
#pragma warning restore 219
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612

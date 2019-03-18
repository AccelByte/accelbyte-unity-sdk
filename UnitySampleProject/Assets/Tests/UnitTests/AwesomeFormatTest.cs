using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization;
using AccelByte.Api;
using AccelByte.Core;
using AccelByte.Models;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Tests.AUnitTests
{
    [TestFixture]
    public class AwesomeFormatTest
    {
        [DataContract]
        public class AllString
        {
            [DataMember] public string alpha;
            [DataMember] public string beta;
            [DataMember] public string gamma;
            [DataMember] public string delta;
            public string epsilon;
            public string zeta;
            public string eta;
            public string theta;
        }

        [DataContract]
        public class AllStringNoTypeName
        {
            [DataMember] public string alpha;
            [DataMember] public string beta;
            [DataMember] public string gamma;
            [DataMember] public string delta;
            public string epsilon;
            public string zeta;
            public string eta;
            public string theta;
        }

        [DataContract]
        public class AllPrimitives
        {
            [DataMember] public int alpha;
            [DataMember] public bool beta;
            [DataMember] public double gamma;
            [DataMember] public string delta;
            public string epsilon;
            public string zeta;
            public string eta;
            public string theta;
        }
        
        [DataContract]
        public class PrimitivesAndStringArrays
        {
            [DataMember] public int alpha;
            [DataMember] public bool beta;
            [DataMember] public double gamma;
            [DataMember] public string[] delta;
            public string epsilon;
            public string zeta;
            public string eta;
            public string theta;
        }

        [DataContract]
        public class PrimitivesAndBoolArrays
        {
            [DataMember] public int alpha;
            [DataMember] public bool beta;
            [DataMember] public double gamma;
            [DataMember] public bool[] delta;
            public string epsilon;
            public string zeta;
            public string eta;
            public string theta;
        }

        [DataContract]
        public class PrimitivesAndDoubleArrays
        {
            [DataMember] public int alpha;
            [DataMember] public bool beta;
            [DataMember] public double gamma;
            [DataMember] public double[] delta;
            public string epsilon;
            public string zeta;
            public string eta;
            public string theta;
        }
        
        [DataContract]
        public class PrimitivesAndDateTime
        {
            [DataMember] public int alpha;
            [DataMember] public bool beta;
            [DataMember] public double gamma;
            [DataMember] public DateTime delta;
            public string epsilon;
            public string zeta;
            public string eta;
            public string theta;
        }
        
        [UnityTest]
        public IEnumerator Deserialize_WithId_Successful()
        {
            string input =
                "type: unknown\n" +
                "id: 191919\n" +
                "alpha: iki_huruf_alpa\n" +
                "beta: iki huruf beta\n" +
                "gamma: iki_hurup_gama\n" +
                "delta: iki hurop delta\n" +
                "epsilon: iki kudune ra kanggo\n" +
                "zeta: iki kudune dibuang";

            MessageType type;
            long id;
            AllStringNoTypeName payload;
            ErrorCode headerErr = AwesomeFormat.ReadHeader(input, out type, out id);
            ErrorCode payloadErr = AwesomeFormat.ReadPayload(input, out payload);
            
            Assert.That(headerErr, Is.EqualTo(ErrorCode.None));
            Assert.That(id, Is.EqualTo(191919));
            Assert.That(payloadErr, Is.EqualTo(ErrorCode.None));
            Assert.That(payload.alpha, Is.EqualTo("iki_huruf_alpa"));
            Assert.That(payload.beta, Is.EqualTo("iki huruf beta"));
            Assert.That(payload.gamma, Is.EqualTo("iki_hurup_gama"));
            Assert.That(payload.delta, Is.EqualTo("iki hurop delta"));
            
            yield break;
        }
        
        [UnityTest]
        public IEnumerator Serialize_WithId_Successful()
        {
            var payload = new AllString
            {
                alpha = "the content of alpha",
                beta = "beta content should be something too",
                gamma = "gamma also needs placeholder string",
                delta = "I've run out of idea for delta content",
                epsilon = "epsilon should not be serialized",
                zeta = "I myself confused I still need to have this string",
                eta = "So insecure, I also write something into this field"
            };

            StringWriter writer = new StringWriter();
            AwesomeFormat.WriteHeader(writer, MessageType.unknown, 191919);
            AwesomeFormat.WritePayload(writer, payload);

            Assert.That(writer.ToString(), Is.EqualTo(
                "type: unknown\n" +
                "id: 191919\n" +
                "alpha: the content of alpha\n" +
                "beta: beta content should be something too\n" +
                "gamma: gamma also needs placeholder string\n" +
                "delta: I've run out of idea for delta content"));
            
            yield break;
        }
        
        [UnityTest]
        public IEnumerator Deserialize_WithIdAndCode_Successful()
        {
            string input =
                "type: unknown\n" +
                "id: 191919\n" +
                "code: 1111\n" +
                "alpha: iki_huruf_alpa\n" +
                "beta: iki huruf beta\n" +
                "gamma: iki_hurup_gama\n" +
                "delta: iki hurop delta\n" +
                "epsilon: iki kudune ra kanggo\n" +
                "zeta: iki kudune dibuang";

            MessageType type;
            long id;
            AllStringNoTypeName payload;
            ErrorCode headerErr = AwesomeFormat.ReadHeader(input, out type, out id);
            ErrorCode payloadErr = AwesomeFormat.ReadPayload(input, out payload);
            
            Assert.That(headerErr, Is.EqualTo((ErrorCode)1111));
            Assert.That(id, Is.EqualTo(191919));
            Assert.That(payloadErr, Is.EqualTo(ErrorCode.None));
            Assert.That(payload.alpha, Is.EqualTo("iki_huruf_alpa"));
            Assert.That(payload.beta, Is.EqualTo("iki huruf beta"));
            Assert.That(payload.gamma, Is.EqualTo("iki_hurup_gama"));
            Assert.That(payload.delta, Is.EqualTo("iki hurop delta"));
            
            yield break;
        }
        
        [UnityTest]
        public IEnumerator Serialize_WithIdAndCode_Successful()
        {
            var payload = new AllString
            {
                alpha = "the content of alpha",
                beta = "beta content should be something too",
                gamma = "gamma also needs placeholder string",
                delta = "I've run out of idea for delta content",
                epsilon = "epsilon should not be serialized",
                zeta = "I myself confused I still need to have this string",
                eta = "So insecure, I also write something into this field"
            };

            StringWriter writer = new StringWriter();
            AwesomeFormat.WriteHeader(writer, MessageType.unknown, 191919, ErrorCode.None);
            AwesomeFormat.WritePayload(writer, payload);

            Assert.That(writer.ToString(), Is.EqualTo(
                "type: unknown\n" +
                "id: 191919\n" +
                "code: 0\n" +
                "alpha: the content of alpha\n" +
                "beta: beta content should be something too\n" +
                "gamma: gamma also needs placeholder string\n" +
                "delta: I've run out of idea for delta content"));
            
            yield break;
        }

        [UnityTest]
        public IEnumerator Deserialize_WithNonErrorCode_Successful()
        {
            string input =
                "type: unknown\n" +
                "id: 0\n" +
                "code: 0\n" +
                "alpha: iki_huruf_alpa\n" +
                "beta: iki huruf beta\n" +
                "gamma: iki_hurup_gama\n" +
                "delta: iki hurop delta\n" +
                "epsilon: iki kudune ra kanggo\n" +
                "zeta: iki kudune dibuang";

            MessageType type;
            long id;
            AllStringNoTypeName payload;
            ErrorCode headerErr = AwesomeFormat.ReadHeader(input, out type, out id);
            ErrorCode payloadErr = AwesomeFormat.ReadPayload(input, out payload);
            
            Assert.That(headerErr, Is.EqualTo(ErrorCode.None));
            Assert.That(payloadErr, Is.EqualTo(ErrorCode.None));
            Assert.That(payload.alpha, Is.EqualTo("iki_huruf_alpa"));
            Assert.That(payload.beta, Is.EqualTo("iki huruf beta"));
            Assert.That(payload.gamma, Is.EqualTo("iki_hurup_gama"));
            Assert.That(payload.delta, Is.EqualTo("iki hurop delta"));
            
            yield break;
        }

        [UnityTest]
        public IEnumerator Deserialize_WithErrorCode_IsError()
        {
            string input =
                "type: unknown\n" +
                "id: 1111\n" +
                "code: 9999\n" +
                "alpha: iki_huruf_alpa\n" +
                "beta: iki huruf beta\n" +
                "gamma: iki_hurup_gama\n" +
                "delta: iki hurop delta\n" +
                "epsilon: iki kudune ra kanggo\n" +
                "zeta: iki kudune dibuang";

            MessageType type;
            long id;
            ErrorCode headerErr = AwesomeFormat.ReadHeader(input, out type, out id);
            
            Assert.That(headerErr, Is.Not.EqualTo(ErrorCode.None));
            
            yield break;
        }

        [UnityTest]
        public IEnumerator Deserialize_WithNonNumericCode_IsError()
        {
            string input =
                "type: unknown\n" +
                "id: 1111\n" +
                "code: 400welgeduwel\n" +
                "alpha: iki_huruf_alpa\n" +
                "beta: iki huruf beta\n" +
                "gamma: iki_hurup_gama\n" +
                "delta: iki hurop delta\n" +
                "epsilon: iki kudune ra kanggo\n" +
                "zeta: iki kudune dibuang";

            MessageType type;
            long id;
            AllStringNoTypeName payload;
            ErrorCode headerErr = AwesomeFormat.ReadHeader(input, out type, out id);
            
            Assert.That(headerErr, Is.Not.EqualTo(ErrorCode.None));
            
            yield break;
        }
        
        [UnityTest]
        public IEnumerator Deserialize_WithNoTypeName_Successful()
        {
            string input =
                "type: unknown\n" +
                "id: 1111\n" +
                "alpha: iki_huruf_alpa\n" +
                "beta: iki huruf beta\n" +
                "gamma: iki_hurup_gama\n" +
                "delta: iki hurop delta\n" +
                "epsilon: iki kudune ra kanggo\n" +
                "zeta: iki kudune dibuang";

            MessageType type;
            long id;
            AllStringNoTypeName payload;
            ErrorCode headerErr = AwesomeFormat.ReadHeader(input, out type, out id);
            ErrorCode payloadErr = AwesomeFormat.ReadPayload(input, out payload);

            
            Assert.That(headerErr, Is.EqualTo(ErrorCode.None));
            Assert.That(payloadErr, Is.EqualTo(ErrorCode.None));
            Assert.That(payload.alpha, Is.EqualTo("iki_huruf_alpa"));
            Assert.That(payload.beta, Is.EqualTo("iki huruf beta"));
            Assert.That(payload.gamma, Is.EqualTo("iki_hurup_gama"));
            Assert.That(payload.delta, Is.EqualTo("iki hurop delta"));
            
            yield break;
        }

        [UnityTest]
        public IEnumerator Deserialize_WithAllStringMembers_ForAllDataMemberAttributes_Successful()
        {
            string input =
                "type: unknown\n" +
                "id: 1111\n" +
                "alpha: iki_huruf_alpa\n" +
                "beta: iki huruf beta\n" +
                "gamma: iki_hurup_gama\n" +
                "delta: iki hurop delta\n" +
                "epsilon: iki kudune ra kanggo\n" +
                "zeta: iki kudune dibuang";

            MessageType type;
            long id;
            AllStringNoTypeName payload;
            ErrorCode headerErr = AwesomeFormat.ReadHeader(input, out type, out id);
            ErrorCode payloadErr = AwesomeFormat.ReadPayload(input, out payload);
            
            Assert.That(headerErr, Is.EqualTo(ErrorCode.None));
            Assert.That(payloadErr, Is.EqualTo(ErrorCode.None));
            Assert.That(payload.alpha, Is.EqualTo("iki_huruf_alpa"));
            Assert.That(payload.beta, Is.EqualTo("iki huruf beta"));
            Assert.That(payload.gamma, Is.EqualTo("iki_hurup_gama"));
            Assert.That(payload.delta, Is.EqualTo("iki hurop delta"));
            
            yield break;
        }

        [UnityTest]
        public IEnumerator Serialize_WithAllStringMembers_ForAllDataMemberAttributes_Successful()
        {
            var payload = new AllString
            {
                alpha = "the content of alpha",
                beta = "beta content should be something too",
                gamma = "gamma also needs placeholder string",
                delta = "I've run out of idea for delta content",
                epsilon = "epsilon should not be serialized",
                zeta = "I myself confused I still need to have this string",
                eta = "So insecure, I also write something into this field"
            };

            StringWriter writer = new StringWriter();
            AwesomeFormat.WriteHeader(writer, MessageType.unknown);
            AwesomeFormat.WritePayload(writer, payload);

            Assert.That(writer.ToString(), Is.EqualTo(
                "type: unknown\n" +
                "alpha: the content of alpha\n" +
                "beta: beta content should be something too\n" +
                "gamma: gamma also needs placeholder string\n" +
                "delta: I've run out of idea for delta content"));

            yield break;
        }

        [UnityTest]
        public IEnumerator Deserialize_WithPrimitives_ForAllDataMemberAttributes_Successful()
        {
            string input =
                "type: unknown\n" +
                "id: 1111\n" +
                "alpha: 123777\n" +
                "beta: True\n" +
                "gamma: 123e+3\n" +
                "delta: isih string iki\n" +
                "epsilon: iki kudune ra kanggo\n" +
                "zeta: iki kudune dibuang";

            MessageType type;
            long id;
            AllPrimitives payload;
            ErrorCode headerErr = AwesomeFormat.ReadHeader(input, out type, out id);
            ErrorCode payloadErr = AwesomeFormat.ReadPayload(input, out payload);
            
            Assert.That(headerErr, Is.EqualTo(ErrorCode.None));
            Assert.That(payloadErr, Is.EqualTo(ErrorCode.None));
            Assert.That(payload.alpha, Is.EqualTo(123777));
            Assert.That(payload.beta, Is.EqualTo(true));
            Assert.That(payload.gamma, Is.EqualTo(123000).Within(0.1));
            Assert.That(payload.delta, Is.EqualTo("isih string iki"));
            yield break;
        }
        
        [UnityTest]
        public IEnumerator Serialize_WithPrimitives_ForAllDataMemberAttributes_Successful()
        {
            var payload = new AllPrimitives
            {
                alpha = 707070,
                beta = true,
                gamma = 1230,
                delta = "this should be string, still",
                epsilon = "epsilon should not be serialized",
                zeta = "I myself confused I still need to have this string",
                eta = "So insecure, I also write something into this field"
            };

            var writer = new StringWriter();
            AwesomeFormat.WriteHeader(writer, MessageType.unknown);
            AwesomeFormat.WritePayload(writer, payload);

            Assert.That(writer.ToString(), Is.EqualTo(
                "type: unknown\n" +
                "alpha: 707070\n" +
                "beta: True\n" +
                "gamma: 1230\n" +
                "delta: this should be string, still"));

            yield break;
        }

        [UnityTest]
        public IEnumerator Deserialize_WithPrimitivesAndStringArrays_Successful()
        {
            string input =
                "type: unknown\n" +
                "id: 1111\n" +
                "alpha: 123777\n" +
                "beta: true\n" +
                "gamma: 123e+3\n" +
                "delta: deserialize, string, arrays,so,this,should,  work,\n" +
                "epsilon: iki kudune ra kanggo\n" +
                "zeta: iki kudune dibuang";

            MessageType type;
            long id;
            PrimitivesAndStringArrays payload;
            ErrorCode headerErr = AwesomeFormat.ReadHeader(input, out type, out id);
            ErrorCode payloadErr = AwesomeFormat.ReadPayload(input, out payload);
            
            Assert.That(headerErr, Is.EqualTo(ErrorCode.None));
            Assert.That(payloadErr, Is.EqualTo(ErrorCode.None));
            Assert.That(payload.alpha, Is.EqualTo(123777));
            Assert.That(payload.beta, Is.EqualTo(true));
            Assert.That(payload.gamma, Is.EqualTo(123000).Within(0.1));
            Assert.That(payload.delta, Is.EquivalentTo(new []
            {
                "deserialize", " string", " arrays", "so", "this",
                "should", "  work"
            }));
            
            yield break;
        }
        
        [UnityTest]
        public IEnumerator Serialize_WithPrimitivesAndStringArrays_Successful()
        {
            var payload = new PrimitivesAndStringArrays
            {
                alpha = 707070,
                beta = true,
                gamma = 1230,
                delta = new[] {"deserialize", " string", " arrays", "so", "this", "should", "  work"},
                epsilon = "epsilon should not be serialized",
                zeta = "I myself confused I still need to have this string",
                eta = "So insecure, I also write something into this field"
            };

            
            var writer = new StringWriter();
            AwesomeFormat.WriteHeader(writer, MessageType.unknown);
            AwesomeFormat.WritePayload(writer, payload);

            Assert.That(writer.ToString(), Is.EqualTo(
                    "type: unknown\n" +
                    "alpha: 707070\n" +
                    "beta: True\n" +
                    "gamma: 1230\n" +
                    "delta: [deserialize, string, arrays,so,this,should,  work]"));

            yield break;
        }
        
        [UnityTest]
        public IEnumerator Deserialize_WithPrimitivesAndBoolArrays_Successful()
        {
            string input =
                "type: unknown\n" +
                "id: 1111\n" +
                "alpha: 123777\n" +
                "beta: true\n" +
                "gamma: 123e+3\n" +
                "delta: True,False,True,False,True,True,True,\n" +
                "epsilon: iki kudune ra kanggo\n" +
                "zeta: iki kudune dibuang";

            MessageType type;
            long id;
            PrimitivesAndBoolArrays payload;
            ErrorCode headerErr = AwesomeFormat.ReadHeader(input, out type, out id);
            ErrorCode payloadErr = AwesomeFormat.ReadPayload(input, out payload);
            
            Assert.That(headerErr, Is.EqualTo(ErrorCode.None));
            Assert.That(payloadErr, Is.EqualTo(ErrorCode.None));
            Assert.That(payload.alpha, Is.EqualTo(123777));
            Assert.That(payload.beta, Is.EqualTo(true));
            Assert.That(payload.gamma, Is.EqualTo(123000).Within(0.1));
            Assert.That(payload.delta, Is.EquivalentTo(new[] {true, false, true, false, true, true, true}));
            yield break;
        }
        
        [UnityTest]
        public IEnumerator Serialize_WithPrimitivesAndBoolArrays_Successful()
        {
            var payload = new PrimitivesAndBoolArrays
            {
                alpha = 707070,
                beta = true,
                gamma = 1230,
                delta = new[] {true, false, true, false, true, true, true},
                epsilon = "epsilon should not be serialized",
                zeta = "I myself confused I still need to have this string",
                eta = "So insecure, I also write something into this field"
            };

            var writer = new StringWriter();
            AwesomeFormat.WriteHeader(writer, MessageType.unknown);
            AwesomeFormat.WritePayload(writer, payload);

            Assert.That(writer.ToString(), Is.EqualTo(
                    "type: unknown\n" +
                    "alpha: 707070\n" +
                    "beta: True\n" +
                    "gamma: 1230\n" +
                    "delta: [True,False,True,False,True,True,True]"));

            yield break;
        }
        
        [UnityTest]
        public IEnumerator Deserialize_WithPrimitivesAndDoubleArrays_Successful()
        {
            string input =
                "type: unknown\n" +
                "id: 1111\n" +
                "alpha: 123777\n" +
                "beta: true\n" +
                "gamma: 123e+3\n" +
                "delta: 16.125,2.25,7.375,8.000,\n" +
                "epsilon: iki kudune ra kanggo\n" +
                "zeta: iki kudune dibuang";

            MessageType type;
            long id;
            PrimitivesAndDoubleArrays payload;
            ErrorCode headerErr = AwesomeFormat.ReadHeader(input, out type, out id);
            ErrorCode payloadErr = AwesomeFormat.ReadPayload(input, out payload);
            
            Assert.That(headerErr, Is.EqualTo(ErrorCode.None));
            Assert.That(payloadErr, Is.EqualTo(ErrorCode.None));
            Assert.That(payload.alpha, Is.EqualTo(123777));
            Assert.That(payload.beta, Is.EqualTo(true));
            Assert.That(payload.gamma, Is.EqualTo(123000).Within(0.1));
            Assert.That(payload.delta, Is.EquivalentTo(new[] {16.125, 2.25, 7.375, 8}));
            
            yield break;
        }
        
        [UnityTest]
        public IEnumerator Serialize_WithPrimitivesAndDoubleArrays_Successful()
        {
            var payload = new PrimitivesAndDoubleArrays
            {
                alpha = 707070,
                beta = true,
                gamma = 1230,
                delta = new[] {16.125, 2.25, 7.375, 8.00},
                epsilon = "epsilon should not be serialized",
                zeta = "I myself confused I still need to have this string",
                eta = "So insecure, I also write something into this field"
            };

            var writer = new StringWriter();
            AwesomeFormat.WriteHeader(writer, MessageType.unknown);
            AwesomeFormat.WritePayload(writer, payload);

            Assert.That(writer.ToString(), Is.EqualTo(
                    "type: unknown\n" +
                    "alpha: 707070\n" +
                    "beta: True\n" +
                    "gamma: 1230\n" +
                    "delta: [16.125,2.25,7.375,8]"));

            yield break;
        }
        
        [UnityTest]
        public IEnumerator Deserialize_WithPrimitivesAndDateTime_Successful()
        {
            string input =
                "type: unknown\n" +
                "id: 1111\n" +
                "alpha: 123777\n" +
                "beta: true\n" +
                "gamma: 123e+3\n" +
                "delta: 2018-03-01T07:00:00.0000000Z\n" +
                "epsilon: iki kudune ra kanggo\n" +
                "zeta: iki kudune dibuang";

            MessageType type;
            long id;
            PrimitivesAndDateTime payload;
            ErrorCode headerErr = AwesomeFormat.ReadHeader(input, out type, out id);
            ErrorCode payloadErr = AwesomeFormat.ReadPayload(input, out payload);
            
            Assert.That(headerErr, Is.EqualTo(ErrorCode.None));
            Assert.That(payloadErr, Is.EqualTo(ErrorCode.None));
            Assert.That(payload.alpha, Is.EqualTo(123777));
            Assert.That(payload.beta, Is.EqualTo(true));
            Assert.That(payload.gamma, Is.EqualTo(123000).Within(0.1));
            Assert.That(payload.delta, Is.EqualTo(new DateTime(2018, 03, 01, 07, 00, 00, DateTimeKind.Utc)));
            
            yield break;
        }
        
        [UnityTest]
        public IEnumerator Serialize_WithPrimitivesAndDateTime_Successful()
        {
            var payload = new PrimitivesAndDateTime
            {
                alpha = 707070,
                beta = true,
                gamma = 1230,
                delta = new DateTime(2018, 03, 01, 07, 00, 00, DateTimeKind.Utc),
                epsilon = "epsilon should not be serialized",
                zeta = "I myself confused I still need to have this string",
                eta = "So insecure, I also write something into this field"
            };

            var writer = new StringWriter();
            AwesomeFormat.WriteHeader(writer, MessageType.unknown);
            AwesomeFormat.WritePayload(writer, payload);

            Assert.That(writer.ToString(), Is.EqualTo(
                "type: unknown\n" +
                "alpha: 707070\n" +
                "beta: True\n" +
                "gamma: 1230\n" +
                "delta: 2018-03-01T07:00:00.0000000Z"));

            yield break;
        }
        
        [UnityTest]
        public IEnumerator Deserialize_WithMissingFields_IsError()
        {
            string input =
                "type: unknown\n" +
                "id: 1111\n" +
                "beta: true\n" +
                "gamma: 123e+3\n" +
                "delta: 16.125,2.25,7.375,8.000,\n" +
                "epsilon: iki kudune ra kanggo\n" +
                "zeta: iki kudune dibuang";

            MessageType type;
            long id;
            PrimitivesAndDoubleArrays payload;
            ErrorCode headerErr = AwesomeFormat.ReadHeader(input, out type, out id);
            ErrorCode payloadErr = AwesomeFormat.ReadPayload(input, out payload);
            
            Assert.That(headerErr, Is.EqualTo(ErrorCode.None));
            Assert.That(payloadErr, Is.Not.EqualTo(ErrorCode.None));
            
            yield break;
        }
        
        [UnityTest]
        public IEnumerator Deserialize_WithEmptyNonStringField_IsError()
        {
            string input =
                "type: unknown\n" +
                "id: 1111\n" +
                "alpha: 707070\n" +
                "beta: \n" +
                "gamma: 123e+3\n" +
                "delta: 16.125,2.25,7.375,8.000,\n" +
                "epsilon: iki kudune ra kanggo\n" +
                "zeta: iki kudune dibuang";

            MessageType type;
            long id;
            PrimitivesAndDoubleArrays payload;
            ErrorCode headerErr = AwesomeFormat.ReadHeader(input, out type, out id);
            ErrorCode payloadErr = AwesomeFormat.ReadPayload(input, out payload);
            
            Assert.That(headerErr, Is.EqualTo(ErrorCode.None));
            Assert.That(payloadErr, Is.Not.EqualTo(ErrorCode.None));
            
            yield break;
        }
        
        [UnityTest]
        public IEnumerator Deserialize_WithInvalidMessage_IsError()
        {
            string input =
                "type: unknown\n" +
                "id: 1111\n" +
                ": 123777\n" +
                ": true\n" +
                "gamma: 123e+3\n" +
                "delta: 16.125,2.25,7.375,8.000,\n" +
                "epsilon: iki kudune ra kanggo\n" +
                "zeta: iki kudune dibuang\n";

            MessageType type;
            long id;
            PrimitivesAndDoubleArrays payload;
            ErrorCode headerErr = AwesomeFormat.ReadHeader(input, out type, out id);
            ErrorCode payloadErr = AwesomeFormat.ReadPayload(input, out payload);
            
            Assert.That(headerErr, Is.EqualTo(ErrorCode.None));
            Assert.That(payloadErr, Is.Not.EqualTo(ErrorCode.None));
            
            yield break;
        }

        [UnityTest]
        public IEnumerator Deserialize_WithDuplicateFields_IsError()
        {
            string input =
                "type: unknown\n" +
                "id: 1111\n" +
                "alpha: 123777\n" +
                "beta: true\n" +
                "alpha: 123000\n" +
                "delta: 16.125,2.25,7.375,8.000,\n" +
                "epsilon: iki kudune ra kanggo\n" +
                "zeta: iki kudune dibuang";

            MessageType type;
            long id;
            PrimitivesAndDoubleArrays payload;
            ErrorCode headerErr = AwesomeFormat.ReadHeader(input, out type, out id);
            ErrorCode payloadErr = AwesomeFormat.ReadPayload(input, out payload);
            
            Assert.That(headerErr, Is.EqualTo(ErrorCode.None));
            Assert.That(payloadErr, Is.Not.EqualTo(ErrorCode.None));
            
            yield break;
        }

        [UnityTest]
        public IEnumerator Deserialize_WithNoType_IsError()
        {
            string input =
                "alpha: 707070\n" +
                "id: 1111\n" +
                "beta: \n" +
                "gamma: 123e+3\n" +
                "delta: 16.125,2.25,7.375,8.000,\n" +
                "epsilon: iki kudune ra kanggo\n" +
                "zeta: iki kudune dibuang";

            MessageType type;
            long id;
            ErrorCode headerErr = AwesomeFormat.ReadHeader(input, out type, out id);
            
            Assert.That(headerErr, Is.Not.EqualTo(ErrorCode.None));
            
            yield break;
        }

        [UnityTest]
        public IEnumerator Deserialize_FriendStatusResponse_CorrectNaming()
        {
            string input = 
            "type: friendsStatusResponse" + "\n" +
            "id: 1" + "\n" +
            "code: 0" + "\n" +
            "friendsId: [966f80fdd4fc4b57b6b4ba18a79da3e5,9a9a769fd97248fc883174f4602ff7b0,ada9bb9853554f1d89693837e0877159,7def7f4e5edb4c30bdbd6b11a292e3ad,befa1ca938c6471095b3d79f68d04534,c86bd5dfb3634f4c80a867c6047f0a68,d31e2f9ff23b46d98dc1a5fd6bfd906e,101e5e76984b4c3bbddf1da657e972f4,236bf24904b348f8ac7eba47782f077e,f587612d078740f193b71efb838e390f,f613541fcb6f4f8f89547cf99ee366bb,8f4a31dc230e42c7854686eebde1ccf2,b5d83773f47e4a05bbea8ea41d356330,cdeb6625dfe84bff8575ee235c74d03a,cec1cee7e1784f5db7fa542fc35119f8,11af2264745041a4972b2b62f39b57ca,ce6481353e9e43acb874528e30cd87b1,027dc74e7ad5460db441e00e07fb8163,a9e08943ff8a4d6c85aada75786bf4de,da2cd86e322c4f19a1e86bde0cdebb1d,133d6b5b3ca0456e842b73c8274951a7,0b360bd09f3140a58345b604661f265b,f9de8861dc994456b650238269031ca5,602a3a628bed474c8a583b83b340d430,0895bf646c84439087203d0eb0bb0ca2,514abb596ae645b497190f4171a41ff8,40d7f57c4b3540ecb5a55014aed720c9,85a8ea9c83af4b8f98b7f4efe389a130,]" + "\n" +
            "availability: [0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,1,0,0,0,0,0,1,0,0,0,0,0,0,]" + "\n" +
            "activity: [nil,nil,nil,nil,nil,nil,nil,nil,nil,nil,nil,nil,random activity,nil,nil,random activity,nil,nil,nil,nil,nil,random activity,nil,nil,nil,nil,nil,nil,]" + "\n" +
            "lastSeenAt: [2018-12-18T03:33:35Z,2018-12-18T03:33:36Z,2018-12-18T03:21:26Z,2018-12-18T03:26:51Z,2018-12-18T03:18:10Z,2018-12-18T02:58:09Z,2018-12-18T02:55:04Z,2018-12-18T03:33:35Z,2018-12-18T03:33:36Z,2018-12-18T02:58:35Z,2018-12-18T02:54:48Z,2018-12-18T03:26:32Z,2018-12-18T03:44:19Z,2018-12-18T03:26:42Z,2018-12-18T03:21:26Z,2018-12-18T03:44:20Z,2018-12-18T03:33:35Z,2018-12-18T03:26:32Z,2018-12-18T03:19:17Z,2018-12-18T03:00:43Z,2018-12-18T03:26:43Z,2018-12-18T03:44:19Z,2018-12-18T03:18:10Z,2018-12-18T03:13:25Z,2018-12-18T02:54:48Z,2018-12-18T03:19:17Z,2018-12-18T03:13:25Z,2018-12-18T03:07:16Z,]";
            
            MessageType type;
            long id;
            ErrorCode headerErrorCode = AwesomeFormat.ReadHeader(input, out type, out id);
            
            Assert.That(headerErrorCode, Is.EqualTo(ErrorCode.None));
            yield return null;
            
        }
    }
}
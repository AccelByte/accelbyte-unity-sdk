using System;
using System.Collections;
using System.Diagnostics;
using System.Text.RegularExpressions;
using NUnit.Framework;
using UnityEngine.TestTools;
using Debug = UnityEngine.Debug;

namespace Tests.UnitTests
{
    [TestFixture]
    public class WaitTest
    {
        [UnityTest]
        public IEnumerator WaitForValue_Timeout()
        {
            var test = TestHelper.WaitForValue(
                () => null, 
                nameof(WaitForValue_Timeout), 
                3000);

            return FailIfNoException<TimeoutException>(test);
        }
        
        [UnityTest]
        public IEnumerator WaitUntil_Ok()
        {
            var sw = Stopwatch.StartNew();

            var test = TestHelper.WaitUntil(
                () => sw.Elapsed > TimeSpan.FromMilliseconds(1000),
                nameof(WaitUntil_Ok), 
                3000);

            return test;
        }

        [UnityTest]
        public IEnumerator WaitUntil_Timeout()
        {
            var test = TestHelper.WaitUntil(
                () => false, 
                nameof(WaitUntil_Timeout), 
                3000);

            return FailIfNoException<TimeoutException>(test);
        }
        
        [UnityTest]
        public IEnumerator WaitForValue_Ok()
        {
            var sw = Stopwatch.StartNew();

            var test = TestHelper.WaitForValue(
                () => sw.Elapsed > TimeSpan.FromMilliseconds(1000) ? new object() : null,
                nameof(WaitForValue_Ok), 
                3000);

            return test;
        }
        
        private static IEnumerator FailIfNoException<T>(IEnumerator test) where T : Exception
        {
            while (true)
            {
                try
                {
                    if (!test.MoveNext())
                    {
                        NUnit.Framework.Assert.Fail(); // Fail if no T exception until the end
                    }
                }
                catch (T ex)
                {
                    yield break;
                }

                yield return test.Current;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using UnityEngine;

namespace Tests.IntegrationTests
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class |
                    AttributeTargets.Interface | AttributeTargets.Assembly,
        AllowMultiple = true)]
    public class TestLogAttribute : Attribute, ITestAction, IApplyToTest
    {
        public void BeforeTest(ITest details)
        {
            try
            {
                Debug.LogFormat($"===== RUNNING TEST {details.FullName} =====");
            }
            catch (Exception ex)
            {
                Debug.Log($"[Error]: {ex} : {ex.StackTrace}");
            }
        }

        public void AfterTest(ITest details)
        {
            try
            {
                Debug.LogFormat($"===== END TEST {details.FullName} =====");
            }
            catch (Exception ex)
            {
                Debug.Log($"[Error]: {ex} : {ex.StackTrace}");
            }
        }

        public ActionTargets Targets => ActionTargets.Test | ActionTargets.Suite;

        private static string GetTestName(ITest test)
        {
            var className = test.ClassName ?? "{no class}";
            className = className.Substring(className.LastIndexOf(".", StringComparison.Ordinal) + 1);
            var method = test.MethodName ?? "{no method}";
            return $"{className}.{method}";
        }

        private static HashSet<string> testNameSet = null;
        private static bool IsDisabled(string testName)
        {
            if (testNameSet == null)
            {
                testNameSet = new HashSet<string>();
                // separated by ;
                // format: {className}.{methodName} or {className}.[{method1},{method2},...]
                // example: LobbyTest.[Lobby_StopReconnectWhenNotReconnectable,Lobby_Disconnect_CloseImmediately];
                var disabledTestsString = Environment.GetEnvironmentVariable("UNITY_SDK_DISABLED_TESTS") ?? "";
                foreach (var name in disabledTestsString.Split(';'))
                {
                    var index = name.Trim().IndexOf(".", StringComparison.Ordinal) + 1;
                    if (name.Length > 3 && index < name.Length && name[index] == '[')
                    {
                        var className = name.Substring(0, index - 1);
                        var methodNames = name.Substring(index + 1, name.Length - index - 2);
                        foreach (var methodName in methodNames.Split(','))
                        {
                            testNameSet.Add($"{className}.{methodName}");
                        }
                    }
                    else if(!string.IsNullOrEmpty(name))
                    {
                        testNameSet.Add(name);
                    }
                }

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Skipped tests list:");
                foreach (var name in testNameSet)
                {
                    sb.Append("* ");
                    sb.AppendLine(name);
                }
                Debug.Log(sb.ToString());
            }

            foreach (var name in testNameSet)
            {
                if(testName.StartsWith(name))
                {
                    return true;
                }
            }

            return false;
        }

        public void ApplyToTest(Test test)
        {
            if (test.RunState == RunState.NotRunnable || test.RunState == RunState.Ignored)
                return;

            var testName = GetTestName(test);

            if (IsDisabled(testName))
            {
                test.RunState = RunState.Ignored;
                test.Properties.Set("_SKIPREASON", (object) "Test disabled by UNITY_SDK_DISABLED_TESTS env var");
            }
        }
    }
}
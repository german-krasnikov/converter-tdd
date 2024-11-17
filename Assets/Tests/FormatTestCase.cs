using System;
using System.Collections;
using NUnit.Framework;

namespace Tests.Editor
{
    public static class FormatTestCase
    {
        public static IEnumerable FormatAs<T>(this object[] values, Func<T, string> format)
        {
            foreach (var source in values)
            {
                TestCaseData data = new TestCaseData((T)source);
                var args = (T)source;
                data.SetName(format(args));
                yield return data;
            }
        }
        
        public static IEnumerable FormatAsObjects(this object[] values, Func<object[], string> format)
        {
            foreach (var source in values)
            {
                TestCaseData data = new TestCaseData((object[])source);
                var args = (object[])source;
                data.SetName(format(args));
                yield return data;
            }
        }
        
        public static IEnumerable FormatAsString(this object[] values, Func<string, string> format) => FormatAs<string>(values, format);

        public static string IntArrayToString(this object values)
        {
            return $"[{string.Join(',', ((int[])values))}]";
        }
        //public static IEnumerable FormatAsObjects(this object[] values, Func<object[], string> format) => FormatAs(values, format);
    }
}
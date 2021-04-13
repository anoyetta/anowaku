using System;
using System.Collections.Generic;
using System.Linq;

namespace anowaku
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Walk<T>(
            this IEnumerable<T> e,
            Action<T> action)
        {
            foreach (var item in e)
            {
                action(item);
            }

            return e;
        }

        public static double Stdev<T>(this IEnumerable<T> src)
        {
            if (!src.Any())
            {
                return 0;
            }

            // Doubleにキャストして処理を進める
            var doubleList = src.Select(a => Convert.ToDouble(a)).ToArray();

            // 平均値算出
            double mean = doubleList.Average();

            // 自乗和算出
            double sum2 = doubleList.Select(a => a * a).Sum();

            // 分散 = 自乗和 / 要素数 - 平均値^2
            double variance = sum2 / doubleList.Count() - mean * mean;

            // 標準偏差 = 分散の平方根
            return Math.Sqrt(variance);
        }
    }
}

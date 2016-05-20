using System;

namespace Skeleton.Common.Extensions
{
    public static class DecimalExtensions
    {
        public static decimal RoundDecimalPoints(this decimal value, int decimalPoints)
        {
            return Math.Round(value, decimalPoints);
        }

        public static decimal RoundToTwoDecimalPoints(this decimal value)
        {
            return Math.Round(value, 2);
        }
    }
}
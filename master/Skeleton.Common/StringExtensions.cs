﻿using System;
using System.Globalization;

namespace Skeleton.Common
{
    public static class StringExtensions
    { 
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        public static bool EquivalentTo(this string value, string other)
        {
            value.ThrowIfNullOrEmpty(() => value);
            other.ThrowIfNullOrEmpty(() => other);

            return string.Equals(value, other, StringComparison.OrdinalIgnoreCase);
        }

        public static string FormatWith(this string value, params object[] parameters)
        {
            value.ThrowIfNullOrEmpty(() => value);

            return string.Format(CultureInfo.InvariantCulture, value, parameters);
        }
    }
}
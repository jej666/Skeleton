namespace Skeleton.Common.Extensions
{
    public static class NullableExtensions
    {
        public static T FromNullable<T>(this T? value) where T : struct
        {
            return value ?? default(T);
        }

        public static T? ToNullable<T>(this T value) where T : struct
        {
            return value.Equals(default(T)) ? null : (T?) value;
        }
    }
}
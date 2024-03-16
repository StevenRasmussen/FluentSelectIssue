using System.ComponentModel;
using System.Reflection;
using System.Text.RegularExpressions;

namespace FluentSelectIssue.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDisplayString<T>(this T value) where T : struct, Enum
        {
            var enumName = value.ToString();
            var enumType = typeof(T);
            var memberInfos = enumType.GetMember(enumName);
            var enumValueMemberInfo = memberInfos.FirstOrDefault(m => m.DeclaringType == enumType);
            if (enumValueMemberInfo != null)
            {
                var valueAttribute = enumValueMemberInfo.GetCustomAttribute<DescriptionAttribute>();
                if (valueAttribute is not null)
                {
                    return valueAttribute.Description;
                }
            }

            return enumName.SplitCamelCase() ?? enumName;
        }

        public static string? SplitCamelCase(this string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            return Regex.Replace(input, "(?<!\\b[A-Z])([A-Z])", " $1", RegexOptions.Compiled).Trim();
        }
    }
}

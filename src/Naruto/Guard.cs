using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Naruto
{
    /// <summary>
    /// 断言
    /// </summary>
    public static class Guard
    {
        public static T NotNull<T>(T argumentValue, string argumentName) where T : class
        {
            if (null == argumentValue)
            {
                throw new ArgumentNullException(argumentName, "The argument value cannot be null.");
            }
            return argumentValue;
        }

        public static string NotNullOrEmpty(string argumentValue, string argumentName)
        {
            NotNull(argumentValue, argumentName);
            if (string.IsNullOrEmpty(argumentValue))
            {
                throw new ArgumentException(argumentName, "The argument value cannot be empty.");
            }
            return argumentValue;
        }

        public static Guid NotNullOrEmpty(Guid argumentValue, string argumentName)
        {
            if (argumentValue == Guid.Empty)
            {
                throw new ArgumentException(argumentName, "The argument value cannot be empty.");
            }
            return argumentValue;
        }

        public static IEnumerable<T> NotNullOrEmpty<T>(IEnumerable<T> argumentValue, string argumentName)
        {
            NotNull(argumentValue, argumentName);
            if (!argumentValue.Any())
            {
                throw new ArgumentException(argumentName, "The argument value cannot be an empty collection.");
            }
            return argumentValue;
        }

        public static string NotNullOrWhiteSpace(string argumentValue, string argumentName)
        {
            NotNull(argumentValue, argumentName);
            if (string.IsNullOrWhiteSpace(argumentValue))
            {
                throw new ArgumentException(argumentName, "The argument value cannot be a white space.");
            }

            return argumentValue;
        }

        public static Type ArgumentAssignableTo<T>(Type argumentValue, string argumentName)
        {
            Guard.NotNull(argumentValue, argumentName);
            if (!typeof(T).GetTypeInfo().IsAssignableFrom(argumentValue))
            {
                throw new ArgumentException(argumentName, $"The specified type `{argumentValue.FullName}` cannot be assigned to the type `{typeof(T).FullName}`.");
            }

            return argumentValue;
        }
    }
}
